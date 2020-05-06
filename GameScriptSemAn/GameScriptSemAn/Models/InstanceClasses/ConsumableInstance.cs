using GameScript.Models.Script;
using GameScript.Visitors;
using System.Collections.Generic;

namespace GameScript.Models.InstanceClasses
{
    public class ConsumableInstance : ItemInstance
    {
        public void Consume(CharacterInstance character)
        {
            var parameters = new List<Symbol>()
            {
                new Symbol("Self", TypeSystem.Instance["ConsumableInstance"], this.Id),
                new Symbol("Actor", TypeSystem.Instance["CharacterInstance"], character.Id),
            };

            ExecutionVisitor.ExecuteRunBlock(character.GameModel, this, "Consumed", parameters);
            character.Items.Remove(this);
        }
    }
}