﻿using Common.Models;
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
        public SpriteModelsViewModel SpriteModels { get; set; }

        public RelayCommand<Window> SaveSpriteModel { get; set; }

        public SpriteModelViewModel(SpriteModel spriteModel, SpriteModelsViewModel spriteModelsViewModel) : base()
        {
            SpriteModels = spriteModelsViewModel;
            if (spriteModel != null)
            {
                SpriteModel = spriteModel;
                Id = spriteModel.Id;

                FrameWidth = spriteModel.FrameSize.X;
                FrameHeight = spriteModel.FrameSize.Y;

                CollisionBoxX = spriteModel.CollisionBox.X;
                CollisionBoxY = spriteModel.CollisionBox.Y;

                CollisionBoxWidth = spriteModel.CollisionBox.Width;
                CollisionBoxHeight = spriteModel.CollisionBox.Height;

                SpriteSheet = SpriteModels.MainViewModel.Textures.Items.FirstOrDefault(texture => texture.Id == spriteModel.SpriteSheet.Id);
                ReloadItems();
            }
            else
            {
                Items = new ObservableCollection<AnimationViewModel>();
            }

            SaveSpriteModel = new RelayCommand<Window>(ExecuteSaveSpriteModelCommand);
            ReloadItems();
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
                        FrameSize = new Microsoft.Xna.Framework.Vector2(FrameWidth, FrameHeight),
                        CollisionBox = new Microsoft.Xna.Framework.Rectangle(CollisionBoxX, CollisionBoxY, CollisionBoxWidth, CollisionBoxHeight),
                        Animations = Items.Select(animation => animation.Animation).ToList()
                    };
                    SpriteModel = modelToAdd;

                    World.Instance.Models.Add(id, modelToAdd);
                    SpriteModels.Items.Add(this);
                }
                else
                {
                    if (SpriteModel.Id != Id && World.Instance.Models.ContainsKey(Id))
                        throw new ArgumentException();
                    World.Instance.Models.Remove(SpriteModel.Id);
                    var originalViewModel = SpriteModels.Items.First(item => item.Id == SpriteModel.Id);

                    SpriteModel.Id = Id;
                    originalViewModel.Id = Id;

                    SpriteModel.FrameSize = new Microsoft.Xna.Framework.Vector2(FrameWidth, FrameHeight);
                    originalViewModel.FrameHeight = FrameHeight;
                    originalViewModel.FrameWidth = FrameWidth;

                    SpriteModel.CollisionBox = new Microsoft.Xna.Framework.Rectangle(CollisionBoxX, CollisionBoxY, CollisionBoxWidth, CollisionBoxHeight);
                    originalViewModel.CollisionBoxHeight = CollisionBoxHeight;
                    originalViewModel.CollisionBoxWidth = CollisionBoxWidth;
                    originalViewModel.CollisionBoxX = CollisionBoxX;
                    originalViewModel.CollisionBoxY = CollisionBoxY;

                    SpriteModel.SpriteSheet = SpriteSheet.Texture;
                    originalViewModel.SpriteSheet = SpriteSheet;

                    originalViewModel.Items = Items;
                    foreach (var animationVM in Items)
                    {
                        animationVM.Save();
                    }
                    SpriteModel.Animations = Items.Select(animation => animation.Animation).ToList();

                    originalViewModel.NotifyReferencesOfChange();

                    World.Instance.Models.Add(id, SpriteModel);
                }
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to add Sprite Model! A Sprite Model with the same id already exists!");
            }
        }

        public void NotifyReferencesOfChange()
        {
            RaisePropertyChanged("SpriteSheet");
            foreach (var breed in SpriteModels.MainViewModel.Breeds.Items)
            {
                if (breed.SpriteModel == this)
                    breed.NotifyReferencesOfChange();
            }
            foreach (var tile in SpriteModels.MainViewModel.Tiles.Items)
            {
                if (tile.SpriteModel == this)
                    tile.NotifyReferencesOfChange();
            }
        }

        protected override void ReloadItems()
        {
            if (SpriteModel != null)
            {
                var animations = SpriteModel.Animations.Select(animation => new AnimationViewModel(animation, this)).ToList();
                Items = new ObservableCollection<AnimationViewModel>(animations);
            }
        }

        protected override void ExecuteAddItem()
        {
            var animation = new Animation()
            {
                Id = "ANIMATION_" + Items.Count
            };
            Items.Add(new AnimationViewModel(animation, this));
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

        public override ObservableCollection<AnimationViewModel> Items
        {
            get => base.Items;
            set
            {
                base.Items = value;
                RaisePropertyChanged("AnimationsString");
            }
        }

        private string id;
        public string Id
        {
            get => id;
            set => Set(ref id, value);
        }

        private float frameWidth;
        public float FrameWidth
        {
            get => frameWidth;
            set => Set(ref frameWidth, value);
        }

        private float frameHeight;
        public float FrameHeight
        {
            get => frameHeight;
            set => Set(ref frameHeight, value);
        }

        private int collisionBoxWidth;
        public int CollisionBoxWidth
        {
            get => collisionBoxWidth;
            set => Set(ref collisionBoxWidth, value);
        }

        private int collisionBoxHeight;
        public int CollisionBoxHeight
        {
            get => collisionBoxHeight;
            set => Set(ref collisionBoxHeight, value);
        }

        private int collisionBoxX;
        public int CollisionBoxX
        {
            get => collisionBoxX;
            set => Set(ref collisionBoxX, value);
        }

        private int collisionBoxY;
        public int CollisionBoxY
        {
            get => collisionBoxY;
            set => Set(ref collisionBoxY, value);
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
