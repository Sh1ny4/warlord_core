using Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace Warlord.models
{
    class RemoveMilitiaModel : DefaultSettlementMilitiaModel
    {
        public override int MilitiaToSpawnAfterSiege(Town town)
        {
            return 0;
        }

        public override ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false)
        {
            return CalculateMilitiaChangeInternal(settlement, includeDescriptions);
        }

        public override float CalculateEliteMilitiaSpawnChance(Settlement settlement)
        {
            float num = 0f;
            Hero hero = null;
            if (settlement.IsFortification && settlement.Town.Governor != null)
            {
                hero = settlement.Town.Governor;
            }
            else if (settlement.IsVillage)
            {
                Settlement tradeBound = settlement.Village.TradeBound;
                if (((tradeBound != null) ? tradeBound.Town.Governor : null) != null)
                {
                    hero = settlement.Village.TradeBound.Town.Governor;
                }
            }
            if (hero != null && hero.GetPerkValue(DefaultPerks.Leadership.CitizenMilitia))
            {
                num += DefaultPerks.Leadership.CitizenMilitia.PrimaryBonus;
            }
            return num;
        }

        public override void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate)
        {
            meleeTroopRate = 0f;
            rangedTroopRate = 0f;
        }

        private static ExplainedNumber CalculateMilitiaChangeInternal(Settlement settlement, bool includeDescriptions = false)
        {
            ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
            result.Add( - 100f, RetiredText, null);
            return result;
        }
        private static readonly TextObject RetiredText = new TextObject("{=gHnfFi1s}Retired", null);
    }
}
