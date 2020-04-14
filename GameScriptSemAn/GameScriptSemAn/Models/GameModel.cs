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
    public class GameModel
    {
        public Dictionary<string, GameObject> Bases { get; set; } = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObjectInstance> Instances { get; set; } = new Dictionary<string, GameObjectInstance>();
        public Dictionary<string, Region> Rooms { get; set; } = new Dictionary<string, Region>();

        public Queue<string> Messages { get; set; } = new Queue<string>();
        double messagesDeleteTimer = 0;

        public PlayerInstance Player { get; set; }

        public GameModel()
        {
            Rooms.Add("FirstRoom", new Region()
            {
                Id = "FirstRoom",
                Name = "First Room of Dungeon",
                GameModel = this,
                Width = 100,
                Height = 100
            });
        }

        public void Update(GameTime gameTime)
        {
            Player.Region.Update(gameTime);
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
            Player.Region.Draw(gameTime, spriteBatch);

            SpriteFont font = null;
            int i = 0;
            foreach (var message in Messages)
            {
                spriteBatch.DrawString(font, message, new Vector2(0, i * font.LineSpacing), Color.White);
                i++;
            }

            spriteBatch.DrawString(font, $"Player Health: {Player.CurrentHealth}", new Vector2(0, 300), Color.Red);
        }

        public GameObjectInstance Spawn(string baseId, string roomId, Vector2 position, string instanceId = null)
        {
            var instance = Spawn(baseId, instanceId) as ThingInstance;
            instance.Region = Rooms[roomId];
            instance.Position = position;

            Rooms[roomId].InsertThing(instance);

            return instance;
        }

        public GameObjectInstance Spawn(string baseId, string instanceId = null)
        {
            var instance = Bases[baseId].Spawn(instanceId);
            instance.World = this;

            Instances.Add(instance.Id, instance);

            Executer.ExecuteVariableDeclaration(instance);

            return instance;
        }

        public void SpawnAtPlayer(string baseId, string instanceId = null)
        {
            Spawn(baseId, Player.Region.Id, Player.Position, instanceId);
        }

        public void Message(string message)
        {
            Messages.Enqueue(message);
        }

        public object GetById(string id)
        {
            if (Bases.TryGetValue(id, out GameObject gameObject))
            {
                return gameObject;
            }

            if (Instances.TryGetValue(id, out GameObjectInstance gameObjectInstance))
            {
                return gameObjectInstance;
            }

            throw new ArgumentException("No GameObject was found with the specified id.", "id");
        }
    }
}
