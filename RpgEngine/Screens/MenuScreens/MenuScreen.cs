using Common.Utility;
using Microsoft.Xna.Framework;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgEngine.Screens
{
    abstract class MenuScreen : Screen
    {
        protected List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        protected int SelectedIndex { get; set; } = 0;

        protected MenuItem SelectedItem
        {
            get
            {
                if (SelectedIndex >= MenuItems.Count || SelectedIndex < 0)
                    return null;
                return MenuItems[SelectedIndex];
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var item in MenuItems)
            {
                item.Draw(gameTime, this, item == SelectedItem);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var item in MenuItems)
            {
                item.Update(gameTime, this, item == SelectedItem);
            }
        }

        public override void HandleInput()
        {
            if (InputHandler.WasActionJustReleased(InputHandler.Action.Up))
            {
                SelectedIndex--;
                if (SelectedIndex < 0)
                    SelectedIndex = 0;
            }

            if (InputHandler.WasActionJustReleased(InputHandler.Action.Down))
            {
                SelectedIndex++;
                if (SelectedIndex >= MenuItems.Count)
                    SelectedIndex = Math.Max(MenuItems.Count - 1, 0);
            }

            if (InputHandler.WasActionJustReleased(InputHandler.Action.Action))
            {
                if (SelectedItem != null)
                    SelectedItem.OnSelectEntry();
            }

            if (InputHandler.WasActionJustReleased(InputHandler.Action.Back))
            {
                OnCancel();
            }
        }

        protected virtual void OnCancel()
        {
            ScreenManager.RemoveScreen(this);
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }
        
        protected void SetupDefaultMenuItemPositions()
        {
            var centerScreenX = Constants.CanvasWidth / 2;
            var centerScreenY = Constants.CanvasHeight / 2;
            var maxHeight = MenuItems.Max(item => Assets.StandardFont.MeasureString(item.Text).Y);
            var firstItemY = centerScreenY - (MenuItems.Count / 2) * maxHeight;
            if (MenuItems.Count % 2 == 1)
                firstItemY -= maxHeight / 2;

            for (int i = 0; i < MenuItems.Count; i++)
            {
                var item = MenuItems[i];
                var textSize = Assets.StandardFont.MeasureString(item.Text);

                var x = (int)(centerScreenX -  textSize.X / 2);
                var y = (int)(firstItemY + i * maxHeight);
                item.Position = new Vector2(x, y);
            }
        }
    }
}
