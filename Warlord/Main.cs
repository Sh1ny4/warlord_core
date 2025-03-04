using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;
using SandBox.View;
using SandBox;

namespace Warlord
{
    public class Main : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("Warlord.patches").PatchAll();
            TextObject coreContentDisabledReason = new TextObject("{=!}StoryMode Disable", null);
            TextObject _sandBoxAchievementsHint = new TextObject("{=j09m7S2E}Achievements are disabled in SandBox mode!", null);
            Module.CurrentModule.ClearStateOptions();
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("SandBoxNewGame", new TextObject("{=!}Start a New Game", null), 3, delegate () { MBGameManager.StartNewGame(new SandBoxGameManager()); }, () => this.IsSandboxDisabled(), _sandBoxAchievementsHint));
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("CampaignResumeGame", new TextObject("{=!}Load Game", null), 0, delegate () { ScreenManager.PushScreen(SandBoxViewCreator.CreateSaveLoadScreen(false));}, () => this.IsSavedGamesDisabled(), null));
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Options", new TextObject("{=!}Options", null), 9998, delegate () { ScreenManager.PushScreen(ViewCreator.CreateOptionsScreen(true)); }, () => new ValueTuple<bool, TextObject>(false, TextObject.Empty), null));
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Credits", new TextObject("{=!}Credits", null), 9999, delegate () { ScreenManager.PushScreen(ViewCreator.CreateCreditsScreen()); }, () => new ValueTuple<bool, TextObject>(false, TextObject.Empty), null));
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Exit", new TextObject("{=!}Exit Game", null), 10000, delegate () { MBInitialScreenBase.DoExitButtonAction(); }, () => new ValueTuple<bool, TextObject>(Module.CurrentModule.IsOnlyCoreContentEnabled, coreContentDisabledReason), null));
        }
        private ValueTuple<bool, TextObject> IsSandboxDisabled()
        {
            if (Module.CurrentModule.IsOnlyCoreContentEnabled)
            {
                return new ValueTuple<bool, TextObject>(true, new TextObject("{=V8BXjyYq}Disabled during installation.", null));
            }
            return new ValueTuple<bool, TextObject>(false, TextObject.Empty);
        }
        private ValueTuple<bool, TextObject> IsSavedGamesDisabled()
        {
            if (Module.CurrentModule.IsOnlyCoreContentEnabled)
            {
                return new ValueTuple<bool, TextObject>(true, new TextObject("{=V8BXjyYq}Disabled during installation.", null));
            }
            if (MBSaveLoad.NumberOfCurrentSaves == 0)
            {
                return new ValueTuple<bool, TextObject>(true, new TextObject("{=XcVVE1mp}No saved games found.", null));
            }
            return new ValueTuple<bool, TextObject>(false, TextObject.Empty);
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
                starter.AddModel(new models.RemoveInformationRestriction());
                starter.AddModel(new models.RemoveMilitiaModel());
            }
        }
        public override void OnGameInitializationFinished(Game game)
        {
            if (!(game.GameType is Campaign)) return;
            Campaign.Current.CampaignBehaviorManager.RemoveBehavior<BackstoryCampaignBehavior>();
        }
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            AccessTools.Field(typeof(Module), "_splashScreenPlayed").SetValue(Module.CurrentModule, true);
        }
    }
}
