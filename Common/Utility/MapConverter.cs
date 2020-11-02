using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Utility
{
    public class MapConverter : JsonConverter<Region>
    {
        public override Region Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new Region();
            //todo
        }

        public override void Write(Utf8JsonWriter writer, Region value, JsonSerializerOptions options)
        {
            //todo
        }
    }
}
