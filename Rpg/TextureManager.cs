using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg
{
    public static class TextureManager
    {

        public static ContentManager Content;

        public static Texture2D gray { get; set; }
        public static Texture2D green { get; set; }
        public static Texture2D npc { get; set; }
        public static Texture2D player { get; set; }
        public static Texture2D ring { get; set; }
        public static Texture2D door { get; set; }
        public static SpriteFont font { get; set; }

        public static void LoadContent()
        {
            gray = Content.Load<Texture2D>("gray");
            green = Content.Load<Texture2D>("green");
            npc = Content.Load<Texture2D>("npc");
            player = Content.Load<Texture2D>("player");
            ring = Content.Load<Texture2D>("ring");
            door = Content.Load<Texture2D>("door");
            font = Content.Load<SpriteFont>("Alkhemikal");
        }
    }
}
