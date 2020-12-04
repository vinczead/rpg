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
        public int CurrentMana { get; set; }
        public virtual int Damage => (Breed as Creature).Strength;

        private State state;
        public State State
        {
            get => state;
            set
            {
                state = value;
                AnimationTime = TimeSpan.Zero;
            }
        }
        private Direction direction = Direction.Up;
        public Direction Direction
        {
            get => direction; set
            {
                direction = value;
                AnimationTime = TimeSpan.Zero;
            }
        }

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

                deltaX *= elapsedSecs;
                deltaY *= elapsedSecs;

                MovementDelta = new Vector2(deltaX, deltaY);
            }
            else
            {
                MovementDelta = Vector2.Zero;
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
