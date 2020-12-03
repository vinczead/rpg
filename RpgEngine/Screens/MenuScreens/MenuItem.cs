using Common.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgEngine.Screens
{
    class MenuItem
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public Vector2 Position { get; set; }
        public SpriteFont Font { get; set; }
        public object Data { get; set; }

        public event EventHandler Selected;

        protected internal virtual void OnSelectEntry()
        {
            Selected?.Invoke(this, EventArgs.Empty);
        }

        public MenuItem(string text)
        {
            Text = text;
        }

        public virtual void Update(GameTime gameTime, MenuScreen screen, bool isSelected)
        {

        }

        public virtual void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            var color = isSelected ? Assets.HighlightedTextColor : Assets.StandardTextColor;

            var spriteBatch = screen.ScreenManager.SpriteBatch;

            spriteBatch.DrawString(Font, Text, Position, color);
        }
    }
}
