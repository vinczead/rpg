using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WorldEditor.Utility
{
    public class TextureDictionaryConverter : JsonConverter<Dictionary<string, Texture2D>>
    {
        public override Dictionary<string, Texture2D> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //read string array, load texture, return dict<string, texture2d>
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, Texture2D> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
