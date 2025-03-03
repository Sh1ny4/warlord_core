using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;

namespace Warlord.patches
{
    [HarmonyPatch(typeof(SPInventoryVM), "IsSearchAvailable", MethodType.Getter)]
    class IsSearchAvailablePatch
    {
        [HarmonyPostfix]
        static void Postfix(ref bool __result)
        {
            //always enable the search bar in inventory
            __result = true;
        }
    }
}
