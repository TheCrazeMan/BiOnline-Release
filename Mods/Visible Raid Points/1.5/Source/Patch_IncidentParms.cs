using Verse;
using RimWorld;
using HarmonyLib;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(IncidentParms))]
    [HarmonyPatch(nameof(IncidentParms.ExposeData))]
    public static class Patch_IncidentParms_ExposeData
    {
        public static void Postfix(IncidentParms __instance)
        {
            ThreatPointsBreakdown breakdown = ThreatPointsBreakdown.GetAssociated(__instance);
            Scribe_Deep.Look(ref breakdown, "ThreatPointsBreakdown");
            ThreatPointsBreakdown.Associate(__instance, breakdown);
        }
    }
}
