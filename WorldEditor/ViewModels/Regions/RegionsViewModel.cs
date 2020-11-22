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
        protected override void RefreshItems()
        {
            var regions = World.Instance.Regions.Values.Select(region => new RegionViewModel(region));
            Items = new ObservableCollection<RegionViewModel>(regions);
        }
        protected override void ExecuteAddItem()
        {
            var regionViewModel = new RegionViewModel(null);
            var window = new RegionEditWindow()
            {
                DataContext = regionViewModel
            };

            if(window.ShowDialog() == true)
            {
                Items.Add(regionViewModel);
            }
        }

        protected override void ExecuteEditItem()
        {
            new RegionEditWindow()
            {
                DataContext = new RegionViewModel(SelectedItem.Region)
            }.ShowDialog();
            RefreshItems();
            RaisePropertyChanged("Items");
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
