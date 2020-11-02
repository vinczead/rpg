using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Models
{
    public class TextureManager
    {
        private static readonly object padlock = new object();
        private static TextureManager instance = null;
        public static TextureManager Instance
        {
            get
            {
                if (instance == null) instance = new TextureManager();

                return instance;
            }
        }

        public Game Game { get; set; }

        public Dictionary<string, Texture2D> Textures { get; set; } = new Dictionary<string, Texture2D>();

        public SpriteFont Font { get; set; }

        public void LoadTextureFromFile(string id, string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            var texture = Texture2D.FromStream(Game.GraphicsDevice, fileStream);
            Textures[id] = texture;
        }

        public void LoadTextureFromContent(string id, string assetName)
        {
            var texture = Game.Content.Load<Texture2D>(assetName);
            Textures[id] = texture;
        }

        public void LoadFont(string assetName)
        {
            Font = Game.Content.Load<SpriteFont>(assetName);
        }
    }
}
