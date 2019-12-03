using GameModel.Models.InstanceInterfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models.BaseInterfaces
{
    public interface IGameWorldObjectBase : IGameObjectBase
    {
        Type InstanceType { get; }
        Texture2D Texture { get; set; }

        IGameWorldObject Spawn(string instanceId = null);
    }
}
