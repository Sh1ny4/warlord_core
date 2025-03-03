using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Warlord.patches
{
    [HarmonyPatch(typeof(GameStateManager), "CleanAndPushState")]
    class SkipIntroPatch
    {
        public static void Prefix(GameState gameState)
        {
            if (gameState is VideoPlaybackState videoPlaybackState && videoPlaybackState.VideoPath.Contains("campaign_intro"))
            {
                AccessTools.Property(typeof(VideoPlaybackState), "AudioPath").SetValue(videoPlaybackState, null);
            }
        }

        public static void Postfix(GameState gameState)
        {
            if (gameState is VideoPlaybackState videoPlaybackState && videoPlaybackState.VideoPath.Contains("campaign_intro"))
            {
                videoPlaybackState.OnVideoFinished();
            }
        }
    }
}
