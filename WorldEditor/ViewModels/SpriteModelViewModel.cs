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
    public class SpriteModelViewModel : ViewModelBase
    {
        readonly SpriteModel spriteModel;
        public ObservableCollection<TextureViewModel> Textures { get; set; }

        public RelayCommand AddAnimation { get; set; }

        public SpriteModelViewModel(SpriteModel spriteModel, ObservableCollection<TextureViewModel> textures)
        {
            this.spriteModel = spriteModel;
            Textures = textures;

            var animations = spriteModel.Animations.Select(animation => new AnimationViewModel(animation)).ToList();
            Animations = new ObservableCollection<AnimationViewModel>(animations);

            AddAnimation = new RelayCommand(ExecuteAddAnimationCommand);
        }

        public void ExecuteAddAnimationCommand()
        {
            var animation = new Animation()
            {
                Id = "ANIMATION_" + spriteModel.Animations.Count
            };
            spriteModel.Animations.Add(animation);
            Animations.Add(new AnimationViewModel(animation));
        }

        public string Id {
            get => spriteModel.Id;
            set
            {
                if (value == Id)
                    return;
                spriteModel.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public TextureViewModel SpriteSheet {
            get => Textures.FirstOrDefault(texture => texture.Id == spriteModel.SpriteSheetId);
            set
            {
                if(value == null)
                {
                    spriteModel.SpriteSheetId = null;
                    RaisePropertyChanged("SpriteSheet");
                    return;
                }

                if (value.Id == spriteModel.SpriteSheetId)
                    return;

                spriteModel.SpriteSheetId = value.Id;
                RaisePropertyChanged("SpriteSheet");
            }
        }

        public string SpriteSheetId
        {
            get => spriteModel.SpriteSheetId;
            set
            {
                if (value == SpriteSheetId)
                    return;
                spriteModel.SpriteSheetId = value;
                RaisePropertyChanged("SpriteSheetId");
            }
        }

        public ObservableCollection<AnimationViewModel> Animations { get; set; }


        private AnimationViewModel selectedAnimation;

        public AnimationViewModel SelectedAnimation
        {
            get => selectedAnimation;
            set {
                Set(ref selectedAnimation, value);
                RaisePropertyChanged(() => IsAnimationSelected);
            }
        }

        public bool IsAnimationSelected { get => SelectedAnimation != null; }
    }
}
