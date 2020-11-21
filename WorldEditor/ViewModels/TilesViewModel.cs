﻿using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.DataAccess;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public class TilesViewModel : CollectionViewModel<TileViewModel>
    {
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }
        public ObservableCollection<TextureViewModel> Textures { get; set; }

        protected override void RefreshItems()
        {
            if(Textures == null)
            {
                var textures = World.Instance.Textures.Values.Select(texture => new TextureViewModel(texture)).ToList();

                Textures = new ObservableCollection<TextureViewModel>(textures);
            }

            if(SpriteModels == null)
            {
                var spriteModels = World.Instance.Models.Select(spriteModel => new SpriteModelViewModel(spriteModel.Value, Textures)).ToList();

                SpriteModels = new ObservableCollection<SpriteModelViewModel>(spriteModels);
            }

            var tiles = World.Instance.Tiles.Select(tile => new TileViewModel(tile.Value, SpriteModels)).ToList();

            Items = new ObservableCollection<TileViewModel>(tiles);
        }

        protected override void ExecuteAddItem()
        {
            var tileViewModel = new TileViewModel(null, SpriteModels);
            var window = new TileTypeEditWindow()
            {
                DataContext = tileViewModel
            };

            if(window.ShowDialog() == true)
            {
                Items.Add(tileViewModel);
            }
        }

        protected override void ExecuteEditItem()
        {
            new TileTypeEditWindow()
            {
                DataContext = new TileViewModel(SelectedItem.Tile, SpriteModels)
            }.ShowDialog();
            var a = World.Instance.Regions;
            RefreshItems();
            RaisePropertyChanged("Items");
        }

        protected override void ExecuteRemoveItem()
        {
            var references = World.Instance.Regions
                .Where(region => region.Value.Tiles.Cast<Tile>().Any(tile => tile == SelectedItem.Tile))
                .Select(region => region.Value.Id).ToList();

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
