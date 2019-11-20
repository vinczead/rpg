using System;

namespace GameModel.Models
{
    public class EquipmentBase : Models.ItemBase
    {
        public override Type InstanceType { get { return typeof(Equipment); } }
        public EquipmentType Type { get; set; }
    }
}