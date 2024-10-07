using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld.QuestGen;
using System.Reflection;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(QuestGen_Threat))]
    [HarmonyPatch(nameof(QuestGen_Threat.Raid))]
    public static class Patch_QuestGen_Threat_Raid
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool foundPoints = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!foundPoints && instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_Mathf_Max)
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_Clear);
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetInitialValue);
                    foundPoints = true;
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
