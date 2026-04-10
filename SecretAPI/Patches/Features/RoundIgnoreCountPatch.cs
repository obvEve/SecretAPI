namespace SecretAPI.Patches.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using LabApi.Features.Console;
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
        return typeof(RoundSummary).GetNestedMethod(StateMachine, UpdateTargetCount)
               ?? throw new Exception($"Could not locate state machine for {StateMachine} | {UpdateTargetCount}");
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .Start()
            .CreateLabel(out Label skip)
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_1),
                CodeInstruction.Call(typeof(RoundIgnoreCountPatch), nameof(IsPlayerIgnored)),
                new CodeInstruction(OpCodes.Brfalse_S, skip),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ret));

        return matcher.InstructionEnumeration();
    }

    private static bool IsPlayerIgnored(ReferenceHub hub) => Player.Get(hub).RoundIgnoreStatus.HasFlagFast(RoundIgnoreStatus.ScpTargetCount);
}