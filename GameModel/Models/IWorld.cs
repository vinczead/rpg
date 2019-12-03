using GameModel.Models.BaseInterfaces;
using GameModel.Models.InstanceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models
{
    public interface IWorld
    {
        Dictionary<string, IGameObjectBase> GameObjectBases { get; set; }
        Dictionary<string, IGameObject> GameObjects { get; set; }
        List<string> Messages { get; set; }
        IPlayer Player { get; set; }

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Spawn(string baseId, string roomId, Vector2 position, string instanceId = null);
        void Spawn(string baseId, string instanceId = null);
        void Message(string message);
        object GetById(string id);
    }
}
