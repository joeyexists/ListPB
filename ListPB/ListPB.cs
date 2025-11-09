using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Text;
using Color = UnityEngine.Color;

[assembly: MelonInfo(typeof(ListPB.ListPB), "ListPB", "1.1.1", "joeyexists", null)]
[assembly: MelonGame("Little Flag Software, LLC", "Neon White")]

namespace ListPB
{
    public class ListPB : MelonMod
    {
        internal static Game Game => Singleton<Game>.Instance;
        internal static new HarmonyLib.Harmony Harmony { get; private set; }

        public override void OnLateInitializeMelon()
        {
            Harmony = new HarmonyLib.Harmony($"{Info.Author}.{Info.Name}");
            Settings.Initialize(this);
            PatchMenu();
        }

        private void PatchMenu()
        {
            var target = AccessTools.Method(typeof(MenuButtonLevel), nameof(MenuButtonLevel.SetLevelData));
            var patch = new HarmonyMethod(typeof(ListPB), nameof(MenuButtonLevel_SetLevelData_Postfix));
            Harmony.Patch(target, postfix: patch);
        }

        private static void MenuButtonLevel_SetLevelData_Postfix(MenuButtonLevel __instance, LevelData ld, int displayIndex)
        {
            var levelStats = Game.GetGameData()?.GetLevelStats(ld.levelID);

            if (levelStats != null && levelStats.GetCompleted())
            {
                var bestMicro = levelStats.GetTimeBestMicroseconds();
                string timer = Game.GetTimerFormatted(bestMicro);

                var separator = Settings.SeparatorEntry.Value;
                var color = Settings.ColorEntry.Value;
                var size = Settings.SizeEntry.Value;
                var isTinted = Settings.TintedEntry.Value;

                var sb = new StringBuilder();
                
                if (!string.IsNullOrWhiteSpace(separator))
                    sb.Append(separator[0]).Append(' ');

                sb.Append($"<size={size}%>");

                if (isTinted)
                {
                    var tintedTime = CreateTintedTimeString(timer, ld.levelID, bestMicro);
                    if (!string.IsNullOrEmpty(tintedTime)) sb.Append(tintedTime);
                    else sb.Append(timer); // fallback to black
                }
                else
                {
                    sb.Append($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{timer}</color>");
                }
                    
                sb.Append("</size>");

                __instance._textLevelName_Localized.textMeshProUGUI.text += " " + sb.ToString();
            }
        }

        private static string CreateTintedTimeString(string timer, string levelId, long timeMicro)
        {
            if (string.IsNullOrEmpty(timer)) return string.Empty;

            if (!NeonLite.Modules.CommunityMedals.Ready ||
                !NeonLite.Modules.CommunityMedals.medalTimes.ContainsKey(levelId))
                return string.Empty;

            // thanks neonlite 🙂
            var medalIndex = NeonLite.Modules.CommunityMedals.GetMedalIndex(levelId, timeMicro);
            medalIndex = Mathf.Clamp(medalIndex, 0, MedalGradients.Length - 1);

            var (startCol, endCol) = MedalGradients[medalIndex];

            startCol = NeonLite.Modules.CommunityMedals.AdjustedColor(startCol);
            endCol = NeonLite.Modules.CommunityMedals.AdjustedColor(endCol);

            var len = timer.Length;
            StringBuilder sb = new(len * 23 + len);
            
            for (int i = 0; i < len; i++)
            {
                float t = len > 1 ? (float)i / (len - 1) : 0f;
                var c = Color.Lerp(startCol, endCol, t);
                Color32 c32 = c;
                string hex = $"{c32.r:X2}{c32.g:X2}{c32.b:X2}";
                sb.Append($"<color=#{hex}>{timer[i]}</color>");
            }

            return sb.ToString();
        }

        private static readonly (Color start, Color end)[] MedalGradients =
        [
            (new Color32(209, 113, 0, 255), new Color32(160, 80, 29, 255)), // bronze
            (new Color32(207, 209, 199, 255), new Color32(164, 172, 146, 255)), // silver
            (new Color32(242, 193, 0, 255), new Color32(224, 183, 0, 255)), // gold
            (new Color32(166, 231, 224, 255), new Color32(84, 205, 199, 255)), // ace
            (new Color32(241, 103, 129, 255), new Color32(241, 0, 12, 255)), // dev
            (new Color32(114, 229, 174, 255), new Color32(0, 224, 104, 255)), // emerald
            (new Color32(213, 136, 248, 255), new Color32(185, 76, 244, 255)), // amethyst
            (new Color32(0, 128, 255, 255), new Color32(0, 38, 255, 255)), // sapphire
            (new Color32(255, 216, 0, 255), new Color32(255, 106, 0, 255)), // topaz
        ];
    }
}