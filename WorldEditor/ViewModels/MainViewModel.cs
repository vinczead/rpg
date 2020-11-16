using Common.Models;
using Common.Script.Visitors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.DataAccess;
using WorldEditor.Utility;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ToolType selectedTool;
        public ToolType SelectedTool
        {
            get => selectedTool;
            set => Set(ref selectedTool, value);
        }

        private WorldRepository worldRepository;
        public WorldRepository WorldRepository
        {
            get => worldRepository;
            set
            {
                Set(ref worldRepository, value);
            }
        }

        private bool isWorldRepositoryOpen;
        public bool IsWorldRepositoryOpen
        {
            get => isWorldRepositoryOpen;
            set
            {
                Set(ref isWorldRepositoryOpen, value);
                RaisePropertyChanged(() => Title);
                SaveProject.RaiseCanExecuteChanged();
                CloseProject.RaiseCanExecuteChanged();
                SetTool.RaiseCanExecuteChanged();
                OpenContents.RaiseCanExecuteChanged();
                OpenTextures.RaiseCanExecuteChanged();
                OpenSpriteModels.RaiseCanExecuteChanged();
                OpenTiles.RaiseCanExecuteChanged();
                OpenScripts.RaiseCanExecuteChanged();
                OpenMaps.RaiseCanExecuteChanged();
            }
        }

        public string Title { get => "World Editor" + (IsWorldRepositoryOpen ? $" - {World.Instance.FileName}" : ""); }

        private ObservableCollection<MapViewModel> maps;

        public ObservableCollection<MapViewModel> Maps
        {
            get => maps;
            set => Set(ref maps, value);
        }

        public MainViewModel()
        {
            SetTool = new RelayCommand<ToolType>(tool => SelectedTool = tool, tool => IsWorldRepositoryOpen);
            Close = new RelayCommand<Window>(ExecuteCloseWindow);
            NewProject = new RelayCommand(ExecuteNewProjectCommand);
            OpenProject = new RelayCommand(ExecuteOpenProjectCommand);
            SaveProject = new RelayCommand(ExecuteSaveProjectCommand, () => IsWorldRepositoryOpen);
            CloseProject = new RelayCommand(ExecuteCloseProjectCommand, () => IsWorldRepositoryOpen);

            OpenContents = new RelayCommand(ExecuteOpenContentsWindowCommand, () => IsWorldRepositoryOpen);
            OpenTextures = new RelayCommand(ExecuteOpenTexturesWindowCommand, () => IsWorldRepositoryOpen);
            OpenSpriteModels = new RelayCommand(ExectueOpenSpriteModelsWindowCommand, () => IsWorldRepositoryOpen);
            OpenTiles = new RelayCommand(ExectueOpenTilesWindowCommand, () => IsWorldRepositoryOpen);
            OpenScripts = new RelayCommand(ExectueOpenScriptsWindowCommand, () => IsWorldRepositoryOpen);
            OpenMaps = new RelayCommand(ExectueOpenMapsWindowCommand, () => IsWorldRepositoryOpen);
        }

        public RelayCommand<ToolType> SetTool { get; }
        public RelayCommand<Window> Close { get; }
        public RelayCommand NewProject { get; }
        public RelayCommand OpenProject { get; }
        public RelayCommand SaveProject { get; }
        public RelayCommand CloseProject { get; }

        public RelayCommand OpenContents { get; }
        public RelayCommand OpenTextures { get; }
        public RelayCommand OpenSpriteModels { get; }
        public RelayCommand OpenTiles { get; }
        public RelayCommand OpenScripts { get; }
        public RelayCommand OpenMaps { get; }

        private void CreateMaps()
        {
            var maps = World.Instance.Regions.Select(map => new MapViewModel(map.Value)).ToList();

            Maps = new ObservableCollection<MapViewModel>(maps);
        }

        private void ExecuteCloseWindow(Window window)
        {
            //todo: ask to save project before closing

            if (window != null)
                window.Close();
        }

        private void ExecuteOpenProjectCommand()
        {
            if (IsWorldRepositoryOpen)
                CloseProject.Execute(null);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vincze Game Script files|*.vgs";
            if (openFileDialog.ShowDialog() == true)
            {
                ExecutionVisitor.BuildWorldFromFile(openFileDialog.FileName, out var allerrors);  //todo: error handling
                var a = World.Instance;
                IsWorldRepositoryOpen = true;
                CreateMaps();
            }
        }

        public void ExecuteSaveProjectCommand()
        {
            if (!IsWorldRepositoryOpen)
                return;

            File.WriteAllText(World.Instance.FileName, World.Instance.Serialize());
            MessageBox.Show("Project saved.");
        }

        private void ExecuteCloseProjectCommand()
        {
            var messageBoxResult = MessageBox.Show("Do you want to save project before closing?", "Close Project", MessageBoxButton.YesNoCancel);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SaveProject.Execute(null);
            }
            if (messageBoxResult == MessageBoxResult.Cancel)
            {
                return;
            }
            IsWorldRepositoryOpen = false;
        }

        private void ExecuteNewProjectCommand()
        {
            if (IsWorldRepositoryOpen)
                CloseProject.Execute(null);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "NewWorld";
            saveFileDialog.Filter = "Vincze Game Script files|*.vgs";
            if (saveFileDialog.ShowDialog() == true)
            {
                using var file = File.Create(saveFileDialog.FileName);
                World.Instance.FolderPath = Path.GetDirectoryName(saveFileDialog.FileName);
                World.Instance.FileName = saveFileDialog.FileName;
                file.Close();
                CreateMaps();
                IsWorldRepositoryOpen = true;
            }
        }

        private void ExecuteOpenContentsWindowCommand()
        {
            new ContentsWindow().ShowDialog();
        }

        private void ExecuteOpenTexturesWindowCommand()
        {
            var texturesViewModel = new TexturesViewModel();
            var texturesWindow = new TexturesWindow
            {
                DataContext = texturesViewModel
            };

            texturesWindow.ShowDialog();
        }

        private void ExectueOpenSpriteModelsWindowCommand()
        {
            var spriteModelsViewModel = new SpriteModelsViewModel(WorldRepository);
            var spriteModelsWindow = new SpriteModelsWindow
            {
                DataContext = spriteModelsViewModel
            };

            spriteModelsWindow.ShowDialog();
        }

        private void ExectueOpenTilesWindowCommand()
        {
            var tilesViewModel = new TilesViewModel(WorldRepository);
            var tilesWindow = new TilesWindow
            {
                DataContext = tilesViewModel
            };

            tilesWindow.ShowDialog();
        }

        private void ExectueOpenScriptsWindowCommand()
        {
            var scriptFilesViewModel = new ScriptFilesViewModel(WorldRepository);
            var scriptsWindow = new ScriptFilesWindow
            {
                DataContext = scriptFilesViewModel
            };

            scriptsWindow.ShowDialog();
        }

        private void ExectueOpenMapsWindowCommand()
        {
            var mapsViewModel = new MapsViewModel(WorldRepository);
            var mapsWindow = new MapsWindow
            {
                DataContext = mapsViewModel
            };

            mapsWindow.ShowDialog();
            CreateMaps();
        }
    }
}
