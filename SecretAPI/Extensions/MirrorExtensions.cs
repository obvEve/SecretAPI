namespace SecretAPI.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using global::AdminToys;
using LabApi.Features.Wrappers;
using Mirror;
using Logger = LabApi.Features.Console.Logger;

#pragma warning disable SA1117 // The parameters should all be placed on the same line or each parameter should be placed on its own line

/// <summary>
/// Extensions related to Mirror.
/// </summary>
public static class MirrorExtensions
{
    private static readonly Dictionary<Type, Delegate> TypeToWriterCache = new();
    private static readonly ExtraWriteInfo[] SubWriteInfos = new[]
    {
        new ExtraWriteInfo(typeof(AdminToyBase), 16UL /*Max written in AdminToyBase is 16*/, 1),
    };

    /// <summary>
    /// Send a fake rpc message to a player.
    /// </summary>
    /// <param name="target">The target to send the rpc to.</param>
    /// <param name="behaviour">The network behaviour containing the rpc.</param>
    /// <param name="type">The type containing the rpc.</param>
    /// <param name="rpcName">The name of the rpc to call.</param>
    /// <param name="values">The values to write to the writer.</param>
    public static void SendFakeRpcMessage(this Player target, NetworkBehaviour behaviour, Type type, string rpcName, params object[] values)
    {
        // validate the target is connected still
        if (!target.GameObject)
            return;

        NetworkWriterPooled pooledWriter = NetworkWriterPool.Get();

        foreach (object obj in values)
            ProperWrite(pooledWriter, obj);

        RpcMessage rpcMessage = new()
        {
            netId = behaviour.netId,
            componentIndex = behaviour.ComponentIndex,
            functionHash = (ushort)ReflectionExtensions.GetLongFuncName(type, rpcName).GetStableHashCode(),
            payload = pooledWriter.ToArraySegment(),
        };

        target.Connection.Send(rpcMessage);
        NetworkWriterPool.Return(pooledWriter);
    }

    /// <summary>
    /// Sends fake data of a <see cref="SyncList{T}"/> based on parameters.
    /// </summary>
    /// <param name="target">The target to send the fake data to.</param>
    /// <param name="behaviour">The <see cref="NetworkBehaviour"/> containing the <see cref="SyncList{T}"/>.</param>
    /// <param name="listIndex">The index of the <see cref="SyncList{T}"/> on the <see cref="NetworkBehaviour"/>. <b>The index starts at 1</b>.</param>
    /// <param name="change">The type of change to fake sync.</param>
    /// <typeparam name="T">The type contained by the <see cref="SyncList{T}"/>.</typeparam>
    public static void SendFakeSyncListData<T>(this Player target, NetworkBehaviour behaviour, ulong listIndex, SyncListChange<T> change)
    {
        if (listIndex <= 0)
            Logger.Warn($"[MirrorExtensions.SendFakeSyncListData] Index is {listIndex} - Expected 1 or higher!: + {new StackTrace()}");

        SendFakeState(target, behaviour, writer =>
        {
            // write the index of the list
            writer.WriteULong(listIndex);

            // copied from SyncList<T>.OnSerializeDelta
            writer.WriteUInt(1); // only 1 change
            writer.WriteByte((byte)change.Operation);
            switch (change.Operation)
            {
                case SyncList<T>.Operation.OP_ADD:
                    writer.Write(change.Item);
                    break;
                case SyncList<T>.Operation.OP_INSERT:
                case SyncList<T>.Operation.OP_SET:
                    writer.WriteUInt((uint)change.Index);
                    writer.Write(change.Item);
                    break;
                case SyncList<T>.Operation.OP_REMOVEAT:
                    writer.WriteUInt((uint)change.Index);
                    break;
            }
        }, null);
    }

    /// <summary>
    /// Sends a fake state of a <see cref="SyncVarAttribute"/> on a <see cref="NetworkBehaviour"/> to a target.
    /// </summary>
    /// <param name="target">The player to send the fake state to.</param>
    /// <param name="behaviour">The behaviour to send fake data of.</param>
    /// <param name="dirtyBit">The dirty bit of the syncvar to fake.</param>
    /// <param name="value">The value to fake the syncvar as.</param>
    /// <typeparam name="T">The type of the sync var.</typeparam>
    public static void SendFakeSyncVar<T>(this Player target, NetworkBehaviour behaviour, ulong dirtyBit, T value)
    {
        SendFakeState(target, behaviour, null, writer =>
        {
            // Always write the dirty bit
            writer.WriteULong(dirtyBit);

            ExtraWriteInfo info = GetExtraWriteInfo(behaviour.GetType());
            bool isWritten = false;

            // the current bit being written is higher than the max of the base, so we need to write extras before the value
            if (dirtyBit > info.MaxDirtyBit)
            {
                WriteExtraDirtyBit(info, writer, dirtyBit);
                isWritten = true;
            }

            writer.Write(value);

            // bit is not higher than the max, and we must write more after the value is written
            if (!isWritten)
                WriteExtraDirtyBit(info, writer, dirtyBit);
        });
    }

    /// <summary>
    /// Sends a fake entity state based on parameters to a target player.
    /// </summary>
    /// <param name="target">The player to send the fake state to.</param>
    /// <param name="behaviour">The <see cref="NetworkBehaviour"/> to send the fake state of.</param>
    /// <param name="syncObjectWriter">An Action indicating how to handle writing data related to <see cref="SyncObject"/>s.</param>
    /// <param name="syncVarWriter">An Action indicating how to handle writing data related to <see cref="SyncVarAttribute"/>s.</param>
    public static void SendFakeState(this Player target, NetworkBehaviour behaviour, Action<NetworkWriter>? syncObjectWriter, Action<NetworkWriter>? syncVarWriter)
    {
        // validate the target is connected still
        if (!target.GameObject)
            return;

        using NetworkWriterPooled pooledWriter = NetworkWriterPool.Get();

        // write the compressed bitmask of the behaviour index
        int index = behaviour.netIdentity.NetworkBehaviours.IndexOf(behaviour);
        ulong mask = (ulong)(1 << index);
        Compression.CompressVarUInt(pooledWriter, mask);

        // from NetworkBehaviour.Serialize
        // start serializing
        int headerPosition = pooledWriter.Position;
        pooledWriter.WriteByte(0);
        int contentPosition = pooledWriter.Position;

        if (syncObjectWriter != null)
            syncObjectWriter.Invoke(pooledWriter);
        else
            pooledWriter.WriteULong(0); // only needs to be written once for sync object

        if (syncVarWriter != null)
        {
            syncVarWriter.Invoke(pooledWriter);
        }
        else
        {
            // dirty bit is always 0 in this case
            pooledWriter.WriteULong(0);
            WriteExtraDirtyBit(GetExtraWriteInfo(behaviour.GetType()), pooledWriter);
        }

        // fill in length hash as the last byte of the 4 byte length
        int endPosition = pooledWriter.Position;
        pooledWriter.Position = headerPosition;
        int size = endPosition - contentPosition;
        byte safety = (byte)(size & 0xFF); // https://github.com/MirrorNetworking/Mirror/blob/master/Assets/Mirror/Core/NetworkBehaviour.cs#L1383
        pooledWriter.WriteByte(safety);
        pooledWriter.Position = endPosition;

        target.Connection.Send(new EntityStateMessage()
        {
            netId = behaviour.netId,
            payload = pooledWriter.ToArraySegment(),
        });
    }

    /// <summary>
    /// Handles writing <see cref="object"/> into a <see cref="NetworkWriter"/>.
    /// </summary>
    /// <param name="writer">The writer to write the object to.</param>
    /// <param name="obj">The object to write.</param>
    public static void ProperWrite(this NetworkWriter writer, object obj)
    {
        Type type = obj.GetType();

        // caching to avoid unnecessary reflection
        if (TypeToWriterCache.TryGetValue(type, out Delegate del))
        {
            del.DynamicInvoke(writer, obj);
            return;
        }

        Type genericType = typeof(Writer<>).MakeGenericType(obj.GetType());
        FieldInfo? writeField = genericType.GetField("write", BindingFlags.Static | BindingFlags.Public);
        if (writeField == null)
        {
            Logger.Warn($"Tried to write type: {obj.GetType()} but has no NetworkWriter!");
            return;
        }

        object? writeDelegate = writeField.GetValue(null);
        if (writeDelegate is not Delegate dele)
        {
            Logger.Warn($"Writer<{obj.GetType()}>.write is not a delegate!");
            return;
        }

        TypeToWriterCache.Add(type, dele);
        dele.DynamicInvoke(writer, obj);
    }

    private static void WriteExtraDirtyBit(ExtraWriteInfo writeInfo, NetworkWriter writer, ulong dirtyBit = 0)
    {
        if (!writeInfo.IsSet)
            return;

        for (int index = 0; index < writeInfo.ExtraWriteCount; ++index)
            writer.WriteULong(dirtyBit);
    }

    private static ExtraWriteInfo GetExtraWriteInfo(Type type)
    {
        foreach (ExtraWriteInfo subWriteInfo in SubWriteInfos)
        {
            if (type.IsSubclassOf(subWriteInfo.ClassType))
                return subWriteInfo;
        }

        return ExtraWriteInfo.None;
    }

    /// <summary>
    /// Public representation of the internal <see cref="SyncList{T}"/> Change class.
    /// </summary>
    /// <typeparam name="T">The item being contained by the <see cref="SyncList{T}"/>.</typeparam>
    public struct SyncListChange<T>
    {
        /// <summary>
        /// The operation to do on the <see cref="SyncList{T}"/>.
        /// </summary>
        public required SyncList<T>.Operation Operation;

        /// <summary>
        /// The index on which the Operation should be performed.
        /// </summary>
        /// <remarks>This is ignored when Operation is <see cref="SyncList{T}.Operation.OP_CLEAR"/> or <see cref="SyncList{T}.Operation.OP_ADD"/>.</remarks>
        public int Index;

        /// <summary>
        /// The value/item to use. Ignored when Operation is <see cref="SyncList{T}.Operation.OP_REMOVEAT"/> or <see cref="SyncList{T}.Operation.OP_CLEAR"/>.
        /// </summary>
        public T Item;
    }

    private readonly struct ExtraWriteInfo(Type classType, ulong dirtyBit, int writeCount)
    {
        public static readonly ExtraWriteInfo None = new(null!, 0, 0);

        public readonly Type ClassType = classType;
        public readonly ulong MaxDirtyBit = dirtyBit;
        public readonly int ExtraWriteCount = writeCount;
        public readonly bool IsSet = classType != null;
    }
}