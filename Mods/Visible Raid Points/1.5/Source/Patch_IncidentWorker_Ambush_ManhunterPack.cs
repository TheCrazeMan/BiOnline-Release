using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(IncidentWorker_Ambush_ManhunterPack))]
    [HarmonyPatch("AdjustedPoints")]
    public static class Patch_IncidentWorker_Ambush_ManhunterPack_AdjustedPoints
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Mul)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 0.75f);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetAmbushManhunterFactorDesc);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}
