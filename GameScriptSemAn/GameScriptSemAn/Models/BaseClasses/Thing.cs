using GameScript.Models.InstanceClasses;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameScript.Models.BaseClasses
{
    public class Thing : GameObject
    {
        public override Type InstanceType { get { return typeof(ThingInstance); } }
        public Texture2D Texture { get; set; }
    }
}
