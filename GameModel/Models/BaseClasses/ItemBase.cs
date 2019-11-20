using System;

namespace GameModel.Models
{
    public abstract class ItemBase : GameObjectBase
    {
        public override Type InstanceType { get { return typeof(Item); } }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Weight { get; set; }
    }
}