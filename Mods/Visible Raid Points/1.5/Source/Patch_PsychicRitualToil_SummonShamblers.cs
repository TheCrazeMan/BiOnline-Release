using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Verse.AI.Group;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(PsychicRitualToil_SummonShamblers))]
    [HarmonyPatch(nameof(PsychicRitualToil_SummonShamblers.Start))]
    public static class Patch_PsychicRitualToil_SummonShamblers_Start
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_FloatRange_LerpThroughRange)
                {
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_Clear);
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 0f);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetInitialValue);
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Add);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetPsychicRitualQualityDesc);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
