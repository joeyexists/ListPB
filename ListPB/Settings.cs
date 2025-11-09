using MelonLoader;
using UnityEngine;

namespace ListPB
{
    public static class Settings
    {
        public static MelonPreferences_Entry<string> SeparatorEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> ColorEntry { get; private set; }
        public static MelonPreferences_Entry<int> SizeEntry { get; private set; }
        public static MelonPreferences_Entry<bool> ColorMedalEntry { get; private set; }

        public static MelonPreferences_Entry<Color32> BronzeStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> BronzeEndColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> SilverStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> SilverEndColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> GoldStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> GoldEndColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> AceStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> AceEndColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> DevStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> DevEndColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> EmeraldStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> EmeraldEndColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> AmethystStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> AmethystEndColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> SapphireStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> SapphireEndColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> TopazStartColEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> TopazEndColEntry { get; private set; }

        public static MelonPreferences_Entry<bool> ApplyHueShiftEntry { get; private set; }

        public static void Initialize(ListPB modInstance)
        {
            var category = MelonPreferences.CreateCategory(modInstance.Info.Name);

            SeparatorEntry = category.CreateEntry("Separator", "»");
            SizeEntry = category.CreateEntry("Size", 90,
                validator: new MelonLoader.Preferences.ValueRange<int>(60, 100));
            ColorMedalEntry = category.CreateEntry("Color PB Based on Medal", true);
            ColorEntry = category.CreateEntry("Color", new Color32(112, 112, 112, 255));

            BronzeStartColEntry = category.CreateEntry("Bronze Gradient Start Color",
                new Color32(209, 113, 0, 255), is_hidden: true);
            BronzeEndColEntry = category.CreateEntry("Bronze Gradient End Color",
                new Color32(160, 80, 29, 255), is_hidden: true);
            SilverStartColEntry = category.CreateEntry("Silver Gradient Start Color",
                new Color32(207, 209, 199, 255), is_hidden: true);
            SilverEndColEntry = category.CreateEntry("Silver Gradient End Color",
                new Color32(164, 172, 146, 255), is_hidden: true);
            GoldStartColEntry = category.CreateEntry("Gold Gradient Start Color",
                new Color32(242, 193, 0, 255), is_hidden: true);
            GoldEndColEntry = category.CreateEntry("Gold Gradient End Color",
                new Color32(224, 183, 0, 255), is_hidden: true);
            AceStartColEntry = category.CreateEntry("Ace Gradient Start Color",
                new Color32(166, 231, 224, 255), is_hidden: true);
            AceEndColEntry = category.CreateEntry("Ace Gradient End Color",
                new Color32(84, 205, 199, 255), is_hidden: true);
            DevStartColEntry = category.CreateEntry("Dev Gradient Start Color",
                new Color32(241, 103, 129, 255), is_hidden: true);
            DevEndColEntry = category.CreateEntry("Dev Gradient End Color",
                new Color32(241, 0, 12, 255), is_hidden: true);
            EmeraldStartColEntry = category.CreateEntry("Emerald Gradient Start Color",
                new Color32(114, 229, 174, 255), is_hidden: true);
            EmeraldEndColEntry = category.CreateEntry("Emerald Gradient End Color",
                new Color32(0, 224, 104, 255), is_hidden: true);
            AmethystStartColEntry = category.CreateEntry("Amethyst Gradient Start Color",
                new Color32(213, 136, 248, 255), is_hidden: true);
            AmethystEndColEntry = category.CreateEntry("Amethyst Gradient End Color",
                new Color32(185, 76, 244, 255), is_hidden: true);
            SapphireStartColEntry = category.CreateEntry("Sapphire Gradient Start Color",
                new Color32(0, 128, 255, 255), is_hidden: true);
            SapphireEndColEntry = category.CreateEntry("Sapphire Gradient End Color",
                new Color32(0, 38, 255, 255), is_hidden: true);
            TopazStartColEntry = category.CreateEntry("Topaz Gradient Start Color",
                new Color32(255, 216, 0, 255), is_hidden: true);
            TopazEndColEntry = category.CreateEntry("Topaz Gradient End Color",
                new Color32(255, 106, 0, 255), is_hidden: true);

            ApplyHueShiftEntry = category.CreateEntry("Apply Hue Shift", true, is_hidden: true,
                description: "If enabled, hue shift from NeonLite/Medals will apply to the text.");
        }
    }
}
