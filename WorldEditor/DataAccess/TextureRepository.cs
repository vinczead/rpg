using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldEditor.Utility;

namespace WorldEditor.DataAccess
{
    public class TextureRepository
    {
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
            SpriteModelAdded(this, new EntityEventArgs<RpgTexture>(rpgTexture));
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
            SpriteModelRemoved(this, new EntityEventArgs<RpgTexture>(texture));
        }


        public event EventHandler<EntityEventArgs<RpgTexture>> SpriteModelAdded;
        public event EventHandler<EntityEventArgs<RpgTexture>> SpriteModelRemoved;
    }
}
