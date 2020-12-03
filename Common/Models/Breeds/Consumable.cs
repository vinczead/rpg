using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Consumable : Item
    {
        public override Type InstanceType { get { return typeof(ConsumableInstance); } }
    }
}
