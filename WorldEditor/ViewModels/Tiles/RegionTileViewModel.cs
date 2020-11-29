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
        private RegionViewModel region;
        public RegionViewModel Region { get => region; set => Set(ref region, value); }

        public RegionTileViewModel(Tile tile, RegionViewModel regionViewModel, int x, int y)
        {
            Tile = tile;
            Position = new Vector2(x, y);
            Region = regionViewModel;

            if (tile!= null) {
                IsWalkable = tile.IsWalkable;
                var animation = tile.Model.Animations.Find(animation => animation.Id.Contains("IDLE")) ?? tile.Model.Animations[0];
                var source = animation.Frames?[0].Source ?? new Microsoft.Xna.Framework.Rectangle(0, 0, Region.TileWidth, Region.TileHeight);
                OffsetX = -source.X;
                OffsetY = -source.Y;
            }
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

        public int OffsetX { get; }
        public int OffsetY { get; }

    }
}
