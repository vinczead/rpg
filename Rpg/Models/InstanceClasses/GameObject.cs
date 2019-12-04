using GameModel.Models;
using GameModel.Models.InstanceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Models.InstanceClasses
{
    public class GameObject : IGameObject
    {
        public string Id { get; set; }
        public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
        public IWorld World { get; set; }
    }
}
