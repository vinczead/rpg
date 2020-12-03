using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ConsumableInstance : ItemInstance
    {
        public void Consume(CharacterInstance character)
        {
            character.Items.Remove(this);
            World.Instance.Instances.Remove(Id);
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], character.Id);
            ExecutionVisitor.ExecuteRunBlock(this, "Consumed", new List<Symbol>() { characterSymbol });
        }
    }
}
