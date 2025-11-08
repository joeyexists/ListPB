using MelonLoader;
using UnityEngine;

namespace ListPB
{
    public static class Settings
    {
        public static MelonPreferences_Entry<string> SeparatorEntry { get; private set; }
        public static MelonPreferences_Entry<Color32> ColorEntry { get; private set; }
        public static MelonPreferences_Entry<int> SizeEntry { get; private set; }
        public static MelonPreferences_Entry<bool> TintedEntry { get; private set; }

        public static void Initialize(ListPB modInstance)
        {
            var category = MelonPreferences.CreateCategory(modInstance.Info.Name);

            SeparatorEntry = category.CreateEntry("Separator", "»");
            SizeEntry = category.CreateEntry("Size", 90,
                validator: new MelonLoader.Preferences.ValueRange<int>(60, 100));
            TintedEntry = category.CreateEntry("Color PB Based on Medal", true);
            ColorEntry = category.CreateEntry("Color", new Color32(112, 112, 112, 255));
        }
    }
}
