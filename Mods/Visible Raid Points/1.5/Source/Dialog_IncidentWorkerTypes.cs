using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VisibleRaidPoints
{
    public class Dialog_IncidentWorkerTypes : Window
    {
        private Vector2 scrollPos;

        public Dialog_IncidentWorkerTypes()
        {
            this.doCloseX = true;
            this.doCloseButton = true;
            this.closeOnClickedOutside = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (Widgets.ButtonText(new Rect(0f, 0f, inRect.width / 2, 24f), "VisibleRaidPoints_EnableAll".Translate()))
            {
                EnableAll(true);
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
            }
            if (Widgets.ButtonText(new Rect(inRect.width / 2, 0f, inRect.width / 2, 24f), "VisibleRaidPoints_DisableAll".Translate()))
            {
                EnableAll(false);
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
            }

            Rect outRect = new Rect(inRect);
            outRect.y += 24f;
            outRect.yMax -= Window.CloseButSize.y + 24f;
            outRect.yMin += 8f;
            Listing_Standard listingStandard = new Listing_Standard(inRect, () => scrollPos);
            Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, 24f * VisibleRaidPointsSettings.GetIncidentWorkerTypeCount());
            Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);
            listingStandard.Begin(viewRect);
            
            listingStandard.CheckboxLabeled("VisibleRaidPoints_AggressiveAnimals".Translate(), ref VisibleRaidPointsSettings.AggressiveAnimals);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_AmbushEnemyFaction".Translate(), ref VisibleRaidPointsSettings.AmbushEnemyFaction);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_AmbushManhunterPack".Translate(), ref VisibleRaidPointsSettings.AmbushManhunterPack);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_AnimalInsanityMass".Translate(), ref VisibleRaidPointsSettings.AnimalInsanityMass);
            if (ModsConfig.AnomalyActive)
            {
                listingStandard.CheckboxLabeled("VisibleRaidPoints_AnomalyEndgame".Translate(), ref VisibleRaidPointsSettings.AnomalyEndgame);
            }
            listingStandard.CheckboxLabeled("VisibleRaidPoints_CaravanDemand".Translate(), ref VisibleRaidPointsSettings.CaravanDemand);
            if (ModsConfig.AnomalyActive)
            {
                listingStandard.CheckboxLabeled("VisibleRaidPoints_ChimeraAssault".Translate(), ref VisibleRaidPointsSettings.ChimeraAssault);
            }
            listingStandard.CheckboxLabeled("VisibleRaidPoints_CrashedShipPart".Translate(), ref VisibleRaidPointsSettings.CrashedShipPart);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_CropBlight".Translate(), ref VisibleRaidPointsSettings.CropBlight);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_DeepDrillInfestation".Translate(), ref VisibleRaidPointsSettings.DeepDrillInfestation);
            if (ModsConfig.AnomalyActive)
            {
                listingStandard.CheckboxLabeled("VisibleRaidPoints_DevourerAssault".Translate(), ref VisibleRaidPointsSettings.DevourerAssault);
                listingStandard.CheckboxLabeled("VisibleRaidPoints_DevourerWaterAssault".Translate(), ref VisibleRaidPointsSettings.DevourerWaterAssault);
                listingStandard.CheckboxLabeled("VisibleRaidPoints_FleshbeastAttack".Translate(), ref VisibleRaidPointsSettings.FleshbeastAttack);
                listingStandard.CheckboxLabeled("VisibleRaidPoints_FleshmassHeart".Translate(), ref VisibleRaidPointsSettings.FleshmassHeart);
                listingStandard.CheckboxLabeled("VisibleRaidPoints_GorehulkAssault".Translate(), ref VisibleRaidPointsSettings.GorehulkAssault);
                listingStandard.CheckboxLabeled("VisibleRaidPoints_HateChanters".Translate(), ref VisibleRaidPointsSettings.HateChanters);
            }
            listingStandard.CheckboxLabeled("VisibleRaidPoints_Infestation".Translate(), ref VisibleRaidPointsSettings.Infestation);
            if (ModsConfig.RoyaltyActive)
            {
                listingStandard.CheckboxLabeled("VisibleRaidPoints_MechCluster".Translate(), ref VisibleRaidPointsSettings.MechCluster);
            }
            if (ModsConfig.AnomalyActive)
            {
                listingStandard.CheckboxLabeled("VisibleRaidPoints_NoctolAttack".Translate(), ref VisibleRaidPointsSettings.NoctolAttack);
                listingStandard.CheckboxLabeled("VisibleRaidPoints_PitGateEmergence".Translate(), ref VisibleRaidPointsSettings.PitGateEmergence);
            }
            listingStandard.CheckboxLabeled("VisibleRaidPoints_PsychicDrone".Translate(), ref VisibleRaidPointsSettings.PsychicDrone);
            if (ModsConfig.AnomalyActive)
            {
                listingStandard.CheckboxLabeled("VisibleRaidPoints_PsychicRitualSiege".Translate(), ref VisibleRaidPointsSettings.PsychicRitualSiege);
            }
            listingStandard.CheckboxLabeled("VisibleRaidPoints_RaidEnemy".Translate(), ref VisibleRaidPointsSettings.RaidEnemy);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_RaidFriendly".Translate(), ref VisibleRaidPointsSettings.RaidFriendly);
            if (ModsConfig.AnomalyActive)
            {
                listingStandard.CheckboxLabeled("VisibleRaidPoints_ShamblerAssault".Translate(), ref VisibleRaidPointsSettings.ShamblerAssault);
                listingStandard.CheckboxLabeled("VisibleRaidPoints_ShamblerSwarm".Translate(), ref VisibleRaidPointsSettings.ShamblerSwarm);
                listingStandard.CheckboxLabeled("VisibleRaidPoints_SightstealerSwarm".Translate(), ref VisibleRaidPointsSettings.SightstealerSwarm);
            }
            if (ModsConfig.BiotechActive)
            {
                listingStandard.CheckboxLabeled("VisibleRaidPoints_WastepackInfestation".Translate(), ref VisibleRaidPointsSettings.WastepackInfestation);
            }
            
            listingStandard.End();
            Widgets.EndScrollView();
        }

        private void EnableAll(bool enable)
        {
            VisibleRaidPointsSettings.AggressiveAnimals = enable;
            VisibleRaidPointsSettings.AmbushEnemyFaction = enable;
            VisibleRaidPointsSettings.AmbushManhunterPack = enable;
            VisibleRaidPointsSettings.AnimalInsanityMass = enable;
            VisibleRaidPointsSettings.CaravanDemand = enable;
            VisibleRaidPointsSettings.ChimeraAssault = enable;
            VisibleRaidPointsSettings.CrashedShipPart = enable;
            VisibleRaidPointsSettings.CropBlight = enable;
            VisibleRaidPointsSettings.DeepDrillInfestation = enable;
            VisibleRaidPointsSettings.DevourerAssault = enable;
            VisibleRaidPointsSettings.DevourerWaterAssault = enable;
            VisibleRaidPointsSettings.FleshbeastAttack = enable;
            VisibleRaidPointsSettings.FleshmassHeart = enable;
            VisibleRaidPointsSettings.GorehulkAssault = enable;
            VisibleRaidPointsSettings.HateChanters = enable;
            VisibleRaidPointsSettings.Infestation = enable;
            VisibleRaidPointsSettings.MechCluster = enable;
            VisibleRaidPointsSettings.NoctolAttack = enable;
            VisibleRaidPointsSettings.PitGateEmergence = enable;
            VisibleRaidPointsSettings.PsychicDrone = enable;
            VisibleRaidPointsSettings.PsychicRitualSiege = enable;
            VisibleRaidPointsSettings.RaidEnemy = enable;
            VisibleRaidPointsSettings.RaidFriendly = enable;
            VisibleRaidPointsSettings.ShamblerAssault = enable;
            VisibleRaidPointsSettings.ShamblerSwarm = enable;
            VisibleRaidPointsSettings.SightstealerSwarm = enable;
            VisibleRaidPointsSettings.WastepackInfestation = enable;
        }
    }
}
