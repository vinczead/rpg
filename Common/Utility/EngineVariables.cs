using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility
{
    public static class EngineVariables
    {
        public static bool ShowEntityBoundingBox;
        public static bool ShowEntityCollisionBox;
        public static List<string> ConsoleContents = new List<string>();
        public static Queue<string> Messages = new Queue<string>();
    }
}
