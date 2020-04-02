using GameScript.Models.InstanceClasses;
using System;

namespace GameScript.Models.BaseClasses
{
    public class Equipment : Item
    {
        public override Type InstanceType { get { return typeof(EquipmentInstance); } }
        public EquipmentType Type { get; set; }
    }

    public enum EquipmentType
    {
        Weapon,
        Armor,
        Boots
    }
}