namespace SecretAPI.Patches.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using LabApi.Features.Wrappers;
using Mirror;
using SecretAPI.Attributes;
using SecretAPI.Enums;
using SecretAPI.Extensions;
using SecretAPI.Features;

/// <summary>
/// Handles patching to implement <see cref="RoundIgnoreStatus.ScpTargetCount"/> into <see cref="RoundSummary.UpdateTargetCount"/>.
/// </summary>
[HarmonyPatchCategory(nameof(PlayerRoundIgnore))]
[HarmonyPatch]
internal static class RoundIgnoreCountPatch
{
    private const string StateMachine = "<>c";
    private const string UpdateTargetCount = "UpdateTargetCount";

    private static MethodInfo TargetMethod()
    {
        Type nestedType = typeof(RoundSummary).GetNestedTypes(AccessTools.all)
            .FirstOrDefault(currentType => currentType.Name is StateMachine) ?? throw new Exception("Could not locate state machine for RoundSummary::<>c");

        // nestedType.GetMethods(AccessTools.all).ForEach(method => Logger.Debug(method.Name));
        MethodInfo updateCountMethod = nestedType.GetMethods(AccessTools.all)
            .FirstOrDefault(x => x.Name.Contains(UpdateTargetCount)) ?? throw new Exception($"Could not locate {UpdateTargetCount} method in state machine");

        return updateCountMethod;
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .Start()
            .CreateLabel(out Label skip)
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_0),
                CodeInstruction.Call(typeof(RoundIgnoreCountPatch), nameof(IsPlayerIgnored)),
                new CodeInstruction(OpCodes.Brtrue_S, skip));

        return matcher.InstructionEnumeration();
    }

    private static bool IsPlayerIgnored(ReferenceHub hub)
    {
        if (!NetworkServer.active || !Round.IsRoundStarted)
            return false;

        if (hub == null)
            return false;

        return hub && Player.Get(hub).RoundIgnoreStatus.HasFlagFast(RoundIgnoreStatus.ScpTargetCount);
    }
}