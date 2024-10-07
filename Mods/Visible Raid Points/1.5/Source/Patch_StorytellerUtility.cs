using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Verse;
using System.Linq;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(StorytellerUtility))]
    [HarmonyPatch("DefaultThreatPointsNow")]
    public static class Patch_StorytellerUtility_DefaultThreatPointsNow
    {
        private enum Step
        {
            PlayerWealthForStoryteller,
            PointsFromWealth,
            PlayerPawnsForStoryteller,
            PointsPerPawnName,
            PointsPerPawnPoints,
            RandomFactor,
            AdaptationFactor,
            ThreatScale,
            GraceFactor,
            Clamp,
            Done
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            LocalBuilder pawn = il.DeclareLocal(typeof(Pawn));

            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_Clear);
            yield return new CodeInstruction(OpCodes.Ldc_R4, 0f);
            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetInitialValue);

            Step step = Step.PlayerWealthForStoryteller;

            List<CodeInstruction> instructionsList = instructions.ToList();
            for (int i = 0; i < instructionsList.Count; i++)
            {
                CodeInstruction instruction = instructionsList[i];

                switch (step)
                {

                    case Step.PlayerWealthForStoryteller:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_IIncidentTarget_get_PlayerWealthForStoryteller)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetPlayerWealthForStoryteller);
                            step = Step.PointsFromWealth;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PointsFromWealth:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_SimpleCurve_Evaluate)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Add);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetPointsFromWealthDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                            step = Step.PlayerPawnsForStoryteller;
                        } 
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PlayerPawnsForStoryteller:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_IIncidentTarget_get_PlayerPawnsForStoryteller)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Add);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            step = Step.PointsPerPawnName;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PointsPerPawnName:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_IEnumeratorPawn_get_Current)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stloc_S, pawn);
                        }
                        else if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand.ToString() == "0")
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, pawn);
                            yield return new CodeInstruction(OpCodes.Callvirt, VisibleRaidPointsRefs.m_Pawn_get_LabelShort);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetPawnPointsName);
                            yield return instruction;
                            step = Step.PointsPerPawnPoints;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PointsPerPawnPoints:
                        CodeInstruction nextInstruction = instructionsList[i + 1];
                        if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand.ToString() == "0")
                        {
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetPawnPointsPoints);
                            yield return instruction;
                        }
                        else if (instruction.opcode == OpCodes.Add && nextInstruction.opcode == OpCodes.Stloc_3)
                        {
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetPawnPointsPoints);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                            step = Step.RandomFactor;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.RandomFactor:
                        if (instruction.opcode == OpCodes.Add)
                        {
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetPointsFromPawnsDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                        }
                        else if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_FloatRange_get_RandomInRange)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetTargetRandomFactorDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                        }
                        else if (instruction.opcode == OpCodes.Mul)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                            step = Step.AdaptationFactor;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.AdaptationFactor:
                        if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_Mathf_Lerp_float_float_float)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetAdaptationFactorDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                        }
                        else if (instruction.opcode == OpCodes.Mul)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                            step = Step.ThreatScale;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.ThreatScale:
                        if (instruction.opcode == OpCodes.Ldfld && (FieldInfo)instruction.operand == VisibleRaidPointsRefs.f_Difficulty_threatScale)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetThreatScaleDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                        }
                        else if (instruction.opcode == OpCodes.Mul)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                            step = Step.GraceFactor;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.GraceFactor:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_SimpleCurve_Evaluate)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Mul);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationValue);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetGraceFactorDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                        }
                        else if (instruction.opcode == OpCodes.Mul)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                            step = Step.Clamp;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.Clamp:
                        if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_Mathf_Clamp)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Max);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetClampHighDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Min);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetClampLowDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                            step = Step.Done;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.Done:
                        // Support for Unlimited Threat Scale
                        if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_Mathf_Max)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_I4, (int)ThreatPointsBreakdown.OperationType.Min);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationType);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_TextGenerator_GetClampLowDesc);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationDescription);
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetOperationRunningTotal);
                            continue;
                        }

                        yield return instruction;
                        break;
                }
            }
        }
    }
}
