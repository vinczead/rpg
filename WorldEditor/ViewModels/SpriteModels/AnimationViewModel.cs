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
    public class AnimationViewModel : CollectionViewModel<FrameViewModel>
    {
        public Animation Animation { get; private set; }

        public AnimationViewModel(Animation animation) : base()
        {
            if(animation != null)
            {
                Animation = animation;
                Id = animation.Id;
                IsLooping = animation.IsLooping;
            }
            
            RefreshItems();
        }

        public void Save()
        {
            if (Animation == null)
                Animation = new Animation();
            Animation.Id = Id;
            Animation.IsLooping = IsLooping;
            foreach (var frameVM in Items)
            {
                frameVM.Save();
            }
            Animation.Frames = Items.Select(frameVM => frameVM.Frame).ToList();
        }

        protected override void RefreshItems()
        {
            if (Animation != null)
            {
                var frames = Animation.Frames.Select(frame => new FrameViewModel(frame)).ToList();
                Items = new ObservableCollection<FrameViewModel>(frames);
            }
        }

        protected override void ExecuteAddItem()
        {
            Items.Add(new FrameViewModel(null));
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

        private bool isLooping;
        public bool IsLooping
        {
            get => isLooping;
            set => Set(ref isLooping, value);
        }

    }
}
