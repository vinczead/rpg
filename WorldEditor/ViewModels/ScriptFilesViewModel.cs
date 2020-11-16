using Common.Models;
using Common.Utility;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.DataAccess;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public class ScriptFilesViewModel : ItemsViewModel<ScriptFileViewModel>
    {
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }
        public ObservableCollection<TextureViewModel> Textures { get; set; }

        public RelayCommand RenameItem { get; }

        public ScriptFilesViewModel(WorldRepository worldRepository) : base(worldRepository)
        {
            CreateTextures();
            CreateSpriteModels();
            CreateScriptFiles();
            worldRepository.ScriptFiles.ScriptFileAdded += ScriptFileAdded;
            RenameItem = new RelayCommand(ExecuteRenameItem);
        }

        private void ScriptFileAdded(object sender, Utility.EntityEventArgs<ScriptFile> e)
        {
            var scriptFileViewModel = new ScriptFileViewModel(e.Entity);
            Items.Add(scriptFileViewModel);
        }

        void CreateTextures()
        {
            var textures = World.Instance.Textures.Values.Select(texture => new TextureViewModel(texture)).ToList();

            Textures = new ObservableCollection<TextureViewModel>(textures);
        }

        void CreateSpriteModels()
        {
            var spriteModels = WorldRepository.SpriteModels.GetSpriteModels().Select(spriteModel => new SpriteModelViewModel(spriteModel, Textures)).ToList();

            SpriteModels = new ObservableCollection<SpriteModelViewModel>(spriteModels);
        }

        void CreateScriptFiles()
        {
            var scriptFiles = WorldRepository.ScriptFiles.GetScriptFiles().Select(scriptFile => new ScriptFileViewModel(scriptFile)).ToList();
            Items = new ObservableCollection<ScriptFileViewModel>(scriptFiles);
        }

        protected override void ExecuteAddItem()
        {
            WorldRepository.ScriptFiles.AddNewScriptFile();
        }

        protected override void ExecuteRemoveItem()
        {
            WorldRepository.ScriptFiles.RemoveAt(SelectedIndex);
        }

        private void ExecuteRenameItem()
        {
            var dialog = new TextInputDialog($"Rename file {SelectedItem.Name}", "File name:", SelectedItem.Name);
            if(dialog.ShowDialog() == true)
            {
                SelectedItem.Name = dialog.Text;
            }
        }
    }
}
