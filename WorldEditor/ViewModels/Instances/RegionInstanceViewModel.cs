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
        private Region region;
        public RelayCommand<Window> SaveInstance { get; set; }
        public RelayCommand<Window> RemoveInstance { get; set; }
        public RegionInstanceViewModel(ThingInstance thingInstance, Region region)
        {
            this.region = region;
            Id = thingInstance.Id;
            X = (int)thingInstance.Position.X;
            Y = (int)thingInstance.Position.Y;
            ThingInstance = thingInstance;
            var model = thingInstance.Breed.Model;
            var animation = model.Animations.Find(animation => animation.Id.Contains("IDLE")) ?? model.Animations[0];
            var source = animation.Frames?[0].Source ?? new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 32);
            Top = Y - source.Height;
            Left = X - source.Width / 2;
            FrameOffsetX = -source.X;
            FrameOffsetY = -source.Y;
            FrameHeight = source.Height;
            FrameWidth = source.Width;
            SaveInstance = new RelayCommand<Window>(ExecuteSaveInstance);
            RemoveInstance = new RelayCommand<Window>(ExecuteRemoveInstance);
        }

        private void ExecuteSaveInstance(Window window)
        {
            try
            {
                if (ThingInstance.Id != Id && World.Instance.Instances.ContainsKey(Id))
                    throw new ArgumentException();
                World.Instance.Instances.Remove(ThingInstance.Id);
                if (ThingInstance.Id != Id)
                    ThingInstance.IsIdGenerated = false;
                ThingInstance.Id = Id;
                ThingInstance.Position = new Microsoft.Xna.Framework.Vector2(X, Y);
                World.Instance.Instances.Add(Id, ThingInstance);
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to save Instance! An Instance with the same Id already exists!");
            }
        }

        private void ExecuteRemoveInstance(Window window)
        {
            try
            {
                World.Instance.Instances.Remove(ThingInstance.Id);
                region.instances.Remove(ThingInstance);
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to remove instance!");
            }
        }

        public byte[] Texture { get => ThingInstance.Breed.Model.SpriteSheet.ByteArrayValue; }

        private string id;
        public string Id
        {
            get => id;
            set => Set(ref id, value);
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

        public int Top { get; }
        public int Left { get; }
        public int FrameOffsetX { get; }
        public int FrameOffsetY { get; }
        public int FrameWidth { get; }
        public int FrameHeight { get; }
        public string WindowTitle { get => $"Edit {ThingInstance.Id}"; }
    }
}
