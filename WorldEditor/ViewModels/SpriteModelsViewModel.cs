using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WorldEditor.DataAccess;

namespace WorldEditor.ViewModels
{
    public class SpriteModelsViewModel : ViewModelBase
    {
        public WorldRepository WorldRepository { get; set; }
        public ObservableCollection<SpriteModelViewModel> SpriteModels { get; set; }
        public ObservableCollection<TextureViewModel> Textures { get; set; }

        private SpriteModelViewModel selectedSpriteModel;

        public SpriteModelViewModel SelectedSpriteModel
        {
            get => selectedSpriteModel;
            set
            {
                Set(ref selectedSpriteModel, value);
                RaisePropertyChanged(() => IsSpriteModelSelected);
            }
        }

        public bool IsSpriteModelSelected { get => SelectedSpriteModel != null; }

        public RelayCommand<SpriteModelViewModel> SelectSpriteModel { get; }
        public RelayCommand AddSpriteModel { get; }
        public RelayCommand<SpriteModelViewModel> RemoveSpriteModel { get; }

        public SpriteModelsViewModel(WorldRepository worldRepository)
        {
            WorldRepository = worldRepository;
            SelectSpriteModel = new RelayCommand<SpriteModelViewModel>(ExecuteSelectSpriteModel);
            AddSpriteModel = new RelayCommand(ExecuteAddSpriteModel);
            RemoveSpriteModel = new RelayCommand<SpriteModelViewModel>(ExecuteRemoveSpriteModel);

            worldRepository.SpriteModels.SpriteModelAdded += WorldRepository_SpriteModelAdded;

            CreateTextures();
            CreateSpriteModels();
        }

        private void WorldRepository_SpriteModelAdded(object sender, Utility.EntityEventArgs<SpriteModel> e)
        {
            var spriteModelViewModel = new SpriteModelViewModel(e.Entity, Textures);
            this.SpriteModels.Add(spriteModelViewModel);
        }

        void CreateSpriteModels()
        {
            var spriteModels = WorldRepository
                .SpriteModels
                .GetSpriteModels()
                .Select(spriteModel => new SpriteModelViewModel(spriteModel, Textures))
                .ToList();

            SpriteModels = new ObservableCollection<SpriteModelViewModel>(spriteModels);
        }

        void CreateTextures()
        {
            var textures = WorldRepository.Textures.GetTextures().Select(texture => new TextureViewModel(texture)).ToList();

            Textures = new ObservableCollection<TextureViewModel>(textures);
        }


        private void ExecuteSelectSpriteModel(SpriteModelViewModel spriteModelViewModel)
        {
            SelectedSpriteModel = spriteModelViewModel;
        }

        private void ExecuteAddSpriteModel()
        {
            WorldRepository.SpriteModels.AddNewSpriteModel();
        }

        private void ExecuteRemoveSpriteModel(SpriteModelViewModel spriteModelViewModel)
        {
            WorldRepository.SpriteModels.RemoveSpriteModel(spriteModelViewModel.Id);
        }
    }
}
