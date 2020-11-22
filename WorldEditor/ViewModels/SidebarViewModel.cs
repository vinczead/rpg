using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WorldEditor.Utility;

namespace WorldEditor.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; set; }
        public RelayCommand<ToolType> SetTool { get; }

        public SidebarViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            SetTool = new RelayCommand<ToolType>(tool => SelectedTool = tool, tool => MainViewModel.IsProjectOpen);
            BrushSize = 1;
            RefreshItems();
        }

        public void RefreshItems()
        {
            Tiles = new ObservableCollection<Tile>(World.Instance.Tiles.Values.ToList());
        }

        private Tile selectedTile;
        public Tile SelectedTile
        {
            get => selectedTile;
            set => Set(ref selectedTile, value);
        }

        public int[] BrushSizes { get; } = { 1, 3, 5, 7, 9 };

        private int brushSize;

        public int BrushSize
        {
            get => brushSize;
            set => Set(ref brushSize, value);
        }

        private ToolType selectedTool;
        public ToolType SelectedTool
        {
            get => selectedTool;
            set => Set(ref selectedTool, value);
        }

        private ObservableCollection<Tile> tiles;
        public ObservableCollection<Tile> Tiles
        {
            get => tiles;
            set => Set(ref tiles, value);
        }
    }
}
