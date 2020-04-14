using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace GameScript.Models.InstanceClasses
{
    public class PlayerInstance : CharacterInstance
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
                foreach (var thing in Region.Things.Values)
                {
                    if (thing is ItemInstance)
                    {
                        var item = thing as ItemInstance;
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