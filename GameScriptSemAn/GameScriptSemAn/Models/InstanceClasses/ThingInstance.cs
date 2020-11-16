using GameScript.Models.BaseClasses;
using GameScript.Models.Script;
using GameScript.Visitors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameScript.Models.InstanceClasses
{
    public abstract class ThingInstance : GameObjectInstance
    {
        public Vector2 Position { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw((BASE as Thing).Texture, Position, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
            var elapsedMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds.ToString();
            var parameters = new List<Symbol>()
            {
                new Symbol("ElapsedTime", TypeSystem.Instance["Number"], elapsedMilliseconds)
            };
            ExecutionVisitor.ExecuteRunBlock(Region.GameModel, this, "InGame", parameters);
        }
    }
}