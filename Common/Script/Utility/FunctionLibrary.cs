using Common.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Script.Utility
{
    public static class FunctionLibrary
    {
        static Random random = new Random();

        public static double Random()
        {
            return random.NextDouble();
        }

        public static double StrLength(object[] parameters)
        {
            return parameters[0].ToString().Length;
        }

        public static bool SetPos(object[] parameters)
        {
            var baseObject = parameters[0] as ThingInstance;
            var x = Convert.ToSingle(parameters[1]);
            var y = Convert.ToSingle(parameters[2]);

            baseObject.Position = new Vector2(x, y);

            return true;
        }

        public static bool SetTile(object[] parameters)
        {
            try { 
                var region = parameters[0] as Region;
                var x = (int)parameters[1];
                var y = (int)parameters[2];
                var tile = parameters[3] as Tile;

                region.Tiles[x][y] = tile;

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
