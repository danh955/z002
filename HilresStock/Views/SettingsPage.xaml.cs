// <copyright file="SettingsPage.xaml.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Views
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using HilresStock.Helpers;
    using HilresStock.Services;
    using Windows.ApplicationModel;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// Settings Page class.
    /// TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/settings-codebehind.md
    /// TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere.
    /// </summary>
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        private ElementTheme elementTheme = ThemeSelectorService.Theme;
        private string versionDescription;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets element Theme.
        /// </summary>
        public ElementTheme ElementTheme
        {
            get { return this.elementTheme; }

            set { this.Set(ref this.elementTheme, value); }
        }

        /// <summary>
        /// Gets or sets version Description.
        /// </summary>
        public string VersionDescription
        {
            get { return this.versionDescription; }

            set { this.Set(ref this.versionDescription, value); }
        }

        /// <inheritdoc/>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.InitializeAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Get Version Description.
        /// </summary>
        /// <returns>Version string.</returns>
        private static string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        /// <summary>
        /// Theme Changed Checked Async event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Routed event arguments.</param>
#pragma warning disable CA1822 // Mark members as static

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
#pragma warning restore CA1822 // Mark members as static
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param).ConfigureAwait(true);
            }
        }

        private async Task InitializeAsync()
        {
            this.VersionDescription = GetVersionDescription();
            await Task.CompletedTask.ConfigureAwait(true);
        }

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
