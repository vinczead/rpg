using GameModel.Models.BaseInterfaces;
using GameModel.Models.InstanceInterfaces;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Rpg.Models
{
    public abstract class GameWorldObjectBase : GameObjectBase, IGameWorldObjectBase
    {
        public Texture2D Texture { get; set; }
        public virtual Type InstanceType { get { return typeof(GameWorldObject); } }

        public IGameWorldObject Spawn(string instanceId = null)
        {
            IGameWorldObject instance = System.Activator.CreateInstance(InstanceType) as IGameWorldObject;
            instance.Base = this;
            instance.Id = instanceId ?? Guid.NewGuid().ToString();
            return instance;
        }
    }
}
