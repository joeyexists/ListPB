using MelonLoader;
using HarmonyLib;
using System.Reflection;

[assembly: MelonInfo(typeof(ListPB.Core), "ListPB", "1.0.0", "joeyexists", null)]
[assembly: MelonGame("Little Flag Software, LLC", "Neon White")]

namespace ListPB
{
    public class Core : MelonMod
    {
        internal static Game GameInstance { get; private set; }
        internal static new HarmonyLib.Harmony HarmonyInstance { get; private set; }

        public override void OnLateInitializeMelon()
        {
            GameInstance = Singleton<Game>.Instance;
            HarmonyInstance = new HarmonyLib.Harmony("joeyexists.ListPB");
            PatchMenu();
        }

        private void PatchMenu()
        {
            MethodInfo MenuButtonLevel_SetLevelData_Original = 
                AccessTools.Method(typeof(MenuButtonLevel), nameof(MenuButtonLevel.SetLevelData));
            HarmonyMethod MenuButtonLevel_SetLevelData_Patch =
                new(typeof(Core), nameof(MenuButtonLevel_SetLevelData_Postfix));
            HarmonyInstance.Patch(MenuButtonLevel_SetLevelData_Original, 
                postfix: MenuButtonLevel_SetLevelData_Patch);
        }

        private static void MenuButtonLevel_SetLevelData_Postfix(MenuButtonLevel __instance, LevelData ld, int displayIndex)
        {
            LevelStats levelStats = GameInstance.GetGameData()?.GetLevelStats(ld.levelID);

            if (levelStats != null && levelStats.GetCompleted())
            {
                string bestTime = Game.GetTimerFormatted(levelStats.GetTimeBestMicroseconds());
                __instance._textLevelName_Localized.textMeshProUGUI.text += $" » {bestTime}";
            }
        }
    }
}