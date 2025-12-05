namespace SecretAPI.Extensions
{
    using System;
    using System.Reflection;
    using LabApi.Features.Console;
    using LabApi.Features.Wrappers;
    using Mirror;
    using Respawning;

    /// <summary>
    /// Extensions related to Mirror.
    /// </summary>
    public static class MirrorExtensions
    {
        /// <summary>
        /// Sends a fake cassie message to a player.
        /// </summary>
        /// <param name="target">The target to send the cassie message to.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="isHeld">Whether the cassie is held.</param>
        /// <param name="isNoisy">Whether the cassie is noisy.</param>
        /// <param name="isSubtitles">Whether there is subtitles on the cassie.</param>
        /// <param name="customSubtitles">The custom subtitles to use for the cassie.</param>
        public static void SendFakeCassieMessage(
            this Player target,
            string message,
            bool isHeld = false,
            bool isNoisy = true,
            bool isSubtitles = true,
            string customSubtitles = "")
        {
            foreach (RespawnEffectsController allController in RespawnEffectsController.AllControllers)
            {
                if (!allController)
                    continue;

                SendFakeRpcMessage(target, allController, typeof(RespawnEffectsController), nameof(RespawnEffectsController.RpcCassieAnnouncement), message, isHeld, isNoisy, isSubtitles, customSubtitles);
            }
        }

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
        /// Handles writing <see cref="object"/> into a <see cref="NetworkWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to write the object to.</param>
        /// <param name="obj">The object to write.</param>
        public static void ProperWrite(this NetworkWriter writer, object obj)
        {
            Type genericType = typeof(Writer<>).MakeGenericType(obj.GetType());
            FieldInfo? writeField = genericType.GetField("write", BindingFlags.Static | BindingFlags.Public);
            if (writeField == null)
            {
                Logger.Warn($"Tried to write type: {obj.GetType()} but has no NetworkWriter!");
                return;
            }

            object? writeDelegate = writeField.GetValue(null);
            if (writeDelegate is not Delegate del)
            {
                Logger.Warn($"Writer<{obj.GetType()}>.write is not a delegate!");
                return;
            }

            del.DynamicInvoke(writer, obj);
        }
    }
}