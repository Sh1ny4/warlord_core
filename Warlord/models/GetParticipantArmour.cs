using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace Warlord.models
{
    class GetParticipantArmour : DefaultTournamentModel
    {
        public override Equipment GetParticipantArmor(CharacterObject participant)
        {
            if (CampaignMission.Current.Mode == MissionMode.Tournament)
            {
                string text = string.Concat(new object[] { "tournament_", Settlement.CurrentSettlement.Culture.StringId });
                return (Game.Current.ObjectManager.GetObject<CharacterObject>(text) ?? Game.Current.ObjectManager.GetObject<CharacterObject>("gear_practice_dummy_empire")).RandomBattleEquipment;
            }
            // The weapon loadout still is changed by the "tournament_template_<culture>_<amount>_participant_set_vX" NPC, but now the armour can be changed by having an NPC with the ID "tournament_<culture>"
            return participant.RandomBattleEquipment;
        }
    }
}
