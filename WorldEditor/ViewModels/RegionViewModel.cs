using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WorldEditor.ViewModels
{
    public class RegionViewModel : ViewModelBase
    {
        public Region Region { get; private set; }
        public RelayCommand<Window> SaveRegion { get; }

        public RegionViewModel(Region region)
        {
            if (region != null)
            {
                Region = region;
                Id = region.Id;
                Width = region.Width;
                Height = region.Height;
                TileWidth = region.TileWidth;
                TileHeight = region.TileHeight;
            }
            SaveRegion = new RelayCommand<Window>(ExecuteSaveRegionCommand);
        }

        private void ResizeTiles()
        {
            var newTiles = new Tile[Width][];
            for (int i = 0; i < Width; i++)
            {
                if (i >= Region.Width)
                    break;
                newTiles[i] = new Tile[Height];
                for (int j = 0; j < Height; j++)
                {
                    if (j >= Region.Height)
                        break;
                    newTiles[i][j] = Region.Tiles[i][j];
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
                    Region.instances.RemoveAt(i);
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
                        Tiles = new Tile[Width][]
                    };
                    for (int i = 0; i < Width; i++)
                    {
                        regionToAdd.Tiles[i] = new Tile[Height];
                    }
                    World.Instance.Regions.Add(id, regionToAdd);
                    window.DialogResult = true;
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
                    if (Region.Width != Width || Region.Height != Height)
                    {
                        ResizeTiles();
                        RemoveExcessInstances();
                    }

                    Region.Id = Id;
                    Region.Width = Width;
                    Region.Height = Height;
                    Region.TileWidth = TileWidth;
                    Region.TileHeight = TileHeight;
                    World.Instance.Regions.Add(id, Region);
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
    }
}
