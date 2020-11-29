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
    public class RegionViewModel : ViewModelBase
    {
        public RegionsViewModel Regions { get; set; }
        public Region Region { get; private set; }

        public RelayCommand<Window> SaveRegion { get; }
        public RelayCommand<Vector2> TileClicked { get; }
        public RelayCommand<RegionInstanceViewModel> InstanceClicked { get; }
        public RelayCommand<Canvas> CreateInstance { get; }

        public RegionViewModel(Region region, RegionsViewModel regionsViewModel)
        {
            Regions = regionsViewModel;
            if (region != null)
            {
                Region = region;
                Id = region.Id;
                Width = region.Width;
                Height = region.Height;
                TileWidth = region.TileWidth;
                TileHeight = region.TileHeight;
                Tiles = new ObservableCollection<ObservableCollection<RegionTileViewModel>>();
                for (int y = 0; y < region.Height; y++)
                {
                    Tiles.Add(new ObservableCollection<RegionTileViewModel>());
                    for (int x = 0; x < region.Width; x++)
                    {
                        Tiles[y].Add(new RegionTileViewModel(region.Tiles[y][x], this, x, y));
                    }
                }
                var instances = Region.instances.Select(instance => new RegionInstanceViewModel(instance, Region));
                Instances = new ObservableCollection<RegionInstanceViewModel>(instances);
            }
            SaveRegion = new RelayCommand<Window>(ExecuteSaveRegionCommand);
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
            if (regionInstanceViewModel.PressedButton == RegionInstanceViewModel.InstanceWindowButton.Save)
            {
                Instances.Remove(regionInstanceViewModel);
                Instances.Add(new RegionInstanceViewModel(regionInstanceViewModel.ThingInstance, Region));  //todo: update instead of new
            }
            if (regionInstanceViewModel.PressedButton == RegionInstanceViewModel.InstanceWindowButton.Remove)
            {
                Instances.Remove(regionInstanceViewModel);
            }
        }

        private void ExecuteTileClicked(Vector2 position)
        {
            switch (Regions.MainViewModel.Sidebar.SelectedTool)
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
            var selectedBreed = Regions.MainViewModel.Sidebar?.SelectedBreed;
            if (selectedBreed == null)
            {
                MessageBox.Show("Please select a Breed!");
                return;
            }

            var centerX = (int)position.X;
            var centerY = (int)position.Y;
            var brushSize = Regions.MainViewModel.Sidebar.BrushSize;
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
                    Region.instances.Add(instance);
                    Instances.Add(new RegionInstanceViewModel(instance, Region));
                }
            }
        }

        private void ExecuteSetTileType(Vector2 position)
        {
            var selectedTile = Regions.MainViewModel.Sidebar?.SelectedTile;
            if (selectedTile == null)
            {
                MessageBox.Show("Please select a Tile Type!");
                return;
            }
            var centerX = (int)position.X;
            var centerY = (int)position.Y;
            var brushSize = Regions.MainViewModel.Sidebar.BrushSize;
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

                    Region.Tiles[y][x] = Regions.MainViewModel.Sidebar.SelectedTile.Tile;
                    Tiles[y][x] = new RegionTileViewModel(Region.Tiles[y][x], this, x, y);    //todo: should update instead
                }
            }

            RaisePropertyChanged("Tiles");
        }

        private void ResizeTiles()
        {
            var newTiles = new Tile[Height][];

            var widthDifference = Width - Region.Width;
            var absWidthDifference = Math.Abs(widthDifference);

            var heightDifference = Height - Region.Height;
            var absHeightDifference = Math.Abs(heightDifference);
            for (int i = 0; i < absHeightDifference; i++)
            {
                if (heightDifference > 0)
                {
                    Tiles.Add(new ObservableCollection<RegionTileViewModel>());
                    for (int x = 0; x < Width; x++)
                    {
                        Tiles[^1].Add(new RegionTileViewModel(null, this, x, Tiles.Count - 1));
                    }
                }
                else
                {
                    Tiles.RemoveAt(Tiles.Count - 1);
                }
            }

            for (int y = 0; y < Height; y++)
            {
                newTiles[y] = new Tile[Width];
                if (y >= Region.Height)
                    continue;
                for (int x = 0; x < Width; x++)
                {
                    if (x >= Region.Width)
                        break;
                    newTiles[y][x] = Region.Tiles[y][x];
                }
                for (int i = 0; i < absWidthDifference; i++)
                {
                    if (widthDifference > 0)
                    {
                        Tiles[y].Add(new RegionTileViewModel(null, this, Region.Width + i, y));
                    }
                    else
                    {
                        Tiles[y].RemoveAt(Tiles[y].Count - 1);
                    }
                }
            }
            Region.Tiles = newTiles;
        }

        private void RemoveExcessInstances()
        {
            for (int i = Region.instances.Count - 1; i >= 0; i--)
            {
                var instance = Region.instances[i];
                if (instance.Position.X > Width * TileWidth || instance.Position.Y > Height * TileHeight)
                {
                    Region.instances.RemoveAt(i);
                    Instances.RemoveAt(i);
                }
            }
        }

        private void ExecuteSaveRegionCommand(Window window)
        {
            try
            {
                if (Region == null)
                {
                    var regionToAdd = new Region()
                    {
                        Id = Id,
                        Width = Width,
                        Height = Height,
                        TileWidth = TileWidth,
                        TileHeight = TileHeight,
                        Tiles = new Tile[Height][]
                    };
                    for (int i = 0; i < Height; i++)
                    {
                        regionToAdd.Tiles[i] = new Tile[Width];
                    }
                    Region = regionToAdd;
                    Tiles = new ObservableCollection<ObservableCollection<RegionTileViewModel>>();
                    Instances = new ObservableCollection<RegionInstanceViewModel>();

                    World.Instance.Regions.Add(id, regionToAdd);
                    Regions.Items.Add(this);
                }
                else
                {
                    if (Region.Id != Id && World.Instance.Regions.ContainsKey(Id))
                        throw new ArgumentException();
                    if (Region.Height > Height || Region.Width > Width)
                    {
                        if (MessageBox.Show("You are about to resize the Region. Excess Tiles and Objects will be removed.", "Warning", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                    }
                    World.Instance.Regions.Remove(Region.Id);
                    var originalViewModel = Regions.Items.First(item => item.Id == Region.Id);

                    originalViewModel.Id = Id;
                    originalViewModel.Width = Width;
                    originalViewModel.Height = Height;
                    originalViewModel.TileWidth = TileWidth;
                    originalViewModel.TileHeight = TileHeight;

                    if (Region.Width != Width || Region.Height != Height)
                    {
                        originalViewModel.ResizeTiles();
                        originalViewModel.RemoveExcessInstances();
                    }

                    Region.Id = Id;
                    Region.Width = Width;
                    Region.Height = Height;
                    Region.TileWidth = TileWidth;
                    Region.TileHeight = TileHeight;

                    World.Instance.Regions.Add(id, Region);
                    originalViewModel.RaisePropertyChanged("Tiles");
                    //todo: raisepropchanged regions.items ?
                }
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to save Region!");
            }
        }

        private string id;
        public string Id
        {
            get => id;
            set => Set(ref id, value);
        }

        private int width;
        public int Width
        {
            get => width;
            set => Set(ref width, value);
        }

        private int height;
        public int Height
        {
            get => height;
            set => Set(ref height, value);
        }

        private int tileWidth;
        public int TileWidth
        {
            get => tileWidth;
            set => Set(ref tileWidth, value);
        }

        private int tileHeight;
        public int TileHeight
        {
            get => tileHeight;
            set => Set(ref tileHeight, value);
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
