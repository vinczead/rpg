using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Models
{
    public class Player : Character
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

            base.Update(gameTime);
        }
    }
}