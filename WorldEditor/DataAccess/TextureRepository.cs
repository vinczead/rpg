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
        public List<RpgTexture> Textures { get; set; } = new List<RpgTexture>();
        public List<RpgTexture> GetTextures()
        {
            return new List<RpgTexture>(Textures);
        }

        public RpgTexture GetTextureById(string id)
        {
            var texture = Textures.FirstOrDefault(texture => texture.Id == id);

            if (texture == null)
                throw new ArgumentException($"No texture was found with id '{id}'.");

            return texture;
        }
        public void AddTexture(RpgTexture rpgTexture)
        {
            if (rpgTexture == null)
                throw new ArgumentNullException("rpgTexture");

            Textures.Add(rpgTexture);
            TextureAdded?.Invoke(this, new EntityEventArgs<RpgTexture>(rpgTexture));
        }
        public void AddNewTexture()
        {
            var i = 1;
            while (Textures.FirstOrDefault(texture => texture.Id == $"Texture{i}") != null)
                i++;

            AddTexture(new RpgTexture() { Id = $"Texture{i}" });
        }

        public void RemoveTexture(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var texture = Textures.FirstOrDefault(texture => texture.Id == id);
            if (texture == null)
                throw new ArgumentException($"No texture was found with id '{id}'.");

            Textures.Remove(texture);
            TextureRemoved(this, new EntityEventArgs<RpgTexture>(texture));
        }

        public void RemoveAt(int index)
        {
            if(index < 0 || index >= Textures.Count)
                throw new ArgumentException($"Invalid index: {index}");

            Textures.RemoveAt(index);

        }


        public event EventHandler<EntityEventArgs<RpgTexture>> TextureAdded;
        public event EventHandler<EntityEventArgs<RpgTexture>> TextureRemoved;
    }
}
