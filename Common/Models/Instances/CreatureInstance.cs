using Common.Script.Utility;
using Common.Script.Visitors;
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
        private int currentHealth;
        public int CurrentHealth
        {
            get => currentHealth; set
            {
                currentHealth = value;
                if (currentHealth == 0)
                    State = State.Dying;
            }
        }
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

        private TimeSpan dyingTimer = TimeSpan.Zero;

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

            if (State == State.Dead)
            {
                Region.RemoveInstance(this);
                World.Instance.Instances.Remove(Id);
            }

            if (State == State.Dying)
            {
                dyingTimer += gameTime.ElapsedGameTime;
                if(dyingTimer.TotalMilliseconds >= CurrentAnimation.RoundDuration)
                {
                    State = State.Dead;
                }
            }
        }

        public void AttackedBy(CreatureInstance creatureInstance)
        {
            var damageDealt = creatureInstance.Damage - (Breed as Creature).Protection;
            CurrentHealth -= Math.Min(damageDealt, CurrentHealth);

            var creatureSymbol = new Symbol("Creature", TypeSystem.Instance["CreatureInstance"], creatureInstance.Id);
            var damageSymbol = new Symbol("Damage", TypeSystem.Instance["Number"], damageDealt.ToString());
            ExecutionVisitor.ExecuteRunBlock(this, "Attacked", new List<Symbol>() { creatureSymbol, damageSymbol });
        }
    }

    public enum State
    {
        Idle,
        Move,
        Dying,
        Dead,
        Attack
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
