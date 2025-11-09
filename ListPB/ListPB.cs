using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Text;
using Color = UnityEngine.Color;

[assembly: MelonInfo(typeof(ListPB.ListPB), "ListPB", "1.2.0", "joeyexists", null)]
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
            medalIndex = Mathf.Clamp(medalIndex, 0, 8);

            (Color startCol, Color endCol) = medalIndex switch
            {
                0 => (Settings.BronzeStartColEntry.Value, Settings.BronzeEndColEntry.Value),
                1 => (Settings.SilverStartColEntry.Value, Settings.SilverEndColEntry.Value),
                2 => (Settings.GoldStartColEntry.Value, Settings.GoldEndColEntry.Value),
                3 => (Settings.AceStartColEntry.Value, Settings.AceEndColEntry.Value),
                4 => (Settings.DevStartColEntry.Value, Settings.DevEndColEntry.Value),
                5 => (Settings.EmeraldStartColEntry.Value, Settings.EmeraldEndColEntry.Value),
                6 => (Settings.AmethystStartColEntry.Value, Settings.AmethystEndColEntry.Value),
                7 => (Settings.SapphireStartColEntry.Value, Settings.SapphireEndColEntry.Value),
                8 => (Settings.TopazStartColEntry.Value, Settings.TopazEndColEntry.Value),
                _ => (Settings.BronzeStartColEntry.Value, Settings.BronzeEndColEntry.Value)
            };

            if (Settings.AllowHueShiftEntry.Value)
            {
                startCol = NeonLite.Modules.CommunityMedals.AdjustedColor(startCol);
                endCol = NeonLite.Modules.CommunityMedals.AdjustedColor(endCol);
            }

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
    }
}