// <copyright file="NavigationViewHeaderBehavior.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Behaviors
{
    using System;
    using HilresStock.Services;
    using Microsoft.Xaml.Interactivity;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using WinUI = Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// Navigation View Header Behavior class.
    /// </summary>
    public class NavigationViewHeaderBehavior : Behavior<WinUI.NavigationView>
    {
        private static readonly DependencyProperty DefaultHeaderProperty = DependencyProperty.Register("DefaultHeader", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => current.UpdateHeader()));
        private static readonly DependencyProperty HeaderModeProperty = DependencyProperty.RegisterAttached("HeaderMode", typeof(bool), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(NavigationViewHeaderMode.Always, (d, e) => current.UpdateHeader()));
        private static readonly DependencyProperty HeaderContextProperty = DependencyProperty.RegisterAttached("HeaderContext", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => current.UpdateHeader()));
        private static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => current.UpdateHeaderTemplate()));

        private static NavigationViewHeaderBehavior current;
        private Page currentPage;

        /// <summary>
        /// Gets or sets default heade template.
        /// </summary>
        public DataTemplate DefaultHeaderTemplate { get; set; }

        /// <summary>
        /// Gets or sets default header.
        /// </summary>
        public object DefaultHeader
        {
            get { return this.GetValue(DefaultHeaderProperty); }
            set { this.SetValue(DefaultHeaderProperty, value); }
        }

        /// <summary>
        /// Navigation view header mode.
        /// </summary>
        /// <param name="item">page item.</param>
        /// <returns>Navigation View Header Mode.</returns>
        public static NavigationViewHeaderMode GetHeaderMode(Page item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return (NavigationViewHeaderMode)item.GetValue(HeaderModeProperty);
        }

        /// <summary>
        /// Set Header Mode.
        /// </summary>
        /// <param name="item">Page item.</param>
        /// <param name="value">Navigation View Header Mode.</param>
        public static void SetHeaderMode(Page item, NavigationViewHeaderMode value)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.SetValue(HeaderModeProperty, value);
        }

        /// <summary>
        /// Get Header Context.
        /// </summary>
        /// <param name="item">Page item.</param>
        /// <returns>Object.</returns>
        public static object GetHeaderContext(Page item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.GetValue(HeaderContextProperty);
        }

        /// <summary>
        /// Set Header Context.
        /// </summary>
        /// <param name="item">Page item.</param>
        /// <param name="value">Object.</param>
        public static void SetHeaderContext(Page item, object value)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.SetValue(HeaderContextProperty, value);
        }

        /// <summary>
        /// Get Header Template.
        /// </summary>
        /// <param name="item">Page item.</param>
        /// <returns>Data Template.</returns>
        public static DataTemplate GetHeaderTemplate(Page item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return (DataTemplate)item.GetValue(HeaderTemplateProperty);
        }

        /// <summary>
        /// Set Header Template.
        /// </summary>
        /// <param name="item">Page item.</param>
        /// <param name="value">Data Template.</param>
        public static void SetHeaderTemplate(Page item, DataTemplate value)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.SetValue(HeaderTemplateProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            current = this;
            NavigationService.Navigated += this.OnNavigated;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            NavigationService.Navigated -= this.OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var frame = sender as Frame;
            if (frame.Content is Page page)
            {
                this.currentPage = page;

                this.UpdateHeader();
                this.UpdateHeaderTemplate();
            }
        }

        private void UpdateHeader()
        {
            if (this.currentPage != null)
            {
                var headerMode = GetHeaderMode(this.currentPage);
                if (headerMode == NavigationViewHeaderMode.Never)
                {
                    this.AssociatedObject.Header = null;
                    this.AssociatedObject.AlwaysShowHeader = false;
                }
                else
                {
                    var headerFromPage = GetHeaderContext(this.currentPage);
                    if (headerFromPage != null)
                    {
                        this.AssociatedObject.Header = headerFromPage;
                    }
                    else
                    {
                        this.AssociatedObject.Header = this.DefaultHeader;
                    }

                    if (headerMode == NavigationViewHeaderMode.Always)
                    {
                        this.AssociatedObject.AlwaysShowHeader = true;
                    }
                    else
                    {
                        this.AssociatedObject.AlwaysShowHeader = false;
                    }
                }
            }
        }

        private void UpdateHeaderTemplate()
        {
            if (this.currentPage != null)
            {
                var headerTemplate = GetHeaderTemplate(this.currentPage);
                this.AssociatedObject.HeaderTemplate = headerTemplate ?? this.DefaultHeaderTemplate;
            }
        }
    }
}
