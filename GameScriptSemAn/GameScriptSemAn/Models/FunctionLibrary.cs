using GameScript.Models.BaseClasses;
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
            //todo: implement SetTexture
            return true;
        }
    }
}
