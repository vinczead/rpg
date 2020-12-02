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

        protected int SelectedItem { get; set; } = 0;

        protected MenuItem SelectedMenuItem
        {
            get
            {
                if (SelectedItem >= MenuItems.Count)
                    return null;
                return MenuItems[SelectedItem];
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            foreach (var item in MenuItems)
            {
                item.Draw(gameTime, this, item == SelectedMenuItem);
            }

            spriteBatch.End();
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
                SelectedItem--;
                if (SelectedItem < 0)
                    SelectedItem = MenuItems.Count - 1;
            }

            if (InputHandler.WasActionJustReleased(InputHandler.Action.Down))
            {
                SelectedItem++;
                if (SelectedItem >= MenuItems.Count)
                    SelectedItem = 0;
            }

            if (InputHandler.WasActionJustReleased(InputHandler.Action.Action))
            {
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
