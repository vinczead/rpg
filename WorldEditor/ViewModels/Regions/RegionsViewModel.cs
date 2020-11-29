using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public class RegionsViewModel : CollectionViewModel<RegionViewModel>
    {
        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel { get => mainViewModel; set => Set(ref mainViewModel, value); }
        public RegionsViewModel(MainViewModel mainViewModel) : base()
        {
            MainViewModel = mainViewModel;
            ReloadItems();
        }

        protected override void ReloadItems()
        {
            var regions = World.Instance.Regions.Values.Select(region => new RegionViewModel(region, this));
            Items = new ObservableCollection<RegionViewModel>(regions);
            RaisePropertyChanged("Items");
        }
        protected override void ExecuteAddItem()
        {
            new RegionEditWindow()
            {
                DataContext = new RegionViewModel(null, this)
            }.ShowDialog();
        }

        protected override void ExecuteEditItem()
        {
            new RegionEditWindow()
            {
                DataContext = new RegionViewModel(SelectedItem.Region, this)
            }.ShowDialog();
        }

        protected override void ExecuteRemoveItem()
        {
            if (MessageBox.Show($"Delete {SelectedItem.Id}?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                World.Instance.Regions.Remove(SelectedItem.Id);
                foreach (var instance in SelectedItem.Region.instances)
                {
                    World.Instance.Instances.Remove(instance.Id);
                }
                Items.Remove(SelectedItem);
                SelectedItem = null;
            }
        }
    }
}
