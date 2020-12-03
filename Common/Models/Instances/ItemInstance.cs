using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ItemInstance : ThingInstance
    {
        public void PickedUp(CharacterInstance character)
        {
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], character.Id);
            ExecutionVisitor.ExecuteRunBlock(this, "PickedUp", new List<Symbol>() { characterSymbol });
        }

        public void Dropped(CharacterInstance character)
        {
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], character.Id);
            ExecutionVisitor.ExecuteRunBlock(this, "Dropped", new List<Symbol>() { characterSymbol });
        }
    }
}
