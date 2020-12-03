using Common.Models;
using GalaSoft.MvvmLight;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorldEditor.ViewModels
{
    public class FrameViewModel : ViewModelBase
    {
        public Frame Frame { get; set; }
        public AnimationViewModel Animation { get; set; }
        public FrameViewModel(Frame frame, AnimationViewModel animationViewModel)
        {
            Animation = animationViewModel;
            if(frame != null)
            {
                Frame = frame;
                X = Frame.Source.X;
                Y = Frame.Source.Y;
                Duration = Frame.TimeSpan.TotalMilliseconds;
            }
        }

        public void Save()
        {
            if (Frame == null)
                Frame = new Frame();
            Frame.Source = new Rectangle(X, Y, Width, Height);
            Frame.TimeSpan = TimeSpan.FromMilliseconds(Duration);
        }

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

        public int Width
        {
            get => (int)Animation.SpriteModel.FrameWidth;
        }

        public int Height
        {
            get => (int)Animation.SpriteModel.FrameHeight;
        }

        private double duration;
        public double Duration
        {
            get => duration;
            set => Set(ref duration, value);
        }
    }
}
