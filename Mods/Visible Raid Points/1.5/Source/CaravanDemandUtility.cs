using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace VisibleRaidPoints
{
    public static class CaravanDemandUtility
    {
        public static TaggedString GenerateCaravanDemandMessageText(IncidentWorker_CaravanDemand instance, Faction enemyFaction, int attackerCount, List<ThingCount> demands, Caravan caravan)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            TaggedString text = "CaravanDemand".Translate(caravan.Name, enemyFaction.NameColored, attackerCount, GenLabel.ThingsLabel(demands, "  - ", false), enemyFaction.def.pawnsPlural).Resolve().CapitalizeFirst();

            if (VisibleRaidPointsSettings.CaravanDemand)
            {
                if (VisibleRaidPointsSettings.ShowInText)
                {
                    text += $"\n\n{TextGenerator.GetThreatPointsIndicatorText(ThreatPointsBreakdown.GetCurrent())}";
                }

                if (VisibleRaidPointsSettings.ShowBreakdown)
                {
                    TaggedString breakdown = TextGenerator.GetThreatPointsBreakdownText(ThreatPointsBreakdown.GetCurrent());

                    if (breakdown != null)
                    {
                        text += $"\n\n{breakdown}";
                    }
                }
            }

            return text;
        }
    }
}
