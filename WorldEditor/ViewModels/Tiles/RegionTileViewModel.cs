using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows;

namespace WorldEditor.ViewModels
{
    public class RegionTileViewModel : ViewModelBase
    {
        public Tile Tile { get; private set; }

        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }

        public RegionTileViewModel(Tile tile, Region region, int x, int y, ObservableCollection<SpriteModelViewModel> spriteModels)
        {
            Tile = tile ?? throw new ArgumentNullException("tile");
            SpriteModels = spriteModels;
            Position = new Vector2(x, y);
            SpriteModel = SpriteModels.FirstOrDefault(spriteModel => spriteModel.Id == tile.Model.Id);
            IsWalkable = tile.IsWalkable;

            TileWidth = region.TileWidth;
            TileHeight = region.TileHeight;

            var animation = tile.Model.Animations.Find(animation => animation.Id.Contains("IDLE")) ?? tile.Model.Animations[0];
            var source = animation.Frames?[0].Source ?? new Microsoft.Xna.Framework.Rectangle(0, 0, TileWidth, TileHeight);
            OffsetX = -source.X;
            OffsetY = -source.Y;
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

        public int TileWidth { get; }
        public int TileHeight { get; }
        public int OffsetX { get; }
        public int OffsetY { get; }

    }
}
