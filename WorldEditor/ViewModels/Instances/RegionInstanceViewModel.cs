using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WorldEditor.ViewModels
{
    public class RegionInstanceViewModel : ViewModelBase
    {
        public ThingInstance ThingInstance { get; set; }
        public RelayCommand<Window> SaveInstance { get; set; }
        public RegionInstanceViewModel(ThingInstance thingInstance)
        {
            X = (int)thingInstance.Position.X;
            Y = (int)thingInstance.Position.Y;
            ThingInstance = thingInstance;
            var model = thingInstance.Breed.Model;
            var animation = model.Animations.Find(animation => animation.Id.Contains("IDLE")) ?? model.Animations[0];
            var source = animation.Frames?[0].Source ?? new Microsoft.Xna.Framework.Rectangle(0,0,32,32);
            Top = Y - source.Height;
            Left = X - source.Width / 2;
            FrameOffsetX = -source.X;
            FrameOffsetY = -source.Y;
            FrameHeight = source.Height;
            FrameWidth = source.Width;
            SaveInstance = new RelayCommand<Window>(ExecuteSaveInstance);
        }

        private void ExecuteSaveInstance(Window window)
        {
            ThingInstance.Position = new Microsoft.Xna.Framework.Vector2(X, Y);
            window.Close();
        }

        public byte[] Texture { get => ThingInstance.Breed.Model.SpriteSheet.ByteArrayValue; }

        private int x;
        public int X
        {
            get => x;
            set => Set(ref x, value);
        }

        private int y;
        public int Y
        {
            get => y;
            set => Set(ref y, value);
        }

        public int Top { get; }
        public int Left { get; }
        public int FrameOffsetX { get; }
        public int FrameOffsetY { get; }
        public int FrameWidth { get; }
        public int FrameHeight { get; }
        public string WindowTitle { get => $"Edit {ThingInstance.Id}"; }
    }
}
