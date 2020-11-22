using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Models
{
    public class SpriteModel
    {
        public string Id { get; set; }
        public Texture SpriteSheet { get; set; }
        public List<Animation> Animations { get; set; } = new List<Animation>();

        public Animation this[string state]
        {
            get
            {
                return Animations.FirstOrDefault(animation => animation.Id == state);
            }
        }
    }
}
