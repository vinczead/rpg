using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace WorldEditor.DataAccess
{
    public class RpgTexture
    {
        public string Id { get; set; }
        [JsonIgnore]
        public byte[] Texture2D { get; set; }
    }
}
