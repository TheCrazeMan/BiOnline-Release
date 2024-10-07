using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld.QuestGen;
using System.Reflection;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(QuestPart_EntityArrival))]
    [HarmonyPatch(nameof(QuestPart_EntityArrival.Notify_QuestSignalReceived))]
    public static class Patch_QuestPart_EntityArrival_Notify_QuestSignalReceived
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool foundFactor = false;
            bool foundMul = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!foundFactor && instruction.opcode == OpCodes.Ldloc_1)
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetVoidAwakeningFactorDesc);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                    foundFactor = true;
                    continue;
                }
                if (foundFactor && !foundMul && instruction.opcode == OpCodes.Mul)
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                    foundMul = true;
                    continue;
                }
                if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_LetterStack_ReceiveLetter)
                {
                    yield return new CodeInstruction(OpCodes.Ldsfld, VisibleRaidPointsRefs.f_VisibleRaidPointsSettings_AnomalyEndgame);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_LetterUtility_ReceiveLetter);
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
