﻿using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace Warlord
{
    public class Main : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("Warlord.patches").PatchAll();
        }

        /*
        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            if (starterObject is CampaignGameStarter)
            {
                CampaignGameStarter campaignGameStarter = starterObject as CampaignGameStarter;
                campaignGameStarter.AddBehavior(new patches.EliteInCastle.CastleRecruitMenu());
            }
        }*/

        public override void OnGameInitializationFinished(Game game)
        {
            if (!(game.GameType is Campaign)) return;
            Campaign.Current.CampaignBehaviorManager.RemoveBehavior<BackstoryCampaignBehavior>();
        }
    }
}
