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
        readonly List<RpgTexture> textures = new List<RpgTexture>();
        public List<RpgTexture> GetTextures()
        {
            return new List<RpgTexture>(textures);
        }

        public RpgTexture GetTextureById(string id)
        {
            var texture = textures.FirstOrDefault(texture => texture.Id == id);

            if (texture == null)
                throw new ArgumentException($"No texture was found with id '{id}'.");

            return texture;
        }
        public void AddTexture(RpgTexture rpgTexture)
        {
            if (rpgTexture == null)
                throw new ArgumentNullException("rpgTexture");

            textures.Add(rpgTexture);
            TextureAdded(this, new RpgTextureEventArgs(rpgTexture));
        }
        public void AddNewTexture()
        {
            var i = 1;
            while (textures.FirstOrDefault(texture => texture.Id == $"Texture{i}") != null)
                i++;

            AddTexture(new RpgTexture() { Id = $"Texture{i}" });
        }

        public void RemoveTexture(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var texture = textures.FirstOrDefault(texture => texture.Id == id);
            if (texture == null)
                throw new ArgumentException($"No texture was found with id '{id}'.");

            textures.Remove(texture);
            TextureRemoved(this, new RpgTextureEventArgs(texture));
        }


        public event EventHandler<RpgTextureEventArgs> TextureAdded;
        public event EventHandler<RpgTextureEventArgs> TextureRemoved;

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
