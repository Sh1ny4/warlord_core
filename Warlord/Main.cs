using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Issues;
using System.Security.AccessControl;

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
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            if (game.GameType is Campaign)
            {
                CampaignGameStarter starter = gameStarterObject as CampaignGameStarter;
                starter.AddModel(new models.GetParticipantArmour());
                starter.AddModel(new models.PartySizeModel());
            }
        }
        public override void OnGameInitializationFinished(Game game)
        {
            if (!(game.GameType is Campaign)) return;
            Campaign.Current.CampaignBehaviorManager.RemoveBehavior<BackstoryCampaignBehavior>();
        }
    }
}
