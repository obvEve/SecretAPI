namespace SecretAPI.Patches.Features;

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using LabApi.Features.Console;

/// <summary>
/// Changes <see cref="PatchClassProcessor"/>'s ReportException to log instead of throwing the exception.
/// </summary>
[HarmonyPatch(typeof(PatchClassProcessor), "ReportException")]
internal static class HarmonyLogger
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher matcher = new(instructions);

        while (matcher.MatchEndForward(new CodeMatch(OpCodes.Throw)).IsValid)
        {
            CodeInstruction callInstruction = CodeInstruction.Call(typeof(HarmonyLogger), nameof(Log));
            callInstruction.labels.AddRange(matcher.Labels);
            callInstruction.blocks.AddRange(matcher.Blocks);

            matcher.SetInstruction(callInstruction);
            matcher.InsertAfter(new CodeInstruction(OpCodes.Ret));
            matcher.Advance();
        }

        return matcher.InstructionEnumeration();
    }

    private static void Log(Exception exception)
    {
        Logger.Error($"Prevented Harmony Exception: {exception.Message}");
    }
}