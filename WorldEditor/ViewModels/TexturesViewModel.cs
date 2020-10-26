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
    public class TexturesViewModel : ViewModelBase
    {
        public WorldRepository WorldRepository { get; set; }
        public ObservableCollection<TextureViewModel> Textures { get; set; }

        private TextureViewModel selectedTexture;
        public TextureViewModel SelectedTexture {
            get => selectedTexture;
            set
            {
                Set(ref selectedTexture, value);
                RaisePropertyChanged(() => IsTextureSelected);
            }
        }

        public bool IsTextureSelected { get => SelectedTexture != null; }

        public RelayCommand<TextureViewModel> SelectTexture { get; }
        public RelayCommand AddTexture { get; }
        public RelayCommand<TextureViewModel> RemoveTexture { get; }

        public TexturesViewModel(WorldRepository worldRepository)
        {
            WorldRepository = worldRepository;
            SelectTexture = new RelayCommand<TextureViewModel>(ExecuteSelectTexture);
            AddTexture = new RelayCommand(ExecuteAddTexture);
            RemoveTexture = new RelayCommand<TextureViewModel>(ExecuteRemoveTexture);

            worldRepository.Textures.TextureAdded += WorldRepository_TextureAdded;

            CreateTextures();
        }

        private void WorldRepository_TextureAdded(object sender, Utility.EntityEventArgs<RpgTexture> e)
        {
            var textureViewModel = new TextureViewModel(e.Entity);
            this.Textures.Add(textureViewModel);
        }

        void CreateTextures()
        {
            var textures = WorldRepository.Textures.GetTextures().Select(texture => new TextureViewModel(texture)).ToList();

            Textures = new ObservableCollection<TextureViewModel>(textures);
        }

        private void ExecuteSelectTexture(TextureViewModel textureViewModel)
        {
            SelectedTexture = textureViewModel;
        }

        private void ExecuteAddTexture()
        {
            WorldRepository.Textures.AddNewTexture();
        }

        private void ExecuteRemoveTexture(TextureViewModel textureViewModel)
        {
            WorldRepository.Textures.RemoveTexture(textureViewModel.Id);
        }
    }
}
