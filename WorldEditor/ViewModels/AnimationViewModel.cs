using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WorldEditor.ViewModels
{
    public class AnimationViewModel : ViewModelBase
    {
        readonly Animation animation;

        public RelayCommand AddFrame { get; set; }

        public AnimationViewModel(Animation animation)
        {
            this.animation = animation;

            var frames = animation.Frames.Select(frame => new FrameViewModel(frame)).ToList();
            Frames = new ObservableCollection<FrameViewModel>(frames);
            AddFrame = new RelayCommand(ExecuteAddFrameCommand);
        }

        public void ExecuteAddFrameCommand()
        {
            var frame = new Frame();
            animation.Frames.Add(frame);
            var fvm = new FrameViewModel(frame)
            {
                Id = "Frame " + Frames.Count
            };
            Frames.Add(fvm);
        }

        public string Id
        {
            get => animation.Id;
            set
            {
                if (value == Id)
                    return;
                animation.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public bool IsLooping
        {
            get => animation.IsLooping;
            set
            {
                if (value == IsLooping)
                    return;
                animation.IsLooping = value;
                RaisePropertyChanged("IsLooping");
            }
        }

        public ObservableCollection<FrameViewModel> Frames { get; set; }

        private FrameViewModel selectedFrame;

        public FrameViewModel SelectedFrame
        {
            get => selectedFrame;
            set {
                Set(ref selectedFrame, value);
                RaisePropertyChanged(() => IsFrameSelected);
            }
        }

        public bool IsFrameSelected { get => SelectedFrame != null; }
    }
}
