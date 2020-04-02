using GameScript.Models.InstanceClasses;
using System;

namespace GameScript.Models.BaseClasses
{
    public abstract class Item : Thing
    {
        public override Type InstanceType { get { return typeof(ItemInstance); } }
        public int Value { get; set; }
        public int Weight { get; set; }
    }
}