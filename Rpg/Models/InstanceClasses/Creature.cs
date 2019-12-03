using GameModel.Models.InstanceInterfaces;

namespace Rpg.Models
{
    public class Creature : GameWorldObject, ICreature
    {
        public int CurrentHealth { get; set; }

        public void Attack(ICreature attacker)
        {

        }

        public void Kill()
        {

        }
    }
}