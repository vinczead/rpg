using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;

namespace WorldEditor.ViewModels
{
    public class RegionTileViewModel : ViewModelBase
    {
        public Tile Tile { get; private set; }

        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }

        public RegionTileViewModel(Tile tile, int x, int y, ObservableCollection<SpriteModelViewModel> spriteModels)
        {
            Tile = tile ?? throw new ArgumentNullException("tile");
            SpriteModels = spriteModels;
            Position = new Vector2(x, y);
            SpriteModel = SpriteModels.FirstOrDefault(spriteModel => spriteModel.Id == tile.Model.Id);
            IsWalkable = tile.IsWalkable;
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

        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set => Set(ref position, value);
        }
    }
}
