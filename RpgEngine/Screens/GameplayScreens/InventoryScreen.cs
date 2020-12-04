using Common.Models;
using Common.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Screens
{
    class InventoryScreen : MenuScreen
    {
        private int firstItemIndex = 0;
        private const int MAX_DISPLAYED_ITEMS = 4;
        private Vector2 inventorySize = new Vector2(250, 150);
        public InventoryScreen()
        {
            IsOverlay = true;
            BlockScreenUpdatesBelow = false;
            for (int i = 0; i < World.Instance.Player.Items.Count; i++)
            {
                var item = World.Instance.Player.Items[i];
                var menuItem = new MenuItem(item.Breed.Name)
                {
                    Font = Assets.StandardFont,
                    Data = item,
                    Description = (item.Breed as Item).Description
                };
                menuItem.Selected += ItemSelected;
                MenuItems.Add(menuItem);
            }
            updateMenuItemPositions();
        }

        private void updateMenuItemPositions()
        {
            for (int i = 0; i < MenuItems.Count; i++)
            {
                var menuItem = MenuItems[i];
                if (i >= firstItemIndex && i < firstItemIndex + MAX_DISPLAYED_ITEMS)
                {
                    var relativePosition = i - firstItemIndex;
                    menuItem.Position = new Vector2(
                        Constants.CanvasWidth / 2 - Assets.InventoryBackground.Width / 2 + 5,
                        Constants.CanvasHeight / 2 - Assets.InventoryBackground.Height / 2 + 30 + relativePosition * 15);
                }
                else
                {
                    menuItem.Position = new Vector2(-100, -100);
                }
            }
        }

        private void ItemSelected(object sender, EventArgs e)
        {
            var itemInstance = (sender as MenuItem).Data as ItemInstance;
            if (itemInstance is ConsumableInstance)
            {
                (itemInstance as ConsumableInstance).Consume(World.Instance.Player);
                MenuItems.Remove(sender as MenuItem);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = ScreenManager.SpriteBatch;

            GuiHelper.DrawCenteredTextureStretched(sb, Assets.TransparentBox, Constants.Canvas / 2, inventorySize, Assets.SemiTransparentBlack);
            var titlePosition = new Vector2(Constants.CanvasWidth / 2, Constants.CanvasHeight / 2 - inventorySize.Y / 2 + 10);
            GuiHelper.DrawCenteredText(sb, Assets.StandardFont, "Inventory", titlePosition, Assets.StandardTextColor);

            if (firstItemIndex != 0)
            {
                var upArrowPosition = new Vector2(
                        Constants.CanvasWidth / 2 - inventorySize.X / 2 + 5,
                        Constants.CanvasHeight / 2 - inventorySize.Y / 2 + 15);
                sb.DrawString(Assets.StandardFont, @"/\", upArrowPosition, Assets.StandardTextColor);
            }

            if (firstItemIndex + MAX_DISPLAYED_ITEMS < MenuItems.Count)
            {
                var upArrowPosition = new Vector2(
                        Constants.CanvasWidth / 2 - inventorySize.X / 2 + 5,
                        Constants.CanvasHeight / 2 - inventorySize.Y / 2 + 30 + (MAX_DISPLAYED_ITEMS) * 15);
                sb.DrawString(Assets.StandardFont, @"\/", upArrowPosition, Assets.StandardTextColor);
            }

            var descriptionPosition = new Vector2(
                Constants.CanvasWidth / 2,
                Constants.CanvasHeight / 2 + inventorySize.Y / 2 - 30);
            GuiHelper.DrawCenteredText(sb, Assets.StandardFont, SelectedItem?.Description ?? "", descriptionPosition, Assets.HighlightedTextColor);

            var helpTextPosition = new Vector2(
                Constants.CanvasWidth / 2,
                Constants.CanvasHeight / 2 + inventorySize.Y / 2 - 13);
            GuiHelper.DrawCenteredText(sb, Assets.StandardFont, "[Tab] Close [Space] Use [Q] Drop", helpTextPosition, Assets.StandardTextColor);
            base.Draw(gameTime);
        }

        public override void HandleInput()
        {
            if (InputHandler.WasActionJustReleased(InputHandler.Action.Inventory))
            {
                ScreenManager.RemoveScreen(this);
            }

            if (InputHandler.WasActionJustReleased(InputHandler.Action.DropItem))
            {
                if (SelectedItem != null)
                {
                    var itemInstance = SelectedItem.Data as ItemInstance;
                    World.Instance.Player.DropItem(itemInstance);
                    MenuItems.Remove(SelectedItem);
                }
            }

            base.HandleInput();
            if (SelectedIndex >= firstItemIndex + MAX_DISPLAYED_ITEMS)
                firstItemIndex++;
            if (SelectedIndex < firstItemIndex)
                firstItemIndex--;
            updateMenuItemPositions();
        }
    }
}
