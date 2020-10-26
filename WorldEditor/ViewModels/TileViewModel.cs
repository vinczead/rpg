using Common.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WorldEditor.ViewModels
{
    public class TileViewModel: ViewModelBase
    {
        readonly Tile tile;
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }
        public TileViewModel(Tile tile, ObservableCollection<SpriteModelViewModel> spriteModels)
        {
            this.tile = tile ?? throw new ArgumentException("tile");
            SpriteModels = spriteModels;
        }

        public string Id
        {
            get => tile.Id;
            set
            {
                if (value == Id)
                    return;
                tile.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public SpriteModelViewModel SpriteModel
        {
            get => SpriteModels.FirstOrDefault(spriteModel => spriteModel.Id == tile.SpriteModelId);
            set
            {
                if (value == null)
                {
                    tile.SpriteModelId = null;
                    RaisePropertyChanged("SpriteModel");
                    return;
                }

                if (value.Id == tile.SpriteModelId)
                    return;

                tile.SpriteModelId = value.Id;
                RaisePropertyChanged("SpriteModel");
            }
        }

        public string SpriteModelId
        {
            get => tile.SpriteModelId;
            set
            {
                if (value == SpriteModelId)
                    return;
                tile.SpriteModelId = value;
                RaisePropertyChanged("SpriteModelId");
            }
        }

        public bool IsWalkable
        {
            get => tile.IsWalkable;
            set
            {
                if (value == IsWalkable)
                    return;
                tile.IsWalkable = value;
                RaisePropertyChanged("IsWalkable");
            }
        }

    }
}
