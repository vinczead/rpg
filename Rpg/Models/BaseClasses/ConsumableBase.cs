using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public class ConsumableBase : Models.ItemBase
    {
        public override Type InstanceType { get { return typeof(Consumable); } }
    }
}