// <copyright file="ChartDetailControl.xaml.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Views
{
    using HilresStock.Core.Models;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Chart Detail Control class.
    /// </summary>
    public sealed partial class ChartDetailControl : UserControl
    {
        /// <summary>
        /// Master Menu Item Property.
        /// </summary>
        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(ChartDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDetailControl"/> class.
        /// </summary>
        public ChartDetailControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets master Menu Item.
        /// </summary>
        public SampleOrder MasterMenuItem
        {
            get { return this.GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { this.SetValue(MasterMenuItemProperty, value); }
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ChartDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
