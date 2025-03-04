using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace Warlord.models
{
    class ManhunterPartyComponant : WarPartyComponent
    {
        public override Hero PartyOwner { get; }
        public override TextObject Name { get; }
        public override Settlement HomeSettlement
        {
            get
            {
                return this._relatedSettlement;
            }
        }
        [SaveableField(3)]
        public readonly Settlement _relatedSettlement;
    }
}
