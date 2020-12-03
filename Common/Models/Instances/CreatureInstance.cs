using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CreatureInstance : ThingInstance
    {
        public int CurrentHealth { get; set; }
        public virtual int Damage => (Breed as Creature).Strength;

        public State State { get; set; }
        public Direction Direction { get; set; } = Direction.Up;
        public override string StateString => $"{State}_{Direction}";

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var elapsedSecs = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (State == State.Move)
            {
                float deltaX = 0, deltaY = 0;
                switch (Direction)
                {
                    case Direction.Up:
                        deltaY = -(Breed as Creature).Speed;
                        break;
                    case Direction.Down:
                        deltaY = (Breed as Creature).Speed;
                        break;
                    case Direction.Left:
                        deltaX = -(Breed as Creature).Speed;
                        break;
                    case Direction.Right:
                        deltaX = (Breed as Creature).Speed;
                        break;
                }

                deltaX = deltaX * elapsedSecs;
                deltaY = deltaY * elapsedSecs;

                Position += new Vector2(deltaX, deltaY);
            }
        }
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
