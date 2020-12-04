using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Utility
{
    public static class GuiHelper
    {
        public static void DrawCenteredTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            if (texture == null)
                throw new ArgumentNullException("texture");

            var topLeftPosition = new Vector2((int)(position.X - texture.Width / 2), (int)(position.Y - texture.Height / 2));
            spriteBatch.Draw(texture, topLeftPosition, color);
        }

        public static void DrawCenteredTextureStretched(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Vector2 size, Color color)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            if (texture == null)
                throw new ArgumentNullException("texture");

            var topLeftPosition = new Vector2((int)(position.X - size.X / 2), (int)(position.Y - size.Y / 2));
            spriteBatch.Draw(texture, new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, (int)size.X, (int)size.Y), color);
        }

        public static void DrawCenteredText(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            if (font == null)
                throw new ArgumentNullException("font");

            if (string.IsNullOrWhiteSpace(text))
                return;

            var textSize = font.MeasureString(text);
            var topLeftPosition = new Vector2((int)(position.X - textSize.X / 2), (int)(position.Y - textSize.Y / 2));

            spriteBatch.DrawString(font, text, topLeftPosition, color);
        }
    }
}
