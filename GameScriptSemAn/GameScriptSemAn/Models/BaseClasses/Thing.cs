using GameScript.Models.InstanceClasses;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameScript.Models.BaseClasses
{
    public abstract class Thing : GameObject
    {
        public Texture2D Texture { get; set; }
        public virtual Type InstanceType { get { return typeof(ThingInstance); } }

        public ThingInstance Spawn(string instanceId = null)
        {
            ThingInstance instance = System.Activator.CreateInstance(InstanceType) as ThingInstance;
            instance.Base = this;
            instance.Id = instanceId ?? Guid.NewGuid().ToString();
            return instance;
        }
    }
}
