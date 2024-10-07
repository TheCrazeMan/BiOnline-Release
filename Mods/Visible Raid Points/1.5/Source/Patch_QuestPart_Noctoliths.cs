using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using RimWorld.QuestGen;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(QuestPart_Noctoliths))]
    [HarmonyPatch("SpawnNoctols")]
    public static class Patch_QuestPart_Noctoliths_SpawnNoctols
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool foundPoints = false;
            bool foundFactor = false;
            bool foundMul = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!foundPoints && instruction.opcode == OpCodes.Ldfld && (FieldInfo)instruction.operand == VisibleRaidPointsRefs.f_QuestPart_Noctoliths_points)
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_Clear);
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetInitialValue);
                    foundPoints = true;
                    continue;
                }
                if (foundPoints && !foundFactor && instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_SimpleCurve_Evaluate)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, VisibleRaidPointsRefs.f_QuestPart_Noctoliths_damagedCount);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetNoctolithsDamagedFactorDesc);
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
                    yield return new CodeInstruction(OpCodes.Ldsfld, VisibleRaidPointsRefs.f_VisibleRaidPointsSettings_NoctolAttack);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_LetterUtility_ReceiveLetter);
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
