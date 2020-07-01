// <copyright file="ChartPage.xaml.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Views
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using HilresStock.Core.Models;
    using HilresStock.Core.Services;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Chart Page class.
    /// </summary>
    public sealed partial class ChartPage : Page, INotifyPropertyChanged
    {
        private SampleOrder selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPage"/> class.
        /// </summary>
        public ChartPage()
        {
            this.InitializeComponent();
            this.Loaded += this.ChartPage_Loaded;
        }

        /// <summary>
        /// Property Changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets sample Items.
        /// </summary>
        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        /// <summary>
        /// Gets or sets selected.
        /// </summary>
        public SampleOrder Selected
        {
            get { return this.selected; }
            set { this.Set(ref this.selected, value); }
        }

        private async void ChartPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SampleItems.Clear();

            var data = await SampleDataService.GetMasterDetailDataAsync().ConfigureAwait(true);

            foreach (var item in data)
            {
                this.SampleItems.Add(item);
            }

            if (this.MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                this.Selected = this.SampleItems.FirstOrDefault();
            }
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
