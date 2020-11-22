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
            }
        }

        public string Title { get => "World Editor" + (IsWorldRepositoryOpen ? $" - {World.Instance.FileName}" : ""); }

        private ObservableCollection<RegionViewModel> maps;

        public ObservableCollection<RegionViewModel> Maps
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
        }

        public RelayCommand<ToolType> SetTool { get; }
        public RelayCommand<Window> Close { get; }
        public RelayCommand NewProject { get; }
        public RelayCommand OpenProject { get; }
        public RelayCommand SaveProject { get; }
        public RelayCommand CloseProject { get; }

        public RelayCommand OpenContents { get; }

        private void CreateMaps()
        {
            var maps = World.Instance.Regions.Select(map => new RegionViewModel(map.Value)).ToList();

            Maps = new ObservableCollection<RegionViewModel>(maps);
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

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Vincze Game Script files|*.vgs"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ExecutionVisitor.BuildWorldFromFile(openFileDialog.FileName, out var allerrors);  //todo: error handling
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

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "NewWorld",
                Filter = "Vincze Game Script files|*.vgs"
            };
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
    }
}
