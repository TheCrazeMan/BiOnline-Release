using Verse;
using RimWorld;
using HarmonyLib;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(IncidentWorker))]
    [HarmonyPatch("SendStandardLetter")]
    [HarmonyPatch(new[] { typeof(TaggedString), typeof(TaggedString), typeof(LetterDef), typeof(IncidentParms), typeof(LookTargets), typeof(NamedArgument[]) })]
    public static class Patch_IncidentWorker_SendStandardLetter
    {
        public static void Prefix(IncidentWorker __instance, ref TaggedString baseLetterLabel, ref TaggedString baseLetterText, IncidentParms parms)
        {
            if (parms.points > 0f)
            {
                if (VisibleRaidPointsSettings.IncidentWorkerTypeEnabled(__instance.GetType()))
                {
                    ThreatPointsBreakdown breakdown = ThreatPointsBreakdown.GetAssociated(parms);
                    if (breakdown == null)
                    {
                        breakdown = ThreatPointsBreakdown.GetCurrent();
                    }

                    if (VisibleRaidPointsSettings.ShowInLabel)
                    {
                        if (parms.customLetterLabel != null)
                        {
                            parms.customLetterLabel = $"({(int)breakdown.GetFinalResult()}) {parms.customLetterLabel}";
                        }
                        else
                        {
                            baseLetterLabel = $"({(int)breakdown.GetFinalResult()}) {baseLetterLabel}";
                        }
                    }

                    if (VisibleRaidPointsSettings.ShowInText)
                    {
                        if (parms.customLetterText != null)
                        {
                            parms.customLetterText += $"\n\n{TextGenerator.GetThreatPointsIndicatorText(breakdown)}";
                        }
                        else
                        {
                            baseLetterText += $"\n\n{TextGenerator.GetThreatPointsIndicatorText(breakdown)}";
                        }
                    }

                    if (VisibleRaidPointsSettings.ShowBreakdown)
                    {
                        TaggedString breakdownText = TextGenerator.GetThreatPointsBreakdownText(breakdown);

                        if (breakdownText != null)
                        {
                            if (parms.customLetterText != null)
                            {
                                parms.customLetterText += $"\n\n{breakdownText}";
                            }
                            else
                            {
                                baseLetterText += $"\n\n{breakdownText}";
                            }
                        }
                    }
                }
            }
        }
    }
}
