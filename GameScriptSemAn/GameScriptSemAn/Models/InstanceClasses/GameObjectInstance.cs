using GameScript.Models.BaseClasses;
using GameScript.Models.Script;
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
        public Dictionary<string, Symbol> Variables { get; set; } = new Dictionary<string, Symbol>();
        public GameModel GameModel { get; set; }
        public Region Region { get; set; }
        public GameObject BASE { get; set; }
    }
}
