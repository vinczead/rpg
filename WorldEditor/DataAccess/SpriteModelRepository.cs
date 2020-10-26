using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldEditor.Utility;

namespace WorldEditor.DataAccess
{
    public class SpriteModelRepository
    {
        readonly List<SpriteModel> spriteModels = new List<SpriteModel>();
        public List<SpriteModel> GetSpriteModels()
        {
            return new List<SpriteModel>(spriteModels);
        }

        public SpriteModel GetSpriteModelById(string id)
        {
            var spriteModel = spriteModels.FirstOrDefault(spriteModel => spriteModel.Id == id);

            if (spriteModel == null)
                throw new ArgumentException($"No sprite model was found with id '{id}'.");

            return spriteModel;
        }
        public void AddSpriteModel(SpriteModel spriteModel)
        {
            if (spriteModel == null)
                throw new ArgumentNullException("spriteModel");

            spriteModels.Add(spriteModel);
            SpriteModelAdded?.Invoke(this, new EntityEventArgs<SpriteModel>(spriteModel));
        }
        public void AddNewSpriteModel()
        {
            var i = 1;
            while (spriteModels.FirstOrDefault(spriteModel => spriteModel.Id == $"SpriteModel{i}") != null)
                i++;

            AddSpriteModel(new SpriteModel() { Id = $"SpriteModel{i}" });
        }

        public void RemoveSpriteModel(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var spriteModel = spriteModels.FirstOrDefault(spriteModel => spriteModel.Id == id);
            if (spriteModel == null)
                throw new ArgumentException($"No sprite model was found with id '{id}'.");

            spriteModels.Remove(spriteModel);
            SpriteModelRemoved(this, new EntityEventArgs<SpriteModel>(spriteModel));
        }


        public event EventHandler<EntityEventArgs<SpriteModel>> SpriteModelAdded;
        public event EventHandler<EntityEventArgs<SpriteModel>> SpriteModelRemoved;
    }
}
