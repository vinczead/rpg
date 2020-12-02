using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Utility
{
    public static class CommonAssets
    {
        public static SpriteFont StandardFont { get; set; }
        public static Color StandardTextColor { get; set; } = new Color(255, 255, 255);
        public static Color HighlightedTextColor { get; set; } = new Color(255, 255, 0);
        public static Texture2D PopupBackground { get; set; }

        public static void LoadGameContent(ContentManager contentManager)
        {
            if (contentManager == null)
                throw new ArgumentNullException("contentManager");
            StandardFont = contentManager.Load<SpriteFont>("Fonts/StandardFont");
            PopupBackground = contentManager.Load<Texture2D>("GUI/PopupBackground");
        }
    }
}
