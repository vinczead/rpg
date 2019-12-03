using System;

namespace Rpg.Models
{
    public class ConsumableBase : Models.ItemBase
    {
        public override Type InstanceType { get { return typeof(Consumable); } }
    }
}