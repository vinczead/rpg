using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows;

namespace WorldEditor.ViewModels
{
    public class RegionEditorViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; set; }
        public Region Region { get; private set; }
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }
        public ObservableCollection<TextureViewModel> Textures { get; set; }
        public RelayCommand<Vector2> TileClicked { get; }

        public RegionEditorViewModel(MainViewModel mainViewModel, Region region)
        {
            MainViewModel = mainViewModel;
            Region = region ?? throw new ArgumentNullException("region");
            RefreshItems();
            TileClicked = new RelayCommand<Vector2>(ExecuteTileClicked);
        }
        private void ExecuteTileClicked(Vector2 position)
        {
            var selectedTile = MainViewModel?.Sidebar?.SelectedTile;
            if(selectedTile == null)
            {
                MessageBox.Show("Please select a Tile Type!");
                return;
            }
            var centerX = (int)position.X;
            var centerY = (int)position.Y;
            var brushSize = MainViewModel.Sidebar.BrushSize;
            var radius = brushSize / 2;

            for (int i = -radius; i <= radius ; i++)
            {
                var x = centerX + i;
                if (x < 0 || x >= Region.Width)
                    continue;
                for (int j = -radius; j <= radius; j++)
                {
                    var y = centerY + j;
                    if (y < 0 || y >= Region.Height)
                        continue;
                    
                    Region.Tiles[x][y] = MainViewModel.Sidebar.SelectedTile;
                    Tiles[x][y] = new RegionTileViewModel(Region.Tiles[x][y], x, y, SpriteModels);
                }
            }

            RaisePropertyChanged("Tiles");
        }

        private void RefreshItems()
        {
            if (Textures == null)
            {
                var textures = World.Instance.Textures.Values.Select(texture => new TextureViewModel(texture)).ToList();

                Textures = new ObservableCollection<TextureViewModel>(textures);
            }

            if (SpriteModels == null)
            {
                var spriteModels = World.Instance.Models.Select(spriteModel => new SpriteModelViewModel(spriteModel.Value, Textures)).ToList();

                SpriteModels = new ObservableCollection<SpriteModelViewModel>(spriteModels);
            }

            Tiles = new ObservableCollection<ObservableCollection<RegionTileViewModel>>();
            for (int i = 0; i < Region.Width; i++)
            {
                Tiles.Add(new ObservableCollection<RegionTileViewModel>());
                for (int j = 0; j < Region.Height; j++)
                {
                    Tiles[i].Add(new RegionTileViewModel(Region.Tiles[i][j], i, j, SpriteModels));
                }
            }
            RaisePropertyChanged("Tiles");
        }

        private ObservableCollection<ObservableCollection<RegionTileViewModel>> tiles;

        public ObservableCollection<ObservableCollection<RegionTileViewModel>> Tiles
        {
            get => tiles;
            set => Set(ref tiles, value);
        }
    }
}
