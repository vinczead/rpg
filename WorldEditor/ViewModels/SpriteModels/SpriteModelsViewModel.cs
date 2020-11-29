using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public class SpriteModelsViewModel : CollectionViewModel<SpriteModelViewModel>
    {
        public MainViewModel MainViewModel { get; set; }

        public SpriteModelsViewModel(MainViewModel mainViewModel) : base()
        {
            MainViewModel = mainViewModel;
            ReloadItems();
        }

        protected override void ReloadItems()
        {
            var spriteModels = World.Instance
                .Models
                .Select(spriteModel => new SpriteModelViewModel(spriteModel.Value, this))
                .ToList();

            Items = new ObservableCollection<SpriteModelViewModel>(spriteModels);
        }

        protected override void ExecuteAddItem()
        {
            new SpriteModelEditWindow()
            {
                DataContext = new SpriteModelViewModel(null, this)
            }.ShowDialog();
        }

        protected override void ExecuteEditItem()
        {
            new SpriteModelEditWindow()
            {
                DataContext = new SpriteModelViewModel(SelectedItem.SpriteModel, this)
            }.ShowDialog();
        }

        protected override void ExecuteRemoveItem()
        {
            var tileReferences = World.Instance.Tiles.Where(tile => tile.Value.Model.Id == SelectedItem.Id).Select(tile => tile.Value.Id).ToList();
            var breedReferences = World.Instance.Breeds.Where(breed => breed.Value.Model?.Id == SelectedItem.Id).Select(breed => breed.Value.Id).ToList();
            var references = tileReferences.Concat(breedReferences).ToList();


            if (references.Count > 0)
                MessageBox.Show($"{SelectedItem.Id} cannot be removed, because it is referenced {references.Count} times: {string.Join(',', references)}", "Error");
            else
            {
                if (MessageBox.Show($"Delete {SelectedItem.Id}?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    World.Instance.Models.Remove(SelectedItem.Id);
                    Items.Remove(SelectedItem);
                    SelectedItem = null;
                }
            }
        }
    }
}
