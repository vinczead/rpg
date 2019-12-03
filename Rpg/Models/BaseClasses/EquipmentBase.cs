using System;

namespace Rpg.Models
{
    public class EquipmentBase : Models.ItemBase
    {
        public override Type InstanceType { get { return typeof(Equipment); } }
        public EquipmentType Type { get; set; }
    }
}