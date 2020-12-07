using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class EquipVisitor : Visitor
    {
        private readonly CharacterInstance equipper;
        public EquipVisitor(CharacterInstance equipper)
        {
            this.equipper = equipper;
        }

        public override void Visit(EquipmentInstance equipment)
        {
            //todo: check if equipped/unequipped
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], equipper.Id);
            ExecutionVisitor.ExecuteRunBlock(equipment, "Equipped", new List<Symbol>() { characterSymbol });
            success = true;
        }
    }
}
