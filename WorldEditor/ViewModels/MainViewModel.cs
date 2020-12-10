using Common.Models;
using Common.Script.Utility;
using Common.Script.Visitors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private SidebarViewModel sidebar;
        public SidebarViewModel Sidebar { get => sidebar; set => Set(ref sidebar, value); }

        private TexturesViewModel textures;
        public TexturesViewModel Textures { get => textures; set => Set(ref textures, value); }

        private SpriteModelsViewModel spriteModels;
        public SpriteModelsViewModel SpriteModels { get => spriteModels; set => Set(ref spriteModels, value); }

        private BreedsViewModel breeds;
        public BreedsViewModel Breeds { get => breeds; set => Set(ref breeds, value); }

        private TilesViewModel tiles;
        public TilesViewModel Tiles { get => tiles; set => Set(ref tiles, value); }

        private RegionsViewModel regions;
        public RegionsViewModel Regions { get => regions; set => Set(ref regions, value); }

        private InstanceViewModel playerInstance;
        public InstanceViewModel PlayerInstance
        {
            get => playerInstance;
            set
            {
                Set(ref playerInstance, value);
                if (value == null)
                    World.Instance.Player = null;
                else
                    World.Instance.Player = value.ThingInstance as CharacterInstance;
            }
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
                Sidebar?.SetTool.RaiseCanExecuteChanged();
                OpenContents.RaiseCanExecuteChanged();
                if(value == true)
                {
                    Sidebar = new SidebarViewModel(this);
                    Textures = new TexturesViewModel(this);
                    SpriteModels = new SpriteModelsViewModel(this);
                    Breeds = new BreedsViewModel(this);
                    Tiles = new TilesViewModel(this);
                    Regions = new RegionsViewModel(this);
                    if(World.Instance.Player?.Id != null)
                    {
                        var playerInstanceVM = Regions.Items
                            .Select(region => region.Instances.FirstOrDefault(instance => instance.Id == World.Instance.Player.Id))
                            .FirstOrDefault(instance => instance != null);
                        PlayerInstance = playerInstanceVM;
                    }
                } else
                {
                    Sidebar = null;
                    Textures = null;
                    SpriteModels = null;
                    Breeds = null;
                    Tiles = null;
                    Regions = null;
                    PlayerInstance = null;
                }
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
                ExecutionVisitor.CheckErrorsAndBuildFromFile(openFileDialog.FileName, out var messages);
                var errorCount = messages.Count(message => message.Severity == ErrorSeverity.Error);
                if (errorCount > 0)
                {
                    MessageBox.Show($"Failed to build World! {errorCount} error(s) found: \n" + string.Join('\n', messages));
                    return;
                }
                IsProjectOpen = true;
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
                World.Instance.FileName = saveFileDialog.FileName;
                file.Close();
                IsProjectOpen = true;
            }
        }

        private void ExecuteOpenContentsWindowCommand()
        {
            new ContentsWindow()
            {
                DataContext = this
            }.ShowDialog();
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
    }
}
