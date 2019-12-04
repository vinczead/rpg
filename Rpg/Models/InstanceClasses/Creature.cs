using GameModel.Models.InstanceInterfaces;

namespace Rpg.Models
{
    public class Creature : GameWorldObject, ICreature
    {
        public int CurrentHealth { get; set; }

        public void Attack(ICreature attacker)
        {

        }

        public double GetHealth()
        {
            return CurrentHealth;
        }

        public void Kill()
        {

        }

        public void SetHealth(double health)
        {
            CurrentHealth = (int)health;
        }
    }
}