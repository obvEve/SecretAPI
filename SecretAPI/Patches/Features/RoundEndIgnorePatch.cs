namespace SecretAPI.Patches.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles;
using SecretAPI.Attributes;
using SecretAPI.Enums;
using SecretAPI.Extensions;
using SecretAPI.Features;

/// <summary>
/// Handles patching to implement <see cref="RoundIgnoreStatus.RoundEndingCheck"/> into <see cref="RoundSummary.UpdateTargetCount"/>.
/// </summary>
[HarmonyPatchCategory(nameof(PlayerRoundIgnore))]
[HarmonyPatch]
internal static class RoundEndIgnorePatch
{
    private const string StateMachine = "<_ProcessServerSideCode>d__58";
    private const string MoveNext = "MoveNext";
    private const int ReferenceHubLocalIndex = 20;

    private static MethodInfo TargetMethod()
    {
        // typeof(RoundSummary).GetNestedTypes(AccessTools.all).ForEach(type => Logger.Debug(type.FullName ?? "NULL"));
        Type nestedType = typeof(RoundSummary).GetNestedTypes(AccessTools.all)
            .FirstOrDefault(type => type.Name is StateMachine) ?? throw new Exception($"Could not locate state machine for {StateMachine}");

        // nestedType.GetMethods(AccessTools.all).ForEach(method => Logger.Debug(method.Name));
        MethodInfo moveNextMethod = nestedType.GetMethods(AccessTools.all)
            .FirstOrDefault(x => x.Name.Contains(MoveNext)) ?? throw new Exception($"Could not locate {MoveNext} method in state machine");

        return moveNextMethod;
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .MatchEndForward(new CodeMatch(CodeInstruction.Call(typeof(PlayerRolesUtils), nameof(PlayerRolesUtils.GetTeam), [typeof(ReferenceHub)])))
            .CreateLabel(out Label skip)
            .Insert(
                new CodeInstruction(OpCodes.Ldloc_S, ReferenceHubLocalIndex),
                CodeInstruction.Call(typeof(RoundEndIgnorePatch), nameof(IsPlayerIgnored)),
                new CodeInstruction(OpCodes.Brtrue_S, skip));

        return matcher.InstructionEnumeration();
    }

    private static bool IsPlayerIgnored(ReferenceHub hub) => Player.Get(hub).RoundIgnoreStatus.HasFlagFast(RoundIgnoreStatus.RoundEndingCheck);
}