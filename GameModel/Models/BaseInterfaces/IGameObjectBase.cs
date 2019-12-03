using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models.BaseInterfaces
{
    public interface IGameObjectBase
    {
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Script { get; set; }
    }
}
