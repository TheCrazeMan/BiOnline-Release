using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace VisibleRaidPoints
{
    public class VisibleRaidPointsSettings : ModSettings
    {
        private static readonly int IncidentWorkerTypeCountVanilla = 12;
        private static readonly int IncidentWorkerTypeCountRoyalty = 1;
        private static readonly int IncidentWorkerTypeCountIdeology = 0;
        private static readonly int IncidentWorkerTypeCountBiotech = 1;
        private static readonly int IncidentWorkerTypeCountAnomaly = 14;

        public static int GetIncidentWorkerTypeCount()
        {
            int count = IncidentWorkerTypeCountVanilla;
            if (ModsConfig.RoyaltyActive)
            {
                count += IncidentWorkerTypeCountRoyalty;
            }
            if (ModsConfig.IdeologyActive)
            {
                count += IncidentWorkerTypeCountIdeology;
            }
            if (ModsConfig.BiotechActive)
            {
                count += IncidentWorkerTypeCountBiotech;
            }
            if (ModsConfig.AnomalyActive)
            {
                count += IncidentWorkerTypeCountAnomaly;
            }
            return count;
        }

        public static bool ShowInLabel = false;
        public static bool ShowInText = true;
        public static bool ShowBreakdown = false;
        public static bool AggressiveAnimals = true;
        public static bool AmbushEnemyFaction = true;
        public static bool AmbushManhunterPack = true;
        public static bool AnimalInsanityMass = false;
        public static bool AnomalyEndgame = true;
        public static bool CaravanDemand = false;
        public static bool ChimeraAssault = true;
        public static bool CrashedShipPart = true;
        public static bool CropBlight = false;
        public static bool DeepDrillInfestation = true;
        public static bool DevourerAssault = true;
        public static bool DevourerWaterAssault = true;
        public static bool FleshbeastAttack = true;
        public static bool FleshmassHeart = true;
        public static bool GorehulkAssault = true;
        public static bool HateChanters = true;
        public static bool Infestation = true;
        public static bool MechCluster = true;
        public static bool NoctolAttack = true;
        public static bool PitGateEmergence = true;
        public static bool PsychicDrone = false;
        public static bool PsychicRitualSiege = true;
        public static bool RaidEnemy = true;
        public static bool RaidFriendly = false;
        public static bool ShamblerAssault = true;
        public static bool ShamblerSwarm = true;
        public static bool SightstealerSwarm = true;
        public static bool WastepackInfestation = true;

        public static bool IncidentWorkerTypeEnabled(Type type)
        {
            if (AggressiveAnimals && type == typeof(IncidentWorker_AggressiveAnimals)) return true;
            if (AmbushEnemyFaction && type == typeof(IncidentWorker_Ambush_EnemyFaction)) return true;
            if (AmbushManhunterPack && type == typeof(IncidentWorker_Ambush_ManhunterPack)) return true;
            if (AnimalInsanityMass && type == typeof(IncidentWorker_AnimalInsanityMass)) return true;
            if (CaravanDemand && type == typeof(IncidentWorker_CaravanDemand)) return true;
            if (ChimeraAssault && type == typeof(IncidentWorker_ChimeraAssault)) return true;
            if (CrashedShipPart && type == typeof(IncidentWorker_CrashedShipPart)) return true;
            if (CropBlight && type == typeof(IncidentWorker_CropBlight)) return true;
            if (DeepDrillInfestation && type == typeof(IncidentWorker_DeepDrillInfestation)) return true;
            if (DevourerAssault && type == typeof(IncidentWorker_DevourerAssault)) return true;
            if (DevourerWaterAssault && type == typeof(IncidentWorker_DevourerWaterAssault)) return true;
            if (FleshbeastAttack && type == typeof(IncidentWorker_FleshbeastAttack)) return true;
            if (FleshmassHeart && type == typeof(IncidentWorker_FleshmassHeart)) return true;
            if (GorehulkAssault && type == typeof(IncidentWorker_GorehulkAssault)) return true;
            if (HateChanters && type == typeof(IncidentWorker_HateChanters)) return true;
            if (Infestation && type == typeof(IncidentWorker_Infestation)) return true;
            if (MechCluster && type == typeof(IncidentWorker_MechCluster)) return true;
            if (PsychicDrone && type == typeof(IncidentWorker_PsychicDrone)) return true;
            if (PsychicRitualSiege && type == typeof(IncidentWorker_PsychicRitualSiege)) return true;
            if (RaidEnemy && type == typeof(IncidentWorker_RaidEnemy)) return true;
            if (RaidFriendly && type == typeof(IncidentWorker_RaidFriendly)) return true;
            if (ShamblerAssault && type == typeof(IncidentWorker_ShamblerAssault)) return true;
            if (ShamblerSwarm && (type == typeof(IncidentWorker_ShamblerSwarm) || type == typeof(IncidentWorker_ShamblerSwarmAnimals))) return true;
            if (SightstealerSwarm && type == typeof(IncidentWorker_SightstealerSwarm)) return true;
            if (WastepackInfestation && type == typeof(IncidentWorker_WastepackInfestation)) return true;
            return false;
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("VisibleRaidPoints_ShowPointsInLetterLabel".Translate(), ref ShowInLabel);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_ShowPointsInLetterText".Translate(), ref ShowInText);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_ShowBreakdownInLetterText".Translate(), ref ShowBreakdown);
            if (listingStandard.ButtonTextLabeled("VisibleRaidPoints_IncidentWorkerTypes".Translate(), "VisibleRaidPoints_Modify".Translate()))
            {
                Find.WindowStack.Add(new Dialog_IncidentWorkerTypes());
            }

            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref ShowInLabel, "ShowInLabel", false);
            Scribe_Values.Look(ref ShowInText, "ShowInText", true);
            Scribe_Values.Look(ref ShowBreakdown, "ShowBreakdown", false);
            Scribe_Values.Look(ref AggressiveAnimals, "AggressiveAnimals", true);
            Scribe_Values.Look(ref AmbushEnemyFaction, "AmbushEnemyFaction", true);
            Scribe_Values.Look(ref AmbushManhunterPack, "AmbushManhunterPack", true);
            Scribe_Values.Look(ref AnimalInsanityMass, "AnimalInsanityMass", false);
            Scribe_Values.Look(ref AnomalyEndgame, "AnomalyEndgame", true);
            Scribe_Values.Look(ref CaravanDemand, "CaravanDemand", false);
            Scribe_Values.Look(ref ChimeraAssault, "ChimeraAssault", true);
            Scribe_Values.Look(ref CrashedShipPart, "CrashedShipPart", true);
            Scribe_Values.Look(ref CropBlight, "CropBlight", false);
            Scribe_Values.Look(ref DeepDrillInfestation, "DeepDrillInfestation", true);
            Scribe_Values.Look(ref DevourerAssault, "DevourerAssault", true);
            Scribe_Values.Look(ref DevourerWaterAssault, "DevourerWaterAssault", true);
            Scribe_Values.Look(ref FleshbeastAttack, "FleshbeastAttack", true);
            Scribe_Values.Look(ref FleshmassHeart, "FleshmassHeart", true);
            Scribe_Values.Look(ref GorehulkAssault, "GorehulkAssault", true);
            Scribe_Values.Look(ref HateChanters, "HateChanters", true);
            Scribe_Values.Look(ref Infestation, "Infestation", true);
            Scribe_Values.Look(ref MechCluster, "MechCluster", true);
            Scribe_Values.Look(ref NoctolAttack, "NoctolAttack", true);
            Scribe_Values.Look(ref PitGateEmergence, "PitGateEmergence", true);
            Scribe_Values.Look(ref PsychicDrone, "PsychicDrone", false);
            Scribe_Values.Look(ref PsychicRitualSiege, "PsychicRitualSiege", true);
            Scribe_Values.Look(ref RaidEnemy, "RaidEnemy", true);
            Scribe_Values.Look(ref RaidFriendly, "RaidFriendly", false);
            Scribe_Values.Look(ref ShamblerAssault, "ShamblerAssault", true);
            Scribe_Values.Look(ref ShamblerSwarm, "ShamblerSwarm", true);
            Scribe_Values.Look(ref SightstealerSwarm, "SightstealerSwarm", true);
            Scribe_Values.Look(ref WastepackInfestation, "WastepackInfestation", true);
        }
    }
}
