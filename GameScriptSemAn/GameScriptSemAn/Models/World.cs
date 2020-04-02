using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameScript.Models.BaseClasses;
using GameScript.Models.InstanceClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameScript.Models
{
    public class World
    {
        public Dictionary<string, GameObject> GameObjects { get; set; } = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObjectInstance> GameObjectInstances { get; set; } = new Dictionary<string, GameObjectInstance>();
        public Dictionary<string, Room> Rooms { get; set; } = new Dictionary<string, Room>();

        public Queue<string> Messages { get; set; } = new Queue<string>();
        double messagesDeleteTimer = 0;

        public PlayerInstance Player { get; set; }

        public void Update(GameTime gameTime)
        {
            Player.Room.Update(gameTime);
            if (Messages.Count > 0)
                messagesDeleteTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            else
                messagesDeleteTimer = 0;
            if (messagesDeleteTimer > 1500)
            {
                if (Messages.Count > 0)
                    Messages.Dequeue();
                messagesDeleteTimer -= 1500;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Player.Room.Draw(gameTime, spriteBatch);

            SpriteFont font = null;
            int i = 0;
            foreach (var message in Messages)
            {
                spriteBatch.DrawString(font, message, new Vector2(0, i * font.LineSpacing), Color.White);
                i++;
            }

            spriteBatch.DrawString(font, $"Player Health: {Player.CurrentHealth}", new Vector2(0, 300), Color.Red);
        }

        public World()
        {

        }

        public void Spawn(string baseId, string roomId, Vector2 position, string instanceId = null)
        {
            var instance = (GameObjects[baseId] as Thing).Spawn(instanceId);
            instance.Room = Rooms[roomId];
            instance.Position = position;
            instance.World = this;

            GameObjectInstances.Add(instance.Id, instance);
            Rooms[roomId].InsertThing(instance);

            Executer.ExecuteVariableDeclaration(instance);
        }

        public void Spawn(string baseId, string instanceId = null)
        {
            Spawn(baseId, Player.Room.Id, Player.Position, instanceId);
        }

        public void Message(string message)
        {
            Messages.Enqueue(message);
        }

        public object GetById(string id)
        {
            if (GameObjects.TryGetValue(id, out GameObject gameObject))
            {
                return gameObject;
            }

            if (GameObjectInstances.TryGetValue(id, out GameObjectInstance gameObjectInstance))
            {
                return gameObjectInstance;
            }

            throw new ArgumentException("No GameObject was found with the specified id.", "id");
        }
    }
}
