using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(IncidentWorker_FleshbeastAttack))]
    [HarmonyPatch("TryExecuteWorker")]
    public static class Patch_IncidentWorker_FleshbeastAttack_TryExecuteWorker
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool foundMul = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!foundMul && instruction.opcode == OpCodes.Mul)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 0.6f);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetFleshbeastAttackFactorDesc);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                    foundMul = true;
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}
