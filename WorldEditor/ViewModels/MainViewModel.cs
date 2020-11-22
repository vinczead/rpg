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
    public class MainViewModel : CollectionViewModel<RegionViewModel>
    {
        private ToolType selectedTool;
        public ToolType SelectedTool
        {
            get => selectedTool;
            set => Set(ref selectedTool, value);
        }

        private bool isProjectOpen;
        public bool IsProjectOpen
        {
            get => isProjectOpen;
            set
            {
                Set(ref isProjectOpen, value);
                RaisePropertyChanged(() => Title);
                SaveProject.RaiseCanExecuteChanged();
                CloseProject.RaiseCanExecuteChanged();
                SetTool.RaiseCanExecuteChanged();
                OpenContents.RaiseCanExecuteChanged();
            }
        }

        public string Title { get => "World Editor" + (IsProjectOpen ? $" - {World.Instance.FileName}" : ""); }

        public MainViewModel() : base()
        {
            SetTool = new RelayCommand<ToolType>(tool => SelectedTool = tool, tool => IsProjectOpen);
            Close = new RelayCommand<Window>(ExecuteCloseWindow);
            NewProject = new RelayCommand(ExecuteNewProjectCommand);
            OpenProject = new RelayCommand(ExecuteOpenProjectCommand);
            SaveProject = new RelayCommand(ExecuteSaveProjectCommand, () => IsProjectOpen);
            CloseProject = new RelayCommand(ExecuteCloseProjectCommand, () => IsProjectOpen);

            OpenContents = new RelayCommand(ExecuteOpenContentsWindowCommand, () => IsProjectOpen);
        }

        public RelayCommand<ToolType> SetTool { get; }
        public RelayCommand<Window> Close { get; }
        public RelayCommand NewProject { get; }
        public RelayCommand OpenProject { get; }
        public RelayCommand SaveProject { get; }
        public RelayCommand CloseProject { get; }

        public RelayCommand OpenContents { get; }

        private void ExecuteCloseWindow(Window window)
        {
            //todo: ask to save project before closing

            if (window != null)
                window.Close();
        }

        private void ExecuteOpenProjectCommand()
        {
            if (IsProjectOpen)
                CloseProject.Execute(null);

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Vincze Game Script files|*.vgs"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ExecutionVisitor.BuildWorldFromFile(openFileDialog.FileName, out var allerrors);  //todo: error handling
                IsProjectOpen = true;
                RefreshItems();
            }
        }

        public void ExecuteSaveProjectCommand()
        {
            if (!IsProjectOpen)
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
            IsProjectOpen = false;
        }

        private void ExecuteNewProjectCommand()
        {
            if (IsProjectOpen)
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
                RefreshItems();
                IsProjectOpen = true;
            }
        }

        private void ExecuteOpenContentsWindowCommand()
        {
            new ContentsWindow().ShowDialog();
        }

        protected override void RefreshItems()
        {
            var regions = World.Instance.Regions.Select(region => new RegionViewModel(region.Value)).ToList();
            Items = new ObservableCollection<RegionViewModel>(regions);
            RaisePropertyChanged("Items");
        }

        protected override void ExecuteAddItem() {}

        protected override void ExecuteEditItem()
        {
            MessageBox.Show("Region edited."); //todo: actually implement
        }

        protected override void ExecuteRemoveItem()
        {
            MessageBox.Show("Region removed."); //todo: actually implement
        }
    }
}
