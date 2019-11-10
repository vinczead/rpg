using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Models
{
    public abstract class GameObjectBase
    {
        public virtual Type InstanceType { get { return typeof(GameObject); } }
        public string Id { get; set; }
        public string Name { get; set; }
        public Texture2D Texture { get; set; }

        public string Script { get; set; }

        public GameObject Spawn(string instanceId = null)
        {
            var instance = System.Activator.CreateInstance(InstanceType) as GameObject;
            instance.Base = this;
            instance.Id = instanceId ?? Guid.NewGuid().ToString();
            return instance;
        }
    }
}
