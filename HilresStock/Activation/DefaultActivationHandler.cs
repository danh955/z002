// <copyright file="DefaultActivationHandler.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Activation
{
    using System;
    using System.Threading.Tasks;
    using HilresStock.Services;
    using Windows.ApplicationModel.Activation;

    /// <summary>
    /// Default Activation Handler class.
    /// </summary>
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly Type navElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultActivationHandler"/> class.
        /// </summary>
        /// <param name="navElement">Navagation type element.</param>
        public DefaultActivationHandler(Type navElement)
        {
            this.navElement = navElement;
        }

        /// <summary>
        /// Handle Internal Async.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Task.</returns>
        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            object arguments = null;
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                arguments = launchArgs.Arguments;
            }

            NavigationService.Navigate(this.navElement, arguments);
            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Can Handle Internal.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>True if can handle internal.</returns>
        protected override bool CanHandleInternal(IActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null && this.navElement != null;
        }
    }
}
