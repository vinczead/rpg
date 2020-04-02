using GameScript.Models.InstanceClasses;
using System;

namespace GameScript.Models.BaseClasses
{
    public class Consumable : Item
    {
        public override Type InstanceType { get { return typeof(ConsumableInstance); } }
    }
}