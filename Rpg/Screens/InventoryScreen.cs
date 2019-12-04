using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel;
using GameModel.Models;
using GameModel.Models.InstanceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Screens
{
    public class InventoryScreen : Screen
    {
        private World world;
        private int selectedItem = 0;

        public InventoryScreen(RpgGame game, World world) : base(game)
        {
            IsOverlay = true;
            this.world = world;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var font = TextureManager.font;
            int i = 0;

            spriteBatch.DrawString(font, "Inventory", new Vector2(0, 200), Color.Gray);
            foreach (var item in world.Player.Items)
            {
                spriteBatch.DrawString(font, item.Base.Name, new Vector2(0, i * font.LineSpacing), i == selectedItem ? Color.Yellow : Color.White);
                i++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (WasKeyPressed(Keys.Down))
            {
                selectedItem = Math.Min(world.Player.Items.Count, selectedItem + 1);
            }

            if (WasKeyPressed(Keys.Up))
            {
                selectedItem = Math.Max(0, selectedItem - 1);
            }

            if (WasKeyPressed(Keys.Space))
            {
                world.Player.DropItem(world.Player.Items[selectedItem]);
            }

            if (WasKeyPressed(Keys.Escape))
            {
                game.RemoveScreen(this);
            }
        }
    }
}
