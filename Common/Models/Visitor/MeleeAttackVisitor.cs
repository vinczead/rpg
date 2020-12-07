using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class MeleeAttackVisitor : Visitor
    {
        private readonly CreatureInstance attacker;
        public MeleeAttackVisitor(CreatureInstance attacker)
        {
            this.attacker = attacker;
        }

        public override void Visit(CreatureInstance defender)
        {
            var damage = attacker.Damage - (defender.Breed as Creature).Protection;
            var damageDealt = Math.Min(damage, defender.CurrentHealth);
            defender.CurrentHealth -= damageDealt;

            var creatureSymbol = new Symbol("Creature", TypeSystem.Instance["CreatureInstance"], attacker.Id);
            var damageSymbol = new Symbol("Damage", TypeSystem.Instance["Number"], damageDealt.ToString());
            ExecutionVisitor.ExecuteRunBlock(defender, "Attacked", new List<Symbol>() { creatureSymbol, damageSymbol });

            success = true;
        }

        public override void Visit(CharacterInstance character)
        {
            Visit(character as CreatureInstance);
        }
    }
}
