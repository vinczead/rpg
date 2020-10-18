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
        readonly Frame frame;
        public FrameViewModel(Frame frame)
        {
            this.frame = frame;
        }

        public int X
        {
            get => frame.Source.X;
            set
            {
                if (value == frame.Source.X)
                    return;
                frame.Source = new Rectangle(value, frame.Source.Y, frame.Source.Width, frame.Source.Height);
                RaisePropertyChanged("X");
            }
        }

        public int Y
        {
            get => frame.Source.Y;
            set
            {
                if (value == frame.Source.Y)
                    return;
                frame.Source = new Rectangle(frame.Source.X, value, frame.Source.Width, frame.Source.Height);
                RaisePropertyChanged("Y");
            }
        }

        public int Width
        {
            get => frame.Source.Width;
            set
            {
                if (value == frame.Source.Width)
                    return;
                frame.Source = new Rectangle(frame.Source.X, frame.Source.Y, value, frame.Source.Height);
                RaisePropertyChanged("Width");
            }
        }

        public int Height
        {
            get => frame.Source.Height;
            set
            {
                if (value == frame.Source.Height)
                    return;
                frame.Source = new Rectangle(frame.Source.X, frame.Source.Y, frame.Source.Width, value);
                RaisePropertyChanged("Height");
            }
        }

        public double Duration
        {
            get => frame.TimeSpan.TotalMilliseconds;
            set
            {
                if (value == frame.TimeSpan.TotalMilliseconds)
                    return;
                frame.TimeSpan = TimeSpan.FromMilliseconds(value);
                RaisePropertyChanged("Duration");
            }
        }
    }
}
