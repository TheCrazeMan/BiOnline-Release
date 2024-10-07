using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(MechClusterGenerator))]
    [HarmonyPatch(nameof(MechClusterGenerator.GenerateClusterSketch))]
    public static class Patch_MechClusterGenerator_GenerateClusterSketch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool foundMin = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!foundMin && instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_Mathf_Min)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Max);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetMechClusterMaxDesc);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                    foundMin = true;
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
