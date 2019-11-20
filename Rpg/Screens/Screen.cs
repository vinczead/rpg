using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Screens
{
    public abstract class Screen
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Update(GameTime gameTime)
        {
            var currentPressedKeys = Keyboard.GetState().GetPressedKeys();

            releasedKeys = prevPressedKeys.Where(k => !currentPressedKeys.Contains(k)).ToArray();

            prevPressedKeys = currentPressedKeys;
        }

        public bool IsOverlay { get; set; }

        protected RpgGame game;

        protected Keys[] prevPressedKeys = new Keys[0];
        protected Keys[] releasedKeys = new Keys[0];

        protected bool WasKeyPressed(Keys key)
        {
            return releasedKeys.Contains(key);
        }

        public Screen(RpgGame game)
        {
            this.game = game;
        }
    }
}
