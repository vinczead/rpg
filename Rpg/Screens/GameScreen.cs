using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameScript.Models;
using GameScript.Models.InstanceClasses;
using GameScript.Models.Script;
using GameScript.Visitors;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Screens
{
    public class GameScreen : Screen
    {
        public GameModel gameModel { get; set; }

        public GameScreen(RpgGame game) : base(game)
        {
            var scriptFile = ScriptFile.CreateFromFile("SampleWorld.gsc");

            gameModel = ExecutionVisitor.Build(new List<ScriptFile>() { scriptFile}, out _);
			gameModel.Player = gameModel.Instances["$PC"] as PlayerInstance;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameModel.Draw(gameTime, spriteBatch);
            if (releasedKeys.Length > 0)
                Console.WriteLine(releasedKeys.Length);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            gameModel.Update(gameTime);
            if (WasKeyPressed(Keys.Space))
            {
                ThingInstance selectedThing = null;

                if (selectedThing is ItemInstance)
                {
                    (selectedThing as ItemInstance).PickUp(gameModel.Player);
                }
            }
            if (WasKeyPressed(Keys.F12))
            {
                game.AddScreen(new ConsoleScreen(game, gameModel));
            }

            if (WasKeyPressed(Keys.I))
            {
                game.AddScreen(new InventoryScreen(game, gameModel));
            }
        }
    }
}