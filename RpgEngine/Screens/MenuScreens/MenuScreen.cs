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
        protected List<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();

        protected int SelectedIndex { get; set; } = 0;

        protected MenuItem SelectedMenuItem
        {
            get
            {
                if (SelectedIndex >= MenuItems.Count)
                    return null;
                return MenuItems[SelectedIndex];
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var item in MenuItems)
            {
                item.Draw(gameTime, this, item == SelectedMenuItem);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var item in MenuItems)
            {
                item.Update(gameTime, this, item == SelectedMenuItem);
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
                    SelectedIndex = MenuItems.Count - 1;
            }

            if (InputHandler.WasActionJustReleased(InputHandler.Action.Action))
            {
                if (SelectedMenuItem != null)
                    SelectedMenuItem.OnSelectEntry();
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
    }
}
