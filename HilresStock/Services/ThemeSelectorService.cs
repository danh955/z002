// <copyright file="ThemeSelectorService.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Services
{
    using System;
    using System.Threading.Tasks;
    using HilresStock.Helpers;
    using Windows.ApplicationModel.Core;
    using Windows.Storage;
    using Windows.UI.Core;
    using Windows.UI.Xaml;

    /// <summary>
    /// Theme Selector Service.
    /// </summary>
    public static class ThemeSelectorService
    {
        private const string SettingsKey = "AppBackgroundRequestedTheme";

        /// <summary>
        /// Gets or sets theme.
        /// </summary>
        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        /// <summary>
        /// Initialize Async.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Set Theme Async.
        /// </summary>
        /// <param name="theme">Element Theme.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            await SetRequestedThemeAsync().ConfigureAwait(false);
            await SaveThemeInSettingsAsync(Theme).ConfigureAwait(false);
        }

        /// <summary>
        /// Set Requested Theme Async.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task SetRequestedThemeAsync()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = Theme;
                    }
                });
            }
        }

        private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(themeName))
            {
                _ = Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString()).ConfigureAwait(false);
        }
    }
}
