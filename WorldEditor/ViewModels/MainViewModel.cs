using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
                RaisePropertyChanged(() => IsWorldRepositoryOpen);
                SaveProject.RaiseCanExecuteChanged();
                CloseProject.RaiseCanExecuteChanged();
                SetTool.RaiseCanExecuteChanged();
                OpenTextures.RaiseCanExecuteChanged();
            }
        }
        
        public bool IsWorldRepositoryOpen
        {
            get => WorldRepository != null;
        }

        public MainViewModel()
        {
            SetTool = new RelayCommand<ToolType>(tool => SelectedTool = tool, tool => IsWorldRepositoryOpen);
            Close = new RelayCommand<Window>(ExecuteCloseWindow);
            NewProject = new RelayCommand(ExecuteNewProjectCommand);
            OpenProject = new RelayCommand(ExecuteOpenProjectCommand);
            SaveProject = new RelayCommand(() => WorldRepository.SaveWorldDescriptor(), () => IsWorldRepositoryOpen);
            CloseProject = new RelayCommand(ExecuteCloseProjectCommand, () => IsWorldRepositoryOpen);

            OpenTextures = new RelayCommand(ExecuteOpenTexturesWindowCommand, () => IsWorldRepositoryOpen);
        }

        public RelayCommand<ToolType> SetTool { get; }
        public RelayCommand<Window> Close { get; }
        public RelayCommand NewProject { get; }
        public RelayCommand OpenProject { get; }
        public RelayCommand SaveProject { get; }
        public RelayCommand CloseProject { get; }

        public RelayCommand OpenTextures { get; }

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
            openFileDialog.Filter = "JSON files|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                WorldRepository = new WorldRepository(openFileDialog.FileName, false);
            }
        }

        private void ExecuteCloseProjectCommand()
        {
            var messageBoxResult = MessageBox.Show("Do you want to save project before closing?", "Close Project", MessageBoxButton.YesNoCancel);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SaveProject.Execute(null);
            }
            if(messageBoxResult == MessageBoxResult.Cancel)
            {
                return;
            }

            WorldRepository = null;
        }

        private void ExecuteNewProjectCommand()
        {
            if (IsWorldRepositoryOpen)
                CloseProject.Execute(null);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "NewWorld";
            saveFileDialog.Filter = "JSON files|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                WorldRepository = new WorldRepository(saveFileDialog.FileName, true);
            }
        }

        private void ExecuteOpenTexturesWindowCommand()
        {
            var texturesViewModel = new TexturesViewModel(WorldRepository);
            var texturesWindow = new TexturesWindow();
            texturesWindow.DataContext = texturesViewModel;

            texturesWindow.ShowDialog();
        }
    }
}
