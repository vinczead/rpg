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
    public class TilesViewModel : CollectionViewModel<TileViewModel>
    {
        public MainViewModel MainViewModel { get; set; }

        public TilesViewModel(MainViewModel mainViewModel) : base()
        {
            MainViewModel = mainViewModel;
            ReloadItems();
        }
        protected override void ReloadItems()
        {
            var tiles = World.Instance.Tiles.Select(tile => new TileViewModel(tile.Value, this)).ToList();

            Items = new ObservableCollection<TileViewModel>(tiles);
        }

        protected override void ExecuteAddItem()
        {
            new TileTypeEditWindow()
            {
                DataContext = new TileViewModel(null, this)
            }.ShowDialog();
        }

        protected override void ExecuteEditItem()
        {
            new TileTypeEditWindow()
            {
                DataContext = new TileViewModel(SelectedItem.Tile, this)
            }.ShowDialog();
        }

        protected override void ExecuteRemoveItem()
        {
            var references = World.Instance.Regions.Where(region => region.Value.Tiles.Count(tiles => tiles.Count(tile => tile == SelectedItem.Tile) > 0) > 0).ToList();

            if (references.Count > 0)
                MessageBox.Show($"{SelectedItem.Id} cannot be removed, because it is referenced in {references.Count} regions: {string.Join(',', references)}", "Error");
            else
            {
                if (MessageBox.Show($"Delete {SelectedItem.Id}?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    World.Instance.Tiles.Remove(SelectedItem.Id);
                    Items.Remove(SelectedItem);
                    SelectedItem = null;
                }
            }
        }
    }
}
