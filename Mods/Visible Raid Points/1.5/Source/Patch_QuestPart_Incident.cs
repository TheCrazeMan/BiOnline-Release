using RimWorld;
using HarmonyLib;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(QuestPart_Incident))]
    [HarmonyPatch(nameof(QuestPart_Incident.SetIncidentParmsAndRemoveTarget))]
    public static class Patch_QuestPart_Incident_SetIncidentParmsAndRemoveTarget
    {
        public static void Postfix(IncidentParms value)
        {
            if (value.points > 0f)
            {
                ThreatPointsBreakdown.Associate(value, ThreatPointsBreakdown.GetCurrent());
            }
        }
    }
}
