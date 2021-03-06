﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility
{
    public static class Assets
    {
        public static SpriteFont StandardFont { get; set; }
        public static Color StandardTextColor { get; set; } = new Color(255, 255, 255);
        public static Color HighlightedTextColor { get; set; } = new Color(255, 255, 0);
        public static Color SemiTransparentWhite { get; set; } = new Color(255, 255, 255, 125);
        public static Color TranslucentBlack1 { get; set; } = new Color(0, 0, 0, 125);
        public static Color TranslucentBlack2 { get; set; } = new Color(0, 0, 0, 200);
        public static Color SemiTransparentRed { get; set; } = new Color(255, 0, 0, 200);

        public static Texture2D PopupBackground { get; set; }
        public static Texture2D TransparentBox { get; set; }
        public static Texture2D SolidBox { get; set; }
        public static Texture2D MenuBackground { get; set; }

        public static void LoadGameContent(ContentManager contentManager)
        {
            if (contentManager == null)
                throw new ArgumentNullException("contentManager");
            StandardFont = contentManager.Load<SpriteFont>("Fonts/StandardFont");
            PopupBackground = contentManager.Load<Texture2D>("GUI/PopupBackground");
            TransparentBox = contentManager.Load<Texture2D>("GUI/TransparentBox");
            SolidBox = contentManager.Load<Texture2D>("GUI/SolidBox");
            MenuBackground = contentManager.Load<Texture2D>("GUI/MenuBackground");
        }
    }
}
