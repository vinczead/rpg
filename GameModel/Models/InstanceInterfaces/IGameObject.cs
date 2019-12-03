using GameModel.Models.BaseInterfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models.InstanceInterfaces
{
    public interface IGameObject
    {
        string Id { get; set; }
        Dictionary<string, Variable> Variables { get; set; }

        void Update(GameTime gameTime);
    }
}
