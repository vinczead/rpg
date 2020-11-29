using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.Utility;

namespace WorldEditor.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public Tile Tile { get; private set; }
        public TilesViewModel Tiles { get; set; }

        public RelayCommand<Window> SaveTileType { get; set; }

        public TileViewModel(Tile tile, TilesViewModel tilesViewModel)
        {
            Tiles = tilesViewModel;
            if (tile != null)
            {
                Tile = tile;
                Id = tile.Id;
                SpriteModel = Tiles.MainViewModel.SpriteModels.Items.FirstOrDefault(spriteModel => spriteModel.Id == tile.Model.Id);
                IsWalkable = tile.IsWalkable;
            }

            SaveTileType = new RelayCommand<Window>(ExecuteSaveSpriteModelCommand);
        }

        private void ExecuteSaveSpriteModelCommand(Window window)
        {
            try
            {
                if (Tile == null)
                {
                    if (World.Instance.Tiles.ContainsKey(Id))
                        throw new ArgumentException();
                    var tileToAdd = new Tile()
                    {
                        Id = Id,
                        Model = SpriteModel.SpriteModel,
                        IsWalkable = IsWalkable
                    };
                    World.Instance.Tiles.Add(Id, tileToAdd);
                    Tile = tileToAdd;
                    Tiles.Items.Add(this);
                    window.DialogResult = true;
                }
                else
                {
                    if (Tile.Id != Id && World.Instance.Tiles.ContainsKey(Id))
                        throw new ArgumentException();
                    World.Instance.Tiles.Remove(Tile.Id);
                    var originalViewModel = Tiles.Items.First(item => item.Id == Tile.Id);

                    Tile.Id = Id;
                    originalViewModel.Id = Id;

                    Tile.IsWalkable = IsWalkable;
                    originalViewModel.IsWalkable = IsWalkable;

                    Tile.Model = SpriteModel.SpriteModel;
                    originalViewModel.SpriteModel = SpriteModel;

                    originalViewModel.NotifyReferencesOfChange();

                    World.Instance.Tiles.Add(id, Tile);
                }
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to add Tile Type! A Tile Type with the same id already exists!");
            }
        }

        public void NotifyReferencesOfChange()
        {
            RaisePropertyChanged("SpriteModel");
            foreach (var region in Tiles.MainViewModel.Regions.Items)
            {
                foreach (var tileRow in region.Tiles)
                {
                    foreach (var tile in tileRow)
                    {
                        if (tile.Tile == this)
                            tile.RaisePropertiesChanged();
                    }
                }
            }
        }

        private string id;
        public string Id
        {
            get => id;
            set => Set(ref id, value);
        }

        private SpriteModelViewModel spriteModel;
        public SpriteModelViewModel SpriteModel
        {
            get => spriteModel;
            set => Set(ref spriteModel, value);
        }

        private bool isWalkable;
        public bool IsWalkable
        {
            get => isWalkable;
            set => Set(ref isWalkable, value);
        }
    }
}
