using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class PickUpVisitor : Visitor
    {
        private readonly CharacterInstance picker;
        public PickUpVisitor(CharacterInstance picker)
        {
            this.picker = picker;
        }

        public override void Visit(ItemInstance item)
        {
            item.Region.RemoveInstance(item);
            picker.Items.Add(item);
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], picker.Id);
            ExecutionVisitor.ExecuteRunBlock(item, "PickedUp", new List<Symbol>() { characterSymbol });
            success = true;
        }
        public override void Visit(ConsumableInstance consumable)
        {
            Visit(consumable as ItemInstance);
        }

        public override void Visit(EquipmentInstance equipment)
        {
            Visit(equipment as ItemInstance);
        }
    }
}
