using GameModel.Models.BaseInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameModel.Models.InstanceInterfaces
{
    public interface IGameWorldObject : IGameObject
    {
        IGameWorldObjectBase Base { get; set; }
        IRoom Room { get; set; }
        Vector2 Position { get; set; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}
