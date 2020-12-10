using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WorldEditor.ViewModels
{
    public class InstanceViewModel : ViewModelBase
    {
        public ThingInstance ThingInstance { get; set; }
        private RegionViewModel region;
        private RegionViewModel Region
        {
            get => region;
            set => Set(ref region, value);
        }
        public RelayCommand<Window> SaveInstance { get; set; }
        public RelayCommand<Window> RemoveInstance { get; set; }
        public InstanceViewModel(BreedViewModel breedViewModel, ThingInstance thingInstance, RegionViewModel regionViewModel)
        {
            Breed = breedViewModel;
            Region = regionViewModel;
            Id = thingInstance.Id;
            X = (int)thingInstance.Position.X;
            Y = (int)thingInstance.Position.Y;
            ThingInstance = thingInstance;

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
                var originalViewModel = Region.Instances.First(instance => instance.Id == ThingInstance.Id);

                if (ThingInstance.Id != Id)
                    ThingInstance.IsIdGenerated = false;

                ThingInstance.Id = Id;
                originalViewModel.Id = Id;

                ThingInstance.Position = new Microsoft.Xna.Framework.Vector2(X, Y);
                originalViewModel.X = X;
                originalViewModel.Y = Y;

                RaisePropertiesChanged();

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
                Region.Region.instances.RemoveAll(instance => instance.Id == ThingInstance.Id);
                var originalViewModel = Region.Instances.First(instance => instance.Id == ThingInstance.Id);
                Region.Instances.Remove(originalViewModel);
                Region.Regions.MainViewModel.PlayerInstance = null;
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to remove instance!");
            }
        }

        public void RaisePropertiesChanged()
        {
            RaisePropertyChanged("Breed");
            RaisePropertyChanged("Top");
            RaisePropertyChanged("Left");
            RaisePropertyChanged("FrameOffsetX");
            RaisePropertyChanged("FrameOffsetY");
            RaisePropertyChanged("FrameWidth");
            RaisePropertyChanged("FrameHeight");
        }

        private BreedViewModel breed;
        public BreedViewModel Breed
        {
            get => breed;
            set
            {
                Set(ref breed, value);
                RaisePropertyChanged("Top");
                RaisePropertyChanged("Left");
                RaisePropertyChanged("FrameOffsetX");
                RaisePropertyChanged("FrameOffsetY");
                RaisePropertyChanged("FrameWidth");
                RaisePropertyChanged("FrameHeight");
            }
        }


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
            set
            {
                Set(ref x, value);
                RaisePropertyChanged("Left");
            }
        }

        private int y;
        public int Y
        {
            get => y;
            set
            {
                Set(ref y, value);
                RaisePropertyChanged("Top");
            }
        }

        public int Top { get => Y - Breed?.SpriteModel?.Items?[0]?.Items?[0]?.Height ?? 0; }
        public int Left { get => X - Breed?.SpriteModel?.Items?[0]?.Items?[0]?.Width / 2 ?? 0; }
        public int FrameOffsetX { get => -Breed?.SpriteModel?.Items?[0]?.Items?[0]?.X ?? 0; }
        public int FrameOffsetY { get => -Breed?.SpriteModel?.Items?[0]?.Items?[0]?.Y ?? 0; }
        public int FrameWidth { get => Breed?.SpriteModel?.Items?[0]?.Items?[0]?.Width ?? 0; }
        public int FrameHeight { get => Breed?.SpriteModel?.Items?[0]?.Items?[0]?.Height ?? 0; }
        public string WindowTitle { get => $"Edit {ThingInstance.Id}"; }
    }
}
