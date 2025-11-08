using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Text;
using Color = UnityEngine.Color;

[assembly: MelonInfo(typeof(ListPB.ListPB), "ListPB", "1.1.0", "joeyexists", null)]
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
            (new Color32(0xA5, 0x65, 0x29, 255), new Color32(0xCD, 0x7F, 0x32, 255)), // bronze
            (new Color32(0x9E, 0x9E, 0x9E, 255), new Color32(0xD3, 0xD3, 0xD3, 255)), // silver
            (new Color32(0xDB, 0xB6, 0x00, 255), new Color32(0xF2, 0xC1, 0x00, 255)), // gold
            (new Color32(0x7F, 0xC9, 0xFF, 255), new Color32(0x7F, 0xE1, 0xFF, 255)), // ace
            (new Color32(0xCC, 0x10, 0x25, 255), new Color32(0xFD, 0x44, 0x65, 255)), // dev
            (new Color32(0x00, 0xD3, 0x74, 255), new Color32(0x72, 0xE5, 0xAE, 255)), // emerald
            (new Color32(0xB2, 0x00, 0xFF, 255), new Color32(0xD6, 0x7F, 0xFF, 255)), // amethyst
            (new Color32(0x00, 0x80, 0xFF, 255), new Color32(0x00, 0x26, 0xFF, 255)), // sapphire
            (new Color32(0xFF, 0xD8, 0x00, 255), new Color32(0xFF, 0x6A, 0x00, 255)), // topaz
        ];
    }
}