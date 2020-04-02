
namespace GameScript.Models.InstanceClasses
{
    public class CreatureInstance : ThingInstance
    {
        public int CurrentHealth { get; set; }

        public void Attack(CreatureInstance attacker)
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