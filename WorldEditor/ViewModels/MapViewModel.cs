using Common.Models;
using GalaSoft.MvvmLight;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorldEditor.ViewModels
{
    public class MapViewModel: ViewModelBase
    {
        readonly Map map;

        public MapViewModel(Map map)
        {
            this.map = map ?? throw new ArgumentException("map");
        }

        public string Id
        {
            get => map.Id;
            set
            {
                if (value == Id)
                    return;
                map.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public string Name
        {
            get => map.Name;
            set
            {
                if (value == Name)
                    return;
                map.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string NameAndId { get => $"{Name} ({Id})"; }

        public int Width
        {
            get => map.Width;
            set
            {
                if (value == Width)
                    return;
                map.Width = value;
                RaisePropertyChanged("Width");
            }
        }

        public int Height
        {
            get => map.Height;
            set
            {
                if (value == Height)
                    return;
                map.Height = value;
                RaisePropertyChanged("Height");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(MapViewModel))
                return false;

            return map == (obj as MapViewModel).map;
        }
    }
}
