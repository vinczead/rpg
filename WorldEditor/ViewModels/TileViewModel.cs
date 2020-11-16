using Common.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WorldEditor.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public Tile Tile { get; private set; }
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }
        public TileViewModel(Tile tile, ObservableCollection<SpriteModelViewModel> spriteModels)
        {
            SpriteModels = spriteModels;
            if (tile != null)
            {
                Id = tile.Id;
                SpriteModel = SpriteModels.FirstOrDefault(spriteModel => spriteModel.Id == tile.Model.Id);
                IsWalkable = tile.IsWalkable;
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
