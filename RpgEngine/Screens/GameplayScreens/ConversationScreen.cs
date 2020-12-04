using Common.Models;
using Common.Script.Utility;
using Common.Script.Visitors;
using Common.Utility;
using Microsoft.Xna.Framework;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Screens
{
    class ConversationScreen : MenuScreen
    {
        private CharacterInstance otherCharacter;
        private Vector2 screenSize = new Vector2(300, 60);
        private Vector2 topicSelectorPosition = new Vector2(Constants.CanvasWidth / 2, Constants.CanvasHeight - 50);
        private Vector2 speechTextPosition = new Vector2(Constants.CanvasWidth / 2, 50);

        private TimeSpan timer = TimeSpan.Zero;

        public ConversationScreen(CharacterInstance otherCharacter)
        {
            IsOverlay = true;
            BlockScreenUpdatesBelow = false;
            this.otherCharacter = otherCharacter;
            RefreshMenuItems();
        }

        private void OnTopicSelected(object sender, MenuItemSelectedEventArgs eventArgs)
        {
            var playerSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], World.Instance.Player.Id);
            var topicSymbol = new Symbol("Topic", TypeSystem.Instance["String"], eventArgs.MenuItem.Data as string);
            ExecutionVisitor.ExecuteRunBlock(otherCharacter, "TalkedTo", new List<Symbol>() { playerSymbol, topicSymbol });
            RefreshMenuItems();
        }

        private void RefreshMenuItems()
        {
            MenuItems = new List<MenuItem>();
            foreach (var topic in otherCharacter.Topics)
            {
                MenuItems.Add(new MenuItem(topic.Value, "", OnTopicSelected)
                {
                    Data = topic.Key,
                });
            }
            SetupDefaultMenuItemPositions();
            foreach (var item in MenuItems)
            {
                item.Position = new Vector2(item.Position.X, item.Position.Y + 50);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(EngineVariables.SpeechTexts.Count > 0)
            {
                timer += gameTime.ElapsedGameTime;
                if (timer.TotalMilliseconds > 2000) {
                    EngineVariables.SpeechTexts.Dequeue();
                    timer = TimeSpan.Zero;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = ScreenManager.SpriteBatch;

            if (EngineVariables.SpeechTexts.Count == 0)
            {
                GuiHelper.DrawCenteredTextureStretched(sb, Assets.SolidBox, topicSelectorPosition, screenSize, Assets.SemiTransparentBlack);
                base.Draw(gameTime);
            } else
            {
                GuiHelper.DrawCenteredTextureStretched(sb, Assets.SolidBox, speechTextPosition, screenSize, Assets.SemiTransparentBlack);
                GuiHelper.DrawCenteredText(sb, Assets.StandardFont, EngineVariables.SpeechTexts.Peek(), speechTextPosition, Assets.StandardTextColor);
            }
        }

        public override void HandleInput()
        {
            if (InputHandler.WasActionJustReleased(InputHandler.Action.Back))
            {
                ScreenManager.RemoveScreen(this);
            }
            base.HandleInput();
        }
    }
}
