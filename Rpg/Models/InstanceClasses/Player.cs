using GameModel.Models.InstanceInterfaces;
using GameScript;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Rpg.Models
{
    public class Player : Character, IPlayer
    {
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                Position = new Vector2(Position.X, Position.Y + (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                Position = new Vector2(Position.X, Position.Y - (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                Position = new Vector2(Position.X - (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10, Position.Y);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                Position = new Vector2(Position.X + (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10, Position.Y);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                foreach (var gameObject in Room.GameWorldObjects.Values)
                {
                    if (gameObject is IItem)
                    {
                        var item = gameObject as IItem;
                        if ((item.Position - Position).Length() < 10)
                        {
                            item.PickUp(this);
                            Executer.ExecuteRunBlock(this, "WhenPickedUpItem");
                            break;
                        }
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}