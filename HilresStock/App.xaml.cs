// <copyright file="App.xaml.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock
{
    using System;
    using HilresStock.Services;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;

    /// <summary>
    /// App class.
    /// </summary>
    public sealed partial class App : Application
    {
        private readonly Lazy<ActivationService> activationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            this.activationService = new Lazy<ActivationService>(this.CreateActivationService);
        }

        private ActivationService ActivationService
        {
            get { return this.activationService.Value; }
        }

        /// <inheritdoc/>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args != null && !args.PrelaunchActivated)
            {
                await this.ActivationService.ActivateAsync(args).ConfigureAwait(true);
            }
        }

        /// <inheritdoc/>
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await this.ActivationService.ActivateAsync(args).ConfigureAwait(true);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.ChartPage), new Lazy<UIElement>(this.CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
