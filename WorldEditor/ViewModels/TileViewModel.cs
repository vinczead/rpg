using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace WorldEditor.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public Tile Tile { get; private set; }
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }

        public RelayCommand<Window> SaveTileType { get; set; }

        public TileViewModel(Tile tile, ObservableCollection<SpriteModelViewModel> spriteModels)
        {
            SpriteModels = spriteModels;
            if (tile != null)
            {
                Tile = tile;
                Id = tile.Id;
                SpriteModel = SpriteModels.FirstOrDefault(spriteModel => spriteModel.Id == tile.Model.Id);
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
                    window.DialogResult = true;
                }
                else
                {
                    if (Tile.Id != Id && World.Instance.Tiles.ContainsKey(Id))
                        throw new ArgumentException();
                    World.Instance.Tiles.Remove(Tile.Id);
                    Tile.Id = Id;
                    Tile.IsWalkable = IsWalkable;
                    Tile.Model = SpriteModel.SpriteModel;
                    World.Instance.Tiles.Add(id, Tile);
                }
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to add Tile Type! A Tile Type with the same id already exists!");
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
