using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.Utility;

namespace WorldEditor.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel { get => mainViewModel; set => Set(ref mainViewModel, value); }
        public RelayCommand<ToolType> SetTool { get; }

        public SidebarViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            SelectedTool = ToolType.EntityTool;

            SetTool = new RelayCommand<ToolType>(tool => SelectedTool = tool, tool => MainViewModel.IsProjectOpen);
            BrushSize = 1;
        }

        private ToolType selectedTool;
        public ToolType SelectedTool
        {
            get => selectedTool;
            set => Set(ref selectedTool, value);
        }

        public int[] BrushSizes { get; } = { 1, 3, 5, 7, 9 };

        private int brushSize;

        public int BrushSize
        {
            get => brushSize;
            set => Set(ref brushSize, value);
        }

        private TileViewModel selectedTile;
        public TileViewModel SelectedTile
        {
            get => selectedTile;
            set => Set(ref selectedTile, value);
        }

        private BreedViewModel selectedBreed;
        public BreedViewModel SelectedBreed
        {
            get => selectedBreed;
            set => Set(ref selectedBreed, value);
        }
    }
}
