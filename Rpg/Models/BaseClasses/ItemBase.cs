using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public abstract class ItemBase : GameObjectBase
    {
        public override Type InstanceType { get { return typeof(Item); } }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Weight { get; set; }
    }
}