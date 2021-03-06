﻿using Common.Models;
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
                var menuItem = new CarrierMenuItem<ItemInstance>(item.Breed.Name, item)
                {
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
                        Constants.CanvasWidth / 2 - inventorySize.X / 2 + 5,
                        Constants.CanvasHeight / 2 - inventorySize.Y / 2 + 30 + relativePosition * 15);
                }
                else
                {
                    menuItem.Position = new Vector2(-100, -100);
                }
            }
        }

        private void ItemSelected(object sender, EventArgs e)
        {
            var itemInstance = (sender as CarrierMenuItem<ItemInstance>).Data;

            var consumeVisitor = new ConsumeVisitor(World.Instance.Player);
            itemInstance.Accept(consumeVisitor);
            if (consumeVisitor.Success)
            {
                MenuItems.Remove(sender as MenuItem);
                return;
            }

            var equipVisitor = new EquipVisitor(World.Instance.Player);
            itemInstance.Accept(equipVisitor);
            if (equipVisitor.Success)
            {
                return;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = ScreenManager.SpriteBatch;

            GuiHelper.DrawCenteredTextureStretched(sb, Assets.SolidBox, Constants.Canvas / 2, inventorySize, Assets.TranslucentBlack1);
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
                    var itemInstance = (SelectedItem as CarrierMenuItem<ItemInstance>).Data;
                    itemInstance.Accept(new DropVisitor(World.Instance.Player));
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
