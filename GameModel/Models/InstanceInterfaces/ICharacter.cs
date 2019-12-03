using System.Collections.Generic;

namespace GameModel.Models.InstanceInterfaces
{
    public interface ICharacter : ICreature
    {
        List<IItem> Items { get; set; }
        void InsertItem(IGameWorldObject gameObject);
        void DropItem(IGameWorldObject gameObject);
    }
}