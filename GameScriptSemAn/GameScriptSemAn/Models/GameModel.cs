using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameScript.Models.BaseClasses;
using GameScript.Models.InstanceClasses;
using GameScript.Models.Script;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameScript.Models
{
    public class GameModel
    {
        public Dictionary<string, GameObject> Bases { get; set; } = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObjectInstance> Instances { get; set; } = new Dictionary<string, GameObjectInstance>();
        public Dictionary<string, Region> Regions { get; set; } = new Dictionary<string, Region>();

        public Queue<string> Messages { get; set; } = new Queue<string>();
        double messagesDeleteTimer = 0;

        public PlayerInstance Player { get; set; }

        public GameModel()
        {
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

            SpriteFont font = TextureManager.font;
            int i = 0;
            foreach (var message in Messages)
            {
                spriteBatch.DrawString(font, message, new Vector2(0, i * font.LineSpacing), Color.White);
                i++;
            }

            spriteBatch.DrawString(font, $"Player Health: {Player.CurrentHealth}", new Vector2(0, 300), Color.Red);
        }

        public GameObjectInstance Spawn(string baseId, string regionId, Vector2 position, string instanceId = null)
        {
            var instance = Spawn(baseId, regionId, instanceId) as ThingInstance;

            instance.Position = position;

            return instance;
        }

        public GameObjectInstance Spawn(string baseId, string regionId, string instanceId = null)
        {
            var instance = Bases[baseId].Spawn(instanceId) as ThingInstance;
            instance.World = this;
            Regions[regionId].InsertThing(instance);

            Instances.Add(instance.Id, instance);

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

            if (Regions.TryGetValue(id, out Region region))
            {
                return region;
            }

            throw new ArgumentException("No GameObject was found with the specified id.", "id");
        }

        public Env ToEnv()
        {
            //ezt eleg lenne egyszer letrehozni
            var env = new Env(null, "Game Model");
            foreach (var b in Bases)
            {
                env[b.Key] = new Symbol(b.Key, TypeSystem.Instance[b.Value.GetType().Name], b.Key);
            }

            foreach (var i in Instances)
            {
                env[i.Key] = new Symbol(i.Key, TypeSystem.Instance[i.Value.GetType().Name], i.Key);
            }

            foreach (var r in Regions)
            {
                env[r.Key] = new Symbol(r.Key, TypeSystem.Instance[r.Value.GetType().Name], r.Key);
            }

            return env;
        }
    }
}
