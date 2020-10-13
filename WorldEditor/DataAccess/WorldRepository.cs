using Common.Models;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using WorldEditor.Utility;

namespace WorldEditor.DataAccess
{
    public class WorldRepository
    {
        [JsonIgnore]
        public string FileName { get; set; }

        //todo: add things, maps, spritemodels, tiles
        public TextureRepository Textures { get; private set; } = new TextureRepository();
        public SpriteModelRepository SpriteModels { get; private set; } = new SpriteModelRepository();

        public WorldRepository(string fileName, bool creating)
        {
            if (creating)
            {
                var file = File.Create(fileName);
                file.Close();
            }
            else
            {
                LoadWorldDescriptor(fileName);
            }
        }

        private void LoadWorldDescriptor(string fileName)
        {
            FileName = fileName;
            //TODO: implement
        }

        public void SaveWorldDescriptor()
        {
            //TODO: implement
        }
    }
}
