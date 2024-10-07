using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld.QuestGen;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(QuestNode_Raid))]
    [HarmonyPatch("RunInt")]
    public static class Patch_QuestNode_Raid_RunInt
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool foundPoints = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!foundPoints && instruction.opcode == OpCodes.Ldloc_2)
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
