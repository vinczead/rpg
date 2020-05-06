using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models
{
    public static class TextureManager
    {

        public static ContentManager Content;

        public static Dictionary<string, Texture2D> Textures { get; set; } = new Dictionary<string, Texture2D>();

        public static SpriteFont font { get; set; }

        public static void LoadContent()
        {
            Textures.Add("Gray", Content.Load<Texture2D>("gray"));
            Textures.Add("Green", Content.Load<Texture2D>("green"));
            Textures.Add("Npc", Content.Load<Texture2D>("npc"));
            Textures.Add("Player", Content.Load<Texture2D>("player"));
            Textures.Add("Ring", Content.Load<Texture2D>("ring"));
            Textures.Add("Door", Content.Load<Texture2D>("door"));
            Textures.Add("Potion", Content.Load<Texture2D>("potion"));
            Textures.Add("Sword", Content.Load<Texture2D>("sword"));
            Textures.Add("Button", Content.Load<Texture2D>("button"));

            font = Content.Load<SpriteFont>("Alkhemikal");
        }
    }
}
