using GameScript.Models.BaseClasses;
using GameScript.Models.InstanceClasses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models
{
    public static class FunctionLibrary
    {
        static Random random = new Random();

        public static GameModel gm;
        public static double Random()
        {
            return random.NextDouble();
        }

        public static double StrLength(object[] parameters)
        {
            return parameters[0].ToString().Length;
        }

        public static bool SetTexture(object[] parameters)
        {
            var baseObject = parameters[0] as Thing;
            var textureId = parameters[1] as string;

            try
            {
                baseObject.Texture = TextureManager.Textures[textureId];
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public static bool SetPos(object[] parameters)
        {
            var baseObject = parameters[0] as ThingInstance;
            var x = Convert.ToSingle(parameters[1]);
            var y = Convert.ToSingle(parameters[2]);

            baseObject.Position = new Vector2(x, y);

            return true;
        }

        public static bool ShowMessage(object[] paramenters)
        {
            var s = Convert.ToString(paramenters[0]);

            gm.Message(s);

            return true;
        }
    }
}
