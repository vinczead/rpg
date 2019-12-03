using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Models.BaseInterfaces;
using GameModel.Models.InstanceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rpg.Models;

namespace GameModel.Models
{
    public class World : IWorld
    {
        public Dictionary<string, IGameObjectBase> GameObjectBases { get; set; } = GameObjectBaseManager.GameObjectBases;
        public Dictionary<string, IGameObject> GameObjects { get; set; } = new Dictionary<string, IGameObject>();
        public List<string> Messages { get; set; } = new List<string>();

        public IPlayer Player { get; set; }

        public void Update(GameTime gameTime)
        {
            Player.Room.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Player.Room.Draw(gameTime, spriteBatch);

            var font = TextureManager.font;
            for (int i = 0; i < Messages.Count; i++)
            {
                spriteBatch.DrawString(font, Messages[i], new Vector2(0, i * font.LineSpacing), Color.White);
            }
        }

        public World()
        {
            //build example GameModel
            Room exampleRoom = new Room()
            {
                Id = "ExampleRoom",
                Name = "Example Room",
                Width = 500,
                Height = 300,
                Texture = TextureManager.gray,
                World = this
            };
            Room otherRoom = new Room()
            {
                Id = "OtherRoom",
                Name = "Other Room",
                Width = 300,
                Height = 500,
                Texture = TextureManager.green,
                World = this
            };
            GameObjects.Add(exampleRoom.Id, exampleRoom);
            GameObjects.Add(otherRoom.Id, otherRoom);
            Spawn("Player", "ExampleRoom", new Vector2(400, 150), "PlayerInstance");
            Spawn("RingOfExample", "ExampleRoom", new Vector2(200, 100), "RingOfExampleInstance");
            Player = GameObjects["PlayerInstance"] as Player;
            Player.Variables["var1"] = new Variable
            {
                Name = "var1",
                Type = typeof(string),
                Value = "hello world from Player.Variables"
            };
        }

        public void Spawn(string baseId, string roomId, Vector2 position, string instanceId = null)
        {
            var instance = (GameObjectBases[baseId] as IGameWorldObjectBase).Spawn(instanceId);
            instance.Room = GameObjects[roomId] as IRoom;
            instance.Position = position;

            GameObjects.Add(instance.Id, instance);
            (GameObjects[roomId] as IRoom).InsertGameWorldObject(instance);
        }

        public void Spawn(string baseId, string instanceId = null)
        {
            Spawn(baseId, Player.Room.Id, Player.Position, instanceId);
        }

        public void Message(string message)
        {
            Messages.Add(message);
        }

        public object GetById(string id)
        {
            if (GameObjectBases.TryGetValue(id, out IGameObjectBase gameObjectBase))
            {
                return gameObjectBase;
            }

            if (GameObjects.TryGetValue(id, out IGameObject gameObject))
            {
                return gameObject;
            }

            throw new IdNotFoundException(id);
        }
    }
}
