﻿using GameScript.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models.InstanceClasses
{
    public class GameObjectInstance
    {
        public string Id { get; set; }
        public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
        public World World { get; set; }
        public GameObject Base { get; set; }

        public string Script { get; set; } = "";
    }
}
