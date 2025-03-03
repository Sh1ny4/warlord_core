using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
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
