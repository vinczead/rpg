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
using System.Windows.Controls;
using System.Windows.Input;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public class RegionEditorViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; set; }
        public Region Region { get; private set; }
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }
        public ObservableCollection<TextureViewModel> Textures { get; set; }
        public RelayCommand<Vector2> TileClicked { get; }
        public RelayCommand<RegionInstanceViewModel> InstanceClicked { get; }
        public RelayCommand<Canvas> CreateInstance { get; }

        public RegionEditorViewModel(MainViewModel mainViewModel, Region region)
        {
            MainViewModel = mainViewModel;
            Region = region ?? throw new ArgumentNullException("region");
            RefreshItems();
            TileClicked = new RelayCommand<Vector2>(ExecuteTileClicked);
            InstanceClicked = new RelayCommand<RegionInstanceViewModel>(ExecuteInstanceClicked);
            CreateInstance = new RelayCommand<Canvas>(ExecuteCreateInstance);
        }

        private void ExecuteInstanceClicked(RegionInstanceViewModel regionInstanceViewModel)
        {
            new InstanceEditWindow()
            {
                DataContext = regionInstanceViewModel
            }.ShowDialog();
            RefreshInstances();
        }

        private void ExecuteTileClicked(Vector2 position)
        {
            switch (MainViewModel.Sidebar.SelectedTool)
            {
                case Utility.ToolType.SelectionTool:
                    break;
                case Utility.ToolType.ObjectTool:
                    break;
                case Utility.ToolType.EraserTool:
                    break;
                case Utility.ToolType.TileTool:
                    ExecuteSetTileType(position);
                    break;
                default:
                    break;
            }
        }

        private void ExecuteCreateInstance(Canvas canvas)
        {
            var position = Mouse.GetPosition(canvas);
            var selectedBreed = MainViewModel?.Sidebar?.SelectedBreed;
            if (selectedBreed == null)
            {
                MessageBox.Show("Please select a Breed!");
                return;
            }

            var centerX = (int)position.X;
            var centerY = (int)position.Y;
            var brushSize = MainViewModel.Sidebar.BrushSize;
            var radius = brushSize / 2;

            for (int i = -radius; i <= radius; i++)
            {
                var x = centerX + i * Region.TileWidth;
                if (x < 0 || x >= Region.Width * Region.TileWidth)
                    continue;
                for (int j = -radius; j <= radius; j++)
                {
                    var y = centerY + j * Region.TileHeight;
                    if (y < 0 || y >= Region.Height * Region.TileHeight)
                        continue;

                    var instance = World.Instance.Spawn(selectedBreed.Id, Region.Id, new Microsoft.Xna.Framework.Vector2(x, y));
                    Instances.Add(new RegionInstanceViewModel(instance));
                }
            }
        }

        private void ExecuteSetTileType(Vector2 position)
        {
            var selectedTile = MainViewModel?.Sidebar?.SelectedTile;
            if (selectedTile == null)
            {
                MessageBox.Show("Please select a Tile Type!");
                return;
            }
            var centerX = (int)position.X;
            var centerY = (int)position.Y;
            var brushSize = MainViewModel.Sidebar.BrushSize;
            var radius = brushSize / 2;

            for (int i = -radius; i <= radius; i++)
            {
                var x = centerX + i;
                if (x < 0 || x >= Region.Width)
                    continue;
                for (int j = -radius; j <= radius; j++)
                {
                    var y = centerY + j;
                    if (y < 0 || y >= Region.Height)
                        continue;

                    Region.Tiles[y][x] = MainViewModel.Sidebar.SelectedTile;
                    Tiles[y][x] = new RegionTileViewModel(Region.Tiles[y][x], Region, x, y, SpriteModels);
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
            for (int y = 0; y < Region.Height; y++)
            {
                Tiles.Add(new ObservableCollection<RegionTileViewModel>());
                for (int x = 0; x < Region.Width; x++)
                {
                    Tiles[y].Add(new RegionTileViewModel(Region.Tiles[y][x], Region, x, y, SpriteModels));
                }
            }
            RaisePropertyChanged("Tiles");
            RefreshInstances();
        }

        private void RefreshInstances()
        {
            var instances = World.Instance.Instances.Values.Select(instance => new RegionInstanceViewModel(instance));
            Instances = new ObservableCollection<RegionInstanceViewModel>(instances);
            RaisePropertyChanged("Instances");
        }

        private ObservableCollection<ObservableCollection<RegionTileViewModel>> tiles;
        public ObservableCollection<ObservableCollection<RegionTileViewModel>> Tiles
        {
            get => tiles;
            set => Set(ref tiles, value);
        }

        private ObservableCollection<RegionInstanceViewModel> instances;
        public ObservableCollection<RegionInstanceViewModel> Instances
        {
            get => instances;
            set => Set(ref instances, value);
        }
    }
}
