namespace SecretAPI.Patches.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;
using SecretAPI.Attributes;
using SecretAPI.Enums;
using SecretAPI.Extensions;
using SecretAPI.Features;

/// <summary>
/// Handles patching to implement <see cref="RoundIgnoreStatus.RoundEndingCheck"/> into <see cref="RoundSummary._ProcessServerSideCode"/>.
/// </summary>
[HarmonyPatchCategory(nameof(PlayerRoundIgnore))]
[HarmonyPatch]
internal static class RoundEndIgnorePatch
{
    private const string StateMachine = "_ProcessServerSideCode";
    private const string MoveNext = "MoveNext";
    private const int ReferenceHubLocalIndex = 20;

    private static MethodInfo TargetMethod()
    {
        return typeof(RoundSummary).GetNestedMethod(StateMachine, MoveNext)
               ?? throw new Exception($"Could not locate state machine for {StateMachine} | {MoveNext}");
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .MatchEndForward(new CodeMatch(CodeInstruction.Call(typeof(PlayerRolesUtils), nameof(PlayerRolesUtils.GetTeam), [typeof(ReferenceHub)])))
            .CreateLabel(out Label skip)
            .Insert(
                new CodeInstruction(OpCodes.Ldloc_S, ReferenceHubLocalIndex),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(RoundEndIgnorePatch), nameof(IsPlayerIgnored))),
                new CodeInstruction(OpCodes.Brtrue_S, skip));

        return matcher.InstructionEnumeration();
    }

    private static bool IsPlayerIgnored(ReferenceHub hub)
    {
        bool ignore = Player.Get(hub).RoundIgnoreStatus.HasFlagFast(RoundIgnoreStatus.RoundEndingCheck);
        Logger.Debug($"Should ignore {hub.nicknameSync?.DisplayName ?? "(NULL NAME)"} from round : {ignore}");
        return ignore;
    }
}