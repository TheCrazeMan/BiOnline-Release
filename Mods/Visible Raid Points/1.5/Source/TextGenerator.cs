using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace VisibleRaidPoints
{
    public static class TextGenerator
    {
        public static TaggedString GetAmbushManhunterFactorDesc()
        {
            return "VisibleRaidPoints_AmbushManhunterFactorDesc".Translate();
        }

        public static TaggedString GetAnimalInsanityMassCalcDesc()
        {
            return "VisibleRaidPoints_AnimalInsanityMassCalcDesc".Translate();
        }

        public static TaggedString GetCaravanDemandFactorDesc()
        {
            return "VisibleRaidPoints_CaravanDemandFactorDesc".Translate();
        }

        public static TaggedString GetCrashedShipPartFactorDesc()
        {
            return "VisibleRaidPoints_CrashedShipPartFactorDesc".Translate();
        }

        public static TaggedString GetCrashedShipPartMinDesc()
        {
            return "VisibleRaidPoints_CrashedShipPartMinDesc".Translate();
        }

        public static TaggedString GetDeepDrillInfestationFactorDesc()
        {
            return "VisibleRaidPoints_DeepDrillInfestationFactorDesc".Translate();
        }

        public static TaggedString GetDeepDrillInfestationMinDesc()
        {
            return "VisibleRaidPoints_DeepDrillInfestationMinDesc".Translate();
        }

        public static TaggedString GetDeepDrillInfestationMaxDesc()
        {
            return "VisibleRaidPoints_DeepDrillInfestationMaxDesc".Translate();
        }

        public static TaggedString GetInfestationFactorDesc()
        {
            return "VisibleRaidPoints_InfestationFactorDesc".Translate();
        }

        public static TaggedString GetFactionMinDesc(Faction faction)
        {
            return "VisibleRaidPoints_FactionMinDesc".Translate(faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null)) + $" ({faction.def.defName})".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetRaidArrivalModeFactorDesc(PawnsArrivalModeDef def)
        {
            return "VisibleRaidPoints_RaidArrivalModeFactorDesc".Translate() + $" ({def.defName})".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetRaidStrategyFactorDesc(RaidStrategyDef def)
        {
            return "VisibleRaidPoints_RaidStrategyFactorDesc".Translate() + $" ({def.defName})".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetRaidAgeRestrictionFactorDesc(RaidAgeRestrictionDef def)
        {
            return "VisibleRaidPoints_RaidAgeRestrictionFactorDesc".Translate() + $" ({def.defName})".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetRaidStrategyMinDesc(RaidStrategyDef raidStrategy, Faction faction, PawnGroupKindDef groupKind)
        {
            return "VisibleRaidPoints_RaidStrategyMinDesc".Translate(raidStrategy.Worker.MinimumPoints(faction, groupKind) * 1.05f) + $" ({raidStrategy.defName}, {faction.def.defName}, {groupKind.defName})".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetMechClusterMaxDesc()
        {
            return "VisibleRaidPoints_MechClusterMaxDesc".Translate();
        }

        public static TaggedString GetSightstealerSwarmFactorDesc()
        {
            return "VisibleRaidPoints_SightstealerSwarmFactorDesc".Translate();
        }

        public static TaggedString GetGorehulkAssaultMinDesc(float min)
        {
            return "VisibleRaidPoints_GorehulkAssaultMinDesc".Translate((int)min);
        }

        public static TaggedString GetGorehulkAssaultMinFactorDesc()
        {
            return "VisibleRaidPoints_GorehulkAssaultMinFactorDesc".Translate();
        }

        public static TaggedString GetEntitySwarmRandomFactorDesc()
        {
            return "VisibleRaidPoints_EntitySwarmRandomFactorDesc".Translate();
        }

        public static TaggedString GetEntitySwarmMinDesc(PawnGroupKindDef groupKind)
        {
            return "VisibleRaidPoints_EntitySwarmMinDesc".Translate(Faction.OfEntities.def.MinPointsToGeneratePawnGroup(groupKind, null) * 1.05f) + $" ({groupKind.defName})".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetFleshbeastAttackFactorDesc()
        {
            return "VisibleRaidPoints_FleshbeastAttackFactorDesc".Translate();
        }

        public static TaggedString GetNoctolithsDamagedFactorDesc(int damagedCount)
        {
            return "VisibleRaidPoints_NoctolithsDamagedFactorDesc".Translate() + $" ({damagedCount})".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetDevourerAssaultMinDesc(float min)
        {
            return "VisibleRaidPoints_DevourerAssaultMinDesc".Translate((int)min);
        }

        public static TaggedString GetDevourerAssaultMinFactorDesc()
        {
            return "VisibleRaidPoints_DevourerAssaultMinFactorDesc".Translate();
        }

        public static TaggedString GetDevourerWaterAssaultFactorDesc()
        {
            return "VisibleRaidPoints_DevourerWaterAssaultFactorDesc".Translate();
        }

        public static TaggedString GetChimeraAssaultMinDesc(float min)
        {
            return "VisibleRaidPoints_ChimeraAssaultMinDesc".Translate((int)min);
        }

        public static TaggedString GetChimeraAssaultMinFactorDesc()
        {
            return "VisibleRaidPoints_ChimeraAssaultMinFactorDesc".Translate();
        }

        public static TaggedString GetHateChantersMinDesc(float min)
        {
            return "VisibleRaidPoints_HateChantersMinDesc".Translate((int)min);
        }

        public static TaggedString GetHateChantersMinFactorDesc()
        {
            return "VisibleRaidPoints_HateChantersMinFactorDesc".Translate();
        }

        public static TaggedString GetPsychicRitualQualityDesc(float percent)
        {
            return "VisibleRaidPoints_PsychicRitualQualityDesc".Translate() + $" ({(percent * 100).ToString("0.0")}%)".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetPsychicRitualQualityFactorDesc()
        {
            return "VisibleRaidPoints_PsychicRitualQualityFactorDesc".Translate();
        }

        public static TaggedString GetPsychicRitualSiegeThreatDesc(float points)
        {
            return "VisibleRaidPoints_PsychicRitualSiegeThreatDesc".Translate() + $" ({(int)points} points)".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetVoidAwakeningFactorDesc()
        {
            return "VisibleRaidPoints_VoidAwakeningFactorDesc".Translate();
        }

        public static TaggedString GetPollutionRaidFactorDesc()
        {
            return "VisibleRaidPoints_PollutionRaidFactorDesc".Translate();
        }

        public static TaggedString GetStorytellerRandomFactorDesc()
        {
            return "VisibleRaidPoints_StorytellerRandomFactorDesc".Translate();
        }

        public static TaggedString GetPointsFromWealthDesc()
        {
            return "VisibleRaidPoints_BreakdownPointsFromWealthDesc".Translate();
        }

        public static TaggedString GetPointsFromPawnsDesc()
        {
            return "VisibleRaidPoints_BreakdownPointsFromPawnsDesc".Translate();
        }

        public static TaggedString GetTargetRandomFactorDesc()
        {
            return "VisibleRaidPoints_BreakdownTargetRandomFactorDesc".Translate();
        }

        public static TaggedString GetAdaptationFactorDesc()
        {
            return "VisibleRaidPoints_BreakdownAdaptationFactorDesc".Translate();
        }

        public static TaggedString GetThreatScaleDesc()
        {
            return "VisibleRaidPoints_BreakdownThreatScaleDesc".Translate();
        }

        public static TaggedString GetGraceFactorDesc()
        {
            return "VisibleRaidPoints_BreakdownGraceFactorDesc".Translate() + $" ({"VisibleRaidPoints_BreakdownGraceFactorExpl".Translate()})".Colorize(ColoredText.SubtleGrayColor);
        }

        public static TaggedString GetClampLowDesc()
        {
            return "VisibleRaidPoints_BreakdownClampLowDesc".Translate();
        }

        public static TaggedString GetClampHighDesc(float high)
        {
            return "VisibleRaidPoints_BreakdownClampHighDesc".Translate(high);
        }

        public static TaggedString GetThreatPointsIndicatorText(ThreatPointsBreakdown breakdown)
        {
            return $"{"VisibleRaidPoints_RaidPointsUsed".Translate()}: {((int)breakdown.GetFinalResult()).ToString().Colorize(ColoredText.FactionColor_Hostile)}";
        }

        private static TaggedString GetIntermediateTotal(ThreatPointsBreakdown.OperationType? previousOperationType, float previousIntermediateTotal, List<float> intermediateValues, float previousRunningTotal)
        {
            TaggedString text = "";

            if (previousOperationType == ThreatPointsBreakdown.OperationType.Add || previousOperationType == ThreatPointsBreakdown.OperationType.Mul)
            {
                text += $"\n\n{"VisibleRaidPoints_IntermediateTotalDesc".Translate()}";
                text += $"\n{((int)previousIntermediateTotal).ToString().Colorize(ColoredText.FactionColor_Hostile)} ";
                foreach (float value in intermediateValues)
                {
                    if (previousOperationType == ThreatPointsBreakdown.OperationType.Add)
                    {
                        text += $"+ {((int)value).ToString().Colorize(ColoredText.FactionColor_Hostile)} ";
                    }
                    if (previousOperationType == ThreatPointsBreakdown.OperationType.Mul)
                    {
                        text += $"* {value.ToString("0.00").Colorize(ColoredText.ImpactColor)} ";
                    }
                }
                text += $"= {((int)previousRunningTotal).ToString().Colorize(ColoredText.FactionColor_Hostile)}\n";
            }
            else
            {
                text += "\n";
            }

            return text;
        }

        public static TaggedString GetThreatPointsBreakdownText(ThreatPointsBreakdown breakdown)
        {
            TaggedString text = $"=== {"VisibleRaidPoints_PointsBreakdown".Translate()} ===\n";

            if (breakdown.PlayerWealthForStoryteller > 0f)
            {
                text += $"\n{"VisibleRaidPoints_BreakdownPlayerWealthForStorytellerDesc".Translate()}: {$"${(int)breakdown.PlayerWealthForStoryteller}".Colorize(ColoredText.CurrencyColor)} {$"({"VisibleRaidPoints_BreakdownPlayerWealthForStorytellerExpl".Translate()})".Colorize(ColoredText.SubtleGrayColor)}\n";
            }

            if (breakdown.PointsPerPawn.Count > 0)
            {
                text += $"\n{"VisibleRaidPoints_BreakdownPointsPerPawnDesc".Translate()}: {$"({"VisibleRaidPoints_BreakdownPointsPerPawnExpl".Translate()})".Colorize(ColoredText.SubtleGrayColor)}";
                foreach (ThreatPointsBreakdown.PawnPoints p in breakdown.PointsPerPawn)
                {
                    if (p.Points > 0f)
                    {
                        text += $"\n  {p.Name}: {p.Points.ToString(".0").Colorize(ColoredText.FactionColor_Hostile)}";
                    }
                }
                text += "\n";
            }

            text += $"\n{"VisibleRaidPoints_BreakdownInitialValueDesc".Translate()}: {((int)breakdown.InitialValue).ToString().Colorize(ColoredText.FactionColor_Hostile)}";

            float previousRunningTotal = breakdown.InitialValue;
            ThreatPointsBreakdown.OperationType? previousOperationType = null;
            float previousIntermediateTotal = breakdown.InitialValue;
            List<float> intermediateValues = new List<float>();
            foreach (ThreatPointsBreakdown.PointsOperation operation in breakdown.Operations)
            {
                //Debug.Log(operation.Operation + " " + operation.Value + " " + operation.Description + " (" + operation.RunningTotal + ")");
                if (operation.RunningTotal == previousRunningTotal)
                {
                    continue;
                }
                if (operation.Operation == ThreatPointsBreakdown.OperationType.Min && operation.RunningTotal < previousRunningTotal)
                {
                    continue;
                }
                if (operation.Operation == ThreatPointsBreakdown.OperationType.Max && operation.RunningTotal > previousRunningTotal)
                {
                    continue;
                }

                if (operation.Operation != previousOperationType)
                {
                    text += GetIntermediateTotal(previousOperationType, previousIntermediateTotal, intermediateValues, previousRunningTotal);

                    previousIntermediateTotal = previousRunningTotal;
                    intermediateValues.Clear();
                }

                switch (operation.Operation)
                {
                    case ThreatPointsBreakdown.OperationType.Add:
                        text += $"\n  + {((int)operation.Value).ToString().Colorize(ColoredText.FactionColor_Hostile)} {operation.Description}";
                        break;
                    case ThreatPointsBreakdown.OperationType.Mul:
                        text += $"\n  * {operation.Value.ToString("0.00").Colorize(ColoredText.ImpactColor)} {operation.Description}";
                        break;
                    case ThreatPointsBreakdown.OperationType.Min:
                        text += $"\n{operation.Description}";
                        break;
                    case ThreatPointsBreakdown.OperationType.Max:
                        text += $"\n{operation.Description}";
                        break;
                }

                intermediateValues.Add(operation.Value);
                previousOperationType = operation.Operation;
                previousRunningTotal = operation.RunningTotal;
            }

            text += GetIntermediateTotal(previousOperationType, previousIntermediateTotal, intermediateValues, previousRunningTotal);

            text += "\n----------------------";
            text += $"\n{"VisibleRaidPoints_BreakdownTotal".Translate()}: {((int)breakdown.GetFinalResult()).ToString().Colorize(ColoredText.FactionColor_Hostile)}";

            return text;
        }
    }
}
