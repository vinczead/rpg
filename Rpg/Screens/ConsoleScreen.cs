using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel;
using GameModel.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameScript;

namespace Rpg.Screens
{
    public class ConsoleScreen : TextInputScreen
    {
        private List<string> history = new List<string>();
        private World world;

        public ConsoleScreen(RpgGame game, World world) : base(game)
        {
            IsOverlay = true;
            this.world = world;
            Text = "=>Message(var1 + \"This is constant\")";
            history.Add("--- RpgGame Console ---");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var font = TextureManager.font;
            for (int i = 0; i < history.Count; i++)
            {
                spriteBatch.DrawString(font, history[i], new Vector2(0, i * font.LineSpacing), Color.White);
            }
            spriteBatch.DrawString(font, $"> {Text}", new Vector2(0, 200), Color.Gray);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (WasKeyPressed(Keys.Enter))
            {
                //call script runner
                Executer.ExecuteStatement(world.Player, Text);

                history.Add(Text);
                //Text = "";
            }
            if (WasKeyPressed(Keys.Escape))
            {
                game.RemoveScreen(this);
            }
        }
    }
}
