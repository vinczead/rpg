using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ActivateVisitor : Visitor
    {
        private readonly CharacterInstance character;
        public ActivateVisitor(CharacterInstance character)
        {
            this.character = character;
        }

        public override void Visit(ActivatorInstance activator)
        {
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], character.Id);
            ExecutionVisitor.ExecuteRunBlock(activator, "Activated", new List<Symbol>() { characterSymbol });
            success = true;
        }
    }
}
