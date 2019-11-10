using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Screens
{
    public class ConsoleScreen : Screen
    {
        private string command = "Spawn($RingOfExample, $ExampleRoom, 10, 10, \"Ring2\")";
        private List<string> history = new List<string>();

        public ConsoleScreen(RpgGame game) : base(game)
        {
            IsOverlay = true;
            history.Add("--- RpgGame Console ---");
            history.Add("Enter GameScript statement below...");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var font = TextureManager.font;
            for (int i = 0; i < history.Count; i++)
            {
                spriteBatch.DrawString(font, history[i], new Vector2(0, i * font.LineSpacing), Color.White);
            }
            spriteBatch.DrawString(font, $"> {command}", new Vector2(0, 200), Color.Gray);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (releasedKeys.Contains(Keys.A))
                command += "a";
            if (WasKeyPressed(Keys.Enter))
            {
                //call script runner

                history.Add(command);
                command = "";
            }
            if (WasKeyPressed(Keys.Escape))
            {
                game.RemoveScreen(this);
            }
        }
    }
}
