using System;

namespace Rpg.Models
{
    public abstract class ItemBase : GameWorldObjectBase
    {
        public override Type InstanceType { get { return typeof(Item); } }
        public int Value { get; set; }
        public int Weight { get; set; }
    }
}