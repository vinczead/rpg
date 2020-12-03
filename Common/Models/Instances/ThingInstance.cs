using Common.Script.Utility;
using Common.Script.Visitors;
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
        public Dictionary<string, Symbol> Variables { get; set; } = new Dictionary<string, Symbol>();

        public virtual void Update(GameTime gameTime)
        {
            AnimationTime += gameTime.ElapsedGameTime;
            var elapsedTime = new Symbol("ElapsedTime", TypeSystem.Instance["Number"], gameTime.ElapsedGameTime.TotalMilliseconds.ToString());
            ExecutionVisitor.ExecuteRunBlock(this, "Updated", new List<Symbol>() { elapsedTime });
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var currentFrame = Breed.Model[StateString].FrameAt(AnimationTime).Source;
            var spriteSheet = Breed.Model.SpriteSheet;
            var drawPosition = Position - new Vector2(currentFrame.Width / 2, currentFrame.Height);
            var intDrawPosition = new Vector2((int)drawPosition.X, (int)drawPosition.Y);

            spriteBatch.Draw(spriteSheet.Value, intDrawPosition, currentFrame, Color.White);
        }
    }
}
