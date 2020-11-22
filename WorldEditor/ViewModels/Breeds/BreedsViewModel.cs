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
    public class BreedsViewModel : CollectionViewModel<BreedViewModel>
    {
        protected override void RefreshItems()
        {
            var breeds = World.Instance.Breeds.Values.Select(breed => new BreedViewModel(breed));
            Items = new ObservableCollection<BreedViewModel>(breeds);
        }

        protected override void ExecuteAddItem()
        {
            var breedViewModel = new BreedViewModel(null);
            var window = new BreedEditWindow()
            {
                DataContext = breedViewModel
            };
            if(window.ShowDialog() == true)
            {
                Items.Add(breedViewModel);
            }
        }

        protected override void ExecuteEditItem()
        {
            new BreedEditWindow()
            {
                DataContext = new BreedViewModel(SelectedItem.Thing)
            }.ShowDialog();
            RefreshItems();
            RaisePropertyChanged("Items");
        }

        protected override void ExecuteRemoveItem()
        {
            if (MessageBox.Show($"Delete {SelectedItem.Id}?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                World.Instance.Breeds.Remove(SelectedItem.Id);
                Items.Remove(SelectedItem);
                SelectedItem = null;
            }
        }
    }
}
