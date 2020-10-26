using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WorldEditor.DataAccess;

namespace WorldEditor.ViewModels
{
    public class TilesViewModel : ItemsViewModel<TileViewModel>
    {
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }
        public ObservableCollection<TextureViewModel> Textures { get; set; }
        public TilesViewModel(WorldRepository worldRepository) : base(worldRepository)
        {
            CreateTextures();
            CreateSpriteModels();
            CreateTiles();
            worldRepository.Tiles.TileAdded += Tiles_TileAdded;
        }

        private void Tiles_TileAdded(object sender, Utility.EntityEventArgs<Common.Models.Tile> e)
        {
            var tileViewModel = new TileViewModel(e.Entity, SpriteModels);
            Items.Add(tileViewModel);
        }

        void CreateTextures()
        {
            var textures = WorldRepository.Textures.GetTextures().Select(texture => new TextureViewModel(texture)).ToList();

            Textures = new ObservableCollection<TextureViewModel>(textures);
        }

        void CreateSpriteModels()
        {
            var spriteModels = WorldRepository.SpriteModels.GetSpriteModels().Select(spriteModel => new SpriteModelViewModel(spriteModel, Textures)).ToList();

            SpriteModels = new ObservableCollection<SpriteModelViewModel>(spriteModels);
        }

        void CreateTiles()
        {
            var tiles = WorldRepository.Tiles.GetTiles().Select(tile => new TileViewModel(tile, SpriteModels)).ToList();

            Items = new ObservableCollection<TileViewModel>(tiles);
        }

        protected override void ExecuteAddItem()
        {
            WorldRepository.Tiles.AddNewTile();
        }

        protected override void ExecuteRemoveItem()
        {
            WorldRepository.Tiles.RemoveAt(SelectedIndex);
        }
    }
}
