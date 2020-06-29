using System;

using HilresStock.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HilresStock.Views
{
    public sealed partial class ChartDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(ChartDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public ChartDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ChartDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
