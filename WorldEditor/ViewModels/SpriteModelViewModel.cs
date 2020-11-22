using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace WorldEditor.ViewModels
{
    public class SpriteModelViewModel : CollectionViewModel<AnimationViewModel>
    {
        public SpriteModel SpriteModel { get; private set; }
        public ObservableCollection<TextureViewModel> Textures { get; set; }

        public RelayCommand<Window> SaveSpriteModel { get; set; }

        public SpriteModelViewModel(SpriteModel spriteModel, ObservableCollection<TextureViewModel> textures) : base()
        {
            Textures = textures;
            if (spriteModel != null)
            {
                SpriteModel = spriteModel;
                Id = spriteModel.Id;
                SpriteSheet = textures.FirstOrDefault(texture => texture.Id == spriteModel.SpriteSheet.Id);
                RefreshItems();
            }
            else
            {
                Items = new ObservableCollection<AnimationViewModel>();
            }

            SaveSpriteModel = new RelayCommand<Window>(ExecuteSaveSpriteModelCommand);
        }

        public void ExecuteSaveSpriteModelCommand(Window window)
        {
            try
            {
                if (SpriteModel == null)
                {
                    if (World.Instance.Models.ContainsKey(Id))
                        throw new ArgumentException();
                    foreach (var animationVM in Items)
                    {
                        animationVM.Save();
                    }
                    var modelToAdd = new SpriteModel()
                    {
                        Id = Id,
                        SpriteSheet = SpriteSheet.Texture,
                        Animations = Items.Select(animation => animation.Animation).ToList()
                    };
                    World.Instance.Models.Add(id, modelToAdd);
                    SpriteModel = modelToAdd;
                    window.DialogResult = true;
                }
                else
                {
                    if (SpriteModel.Id != Id && World.Instance.Models.ContainsKey(Id))
                        throw new ArgumentException();
                    World.Instance.Models.Remove(SpriteModel.Id);
                    SpriteModel.Id = Id;
                    SpriteModel.SpriteSheet = SpriteSheet.Texture;
                    foreach (var animationVM in Items)
                    {
                        animationVM.Save();
                    }
                    SpriteModel.Animations = Items.Select(animation => animation.Animation).ToList();
                    World.Instance.Models.Add(id, SpriteModel);
                }
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to add Sprite Model! A Sprite Model with the same id already exists!");
            }
        }

        protected override void RefreshItems()
        {
            if (SpriteModel != null)
            {
                var animations = SpriteModel.Animations.Select(animation => new AnimationViewModel(animation)).ToList();
                Items = new ObservableCollection<AnimationViewModel>(animations);
            }
        }

        protected override void ExecuteAddItem()
        {
            var animation = new Animation()
            {
                Id = "ANIMATION_" + Items.Count
            };
            Items.Add(new AnimationViewModel(animation));
        }

        protected override void ExecuteEditItem()
        {
        }

        protected override void ExecuteRemoveItem()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                SelectedItem = null;
            }
        }

        private string id;
        public string Id
        {
            get => id;
            set => Set(ref id, value);
        }

        private TextureViewModel spriteSheet;
        public TextureViewModel SpriteSheet
        {
            get => spriteSheet;
            set => Set(ref spriteSheet, value);
        }

        public string AnimationsString
        {
            get => string.Join(
                ',',
                Items?.Select(animation => animation.Id).ToList() ?? new List<string>()
                );
        }
    }
}
