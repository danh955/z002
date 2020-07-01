// <copyright file="NavHelper.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Helpers
{
    using System;
    using Microsoft.UI.Xaml.Controls;
    using Windows.UI.Xaml;

    // This helper class allows to specify the page that will be shown when you click on a NavigationViewItem
    //
    // Usage in xaml:
    // <winui:NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavHelper.NavigateTo="views:MainPage" />
    //
    // Usage in code:
    // NavHelper.SetNavigateTo(navigationViewItem, typeof(MainPage));

    /// <summary>
    /// Navagation helper class.
    /// </summary>
    public static class NavHelper
    {
        /// <summary>
        /// Navagation to property.
        /// </summary>
        public static readonly DependencyProperty NavigateToProperty = DependencyProperty.RegisterAttached("NavigateTo", typeof(Type), typeof(NavHelper), new PropertyMetadata(null));

        /// <summary>
        /// Get Navigate To.
        /// </summary>
        /// <param name="item">Navigation View Item.</param>
        /// <returns>Type.</returns>
        public static Type GetNavigateTo(NavigationViewItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return (Type)item.GetValue(NavigateToProperty);
        }

        /// <summary>
        /// Set Navigate To.
        /// </summary>
        /// <param name="item">Navigation View Item.</param>
        /// <param name="value">Type value to set.</param>
        public static void SetNavigateTo(NavigationViewItem item, Type value)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.SetValue(NavigateToProperty, value);
        }
    }
}
