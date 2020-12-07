using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Equipment : Item
    {
        public override Type InstanceType { get { return typeof(EquipmentInstance); } }
    }
}
