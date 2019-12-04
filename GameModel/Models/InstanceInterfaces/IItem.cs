using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models.InstanceInterfaces
{
    public interface IItem : IGameWorldObject
    {
        void PickUp(ICharacter character);
        void Drop(ICharacter character);
    }
}
