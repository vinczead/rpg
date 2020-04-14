using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameScript.Models;
using GameScript.Models.InstanceClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Screens
{
    public class GameScreen : Screen
    {
        public GameScript.Models.GameModel world { get; set; }

        public GameScreen(RpgGame game) : base(game)
        {
            world = new GameScript.Models.GameModel();
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
                ThingInstance selectedThing = null;

                if (selectedThing is ItemInstance)
                {
                    (selectedThing as ItemInstance).PickUp(world.Player);
                }
            }
            if (WasKeyPressed(Keys.F12))
            {
                game.AddScreen(new ConsoleScreen(game, world));
            }

            if (WasKeyPressed(Keys.I))
            {
                game.AddScreen(new InventoryScreen(game, world));
            }
        }
    }
}