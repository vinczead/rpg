using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Utility
{
    public class MapConverter : JsonConverter<Map>
    {
        public override Map Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new Map();
            //todo
        }

        public override void Write(Utf8JsonWriter writer, Map value, JsonSerializerOptions options)
        {
            //todo
        }
    }
}
