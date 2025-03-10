﻿using HarmonyLib;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Warlord.patches
{
    [HarmonyPatch(typeof(SandboxCharacterCreationContent), "OnInitialized")]
    public class CharacterCreationRedone : SandboxCharacterCreationContent
    {
        [HarmonyPrefix]
        static bool Prefix(ref CharacterCreationRedone __instance, CharacterCreation characterCreation)
        {
            __instance.AddMenus(characterCreation);
            return false;
        }
        public void AddMenus(CharacterCreation characterCreation)
        {
            FatherBackgroundMenu(characterCreation);
            EducationMenu(characterCreation);
            AdulthoodMenu(characterCreation);
            ReasonForAdventureMenu(characterCreation);
            AgeSelectionMenu(characterCreation);
        }
        public void FatherBackgroundMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Father Background", null), new TextObject("{=!}You were born years ago, in land far away. Your father was...", null), new CharacterCreationOnInit(this.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory parentscategory = characterCreationMenu.AddMenuCategory(null);
            parentscategory.AddCategoryOption(new TextObject("{=!}An impoverished noble", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 20, 2, null, new CharacterCreationOnSelect(this.ImpoverishedNobleOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}You came into the world a (son/daughter) of declining nobility, owning only the house in which they lived. However, despite your family's hardships, they afforded you a good education and trained you from childhood for the rigors of aristocracy and life at court.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A travelling merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 20, 2, null, new CharacterCreationOnSelect(this.TravellingMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}You were born the (son/daughter) of travelling merchants, always moving from place to place in search of a profit. Although your parents were wealthier than most and educated you as well as they could, you found little opportunity to make friends on the road, living mostly for the moments when you could sell something to somebody.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A veteran warrior", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 20, 2, null, new CharacterCreationOnSelect(this.VeteranWarriorOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}As a child, your family scrabbled out a meagre living from your father's wages as a guardsman to the local lord. It was not an easy existence, and you were too poor to get much of an education. You learned mainly how to defend yourself on the streets, with or without a weapon in hand.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A hunter", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, 1, 20, 2, null, new CharacterCreationOnSelect(this.HunterOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}You were the (son/daughter) of a family who lived off the woods, doing whatever they needed to make ends meet. Hunting, woodcutting, making arrows, even a spot of poaching whenever things got tight. Winter was never a good time for your family as the cold took animals and people alike, but you always lived to see another dawn, though your brothers and sister might not be fortunate.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A steppe nomad", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 20, 2, null, new CharacterCreationOnSelect(this.SteppeNomadOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}You were a child of the steppe, born to a tribe of wandering nomads who lived in great camps throughout the arid grasslands. Like the other tribesmen, your family revered horses above almost everything else, and they taught you how to ride almost before you learned how to walk.", null), null, 0, 0, 0, 0, 0);
            parentscategory.AddCategoryOption(new TextObject("{=!}A thief", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Riding }, DefaultCharacterAttributes.Cunning, 1, 20, 2, null, new CharacterCreationOnSelect(this.ThiefOnConsequence), new CharacterCreationApplyFinalEffects(this.ParentsOnApply), new TextObject("{=!}As the (son/daughter) of a thief, you had very little 'formal' education. Instead you were out on the street, begging until you learned how to pick locks, all the way through your childhood. Still, these long years made you streetwise and sharp to the secrets of cities and shadowy backways.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new public void ParentsOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = false;
            characterCreation.HasSecondaryCharacter = false;
            CharacterCreationRedone.ClearMountEntity(characterCreation);
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
        new public void ChangeParentsOutfit(CharacterCreation characterCreation, string fatherItemId = "", string motherItemId = "", bool isLeftHandItemForFather = true, bool isLeftHandItemForMother = true)
        {
            characterCreation.ClearFaceGenPrefab();
            List<Equipment> list = new List<Equipment>();
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new object[] { "mother_char_creation_", base.SelectedParentType, "_", base.GetSelectedCulture().StringId }));
            Equipment equipment = (@object?.DefaultEquipment) ?? MBEquipmentRoster.EmptyEquipment;
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new object[] { "father_char_creation_", base.SelectedParentType, "_", base.GetSelectedCulture().StringId }));
            Equipment equipment2 = (object2?.DefaultEquipment) ?? MBEquipmentRoster.EmptyEquipment;
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
        new public void ChangeParentsAnimation(CharacterCreation characterCreation)
        {
            List<string> actionList = new List<string> { "anim_mother_" + base.SelectedParentType, "anim_father_" + base.SelectedParentType };
            characterCreation.ChangeCharsAnimation(actionList);
        }
        new private void SetParentAndOccupationType(CharacterCreation characterCreation, int parentType, CharacterCreationRedone.OccupationTypes occupationType, string fatherItemId = "", string motherItemId = "", bool isLeftHandItemForFather = true, bool isLeftHandItemForMother = true)
        {
            base.SelectedParentType = parentType;
            this._familyOccupationType = occupationType;
            characterCreation.ChangeFaceGenChars(new List<FaceGenChar> { base.MotherFacegenCharacter, base.FatherFacegenCharacter });
            this.ChangeParentsAnimation(characterCreation);
            this.ChangeParentsOutfit(characterCreation, fatherItemId, motherItemId, isLeftHandItemForFather, isLeftHandItemForMother);
        }
        public bool RhodokParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire";
        }
        public bool SwadianParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia";
        }
        public bool VaegirParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia";
        }
        public bool SarranidParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "aserai";
        }
        public bool NordParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania";
        }
        public bool KhergitParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "khuzait";
        }
        public bool IsMaleOnCondition()
        {
            return !(Hero.MainHero.IsFemale);
        }
        public bool IsFemaleOnCondition()
        {
            return Hero.MainHero.IsFemale;
        }
        public void ParentsOnApply(CharacterCreation characterCreation)
        {
            this.FinalizeParents();
        }
        public void ImpoverishedNobleOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, CharacterCreationRedone.OccupationTypes.Retainer, "", "", true, true);
        }
        public void TravellingMerchantOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, CharacterCreationRedone.OccupationTypes.Merchant, "", "", true, true);
        }
        public void VeteranWarriorOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, CharacterCreationRedone.OccupationTypes.Herder, "", "", true, true);
        }
        public void HunterOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, CharacterCreationRedone.OccupationTypes.Farmer, "", "", true, true);
        }
        public void SteppeNomadOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, CharacterCreationRedone.OccupationTypes.Healer, "", "", true, true);
        }
        public void ThiefOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, CharacterCreationRedone.OccupationTypes.Herder, "", "", true, true);
        }
        new public void FinalizeParents()
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
        new public static List<FaceGenChar> ChangePlayerFaceWithAge(float age, string actionName = "act_childhood_schooled")
        {
            List<FaceGenChar> list = new List<FaceGenChar>();
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, age);
            list.Add(new FaceGenChar(bodyProperties, CharacterObject.PlayerCharacter.Race, new Equipment(), CharacterObject.PlayerCharacter.IsFemale, actionName));
            return list;
        }
        new public Equipment ChangePlayerOutfit(CharacterCreation characterCreation, string outfit)
        {
            List<Equipment> list = new List<Equipment>();
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(outfit);
            Equipment equipment = @object?.DefaultEquipment;
            if (equipment == null)
            {
                Debug.FailedAssert("item shouldn't be null!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\SandboxCharacterCreationContent.cs", "ChangePlayerOutfit", 1048);
                equipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default").DefaultEquipment;
            }
            list.Add(equipment);
            characterCreation.ChangeCharactersEquipment(list);
            return equipment;
        }
        new public static void ChangePlayerMount(CharacterCreation characterCreation, Hero hero)
        {
            if (hero.CharacterObject.HasMount())
            {
                FaceGenMount faceGenMount = new FaceGenMount(MountCreationKey.GetRandomMountKey(hero.CharacterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, hero.CharacterObject.GetMountKeySeed()), hero.CharacterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, hero.CharacterObject.Equipment[EquipmentIndex.HorseHarness].Item, "act_horse_stand_1");
                characterCreation.SetFaceGenMount(faceGenMount);
            }
        }
        new public static void ClearMountEntity(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenMounts();
        }
        public void EducationMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Early Life and Education", null), new TextObject("{=!}You started to learn about the world almost as soon as you could walk and talk. You spent your early life as...", null), new CharacterCreationOnInit(this.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A page at a nobleman's court", null), new MBList<SkillObject> { DefaultSkills.Leadership, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 20, 2, null, new CharacterCreationOnSelect(this.PageOnConsequence), new CharacterCreationApplyFinalEffects(this.EducationOnApply), new TextObject("{=!}You were sent to live in the court of one of the nobles of the land. There, your first lessons were in humility, as you waited upon the lords and ladies of the household. But from their chess games, their gossip, even the poetry of great deeds and courtly love, you quickly began to learn about the adult world of conflict and competition. You also learned from the rough games of the other children, who battered at each other with sticks in imitation of their elders' swords", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A craftsman's apprentice", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, 1, 20, 2, null, new CharacterCreationOnSelect(this.ApprenticeOnConsequence), new CharacterCreationApplyFinalEffects(this.EducationOnApply), new TextObject("{=!}You apprenticed with a local craftsman to learn a trade. After years of hard work and study under your new master, he promoted you to journeyman and employed you as a fully paid craftsman for as long as you wished to stay.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A shop assistant", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, 1, 20, 2, null, new CharacterCreationOnSelect(this.AssistantOnConsequence), new CharacterCreationApplyFinalEffects(this.EducationOnApply), new TextObject("{=!}You apprenticed to a wealthy merchant, picking up the trade over years of working shops and driving caravans. You soon became adept at the art of buying low, selling high, and leaving the customer thinking they'd got the better deal.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A street urchin", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, 1, 20, 2, null, new CharacterCreationOnSelect(this.UrchinOnConsequence), new CharacterCreationApplyFinalEffects(this.EducationOnApply), new TextObject("{=!}You took to the streets doing whatever you must to survive. Begging, thieving and working for gangs to earn your bread, you lived from day to day in this violent world, always one step ahead of the law and those who wished you ill.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A steppe child", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Leadership }, DefaultCharacterAttributes.Social, 1, 20, 2, null, new CharacterCreationOnSelect(this.SteppeChildOnConsequence), new CharacterCreationApplyFinalEffects(this.EducationOnApply), new TextObject("{=!}You rode the great steppes on a horse of your own, learning the ways of the grass and the desert. Although you sometimes went hungry, you became a skillful hunter and pathfinder in this trackless country. Your body too started to harden with muscle as you grew into the life of a nomad (man/woman).", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new public void EducationOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(CharacterCreationRedone.ChangePlayerFaceWithAge((float)this.EducationAge, "act_childhood_schooled"));
            string text = string.Concat(new object[] { "player_char_creation_education_age_", base.GetSelectedCulture().StringId, "_", base.SelectedParentType });
            text += (Hero.MainHero.IsFemale ? "_f" : "_m");
            this.ChangePlayerOutfit(characterCreation, text);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            CharacterCreationRedone.ClearMountEntity(characterCreation);
        }
        new public void RefreshPropsAndClothing(CharacterCreation characterCreation, bool isChildhoodStage, string itemId, bool isLeftHand, string secondItemId = "")
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
        public void PageOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_streets" });
            this.RefreshPropsAndClothing(characterCreation, false, "carry_bostaff_rogue1", true, "");
        }
        public void ApprenticeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_militia" });
            this.RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_1_t1", true, "");
        }
        public void AssistantOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_grit" });
            this.RefreshPropsAndClothing(characterCreation, false, "carry_hammer", true, "");
        }
        public void UrchinOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_peddlers" });
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
        }
        public void SteppeChildOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_sharp" });
            this.RefreshPropsAndClothing(characterCreation, false, "composite_bow", true, "");
        }
        public void EducationOnApply(CharacterCreation characterCreation)
        {
        }
        public void AdulthoodMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Adulthood", null), new TextObject("{=!}Then, as a young adult, life changed as it always does. You became...", null), new CharacterCreationOnInit(this.YouthOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A squire", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 20, 2, new CharacterCreationOnCondition(this.IsMaleOnCondition), new CharacterCreationOnSelect(this.SquireOnConsequence), new CharacterCreationApplyFinalEffects(this.AdulthoodOnApply), new TextObject("{=!}When you were named squire to a noble at court, you practiced long hours with weapons, learning how to deal out hard knocks and how to take them, too. You were instructed in your obligations to your lord, and of your duties to those who might one day be your vassals. But in addition to learning the chivalric ideal, you also learned about the less uplifting side -- old warriors' stories of ruthless power politics, of betrayals and usurpations, of men who used guile as well as valor to achieve their aims.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A lady-in-waiting", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 20, 2, new CharacterCreationOnCondition(this.IsFemaleOnCondition), new CharacterCreationOnSelect(this.LadyOnConsequence), new CharacterCreationApplyFinalEffects(this.AdulthoodOnApply), new TextObject("{=!}You joined the tightly-knit circle of women at court, ladies who all did proper ladylike things, the wives and mistresses of noble men as well as maidens who had yet to find a husband. However, even here you found politics at work as the ladies schemed for prominence and fought each other bitterly to catch the eye of whatever unmarried man was in fashion at court. You soon learned ways of turning these situations and goings-on to your advantage. With it came the realisation that you yourself could wield great influence in the world, if only you applied yourself with a little bit of subtlety.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A troubadour", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 20, 2, null, new CharacterCreationOnSelect(this.TroubadourOnConsequence), new CharacterCreationApplyFinalEffects(this.AdulthoodOnApply), new TextObject("{=!}You set out on your own with nothing except the instrument slung over your back and your own voice. It was a poor existence, with many a hungry night when people failed to appreciate your play, but you managed to survive your music alone. As the years went by you became adept at playing the drunken crowds in your taverns, and even better at talking anyone out of anything you wanted.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A university student", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 20, 2, null, new CharacterCreationOnSelect(this.StudentOnConsequence), new CharacterCreationApplyFinalEffects(this.AdulthoodOnApply), new TextObject("{=!}You found yourself as a student in the university of one of the great cities, where you studied theology, philosophy, and medicine. But not all your lessons were learned in the lecture halls. You may or may not have joined in with your fellows as they roamed the alleys in search of wine, women, and a good fight. However, you certainly were able to observe how a broken jaw is set, or how an angry townsman can be persuaded to set down his club and accept cash compensation for the destruction of his shop.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A goods peddler", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 20, 2, null, new CharacterCreationOnSelect(this.PeddlerOnConsequence), new CharacterCreationApplyFinalEffects(this.AdulthoodOnApply), new TextObject("{=!}Heeding the call of the open road, you travelled from village to village buying and selling what you could. It was not a rich existence, but you became a master at haggling even the most miserly elders into giving you a good price. Soon, you knew, you would be well-placed to start your own trading empire...", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A smith", null), new MBList<SkillObject> { DefaultSkills.Crossbow, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 20, 2, null, new CharacterCreationOnSelect(this.SmithOnConsequence), new CharacterCreationApplyFinalEffects(this.AdulthoodOnApply), new TextObject("{=!}You pursued a career as a smith, crating items of function and beauty out of simple metal. As time wore on you became a master of your trade, and fine work started to fetch fine prices. With food in your belly and logs on your fire, you could take pride in your work and your growing reputation.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}A game poacher", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 20, 2, null, new CharacterCreationOnSelect(this.PoacherOnConsequence), new CharacterCreationApplyFinalEffects(this.AdulthoodOnApply), new TextObject("{=!}Dissatisfied with the common men's desperate scrabble for coin, you took to your local lord's own forests and decided to help yourself to its bounty, laws be damned. You hunted stags, boars and geese and sold the precious meat under the table. You cut down trees right under the watchmen's noses and turned them into firewood that warmed many freezing homes during winter. All for a few silvers, of course.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new public void YouthOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(CharacterCreationRedone.ChangePlayerFaceWithAge((float)this.YouthAge, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            if (base.SelectedTitleType < 1 || base.SelectedTitleType > 10)
            {
                base.SelectedTitleType = 1;
            }
            this.RefreshPlayerAppearance(characterCreation);
        }
        new public void RefreshPlayerAppearance(CharacterCreation characterCreation)
        {
            string text = string.Concat(new object[] { "player_char_creation_", base.GetSelectedCulture().StringId, "_", base.SelectedTitleType });
            text += (Hero.MainHero.IsFemale ? "_f" : "_m");
            this.ChangePlayerOutfit(characterCreation, text);
            this.ApplyEquipments(characterCreation);
        }
        public void AdulthoodOnApply(CharacterCreation characterCreation)
        {
        }
        public void SquireOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_decisive" });
        }
        public void LadyOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_sharp" });
        }
        public void TroubadourOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready" });
        }
        public void StudentOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_apprentice" });
        }
        public void PeddlerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_athlete" });
        }
        public void SmithOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
        }
        public void PoacherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_sharp" });
        }
        public void ReasonForAdventureMenu(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 20);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Reasons for Adventure", null), new TextObject("{=!}But soon everything changed and you decided to strike out on your own as an adventurer. What made you take this decision was...", null), new CharacterCreationOnInit(this.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}Personal revenge", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Vigor, 1, 20, 2, null, new CharacterCreationOnSelect(this.RevengeOnConsequence), new CharacterCreationApplyFinalEffects(this.ReasonOnApply), new TextObject("{=!}Still, it was not a difficult choice to leave, with the rage burning brightly in your heart. You want vengeance. You want justice. What was done to you cannot be undone, and these debts can only be paid in blood...", null), new MBList<TraitObject> { DefaultTraits.Valor }, 1, 20, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}The loss of a loved one", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 20, 2, null, new CharacterCreationOnSelect(this.LossOnConsequence), new CharacterCreationApplyFinalEffects(this.ReasonOnApply), new TextObject("{=!}All you can say is that you couldn't bear to stay, not with the memories of those you loved so close and so painful. Perhaps your new life will let you forget, or honour the name that you can no longer bear to speak...", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}Wanderlust", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 20, 2, null, new CharacterCreationOnSelect(this.WanderlustOnConsequence), new CharacterCreationApplyFinalEffects(this.ReasonOnApply), new TextObject("{=!}You're not even sure when your home became a prison, when the familiar become mundane, but your dreams of wandering have taken over your life. Whether you yearn for some faraway place or merely for the open road and the freedom to travel, you could no longer bear to stay in the same place. You simply went and never looked back...", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}Being forced out of your home", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 20, 2, null, new CharacterCreationOnSelect(this.ForcedOutOnConsequence), new CharacterCreationApplyFinalEffects(this.ReasonOnApply), new TextObject("{=!}However, you know you cannot go back. There's nothing to go back to. Whatever home you may have had is gone now, and you must face the fact that you're out in the wide wide world. Alone to sink or swim...", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}Lust for money and power", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 20, 2, null, new CharacterCreationOnSelect(this.LustOnConsequence), new CharacterCreationApplyFinalEffects(this.ReasonOnApply), new TextObject("{=!}To everyone else, it's clear that you're now motivated solely by personal gain. You want to be rich, powerful, respected, feared. You want to be the one whom others hurry to obey. You want people to know your name, and tremble whenever it is spoken. You want everything, and you won't let anyone stop you from having it...", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new public void AccomplishmentOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(CharacterCreationRedone.ChangePlayerFaceWithAge((float)this.AccomplishmentAge, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            this.RefreshPlayerAppearance(characterCreation);
        }
        public void ReasonOnApply(CharacterCreation characterCreation)
        {
        }
        public void RevengeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_athlete" });
        }
        public void LossOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_gracious" });
        }
        public void WanderlustOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready" });
        }
        public void ForcedOutOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
        }
        public void LustOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
        }
        public void AgeSelectionMenu(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=HDFEAYDk}Starting Age", null), new TextObject("{=VlOGrGSn}Your character started off on the adventuring path at the age of...", null), new CharacterCreationOnInit(this.StartingAgeOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=!}21", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeYoungOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeYoungOnApply), new TextObject("{=2k7adlh7}While lacking experience a bit, you are full with youthful energy, you are fully eager, for the long years of adventuring ahead.", null), null, 0, 0, 0, 2, 1);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}30", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeAdultOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeAdultOnApply), new TextObject("{=NUlVFRtK}You are at your prime, You still have some youthful energy but also have a substantial amount of experience under your belt. ", null), null, 0, 0, 0, 4, 2);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}40", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeMiddleAgedOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeMiddleAgedOnApply), new TextObject("{=5MxTYApM}This is the right age for starting off, you have years of experience, and you are old enough for people to respect you and gather under your banner.", null), null, 0, 0, 0, 6, 3);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}50", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeElderlyOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeElderlyOnApply), new TextObject("{=ePD5Afvy}While you are past your prime, there is still enough time to go on that last big adventure for you. And you have all the experience you need to overcome anything!", null), null, 0, 0, 0, 8, 4);
            characterCreation.AddNewMenu(characterCreationMenu);
        }


        new public void StartingAgeYoungOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)SandboxAgeOptions.YoungAdult, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_focus" });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.YoungAdult;
            this.SetHeroAge(21f);
        }
        new public void StartingAgeYoungOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.YoungAdult;
        }


        new public void StartingAgeAdultOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)SandboxAgeOptions.Adult, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready" });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Adult;
            this.SetHeroAge(30f);
        }
        new public void StartingAgeAdultOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Adult;
        }


        new public void StartingAgeMiddleAgedOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)SandboxAgeOptions.MiddleAged, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_sharp" });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.MiddleAged;
            this.SetHeroAge(40f);
        }
        new public void StartingAgeMiddleAgedOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.MiddleAged;
        }


        new public void StartingAgeElderlyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)SandboxAgeOptions.Elder, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_tough" });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Elder;
            this.SetHeroAge(50f);
        }
        new public void StartingAgeElderlyOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Elder;
        }

        new public enum SandboxAgeOptions
        {
            YoungAdult = 21,
            Adult = 30,
            MiddleAged = 40,
            Elder = 50
        }
    }
}