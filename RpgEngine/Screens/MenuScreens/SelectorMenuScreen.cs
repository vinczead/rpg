using Common.Utility;
using Microsoft.Xna.Framework;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgEngine.Screens
{
    class SelectorMenuScreen : MenuScreen
    {
        Vector2 menuSize;
        string title;

        private SelectorMenuScreen() { }

        public static SelectorMenuScreen Create(string title, string[] items, EventHandler<MenuItemSelectedEventArgs> menuItemSelectedEventHandler)
        {
            var titleWidth = Assets.StandardFont.MeasureString(title).X;
            var maxItemWidth = (int)items.Max(item => Assets.StandardFont.MeasureString(item).X);
            var menuWidth = Math.Max(titleWidth, maxItemWidth);
            var maxHeight = (int)items.Max(item => Assets.StandardFont.MeasureString(item).Y);
            var menuHeight = (items.Length + 2) * maxHeight;

            var screen = new SelectorMenuScreen()
            {
                MenuItems = items.Select(label => new MenuItem(label, "", menuItemSelectedEventHandler)).ToList(),
                title = title,
                IsOverlay = true,
                BlockScreenUpdatesBelow = true,
                menuSize = new Vector2(menuWidth, menuHeight) * 1.1f
            };
            screen.SetupDefaultMenuItemPositions();
            return screen;
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = ScreenManager.SpriteBatch;

            var firstItemHeight = Assets.StandardFont.MeasureString(MenuItems[0].Text).Y/2;

            GuiHelper.DrawCenteredTextureStretched(sb, Assets.SolidBox, Constants.Canvas / 2, menuSize, Assets.TranslucentBlack2);
            GuiHelper.DrawCenteredText(sb, Assets.StandardFont, title, new Vector2(Constants.CanvasWidth / 2, MenuItems[0].Position.Y - firstItemHeight), Assets.StandardTextColor);

            base.Draw(gameTime);
        }
    }
}
