using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CreatureInstance : ThingInstance
    {
        public int CurrHealth { get; set; }
        public virtual int Damage => (Breed as Creature).Strength;

        public State State { get; set; }
        public Direction Direction { get; set; }
        public override string StateString => $"{State}_{Direction}";
    }

    public enum State
    {
        Idle,
        Move,
        Dead
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
