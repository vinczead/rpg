using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models.InstanceInterfaces
{
    public interface ICreature : IGameWorldObject
    {
        int CurrentHealth { get; set; }
        void Attack(ICreature attacker);
        void Kill();
        double GetHealth();
        void SetHealth(double health);
    }
}
