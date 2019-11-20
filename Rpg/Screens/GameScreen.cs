using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameModel.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Screens
{
    public class GameScreen : Screen
    {
        public World world { get; set; }

        public GameScreen(RpgGame game) : base(game)
        {
            world = new World();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            world.Draw(gameTime, spriteBatch);
            if (releasedKeys.Length > 0)
                Console.WriteLine(releasedKeys.Length);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            world.Update(gameTime);
            if (WasKeyPressed(Keys.Space))
            {
                GameObject selectedGameObject = null;

                if (selectedGameObject is Item)
                {
                    (selectedGameObject as Item).PickUp(world.Player);
                }
            }
            if (WasKeyPressed(Keys.F12))
            {
                game.AddScreen(new ConsoleScreen(game, world));
            }
        }
    }
}