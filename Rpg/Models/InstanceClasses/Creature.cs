using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public class Creature : Models.GameObject
    {
        public int CurrentHealth { get; set; }

        public void Attack(Creature attacker)
        {

        }

        public void Kill()
        {

        }
    }
}