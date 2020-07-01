// <copyright file="ActivationService.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HilresStock.Activation;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Activation service class.
    /// For more information on understanding and extending activation flow see
    /// https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/activation.md.
    /// </summary>
    internal class ActivationService
    {
        private readonly App app;
        private readonly Type defaultNavItem;
        private readonly Lazy<UIElement> shell;

        private object lastActivationArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationService"/> class.
        /// </summary>
        /// <param name="app">App.</param>
        /// <param name="defaultNavItem">Default navagation item type.</param>
        /// <param name="shell">Lazy UI element.</param>
        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            this.app = app;
            this.shell = shell;
            this.defaultNavItem = defaultNavItem;
        }

        /// <summary>
        /// Activation Async.
        /// </summary>
        /// <param name="activationArgs">Activation arguments.</param>
        /// <returns>Task.</returns>
        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize services that you need before app activation
                // take into account that the splash screen is shown while this code runs.
                await InitializeAsync().ConfigureAwait(true);

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current?.Content == null)
                {
                    // Create a Shell or Frame to act as the navigation context
                    Window.Current.Content = this.shell?.Value ?? new Frame();
                }
            }

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            await this.HandleActivationAsync(activationArgs).ConfigureAwait(true);
            this.lastActivationArgs = activationArgs;

            if (IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync().ConfigureAwait(true);
            }
        }

        private static async Task InitializeAsync()
        {
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(true);
        }

        private static async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync().ConfigureAwait(true);
        }

        private static IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield break;
        }

        private static bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = GetActivationHandlers().FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs).ConfigureAwait(true);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultActivationHandler(this.defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs).ConfigureAwait(true);
                }
            }
        }
    }
}
