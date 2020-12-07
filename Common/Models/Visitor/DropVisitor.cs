using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class DropVisitor : Visitor
    {
        private readonly CharacterInstance dropper;
        public DropVisitor(CharacterInstance dropper)
        {
            this.dropper = dropper;
        }

        public override void Visit(ItemInstance item)
        {
            dropper.Items.Remove(item);
            dropper.Region.AddInstance(item);
            item.Position = dropper.Position;
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], dropper.Id);
            ExecutionVisitor.ExecuteRunBlock(item, "Dropped", new List<Symbol>() { characterSymbol });
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
