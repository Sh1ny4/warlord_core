using HarmonyLib;
using Helpers;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Warlord.patches
{
    [HarmonyPatch(typeof(SandboxCharacterCreationContent), "OnInitialized")]
    class CharacterCreationRedone : SandboxCharacterCreationContent
    {
        [HarmonyPrefix]
        static bool Prefix(ref CharacterCreationRedone __instance, CharacterCreation characterCreation)
        {
            __instance.AddMenus(characterCreation);
            return false;
        }
        public void AddMenus(CharacterCreation characterCreation)
        {
            this.FatherBackgroundMenu(characterCreation);
            this.EducationMenu(characterCreation);
            this.AdulthoodMenu(characterCreation);
            this.ReasonForAdventureMenu(characterCreation);
            this.AgeSelectionMenu(characterCreation);
        }

        protected override void OnApplyCulture()
        {
        }
        protected void FatherBackgroundMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Father Background", null), new TextObject("{=!}You were born years ago, in land far away. Your father was...", null), new CharacterCreationOnInit(this.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory parentscategory = characterCreationMenu.AddMenuCategory(null);
            parentscategory.AddCategoryOption(new TextObject("{=!}An impoverished noble", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitNoyansKinsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}You came into the world a (son/daughter) of declining nobility, owning only the house in which they lived. However, despite your family's hardships, they afforded you a good education and trained you from childhood for the rigors of aristocracy and life at court.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A travelling merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}You were born the (son/daughter) of travelling merchants, always moving from place to place in search of a profit. Although your parents were wealthier than most and educated you as well as they could, you found little opportunity to make friends on the road, living mostly for the moments when you could sell something to somebody.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A veteran warrior", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}As a child, your family scrabbled out a meagre living from your father's wages as a guardsman to the local lord. It was not an easy existence, and you were too poor to get much of an education. You learned mainly how to defend yourself on the streets, with or without a weapon in hand.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A hunter", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}You were the (son/daughter) of a family who lived off the woods, doing whatever they needed to make ends meet. Hunting, woodcutting, making arrows, even a spot of poaching whenever things got tight. Winter was never a good time for your family as the cold took animals and people alike, but you always lived to see another dawn, though your brothers and sister might not be fortunate.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A steppe nomad", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitShamanOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}You were a child of the steppe, born to a tribe of wandering nomads who lived in great camps throughout the arid grasslands. Like the other tribesmen, your family revered horses above almost everything else, and they taught you how to ride almost before you learned how to walk.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A thief", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Riding }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.KhuzaitNomadOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}As the (son/daughter) of a thief, you had very little 'formal' education. Instead you were out on the street, begging until you learned how to pick locks, all the way through your childhood. Still, these long years made you streetwise and sharp to the secrets of cities and shadowy backways.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new protected void ParentsOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = false;
            characterCreation.HasSecondaryCharacter = false;
            SandboxCharacterCreationContent.ClearMountEntity(characterCreation);
            characterCreation.ClearFaceGenPrefab();
            if (base.PlayerBodyProperties != CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1))
            {
                base.PlayerBodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
                BodyProperties playerBodyProperties = base.PlayerBodyProperties;
                BodyProperties playerBodyProperties2 = base.PlayerBodyProperties;
                FaceGen.GenerateParentKey(base.PlayerBodyProperties, CharacterObject.PlayerCharacter.Race, ref playerBodyProperties, ref playerBodyProperties2);
                playerBodyProperties = new BodyProperties(new DynamicBodyProperties(33f, 0.3f, 0.2f), playerBodyProperties.StaticProperties);
                playerBodyProperties2 = new BodyProperties(new DynamicBodyProperties(33f, 0.5f, 0.5f), playerBodyProperties2.StaticProperties);
                base.MotherFacegenCharacter = new FaceGenChar(playerBodyProperties, CharacterObject.PlayerCharacter.Race, new Equipment(), true, "anim_mother_1");
                base.FatherFacegenCharacter = new FaceGenChar(playerBodyProperties2, CharacterObject.PlayerCharacter.Race, new Equipment(), false, "anim_father_1");
            }
            characterCreation.ChangeFaceGenChars(new List<FaceGenChar> { base.MotherFacegenCharacter, base.FatherFacegenCharacter });
            this.ChangeParentsOutfit(characterCreation, "", "", true, true);
            this.ChangeParentsAnimation(characterCreation);
        }
        new protected void ChangeParentsOutfit(CharacterCreation characterCreation, string fatherItemId = "", string motherItemId = "", bool isLeftHandItemForFather = true, bool isLeftHandItemForMother = true)
        {
            characterCreation.ClearFaceGenPrefab();
            List<Equipment> list = new List<Equipment>();
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new object[] { "mother_char_creation_", base.SelectedParentType, "_", base.GetSelectedCulture().StringId }));
            Equipment equipment = ((@object != null) ? @object.DefaultEquipment : null) ?? MBEquipmentRoster.EmptyEquipment;
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new object[] { "father_char_creation_", base.SelectedParentType, "_", base.GetSelectedCulture().StringId }));
            Equipment equipment2 = ((object2 != null) ? object2.DefaultEquipment : null) ?? MBEquipmentRoster.EmptyEquipment;
            if (motherItemId != "")
            {
                ItemObject object3 = Game.Current.ObjectManager.GetObject<ItemObject>(motherItemId);
                if (object3 != null)
                {
                    equipment.AddEquipmentToSlotWithoutAgent(isLeftHandItemForMother ? EquipmentIndex.WeaponItemBeginSlot : EquipmentIndex.Weapon1, new EquipmentElement(object3, null, null, false));
                }
                else
                {
                    Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(characterCreation.FaceGenChars[0].Race);
                    characterCreation.ChangeCharacterPrefab(motherItemId, isLeftHandItemForMother ? baseMonsterFromRace.MainHandItemBoneIndex : baseMonsterFromRace.OffHandItemBoneIndex);
                }
            }
            if (fatherItemId != "")
            {
                ItemObject object4 = Game.Current.ObjectManager.GetObject<ItemObject>(fatherItemId);
                if (object4 != null)
                {
                    equipment2.AddEquipmentToSlotWithoutAgent(isLeftHandItemForFather ? EquipmentIndex.WeaponItemBeginSlot : EquipmentIndex.Weapon1, new EquipmentElement(object4, null, null, false));
                }
            }
            list.Add(equipment);
            list.Add(equipment2);
            characterCreation.ChangeCharactersEquipment(list);
        }
        new protected void ChangeParentsAnimation(CharacterCreation characterCreation)
        {
            List<string> actionList = new List<string> { "anim_mother_" + base.SelectedParentType, "anim_father_" + base.SelectedParentType };
            characterCreation.ChangeCharsAnimation(actionList);
        }
        new protected void SetParentAndOccupationType(CharacterCreation characterCreation, int parentType, SandboxCharacterCreationContent.OccupationTypes occupationType, string fatherItemId = "", string motherItemId = "", bool isLeftHandItemForFather = true, bool isLeftHandItemForMother = true)
        {
            base.SelectedParentType = parentType;
            this._familyOccupationType = occupationType;
            characterCreation.ChangeFaceGenChars(new List<FaceGenChar> { base.MotherFacegenCharacter, base.FatherFacegenCharacter });
            this.ChangeParentsAnimation(characterCreation);
            this.ChangeParentsOutfit(characterCreation, fatherItemId, motherItemId, isLeftHandItemForFather, isLeftHandItemForMother);
        }
        protected bool RhodokParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire";
        }
        protected bool SwadianParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia";
        }
        protected bool VaegirParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia";
        }
        protected bool SarranidParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "aserai";
        }
        protected bool NordParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania";
        }
        protected bool KhergitParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "khuzait";
        }
        protected void ParentsOnApply(CharacterCreation characterCreation)
        {
            this.FinalizeParents();
        }
        new protected void KhuzaitNoyansKinsmanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, SandboxCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        new protected void KhuzaitMerchantOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, SandboxCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        new protected void KhuzaitTribesmanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, SandboxCharacterCreationContent.OccupationTypes.Herder, "", "", true, true);
        }
        new protected void KhuzaitFarmerOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, SandboxCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        new protected void KhuzaitShamanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Healer, "", "", true, true);
        }
        new protected void KhuzaitNomadOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, SandboxCharacterCreationContent.OccupationTypes.Herder, "", "", true, true);
        }
        new protected void FinalizeParents()
        {
            CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("main_hero_mother");
            CharacterObject object2 = Game.Current.ObjectManager.GetObject<CharacterObject>("main_hero_father");
            @object.HeroObject.ModifyPlayersFamilyAppearance(base.MotherFacegenCharacter.BodyProperties.StaticProperties);
            object2.HeroObject.ModifyPlayersFamilyAppearance(base.FatherFacegenCharacter.BodyProperties.StaticProperties);
            @object.HeroObject.Weight = base.MotherFacegenCharacter.BodyProperties.Weight;
            @object.HeroObject.Build = base.MotherFacegenCharacter.BodyProperties.Build;
            object2.HeroObject.Weight = base.FatherFacegenCharacter.BodyProperties.Weight;
            object2.HeroObject.Build = base.FatherFacegenCharacter.BodyProperties.Build;
            EquipmentHelper.AssignHeroEquipmentFromEquipment(@object.HeroObject, base.MotherFacegenCharacter.Equipment);
            EquipmentHelper.AssignHeroEquipmentFromEquipment(object2.HeroObject, base.FatherFacegenCharacter.Equipment);
            @object.Culture = Hero.MainHero.Culture;
            object2.Culture = Hero.MainHero.Culture;
            StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
            TextObject textObject = GameTexts.FindText("str_player_father_name", Hero.MainHero.Culture.StringId);
            object2.HeroObject.SetName(textObject, textObject);
            TextObject textObject2 = new TextObject("{=XmvaRfLM}{PLAYER_FATHER.NAME} was the father of {PLAYER.LINK}. He was slain when raiders attacked the inn at which his family was staying.", null);
            StringHelpers.SetCharacterProperties("PLAYER_FATHER", object2, textObject2, false);
            object2.HeroObject.EncyclopediaText = textObject2;
            TextObject textObject3 = GameTexts.FindText("str_player_mother_name", Hero.MainHero.Culture.StringId);
            @object.HeroObject.SetName(textObject3, textObject3);
            TextObject textObject4 = new TextObject("{=hrhvEWP8}{PLAYER_MOTHER.NAME} was the mother of {PLAYER.LINK}. She was slain when raiders attacked the inn at which her family was staying.", null);
            StringHelpers.SetCharacterProperties("PLAYER_MOTHER", @object, textObject4, false);
            @object.HeroObject.EncyclopediaText = textObject4;
            @object.HeroObject.UpdateHomeSettlement();
            object2.HeroObject.UpdateHomeSettlement();
            @object.HeroObject.SetHasMet();
            object2.HeroObject.SetHasMet();
        }
        new protected static List<FaceGenChar> ChangePlayerFaceWithAge(float age, string actionName = "act_childhood_schooled")
        {
            List<FaceGenChar> list = new List<FaceGenChar>();
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, age);
            list.Add(new FaceGenChar(bodyProperties, CharacterObject.PlayerCharacter.Race, new Equipment(), CharacterObject.PlayerCharacter.IsFemale, actionName));
            return list;
        }
        new protected Equipment ChangePlayerOutfit(CharacterCreation characterCreation, string outfit)
        {
            List<Equipment> list = new List<Equipment>();
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(outfit);
            Equipment equipment = (@object != null) ? @object.DefaultEquipment : null;
            if (equipment == null)
            {
                Debug.FailedAssert("item shouldn't be null!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\SandboxCharacterCreationContent.cs", "ChangePlayerOutfit", 1048);
                equipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default").DefaultEquipment;
            }
            list.Add(equipment);
            characterCreation.ChangeCharactersEquipment(list);
            return equipment;
        }
        new protected static void ChangePlayerMount(CharacterCreation characterCreation, Hero hero)
        {
            if (hero.CharacterObject.HasMount())
            {
                FaceGenMount faceGenMount = new FaceGenMount(MountCreationKey.GetRandomMountKey(hero.CharacterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, hero.CharacterObject.GetMountKeySeed()), hero.CharacterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, hero.CharacterObject.Equipment[EquipmentIndex.HorseHarness].Item, "act_horse_stand_1");
                characterCreation.SetFaceGenMount(faceGenMount);
            }
        }
        new protected static void ClearMountEntity(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenMounts();
        }
        //education
        protected void EducationMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Early Life and Education", null), new TextObject("{=!}You started to learn about the world almost as soon as you could walk and talk. You spent your early life as...", null), new CharacterCreationOnInit(this.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A page at a nobleman's court", null), new MBList<SkillObject> { DefaultSkills.Leadership, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.RuralAdolescenceHerderOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHerderOnApply), new TextObject("{=!}You were sent to live in the court of one of the nobles of the land. There, your first lessons were in humility, as you waited upon the lords and ladies of the household. But from their chess games, their gossip, even the poetry of great deeds and courtly love, you quickly began to learn about the adult world of conflict and competition. You also learned from the rough games of the other children, who battered at each other with sticks in imitation of their elders' swords", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A craftsman's apprentice", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.RuralAdolescenceSmithyOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceSmithyOnApply), new TextObject("{=!}You apprenticed with a local craftsman to learn a trade. After years of hard work and study under your new master, he promoted you to journeyman and employed you as a fully paid craftsman for as long as you wished to stay.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A shop assistant", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.RuralAdolescenceRepairmanOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceRepairmanOnApply), new TextObject("{=!}You apprenticed to a wealthy merchant, picking up the trade over years of working shops and driving caravans. You soon became adept at the art of buying low, selling high, and leaving the customer thinking they'd got the better deal.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A street urchin", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.RuralAdolescenceGathererOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceGathererOnApply), new TextObject("{=!}You took to the streets doing whatever you must to survive. Begging, thieving and working for gangs to earn your bread, you lived from day to day in this violent world, always one step ahead of the law and those who wished you ill.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A steppe child", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Leadership }, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.RuralAdolescenceHunterOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHunterOnApply), new TextObject("{=!}You rode the great steppes on a horse of your own, learning the ways of the grass and the desert. Although you sometimes went hungry, you became a skillful hunter and pathfinder in this trackless country. Your body too started to harden with muscle as you grew into the life of a nomad (man/woman).", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new protected void EducationOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)this.EducationAge, "act_childhood_schooled"));
            string text = string.Concat(new object[] { "player_char_creation_education_age_", base.GetSelectedCulture().StringId, "_", base.SelectedParentType });
            text += (Hero.MainHero.IsFemale ? "_f" : "_m");
            this.ChangePlayerOutfit(characterCreation, text);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            SandboxCharacterCreationContent.ClearMountEntity(characterCreation);
        }
        new protected void RefreshPropsAndClothing(CharacterCreation characterCreation, bool isChildhoodStage, string itemId, bool isLeftHand, string secondItemId = "")
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ClearCharactersEquipment();
            string text = isChildhoodStage ? string.Concat(new object[] { "player_char_creation_childhood_age_", base.GetSelectedCulture().StringId, "_", base.SelectedParentType }) : string.Concat(new object[] { "player_char_creation_education_age_", base.GetSelectedCulture().StringId, "_", base.SelectedParentType });
            text += (Hero.MainHero.IsFemale ? "_f" : "_m");
            Equipment equipment = this.ChangePlayerOutfit(characterCreation, text).Clone(false);
            if (Game.Current.ObjectManager.GetObject<ItemObject>(itemId) != null)
            {
                ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(itemId);
                equipment.AddEquipmentToSlotWithoutAgent(isLeftHand ? EquipmentIndex.WeaponItemBeginSlot : EquipmentIndex.Weapon1, new EquipmentElement(@object, null, null, false));
                if (secondItemId != "")
                {
                    @object = Game.Current.ObjectManager.GetObject<ItemObject>(secondItemId);
                    equipment.AddEquipmentToSlotWithoutAgent(isLeftHand ? EquipmentIndex.Weapon1 : EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(@object, null, null, false));
                }
            }
            else
            {
                Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(characterCreation.FaceGenChars[0].Race);
                characterCreation.ChangeCharacterPrefab(itemId, isLeftHand ? baseMonsterFromRace.MainHandItemBoneIndex : baseMonsterFromRace.OffHandItemBoneIndex);
            }
            characterCreation.ChangeCharactersEquipment(new List<Equipment> { equipment });
        }
        new protected void RuralAdolescenceHerderOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_streets" });
            this.RefreshPropsAndClothing(characterCreation, false, "carry_bostaff_rogue1", true, "");
        }
        new protected void RuralAdolescenceSmithyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_militia" });
            this.RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_1_t1", true, "");
        }
        new protected void RuralAdolescenceRepairmanOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_grit" });
            this.RefreshPropsAndClothing(characterCreation, false, "carry_hammer", true, "");
        }
        new protected void RuralAdolescenceGathererOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_peddlers" });
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
        }
        new protected void RuralAdolescenceHunterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_sharp" });
            this.RefreshPropsAndClothing(characterCreation, false, "composite_bow", true, "");
        }
        new protected static void RuralAdolescenceHerderOnApply(CharacterCreation characterCreation)
        {
        }
        new protected static void RuralAdolescenceSmithyOnApply(CharacterCreation characterCreation)
        {
        }
        new protected static void RuralAdolescenceRepairmanOnApply(CharacterCreation characterCreation)
        {
        }
        new protected static void RuralAdolescenceGathererOnApply(CharacterCreation characterCreation)
        {
        }
        new protected static void RuralAdolescenceHunterOnApply(CharacterCreation characterCreation)
        {
        }
        //youth
        protected void AdulthoodMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Adulthood", null), new TextObject("{=!}Then, as a young adult, life changed as it always does. You became...", null), new CharacterCreationOnInit(this.YouthOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A squire", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.IsMaleOnCondition), new CharacterCreationOnSelect(this.YouthCommanderOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCommanderOnApply), new TextObject("{=!}When you were named squire to a noble at court, you practiced long hours with weapons, learning how to deal out hard knocks and how to take them, too. You were instructed in your obligations to your lord, and of your duties to those who might one day be your vassals. But in addition to learning the chivalric ideal, you also learned about the less uplifting side -- old warriors' stories of ruthless power politics, of betrayals and usurpations, of men who used guile as well as valor to achieve their aims.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A lady-in-waiting", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.IsFemaleOnCondition), new CharacterCreationOnSelect(this.YouthGroomOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGroomOnApply), new TextObject("{=!}You joined the tightly-knit circle of women at court, ladies who all did proper ladylike things, the wives and mistresses of noble men as well as maidens who had yet to find a husband. However, even here you found politics at work as the ladies schemed for prominence and fought each other bitterly to catch the eye of whatever unmarried man was in fashion at court. You soon learned ways of turning these situations and goings-on to your advantage. With it came the realisation that you yourself could wield great influence in the world, if only you applied yourself with a little bit of subtlety.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A troubadour", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthChieftainOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthChieftainOnApply), new TextObject("{=!}You set out on your own with nothing except the instrument slung over your back and your own voice. It was a poor existence, with many a hungry night when people failed to appreciate your play, but you managed to survive your music alone. As the years went by you became adept at playing the drunken crowds in your taverns, and even better at talking anyone out of anything you wanted.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A university student", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=!}You found yourself as a student in the university of one of the great cities, where you studied theology, philosophy, and medicine. But not all your lessons were learned in the lecture halls. You may or may not have joined in with your fellows as they roamed the alleys in search of wine, women, and a good fight. However, you certainly were able to observe how a broken jaw is set, or how an angry townsman can be persuaded to set down his club and accept cash compensation for the destruction of his shop.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A goods peddler", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthHearthGuardOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthHearthGuardOnApply), new TextObject("{=!}Heeding the call of the open road, you travelled from village to village buying and selling what you could. It was not a rich existence, but you became a master at haggling even the most miserly elders into giving you a good price. Soon, you knew, you would be well-placed to start your own trading empire...", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A smith", null), new MBList<SkillObject> { DefaultSkills.Crossbow, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=!}You pursued a career as a smith, crating items of function and beauty out of simple metal. As time wore on you became a master of your trade, and fine work started to fetch fine prices. With food in your belly and logs on your fire, you could take pride in your work and your growing reputation.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A game poacher", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthOtherGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}Dissatisfied with the common men's desperate scrabble for coin, you took to your local lord's own forests and decided to help yourself to its bounty, laws be damned. You hunted stags, boars and geese and sold the precious meat under the table. You cut down trees right under the watchmen's noses and turned them into firewood that warmed many freezing homes during winter. All for a few silvers, of course.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new protected void YouthOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)this.YouthAge, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            if (base.SelectedTitleType < 1 || base.SelectedTitleType > 10)
            {
                base.SelectedTitleType = 1;
            }
            this.RefreshPlayerAppearance(characterCreation);
        }
        new protected void RefreshPlayerAppearance(CharacterCreation characterCreation)
        {
            string text = string.Concat(new object[] { "player_char_creation_", base.GetSelectedCulture().StringId, "_", base.SelectedTitleType });
            text += (Hero.MainHero.IsFemale ? "_f" : "_m");
            this.ChangePlayerOutfit(characterCreation, text);
            this.ApplyEquipments(characterCreation);
        }
        protected bool IsMaleOnCondition()
        {
            return !Hero.MainHero.IsFemale;
        }
        new protected void YouthCommanderOnApply(CharacterCreation characterCreation)
        {
        }
        protected bool IsFemaleOnCondition()
        {
            return !Hero.MainHero.IsFemale;
        }
        new protected void YouthCommanderOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_decisive" });
        }
        new protected void YouthGroomOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_sharp" });
        }
        new protected void YouthChieftainOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready" });
        }
        new protected void YouthCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_apprentice" });
        }
        new protected void YouthHearthGuardOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_athlete" });
        }
        new protected void YouthOutridersOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_gracious" });
        }
        new protected void YouthOtherOutridersOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_gracious" });
        }
        new protected void YouthInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_fierce" });
        }
        new protected void YouthSkirmisherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_fox" });
        }
        new protected void YouthGarrisonOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
        }
        new protected void YouthOtherGarrisonOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_sharp" });
        }
        new protected void YouthKernOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_apprentice" });
        }
        new protected void YouthCamperOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_militia" });
        }
        new protected void YouthGroomOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthChieftainOnCondition()
        {
            return (base.GetSelectedCulture().StringId == "battania" || base.GetSelectedCulture().StringId == "khuzait") && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        new protected void YouthChieftainOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthCavalryOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "khuzait" || base.GetSelectedCulture().StringId == "aserai" || base.GetSelectedCulture().StringId == "vlandia";
        }
        new protected void YouthCavalryOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthHearthGuardOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia" || base.GetSelectedCulture().StringId == "battania";
        }
        new protected void YouthHearthGuardOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthOutridersOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "khuzait";
        }
        new protected void YouthOutridersOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthOtherOutridersOnCondition()
        {
            return base.GetSelectedCulture().StringId != "empire" && base.GetSelectedCulture().StringId != "khuzait";
        }
        new protected void YouthOtherOutridersOnApply(CharacterCreation characterCreation)
        {
        }
        new protected void YouthInfantryOnApply(CharacterCreation characterCreation)
        {
        }
        new protected void YouthSkirmisherOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthGarrisonOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "vlandia";
        }
        new protected void YouthGarrisonOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthOtherGarrisonOnCondition()
        {
            return base.GetSelectedCulture().StringId != "empire" && base.GetSelectedCulture().StringId != "vlandia";
        }
        new protected void YouthOtherGarrisonOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthSkirmisherOnCondition()
        {
            return base.GetSelectedCulture().StringId != "battania";
        }
        new protected bool YouthKernOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania";
        }
        new protected void YouthKernOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool YouthCamperOnCondition()
        {
            return this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        new protected void YouthCamperOnApply(CharacterCreation characterCreation)
        {
        }
        //accomplishement
        protected void ReasonForAdventureMenu(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", this.SkillLevelToAdd);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Reasons for Adventure", null), new TextObject("{=!}But soon everything changed and you decided to strike out on your own as an adventurer. What made you take this decision was...", null), new CharacterCreationOnInit(this.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}Personal revenge", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentDefeatedEnemyOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentDefeatedEnemyOnApply), new TextObject("{=!}Still, it was not a difficult choice to leave, with the rage burning brightly in your heart. You want vengeance. You want justice. What was done to you cannot be undone, and these debts can only be paid in blood...", null), new MBList<TraitObject> { DefaultTraits.Valor }, 1, 20, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}The loss of a loved one", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentExpeditionOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}All you can say is that you couldn't bear to stay, not with the memories of those you loved so close and so painful. Perhaps your new life will let you forget, or honour the name that you can no longer bear to speak...", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}Wanderlust", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}You're not even sure when your home became a prison, when the familiar become mundane, but your dreams of wandering have taken over your life. Whether you yearn for some faraway place or merely for the open road and the freedom to travel, you could no longer bear to stay in the same place. You simply went and never looked back...", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}Being forced out of your home", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentSavedVillageOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}However, you know you cannot go back. There's nothing to go back to. Whatever home you may have had is gone now, and you must face the fact that you're out in the wide wide world. Alone to sink or swim...", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}Lust for money and power", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentSavedStreetOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}To everyone else, it's clear that you're now motivated solely by personal gain. You want to be rich, powerful, respected, feared. You want to be the one whom others hurry to obey. You want people to know your name, and tremble whenever it is spoken. You want everything, and you won't let anyone stop you from having it...", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new protected void AccomplishmentOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)this.AccomplishmentAge, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            this.RefreshPlayerAppearance(characterCreation);
        }
        new protected void AccomplishmentDefeatedEnemyOnApply(CharacterCreation characterCreation)
        {
        }
        new protected void AccomplishmentExpeditionOnApply(CharacterCreation characterCreation)
        {
        }
        new protected bool AccomplishmentRuralOnCondition()
        {
            return this.RuralType();
        }
        new protected bool AccomplishmentMerchantOnCondition()
        {
            return this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Merchant;
        }
        new protected bool AccomplishmentPosseOnConditions()
        {
            return this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Herder || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Mercenary;
        }
        new protected bool AccomplishmentSavedVillageOnCondition()
        {
            return this.RuralType() && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Retainer && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Herder;
        }
        new protected bool AccomplishmentSavedStreetOnCondition()
        {
            return !this.RuralType() && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Merchant && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Mercenary;
        }
        new protected bool AccomplishmentUrbanOnCondition()
        {
            return !this.RuralType();
        }
        new protected void AccomplishmentWorkshopOnApply(CharacterCreation characterCreation)
        {
        }
        new protected void AccomplishmentSiegeHunterOnApply(CharacterCreation characterCreation)
        {
        }
        new protected void AccomplishmentEscapadeOnApply(CharacterCreation characterCreation)
        {
        }
        new protected void AccomplishmentTreaterOnApply(CharacterCreation characterCreation)
        {
        }
        new protected void AccomplishmentDefeatedEnemyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_athlete" });
        }
        new protected void AccomplishmentExpeditionOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_gracious" });
        }
        new protected void AccomplishmentMerchantOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready" });
        }
        new protected void AccomplishmentSavedVillageOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
        }
        new protected void AccomplishmentSavedStreetOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
        }
        new protected void AccomplishmentWorkshopOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_decisive" });
        }
        new protected void AccomplishmentSiegeHunterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_tough" });
        }
        new protected void AccomplishmentEscapadeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_clever" });
        }
        new protected void AccomplishmentTreaterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_manners" });
        }
        //Age
        protected void AgeSelectionMenu(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", this.SkillLevelToAdd);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Starting Age", null), new TextObject("{=!}Your character started off on the adventuring path at the age of...", null), new CharacterCreationOnInit(this.StartingAgeOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}21", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(this.StartingAgeYoungOnConsequence), new CharacterCreationApplyFinalEffects(this.StartingAgeYoungOnApply), new TextObject("{=2k7adlh7}While lacking experience a bit, you are full with youthful energy, you are fully eager, for the long years of adventuring ahead.", null), null, 0, 0, 0, 2, 1);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}30", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(this.StartingAgeAdultOnConsequence), new CharacterCreationApplyFinalEffects(this.StartingAgeAdultOnApply), new TextObject("{=NUlVFRtK}You are at your prime, You still have some youthful energy but also have a substantial amount of experience under your belt. ", null), null, 0, 0, 0, 4, 2);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}40", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(this.StartingAgeMiddleAgedOnConsequence), new CharacterCreationApplyFinalEffects(this.StartingAgeMiddleAgedOnApply), new TextObject("{=5MxTYApM}This is the right age for starting off, you have years of experience, and you are old enough for people to respect you and gather under your banner.", null), null, 0, 0, 0, 6, 3);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}50", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(this.StartingAgeElderlyOnConsequence), new CharacterCreationApplyFinalEffects(this.StartingAgeElderlyOnApply), new TextObject("{=ePD5Afvy}While you are past your prime, there is still enough time to go on that last big adventure for you. And you have all the experience you need to overcome anything!", null), null, 0, 0, 0, 8, 4);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new protected void StartingAgeOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)this._startingAge, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            this.RefreshPlayerAppearance(characterCreation);
        }
        new protected void StartingAgeYoungOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(21f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_focus" });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.YoungAdult;
            this.SetHeroAge(21f);
        }
        new protected void StartingAgeAdultOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(30f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready" });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.Adult;
            this.SetHeroAge(30f);
        }
        new protected void StartingAgeMiddleAgedOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(40f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_sharp" });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.MiddleAged;
            this.SetHeroAge(40f);
        }
        new protected void StartingAgeElderlyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(50f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_tough" });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.Elder;
            this.SetHeroAge(50f);
        }
        new protected void StartingAgeYoungOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.YoungAdult;
        }
        new protected void StartingAgeAdultOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.Adult;
        }
        new protected void StartingAgeMiddleAgedOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.MiddleAged;
        }
        new protected void StartingAgeElderlyOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.Elder;
        }
        new protected void ApplyEquipments(CharacterCreation characterCreation)
        {
            SandboxCharacterCreationContent.ClearMountEntity(characterCreation);
            string text = string.Concat(new object[] { "player_char_creation_", base.GetSelectedCulture().StringId, "_", base.SelectedTitleType });
            text += (Hero.MainHero.IsFemale ? "_f" : "_m");
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(text);
            base.PlayerStartEquipment = (((@object != null) ? @object.DefaultEquipment : null) ?? MBEquipmentRoster.EmptyEquipment);
            base.PlayerCivilianEquipment = (((@object != null) ? @object.GetCivilianEquipments().FirstOrDefault<Equipment>() : null) ?? MBEquipmentRoster.EmptyEquipment);
            if (base.PlayerStartEquipment != null && base.PlayerCivilianEquipment != null)
            {
                CharacterObject.PlayerCharacter.Equipment.FillFrom(base.PlayerStartEquipment, true);
                CharacterObject.PlayerCharacter.FirstCivilianEquipment.FillFrom(base.PlayerCivilianEquipment, true);
            }
            SandboxCharacterCreationContent.ChangePlayerMount(characterCreation, Hero.MainHero);
        }
        new protected void SetHeroAge(float age)
        {
            Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-age));
        }
        new protected const int FocusToAddYouthStart = 2;
        new protected const int FocusToAddAdultStart = 4;
        new protected const int FocusToAddMiddleAgedStart = 6;
        new protected const int FocusToAddElderlyStart = 8;
        new protected const int AttributeToAddYouthStart = 1;
        new protected const int AttributeToAddAdultStart = 2;
        new protected const int AttributeToAddMiddleAgedStart = 3;
        new protected const int AttributeToAddElderlyStart = 4;
        new protected readonly Dictionary<string, Vec2> _startingPoints = new Dictionary<string, Vec2>
        {
              { "empire", new Vec2(657.95f, 279.08f) },
              { "sturgia", new Vec2(356.75f, 551.52f) },
              { "aserai", new Vec2(300.78f, 259.99f) },
              { "battania", new Vec2(293.64f, 446.39f) },
              { "khuzait", new Vec2(680.73f, 480.8f) },
              { "vlandia", new Vec2(207.04f, 389.04f) }
        };
        new protected SandboxCharacterCreationContent.SandboxAgeOptions _startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.YoungAdult;
        new protected SandboxCharacterCreationContent.OccupationTypes _familyOccupationType;
        new protected TextObject _educationIntroductoryText = new TextObject("{=!}{EDUCATION_INTRO}", null);
        new protected TextObject _youthIntroductoryText = new TextObject("{=!}{YOUTH_INTRO}", null);
        new protected enum SandboxAgeOptions
        {
            YoungAdult = 21,
            Adult = 30,
            MiddleAged = 40,
            Elder = 50
        }

        new protected enum OccupationTypes
        {
            Artisan,
            Bard,
            Retainer,
            Merchant,
            Farmer,
            Hunter,
            Vagabond,
            Mercenary,
            Herder,
            Healer,
            NumberOfTypes
        }
    }
}