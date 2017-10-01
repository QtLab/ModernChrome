using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace ModernChrome
{
    /// <summary>
    ///     A class that allows for the detection and alteration of a theme.
    /// </summary>
    public static class ThemeManager
    {
        private static IList<Uri> _themeList;

        private static string CurrentTheme { get; set; }

        /// <summary>
        /// Gets all available theme dictionaries.
        /// </summary>
        public static IEnumerable<Uri> Themes
        {
            get
            {
                if (_themeList != null)
                    return _themeList;
                try
                {
                    // GetUriFromTheme(GetThemeFromUri(...)) ensures proper pack syntax.
                    // Please don't judge me...
                    _themeList = new ResourceDictionary {Source = GetUriFromTheme("Themes")}.MergedDictionaries
                        .Select(d => GetUriFromTheme(GetThemeFromUri(d.Source))).ToList();
                }
                catch
                {
                    // ignored
                }
                return _themeList;
            }
        }

        /// <summary>
        ///     Changes the theme of the specified application to the provided theme.
        /// </summary>
        /// <param name="app">Application to inject theme into.</param>
        /// <param name="newTheme">New theme to inject.</param>
        public static void ChangeTheme(Application app, string newTheme)
        {
            ResourceDictionary detectedTheme = null;
            // Try to detect the user-loaded theme during the first call.
            if (string.IsNullOrEmpty(CurrentTheme))
                foreach (var theme in Themes)
                {
                    var currentTheme = app.Resources.MergedDictionaries.Reverse().Where(x => x.Source != null)
                        .FirstOrDefault(d => string.Equals(d.Source.ToString(), theme.ToString(),
                            StringComparison.CurrentCultureIgnoreCase));
                    if (currentTheme == null) continue;
                    detectedTheme = currentTheme;
                    CurrentTheme = GetThemeFromUri(currentTheme.Source);
                    break;
                }
            if (CurrentTheme == newTheme) return;
            app.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = GetUriFromTheme(newTheme)
            });

            if (detectedTheme == null)
                detectedTheme = app.Resources.MergedDictionaries.Where(x => x.Source != null).FirstOrDefault(d =>
                    string.Equals(d.Source.ToString(), GetUriFromTheme(CurrentTheme).ToString(),
                        StringComparison.CurrentCultureIgnoreCase));
            app.Resources.MergedDictionaries.Remove(detectedTheme);
            CurrentTheme = newTheme;
            OnThemeChanged(newTheme);
        }

        private static string GetThemeFromUri(Uri uri)
        {
            var matches = Regex.Match(uri.ToString(),
                @"(?:pack://application:,,,/ModernChrome;component/Themes/)?(\w+)\.xaml");
            return matches.Success ? matches.Groups[1].Value : string.Empty;
        }

        private static Uri GetUriFromTheme(string theme) =>
            new Uri($"pack://application:,,,/ModernChrome;component/Themes/{theme}.xaml");

        /// <summary>
        /// This event is fired when the theme has been changed.
        /// </summary>
        public static event EventHandler<ThemeChangedEventArgs> ThemeChanged;

        private static void OnThemeChanged(string newTheme) =>
            ThemeChanged?.Invoke(Application.Current, new ThemeChangedEventArgs(newTheme));
    }
}