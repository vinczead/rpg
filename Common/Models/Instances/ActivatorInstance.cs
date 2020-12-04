using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ActivatorInstance : ThingInstance
    {
        public bool Activated { get; set; }
        public override string StateString => Activated ? "Active" : "Inactive";

        public void Activate(CharacterInstance character)
        {
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], character.Id);
            ExecutionVisitor.ExecuteRunBlock(this, "Activated", new List<Symbol>() { characterSymbol });
        }
    }
}
