using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(IncidentWorker_CaravanDemand))]
    [HarmonyPatch("TryExecuteWorker")]
    public static class Patch_IncidentWorker_CaravanDemand_TryExecuteWorker
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool foundFactor = false;
            bool gotResult = false;
            bool foundTextGeneration = false;
            bool foundCast = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!foundFactor && instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_FloatRange_get_RandomInRange)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetCaravanDemandFactorDesc);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                    foundFactor = true;
                    continue;
                }

                if (foundFactor && !gotResult && instruction.opcode == OpCodes.Mul)
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                    gotResult = true;
                    continue;
                }

                if (!foundTextGeneration && instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_IncidentWorker_CaravanDemand_GenerateMessageText)
                {
                    instruction.operand = VisibleRaidPointsRefs.m_CaravanDemandUtility_GenerateCaravanDemandMessageText;
                    foundTextGeneration = true;
                }

                if (foundTextGeneration && !foundCast && instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_TaggedString_op_Implicit_string)
                {
                    foundCast = true;
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
