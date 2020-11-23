using Common.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorldEditor.ViewModels
{
    public class RegionInstanceViewModel : ViewModelBase
    {
        public ThingInstance ThingInstance { get; set; }
        public RegionInstanceViewModel(ThingInstance thingInstance)
        {
            X = (int)thingInstance.Position.X;
            Y = (int)thingInstance.Position.Y;
            ThingInstance = thingInstance;
        }

        public byte[] Texture { get => ThingInstance.Breed.Model.SpriteSheet.ByteArrayValue; }

        private int x;
        public int X
        {
            get => x;
            set
            {
                Set(ref x, value);
                if (ThingInstance != null)
                    ThingInstance.Position = new Microsoft.Xna.Framework.Vector2(X, Y);
            }
        }

        private int y;
        public int Y
        {
            get => y;
            set
            {
                Set(ref y, value);
                if (ThingInstance != null)
                    ThingInstance.Position = new Microsoft.Xna.Framework.Vector2(X, Y);
            }
        }
    }
}
