// <copyright file="NavigationService.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Services
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Animation;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// Navigation Service class.
    /// </summary>
    public static class NavigationService
    {
        private static Frame frame;
        private static object lastParamUsed;

        /// <summary>
        /// Navigated event.
        /// </summary>
        public static event NavigatedEventHandler Navigated;

        /// <summary>
        /// Navigation has failed event.
        /// </summary>
        public static event NavigationFailedEventHandler NavigationFailed;

        /// <summary>
        /// Gets or sets frame.
        /// </summary>
        public static Frame Frame
        {
            get
            {
                if (frame == null)
                {
                    frame = Window.Current.Content as Frame;
                    RegisterFrameEvents();
                }

                return frame;
            }

            set
            {
                UnregisterFrameEvents();
                frame = value;
                RegisterFrameEvents();
            }
        }

        /// <summary>
        /// Gets a value indicating whether can go back.
        /// </summary>
        public static bool CanGoBack => Frame.CanGoBack;

        /// <summary>
        /// Gets a value indicating whether can go forward.
        /// </summary>
        public static bool CanGoForward => Frame.CanGoForward;

        /// <summary>
        /// Go back one frame.
        /// </summary>
        /// <returns>True if went back one frame.</returns>
        public static bool GoBack()
        {
            if (CanGoBack)
            {
                Frame.GoBack();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Go forward on frame.
        /// </summary>
        public static void GoForward() => Frame.GoForward();

        /// <summary>
        /// Navigate to a frame.
        /// </summary>
        /// <param name="pageType">Page type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="infoOverride">NavigationTransitionInfo.</param>
        /// <returns>True if successful.</returns>
        public static bool Navigate(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            // Don't open the same page multiple times
            if (Frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(lastParamUsed)))
            {
                var navigationResult = Frame.Navigate(pageType, parameter, infoOverride);
                if (navigationResult)
                {
                    lastParamUsed = parameter;
                }

                return navigationResult;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Navigate to a frame.
        /// </summary>
        /// <typeparam name="T">Page type.</typeparam>
        /// <param name="parameter">Parameter.</param>
        /// <param name="infoOverride">NavigationTransitionInfo.</param>
        /// <returns>True if successful.</returns>
        public static bool Navigate<T>(object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : Page
            => Navigate(typeof(T), parameter, infoOverride);

        /// <summary>
        /// Register Frame Events.
        /// </summary>
        private static void RegisterFrameEvents()
        {
            if (frame != null)
            {
                frame.Navigated += Frame_Navigated;
                frame.NavigationFailed += Frame_NavigationFailed;
            }
        }

        /// <summary>
        /// Unregister Frame Events.
        /// </summary>
        private static void UnregisterFrameEvents()
        {
            if (frame != null)
            {
                frame.Navigated -= Frame_Navigated;
                frame.NavigationFailed -= Frame_NavigationFailed;
            }
        }

        private static void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => NavigationFailed?.Invoke(sender, e);

        private static void Frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(sender, e);
    }
}
