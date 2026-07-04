using System;
using System.Linq;
using System.Windows;
using AndroidSyncControl.Themes.Enums;

namespace AndroidSyncControl.Themes
{
    /// <summary>
    /// Swaps the active color palette (Dark/Light) at runtime and persists the choice.
    /// Only the color <see cref="ResourceDictionary"/> is swapped; control styles in
    /// Controls.xaml reference every brush via DynamicResource so they update live.
    /// </summary>
    internal static class ThemeManager
    {
        const string DarkUri = "pack://application:,,,/Themes/Colors.Dark.xaml";
        const string LightUri = "pack://application:,,,/Themes/Colors.Light.xaml";

        // Key that only exists inside the color dictionaries (not in Controls.xaml),
        // used to locate the swappable palette dictionary in the merged collection.
        const string PaletteMarkerKey = "Brush.Window.Background";

        public static AppTheme Current { get; private set; } = AppTheme.Dark;

        public static AppTheme Parse(string value)
            => string.Equals(value, nameof(AppTheme.Light), StringComparison.OrdinalIgnoreCase)
                ? AppTheme.Light
                : AppTheme.Dark;

        /// <summary>
        /// Applies a theme to the running application without persisting it.
        /// </summary>
        public static void Apply(AppTheme theme)
        {
            var app = Application.Current;
            if (app == null)
                return;

            var newDict = new ResourceDictionary
            {
                Source = new Uri(theme == AppTheme.Light ? LightUri : DarkUri, UriKind.Absolute)
            };

            var dicts = app.Resources.MergedDictionaries;
            var existing = dicts.FirstOrDefault(d => d.Contains(PaletteMarkerKey));
            if (existing != null)
                dicts[dicts.IndexOf(existing)] = newDict;
            else
                dicts.Insert(0, newDict);

            Current = theme;
        }

        /// <summary>
        /// Switches to the opposite theme and persists it to the settings file.
        /// </summary>
        public static AppTheme Toggle()
        {
            var next = Current == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
            Apply(next);
            Singleton.Setting.Setting.Theme = next.ToString();
            Singleton.Setting.Save();
            return next;
        }
    }
}
