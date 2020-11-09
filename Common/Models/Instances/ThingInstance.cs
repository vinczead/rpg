using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ThingInstance
    {
        public string Id { get; set; }
        public bool IsIdGenerated { get; set; }
        public Thing Breed { get; set; }
        public Vector2 Position { get; set; }
        public TimeSpan AnimationTime { get; set; }
        public virtual string StateString => $"Idle";
        public Region Region { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            AnimationTime += gameTime.ElapsedGameTime;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var spriteSheet = Breed.Model.SpriteSheet;
            var drawPosition = Position - new Vector2(spriteSheet.Value.Width / 2, spriteSheet.Value.Height);

            spriteBatch.Draw(Breed.Model.SpriteSheet.Value, drawPosition, Breed.Model[StateString].FrameAt(AnimationTime).Source, Color.White);
        }
    }
}
