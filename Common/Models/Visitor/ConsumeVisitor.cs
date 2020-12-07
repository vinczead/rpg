using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ConsumeVisitor : Visitor
    {
        private readonly CharacterInstance consumer;
        public ConsumeVisitor(CharacterInstance consumer)
        {
            this.consumer = consumer;
        }

        public override void Visit(ConsumableInstance consumable)
        {
            consumer.Items.Remove(consumable);
            World.Instance.Instances.Remove(consumable.Id);
            var characterSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], consumer.Id);
            ExecutionVisitor.ExecuteRunBlock(consumable, "Consumed", new List<Symbol>() { characterSymbol });
            success = true;
        }
    }
}
