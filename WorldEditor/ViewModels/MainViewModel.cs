using Common.Models;
using Common.Script.Visitors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.Utility;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public class MainViewModel : CollectionViewModel<RegionEditorViewModel>
    {
        public SidebarViewModel Sidebar { get; set; }

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
                Sidebar.SetTool.RaiseCanExecuteChanged();
                OpenContents.RaiseCanExecuteChanged();
            }
        }

        public string Title { get => "World Editor" + (IsProjectOpen ? $" - {World.Instance.FileName}" : ""); }

        public MainViewModel() : base()
        {
            Close = new RelayCommand<Window>(ExecuteCloseWindow);
            NewProject = new RelayCommand(ExecuteNewProjectCommand);
            OpenProject = new RelayCommand(ExecuteOpenProjectCommand);
            SaveProject = new RelayCommand(ExecuteSaveProjectCommand, () => IsProjectOpen);
            CloseProject = new RelayCommand(ExecuteCloseProjectCommand, () => IsProjectOpen);

            OpenContents = new RelayCommand(ExecuteOpenContentsWindowCommand, () => IsProjectOpen);

            OpenGithubPage = new RelayCommand(ExecuteOpenGithubPage);
            OpenAboutWindow = new RelayCommand(ExecuteOpenAboutWindow);

            Sidebar = new SidebarViewModel(this);
        }

        public RelayCommand<Window> Close { get; }
        public RelayCommand NewProject { get; }
        public RelayCommand OpenProject { get; }
        public RelayCommand SaveProject { get; }
        public RelayCommand CloseProject { get; }

        public RelayCommand OpenContents { get; }

        public RelayCommand OpenGithubPage { get; }
        public RelayCommand OpenAboutWindow { get; }

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
            RefreshItems();
        }

        private void ExecuteOpenGithubPage()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/vinczead/rpg",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void ExecuteOpenAboutWindow()
        {
            MessageBox.Show("Created for Master's Thesis at Budapest Univeristy of Technology and Economics.\nCreated by Ádám Vincze\n2020", "About World Editor");
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get => selectedIndex;
            set => Set(ref selectedIndex, value);
        }


        protected override void RefreshItems()
        {
            var regions = World.Instance.Regions.Select(region => new RegionEditorViewModel(this, region.Value)).ToList();
            Items = new ObservableCollection<RegionEditorViewModel>(regions);
            var oldSelectedIndex = SelectedIndex;
            RaisePropertyChanged("Items");
            SelectedIndex = oldSelectedIndex >= Items.Count ? Items.Count - 1 : oldSelectedIndex;
            Sidebar?.RefreshItems();
        }



        protected override void ExecuteAddItem() {}

        protected override void ExecuteEditItem() {}

        protected override void ExecuteRemoveItem() {}
    }
}
