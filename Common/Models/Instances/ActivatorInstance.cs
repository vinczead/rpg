using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ActivatorInstance : ThingInstance
    {
        private bool activated;
        public bool Activated
        {
            get => activated; set
            {
                activated = value;
                AnimationTime = TimeSpan.Zero;
            }
        }
        public override string StateString => Activated ? "Active" : "Inactive";

        public void Activate(CharacterInstance character)
        {
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], character.Id);
            ExecutionVisitor.ExecuteRunBlock(this, "Activated", new List<Symbol>() { characterSymbol });
        }
    }
}
