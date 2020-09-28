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
        [JsonIgnore]
        public Texture2D SpriteSheet { get; set; }
        public string SpriteSheetId { get; set; }
        public Dictionary<string, Animation> Animations { get; set; }

        public Animation this[string state]
        {
            get
            {
                if(Animations.TryGetValue(state, out var animation))
                {
                    return animation;
                }
                else
                {
                    return new Animation();
                }
                
            }
        }
    }
}
