using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;

namespace Warlord.models
{
    class RemoveInformationRestriction : InformationRestrictionModel
    {
        public override bool DoesPlayerKnowDetailsOf(Settlement settlement)
        {
            return true;
        }

        public override bool DoesPlayerKnowDetailsOf(Hero hero)
        {
            return true;
        }

        // Token: 0x04000787 RID: 1927
        public bool IsDisabledByCheat;
    }
}
