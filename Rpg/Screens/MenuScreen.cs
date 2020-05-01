using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameScript.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Screens
{
    public abstract class MenuScreen : Screen
    {
        protected delegate void ItemHandler();
        protected Dictionary<string, ItemHandler> items { get; set; } = new Dictionary<string, ItemHandler>();
        private int selected;

        public MenuScreen(RpgGame game) : base(game) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var horizontalCenter = spriteBatch.GraphicsDevice.Viewport.Width / 2;
            var verticalCenter = spriteBatch.GraphicsDevice.Viewport.Height / 2;
            var font = TextureManager.font;

            for (int i = 0; i < items.Count; i++)
            {
                var color = i == selected ? Color.Yellow : Color.Gray;
                spriteBatch.DrawString(font, items.Keys.ElementAt(i), new Vector2(0, 0 + i * 15), color);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (WasKeyPressed(Keys.Up))
                selected = selected == 0 ? selected : selected - 1;
            if (WasKeyPressed(Keys.Down))
                selected = selected == items.Count - 1 ? selected : selected + 1;

            if (WasKeyPressed(Keys.Enter))
                items.Values.ElementAt(selected)();
        }
    }
}