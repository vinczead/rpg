using Common.Script.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Models
{
    public class World
    {
        private static readonly object padlock = new object();
        private static World instance = null;
        public static World Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null) instance = new World();
                    return instance;
                }
            }
        }

        public Game Game { get; set; }
        public Dictionary<string, Texture2D> Textures { get; set; }
        public Dictionary<string, SpriteModel> Models { get; set; }
        public Dictionary<string, Thing> Breeds { get; set; }
        public Dictionary<string, ThingInstance> Instances { get; set; }
        public Dictionary<string, Tile> Tiles { get; set; }
        public Dictionary<string, Region> Regions { get; set; }

        public CharacterInstance Player { get; set; }

        World()
        {
            Clear();
        }

        public void Clear()
        {
            Breeds = new Dictionary<string, Thing>();
            Instances = new Dictionary<string, ThingInstance>();
            Regions = new Dictionary<string, Region>();
            Textures = new Dictionary<string, Texture2D>();
            Models = new Dictionary<string, SpriteModel>();
            Tiles = new Dictionary<string, Tile>();
            Player = null;
        }

        public void Update(GameTime gameTime)
        {
            Player.Region.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Region.Draw(spriteBatch);
        }

        public ThingInstance Spawn(string baseId, string regionId, Vector2 position, string instanceId = null)
        {
            var instance = Spawn(baseId, regionId, instanceId);

            instance.Position = position;

            return instance;
        }

        public ThingInstance Spawn(string baseId, string regionId, string instanceId = null)
        {
            if (Breeds.TryGetValue(baseId, out var breed))
            {
                if (Regions.TryGetValue(regionId, out var region))
                {
                    var instance = breed.Spawn(instanceId);
                    region.AddInstance(instance);
                    Instances.Add(instance.Id, instance);
                    return instance;
                }
                else
                {
                    throw new ArgumentException($"Invalid region id: {regionId}", "regionId");
                }
            }
            else
            {
                throw new ArgumentException($"Invalid base id: {baseId}", "baseId");
            }


        }

        public void LoadTextureFromFile(string id, string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            var texture = Texture2D.FromStream(Game.GraphicsDevice, fileStream);
            Textures[id] = texture;
        }

        public void LoadTextureFromContent(string id, string assetName)
        {
            var texture = Game.Content.Load<Texture2D>(assetName);
            Textures[id] = texture;
        }

        public void SpawnAtPlayer(string baseId, string instanceId = null)
        {
            if (Player == null)
                throw new InvalidOperationException("No Player is present in the world.");
            Spawn(baseId, Player.Region.Id, Player.Position, instanceId);
        }

        public Thing GetBreed(string id)
        {
            if (Breeds.TryGetValue(id, out Thing thing))
            {
                return thing;
            }

            throw new ArgumentException("No Breed was found with the specified id.", "id");
        }

        public ThingInstance GetInstance(string id)
        {
            if (Instances.TryGetValue(id, out ThingInstance instance))
            {
                return instance;
            }

            throw new ArgumentException("No Instance was found with the specified id.", "id");
        }

        public Region getRegion(string id)
        {
            if (Regions.TryGetValue(id, out Region region))
            {
                return region;
            }

            throw new ArgumentException("No Region was found with the specified id.", "id");
        }

        public object GetById(string id)
        {
            if (Breeds.TryGetValue(id, out Thing thing))
            {
                return thing;
            }

            if (Instances.TryGetValue(id, out ThingInstance instance))
            {
                return instance;
            }

            if (Regions.TryGetValue(id, out Region region))
            {
                return region;
            }

            throw new ArgumentException("No GameObject was found with the specified id.", "id");
        }

        public Env ToEnv()
        {
            //todo: optimalizalni, ezt eleg lenne egyszer letrehozni
            var env = new Env(null, "Game Model");
            foreach (var b in Breeds)
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
