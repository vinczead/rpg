using Common.Models;
using Common.Script.Utility;
using Common.Script.Visitors;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WorldEditor.Utility;

namespace WorldEditor.DataAccess
{
    public class WorldRepository
    {
        [JsonIgnore]
        public string FileName { get; set; }

        public TextureRepository Textures { get; set; } = new TextureRepository();
        public SpriteModelRepository SpriteModels { get; set; } = new SpriteModelRepository();
        public TileRepository Tiles { get; set; } = new TileRepository();
        public ScriptFileRepository ScriptFiles { get; set; } = new ScriptFileRepository();
        [JsonIgnore]
        public MapRepository Maps { get; set; } = new MapRepository(); //Todo: this should be serialized too

        public WorldRepository()
        {

        }

        public WorldRepository(string fileName, bool creating)
        {
            FileName = fileName;
            if (creating)
            {
                using var file = File.Create(fileName);
                file.Close();
            }
            else
            {
                LoadWorldScript(fileName);
            }
        }

        private void LoadWorldScript(string fileName)
        {
            ExecutionVisitor.BuildWorldFromFile(fileName, out var allerrors);
            var a = World.Instance;

            var asdasd = World.Instance.Serialize();
        }

        public void SaveWorldScript()
        {
            File.WriteAllText(FileName, World.Instance.Serialize());
        }

        private void LoadWorldDescriptor(string fileName)
        {
            var jsonString = File.ReadAllText(fileName);
            var deserialized = JsonSerializer.Deserialize<WorldRepository>(jsonString);
            FileName = fileName;
            Textures = deserialized.Textures;
            SpriteModels = deserialized.SpriteModels;
            Tiles = deserialized.Tiles;
            ScriptFiles = deserialized.ScriptFiles;
            Maps = deserialized.Maps;

            Directory.SetCurrentDirectory(Path.GetDirectoryName(FileName));
            foreach (var texture in Textures.GetTextures())
            {
                texture.Texture2D = File.ReadAllBytes($"Textures/{texture.Id}.png");
            }
        }

        public void SaveWorldDescriptor()
        {
            var jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(FileName, jsonString);

            Directory.SetCurrentDirectory(Path.GetDirectoryName(FileName));
            Directory.CreateDirectory("Textures");
            foreach (var texture in Textures.GetTextures())
            {
                File.WriteAllBytes($"Textures/{texture.Id}.png", texture.Texture2D);
            }
        }
    }
}
