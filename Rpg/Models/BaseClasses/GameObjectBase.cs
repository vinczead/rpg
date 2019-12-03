using GameModel.Models.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Models
{
    public class GameObjectBase : IGameObjectBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Script { get; set; }
    }
}
