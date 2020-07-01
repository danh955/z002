// <copyright file="ShellPage.xaml.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Views
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using HilresStock.Helpers;
    using HilresStock.Services;
    using Windows.System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;
    using WinUI = Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// Shell page class.
    /// </summary>
    //// TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private readonly KeyboardAccelerator altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool isBackEnabled;
        private WinUI.NavigationViewItem selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellPage"/> class.
        /// </summary>
        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.Initialize();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the back button is Enabled.
        /// </summary>
        public bool IsBackEnabled
        {
            get { return this.isBackEnabled; }
            set { this.Set(ref this.isBackEnabled, value); }
        }

        /// <summary>
        /// Gets or sets selected.
        /// </summary>
        public WinUI.NavigationViewItem Selected
        {
            get { return this.selected; }
            set { this.Set(ref this.selected, value); }
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }

        /// <summary>
        /// Is Menu Item For Page Type.
        /// </summary>
        /// <param name="menuItem">WinUI.NavigationViewItem.</param>
        /// <param name="sourcePageType">Type.</param>
        /// <returns>True if menu item is the same as the source page.</returns>
        private static bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private void Initialize()
        {
            NavigationService.Frame = this.shellFrame;
            NavigationService.NavigationFailed += this.Frame_NavigationFailed;
            NavigationService.Navigated += this.Frame_Navigated;
            this.navigationView.BackRequested += this.OnBackRequested;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            this.KeyboardAccelerators.Add(this.altLeftKeyboardAccelerator);
            this.KeyboardAccelerators.Add(this.backKeyboardAccelerator);
            await Task.CompletedTask.ConfigureAwait(true);
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            this.IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                this.Selected = this.navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            this.Selected = this.navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        /// <summary>
        /// On Item Invoked event.
        /// </summary>
        /// <param name="sender">WinUI.NavigationView.</param>
        /// <param name="args">Command line arguments.</param>
        private void OnItemInvoked(WinUI.NavigationView sender, WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(SettingsPage));
                return;
            }

            var item = this.navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            NavigationService.Navigate(pageType);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
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
