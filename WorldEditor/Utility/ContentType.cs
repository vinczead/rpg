using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorldEditor.Utility
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ContentType
    {
        [Description("Textures")]
        Texture,
        [Description("Sprite Models")]
        SpriteModel,
        [Description("Tile Types")]
        Tile,
        [Description("Breeds")]
        Breed,
        [Description("Regions")]
        Region
    }
}
