using Antlr4.StringTemplate;
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
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public Dictionary<string, Texture> Textures { get; set; } = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteModel> Models { get; set; } = new Dictionary<string, SpriteModel>();
        public Dictionary<string, Thing> Breeds { get; set; } = new Dictionary<string, Thing>();
        public Dictionary<string, ThingInstance> Instances { get; set; } = new Dictionary<string, ThingInstance>();
        public Dictionary<string, Tile> Tiles { get; set; } = new Dictionary<string, Tile>();
        public Dictionary<string, Region> Regions { get; set; } = new Dictionary<string, Region>();

        public CharacterInstance Player { get; set; }

        World()
        {
            Clear();
        }

        public void Clear(bool clearFileName = false)
        {
            if(clearFileName) {
                FileName = null;
                FolderPath = null;
            }

            Textures.Clear();
            Models.Clear();
            Breeds.Clear();
            Instances.Clear();
            Tiles.Clear();
            Regions.Clear();
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
            var fullFileName = Path.Combine(Instance.FolderPath, fileName);
            Texture2D texture = null;
            byte[] byteArrayValue = null;
            if(Game != null)
            {
                using FileStream fileStream = new FileStream(fullFileName, FileMode.Open);
                texture = Texture2D.FromStream(Game.GraphicsDevice, fileStream);
            } else
            {
                byteArrayValue = File.ReadAllBytes(fullFileName);
            }
            
            Textures[id] = new Texture()
            {
                Id = id,
                FileName = fileName,
                Value = texture,
                ByteArrayValue = byteArrayValue
            };
        }

        public void LoadTextureFromContent(string id, string assetName)
        {
            var texture = Game.Content.Load<Texture2D>(assetName);
            Textures[id] = new Texture()
            {
                Id = id,
                Value = texture
            };
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

            if (Tiles.TryGetValue(id, out Tile tile))
            {
                return tile;
            }

            if (Models.TryGetValue(id, out SpriteModel model))
            {
                return model;
            }

            if (Textures.TryGetValue(id, out Texture texture))
            {
                return texture;
            }

            throw new ArgumentException("No entity was found with the specified id.", "id");
        }

        public Scope ToScope()
        {
            //todo: optimalizalni, ezt eleg lenne egyszer letrehozni
            var scope = new Scope(null, "Game Model");
            foreach (var b in Breeds)
            {
                scope[b.Key] = new Symbol(b.Key, TypeSystem.Instance[b.Value.GetType().Name], b.Key);
            }

            foreach (var i in Instances)
            {
                scope[i.Key] = new Symbol(i.Key, TypeSystem.Instance[i.Value.GetType().Name], i.Key);
            }

            foreach (var r in Regions)
            {
                scope[r.Key] = new Symbol(r.Key, TypeSystem.Instance[r.Value.GetType().Name], r.Key);
            }

            foreach (var t in Textures)
            {
                scope[t.Key] = new Symbol(t.Key, TypeSystem.Instance[t.Value.GetType().Name], t.Key);
            }

            foreach (var t in Tiles)
            {
                scope[t.Key] = new Symbol(t.Key, TypeSystem.Instance[t.Value.GetType().Name], t.Key);
            }

            foreach (var m in Models)
            {
                scope[m.Key] = new Symbol(m.Key, TypeSystem.Instance[m.Value.GetType().Name], m.Key);
            }

            return scope;
        }

        public string Serialize()
        {
            using var stream = typeof(FunctionManager).Assembly.GetManifestResourceStream("Common.Templates.stg");
            using var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            TemplateGroup templateGroup = new TemplateGroupString(content);
            var template = templateGroup.GetInstanceOf("world");
            template.Add("w", Instance);
            return template.Render();
        }
    }
}
