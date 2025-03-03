using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Warlord.models
{
    class PartySizeModel : DefaultPartySizeLimitModel
    {
        public void DefaultPartySizeLimitModel()
        {
            this._quarterMasterText = GameTexts.FindText("str_clan_role", "quartermaster");
        }

        public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
        {
            ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
            if (!party.IsMobile)
            {
                return result;
            }
            if (party.MobileParty.IsGarrison)
            {
                return this.CalculateGarrisonPartySizeLimit(party.MobileParty, includeDescriptions);
            }
            return this.CalculateMobilePartyMemberSizeLimit(party.MobileParty, includeDescriptions);
        }

        public override ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false)
        {
            if (party.IsSettlement)
            {
                return this.CalculateSettlementPartyPrisonerSizeLimitInternal(party.Settlement, includeDescriptions);
            }
            return this.CalculateMobilePartyPrisonerSizeLimitInternal(party, includeDescriptions);
        }

        private ExplainedNumber CalculateMobilePartyMemberSizeLimit(MobileParty party, bool includeDescriptions = false)
        {
            ExplainedNumber result = new ExplainedNumber(40f, includeDescriptions, this._baseSizeText);
            if (party.LeaderHero != null && party.LeaderHero.Clan != null && !party.IsCaravan)
            {
                this.CalculateBaseMemberSize(party.LeaderHero, party.MapFaction, party.ActualClan, ref result);
                result.Add((float)(party.EffectiveQuartermaster.GetSkillValue(DefaultSkills.Leadership) / 4), this._leadershipSkillLevelBonusText, null);
                if (PartySizeModel._addAdditionalPartySizeAsCheat && party.IsMainParty && Game.Current.CheatMode)
                {
                    result.Add(5000f, new TextObject("{=!}Additional size from extra party cheat", null), null);
                }
            }
            else if (party.IsCaravan)
            {
                if (party.Party.Owner == Hero.MainHero)
                {
                    result.Add(10f, this._randomSizeBonusTemporary, null);
                }
                else
                {
                    Hero owner = party.Party.Owner;
                    if (owner != null && owner.IsNotable)
                    {
                        result.Add((float)(10 * ((party.Party.Owner.Power < 100f) ? 1 : ((party.Party.Owner.Power < 200f) ? 2 : 3))), this._randomSizeBonusTemporary, null);
                    }
                }
            }
            else if (party.IsVillager)
            {
                result.Add(40f, this._randomSizeBonusTemporary, null);
            }
            return result;
        }

        private ExplainedNumber CalculateGarrisonPartySizeLimit(MobileParty party, bool includeDescriptions = false)
        {
            ExplainedNumber result = new ExplainedNumber(200f, includeDescriptions, this._baseSizeText);
            this.GetLeadershipSkillLevelEffect(party, PartySizeModel.LimitType.GarrisonPartySizeLimit, ref result);
            result.Add((float)this.GetTownBonus(party), this._townBonusText, null);
            this.AddGarrisonOwnerPerkEffects(party.CurrentSettlement, ref result);
            this.AddSettlementProjectBonuses(party.Party, ref result);
            return result;
        }

        private ExplainedNumber CalculateSettlementPartyPrisonerSizeLimitInternal(Settlement settlement, bool includeDescriptions = false)
        {
            ExplainedNumber result = new ExplainedNumber(60f, includeDescriptions, this._baseSizeText);
            Town town = settlement.Town;
            int num = (town != null) ? town.GetWallLevel() : 0;
            if (num > 0)
            {
                result.Add((float)(num * 40), this._wallLevelBonusText, null);
            }
            return result;
        }

        private ExplainedNumber CalculateMobilePartyPrisonerSizeLimitInternal(PartyBase party, bool includeDescriptions = false)
        {
            ExplainedNumber result = new ExplainedNumber(10f, includeDescriptions, this._baseSizeText);
            result.Add((float)this.GetCurrentPartySizeEffect(party), this._currentPartySizeBonusText, null);
            this.AddMobilePartyLeaderPrisonerSizePerkEffects(party, ref result);
            if (PartySizeModel._addAdditionalPrisonerSizeAsCheat && party.IsMobile && party.MobileParty.IsMainParty && Game.Current.CheatMode)
            {
                result.Add(5000f, new TextObject("{=!}Additional size from extra prisoner cheat", null), null);
            }
            return result;
        }

        private void GetLeadershipSkillLevelEffect(MobileParty party, PartySizeModel.LimitType type, ref ExplainedNumber partySizeBonus)
        {
            Hero hero;
            if (!party.IsGarrison)
            {
                hero = party.LeaderHero;
            }
            else if (party == null)
            {
                hero = null;
            }
            else
            {
                Settlement currentSettlement = party.CurrentSettlement;
                if (currentSettlement == null)
                {
                    hero = null;
                }
                else
                {
                    Clan ownerClan = currentSettlement.OwnerClan;
                    hero = ((ownerClan != null) ? ownerClan.Leader : null);
                }
            }
            Hero hero2 = hero;
            if (hero2 != null && type == PartySizeModel.LimitType.GarrisonPartySizeLimit)
            {
                SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Leadership, DefaultSkillEffects.LeadershipGarrisonSizeBonus, hero2.CharacterObject, ref partySizeBonus, -1, true, 0);
            }
        }

        private void AddMobilePartyLeaderPrisonerSizePerkEffects(PartyBase party, ref ExplainedNumber result)
        {
            if (party.LeaderHero != null)
            {
                if (party.LeaderHero.GetPerkValue(DefaultPerks.TwoHanded.Terror))
                {
                    result.Add(DefaultPerks.TwoHanded.Terror.SecondaryBonus, DefaultPerks.TwoHanded.Terror.Name, null);
                }
                if (party.LeaderHero.GetPerkValue(DefaultPerks.Athletics.Stamina))
                {
                    result.Add(DefaultPerks.Athletics.Stamina.SecondaryBonus, DefaultPerks.Athletics.Stamina.Name, null);
                }
                if (party.LeaderHero.GetPerkValue(DefaultPerks.Roguery.Manhunter))
                {
                    result.Add(DefaultPerks.Roguery.Manhunter.SecondaryBonus, DefaultPerks.Roguery.Manhunter.Name, null);
                }
                if (party.LeaderHero != null && party.LeaderHero.GetPerkValue(DefaultPerks.Scouting.VantagePoint))
                {
                    result.Add(DefaultPerks.Scouting.VantagePoint.SecondaryBonus, DefaultPerks.Scouting.VantagePoint.Name, null);
                }
            }
        }

        private void AddGarrisonOwnerPerkEffects(Settlement currentSettlement, ref ExplainedNumber result)
        {
            if (currentSettlement != null && currentSettlement.IsTown)
            {
                PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.CorpsACorps, currentSettlement.Town, ref result);
                PerkHelper.AddPerkBonusForTown(DefaultPerks.Leadership.VeteransRespect, currentSettlement.Town, ref result);
            }
        }

        public override int GetTierPartySizeEffect(int tier)
        {
            return 0;
        }

        public override int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan)
        {
            ExplainedNumber explainedNumber = new ExplainedNumber(20f, false, this._baseSizeText);
            if (leaderHero != null && leaderHero.Clan != null)
            {
                this.CalculateBaseMemberSize(leaderHero, partyMapFaction, actualClan, ref explainedNumber);
                explainedNumber.Add((float)(leaderHero.GetSkillValue(DefaultSkills.Leadership) / 4), this._leadershipSkillLevelBonusText, null);
            }
            return (int)explainedNumber.BaseNumber;
        }

        private void AddSettlementProjectBonuses(PartyBase party, ref ExplainedNumber result)
        {
            if (party.Owner != null)
            {
                Settlement currentSettlement = party.MobileParty.CurrentSettlement;
                if (currentSettlement != null && (currentSettlement.IsTown || currentSettlement.IsCastle))
                {
                    foreach (Building building in currentSettlement.Town.Buildings)
                    {
                        float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.GarrisonCapacity);
                        if (buildingEffectAmount > 0f)
                        {
                            result.Add(buildingEffectAmount, building.Name, null);
                        }
                    }
                }
            }
        }

        private int GetTownBonus(MobileParty party)
        {
            Settlement currentSettlement = party.CurrentSettlement;
            if (currentSettlement != null && currentSettlement.IsFortification && currentSettlement.IsTown)
            {
                return 200;
            }
            return 0;
        }

        private int GetCurrentPartySizeEffect(PartyBase party)
        {
            return party.NumberOfHealthyMembers / 2;
        }

        private void CalculateBaseMemberSize(Hero partyLeader, IFaction partyMapFaction, Clan actualClan, ref ExplainedNumber result)
        {
            if (partyMapFaction != null && partyMapFaction.IsKingdomFaction && partyLeader.MapFaction.Leader == partyLeader)
            {
                result.Add(20f, this._factionLeaderText, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.OneHanded.Prestige))
            {
                result.Add(DefaultPerks.OneHanded.Prestige.SecondaryBonus, DefaultPerks.OneHanded.Prestige.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.TwoHanded.Hope))
            {
                result.Add(DefaultPerks.TwoHanded.Hope.SecondaryBonus, DefaultPerks.TwoHanded.Hope.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Athletics.ImposingStature))
            {
                result.Add(DefaultPerks.Athletics.ImposingStature.SecondaryBonus, DefaultPerks.Athletics.ImposingStature.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Bow.MerryMen))
            {
                result.Add(DefaultPerks.Bow.MerryMen.PrimaryBonus, DefaultPerks.Bow.MerryMen.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Tactics.HordeLeader))
            {
                result.Add(DefaultPerks.Tactics.HordeLeader.PrimaryBonus, DefaultPerks.Tactics.HordeLeader.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Scouting.MountedScouts))
            {
                result.Add(DefaultPerks.Scouting.MountedScouts.SecondaryBonus, DefaultPerks.Scouting.MountedScouts.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Leadership.Authority))
            {
                result.Add(DefaultPerks.Leadership.Authority.SecondaryBonus, DefaultPerks.Leadership.Authority.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Leadership.UpliftingSpirit))
            {
                result.Add(DefaultPerks.Leadership.UpliftingSpirit.SecondaryBonus, DefaultPerks.Leadership.UpliftingSpirit.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Leadership.TalentMagnet))
            {
                result.Add(DefaultPerks.Leadership.TalentMagnet.PrimaryBonus, DefaultPerks.Leadership.TalentMagnet.Name, null);
            }
            if (partyLeader.GetSkillValue(DefaultSkills.Leadership) > Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus && partyLeader.GetPerkValue(DefaultPerks.Leadership.UltimateLeader))
            {
                int num = partyLeader.GetSkillValue(DefaultSkills.Leadership) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
                result.Add((float)num * DefaultPerks.Leadership.UltimateLeader.PrimaryBonus, this._leadershipPerkUltimateLeaderBonusText, null);
            }
            if (actualClan != null)
            {
                Hero leader = actualClan.Leader;
                bool? flag = (leader != null) ? new bool?(leader.GetPerkValue(DefaultPerks.Leadership.LeaderOfMasses)) : null;
                bool flag2 = true;
                if (flag.GetValueOrDefault() == flag2 & flag != null)
                {
                    int num2 = 0;
                    using (List<Settlement>.Enumerator enumerator = actualClan.Settlements.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.IsTown)
                            {
                                num2++;
                            }
                        }
                    }
                    float num3 = (float)num2 * DefaultPerks.Leadership.LeaderOfMasses.PrimaryBonus;
                    if (num3 > 0f)
                    {
                        result.Add(num3, DefaultPerks.Leadership.LeaderOfMasses.Name, null);
                    }
                }
            }
            if (partyLeader.Clan.Leader == partyLeader)
            {
                if (partyLeader.Clan.Tier >= 5 && partyMapFaction.IsKingdomFaction && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.NobleRetinues))
                {
                    result.Add(40f, DefaultPolicies.NobleRetinues.Name, null);
                }
                if (partyMapFaction.IsKingdomFaction && partyMapFaction.Leader == partyLeader && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.RoyalGuard))
                {
                    result.Add(60f, DefaultPolicies.RoyalGuard.Name, null);
                }
            }
        }
        private readonly TextObject _leadershipSkillLevelBonusText = GameTexts.FindText("str_leadership_skill_level_bonus", null);
        private readonly TextObject _leadershipPerkUltimateLeaderBonusText = GameTexts.FindText("str_leadership_perk_bonus", null);
        private readonly TextObject _wallLevelBonusText = GameTexts.FindText("str_map_tooltip_wall_level", null);
        private readonly TextObject _baseSizeText = GameTexts.FindText("str_base_size", null);
        private readonly TextObject _clanTierText = GameTexts.FindText("str_clan_tier_bonus", null);
        private readonly TextObject _renownText = GameTexts.FindText("str_renown_bonus", null);
        private readonly TextObject _clanLeaderText = GameTexts.FindText("str_clan_leader_bonus", null);
        private readonly TextObject _factionLeaderText = GameTexts.FindText("str_faction_leader_bonus", null);
        private readonly TextObject _leaderLevelText = GameTexts.FindText("str_leader_level_bonus", null);
        private readonly TextObject _townBonusText = GameTexts.FindText("str_town_bonus", null);
        private readonly TextObject _minorFactionText = GameTexts.FindText("str_minor_faction_bonus", null);
        private readonly TextObject _currentPartySizeBonusText = GameTexts.FindText("str_current_party_size_bonus", null);
        private readonly TextObject _randomSizeBonusTemporary = new TextObject("{=hynFV8jC}Extra size bonus (Perk-like Effect)", null);
        private static bool _addAdditionalPartySizeAsCheat;
        private static bool _addAdditionalPrisonerSizeAsCheat;
        private TextObject _quarterMasterText;
        private enum LimitType
        {
            MobilePartySizeLimit,
            GarrisonPartySizeLimit,
            PrisonerSizeLimit
        }
    }
}
