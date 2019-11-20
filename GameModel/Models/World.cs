using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameModel.Models
{
    public class World
    {
        public Dictionary<string, GameObjectBase> GameObjectBases { get; set; } = GameObjectBaseManager.GameObjectBases;
        public Dictionary<string, GameObject> GameObjects { get; set; } = new Dictionary<string, GameObject>();
        public List<string> Messages { get; set; } = new List<string>();

        public Dictionary<string, Room> Rooms { get; set; } = new Dictionary<string, Room>();
        public Dictionary<string, Quest> Quests { get; set; } = new Dictionary<string, Quest>();

        public Player Player { get; set; }

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
            Rooms.Add(exampleRoom.Id, exampleRoom);
            Rooms.Add(otherRoom.Id, otherRoom);
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
            var instance = GameObjectBases[baseId].Spawn(instanceId);
            instance.Room = Rooms[roomId];
            instance.Position = position;
            
            GameObjects.Add(instance.Id, instance);
            Rooms[roomId].InsertGameObject(instance);
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
            if (GameObjectBases.TryGetValue(id, out GameObjectBase gameObjectBase))
            {
                return gameObjectBase;
            }

            if (GameObjects.TryGetValue(id, out GameObject gameObject))
            {
                return gameObject;
            }

            if (Rooms.TryGetValue(id, out Room room))
            {
                return room;
            }

            if (Quests.TryGetValue(id, out Quest quest))
            {
                return quest;
            }

            throw new IdNotFoundException(id);
        }
    }
}
