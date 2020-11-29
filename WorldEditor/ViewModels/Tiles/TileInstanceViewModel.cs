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
    public class TileInstanceViewModel : ViewModelBase
    {
        private RegionViewModel region;
        public RegionViewModel Region { get => region; set => Set(ref region, value); }

        public TileInstanceViewModel(TileViewModel tileViewModel, RegionViewModel regionViewModel, int x, int y)
        {
            Tile = tileViewModel;
            Position = new Vector2(x, y);
            Region = regionViewModel;
        }

        public void RaisePropertiesChanged()
        {
            RaisePropertyChanged("Tile");
            RaisePropertyChanged("IsWalkable");
            RaisePropertyChanged("OffsetX");
            RaisePropertyChanged("OffsetY");
        }

        private TileViewModel tile;
        public TileViewModel Tile
        {
            get => tile;
            set
            {
                Set(ref tile, value);
                RaisePropertyChanged("IsWalkable");
                RaisePropertyChanged("OffsetX");
                RaisePropertyChanged("OffsetY");
            }
        }

        public int OffsetX { get => -tile?.SpriteModel?.Items?[0]?.Items?[0]?.X ?? 0; }
        public int OffsetY { get => -tile?.SpriteModel?.Items?[0]?.Items?[0]?.Y ?? 0; }

        public bool IsWalkable
        {
            get => Tile?.IsWalkable ?? false;
        }

        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set => Set(ref position, value);
        }
    }
}
