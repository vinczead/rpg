using System.Collections.Generic;

namespace GameModel.Models.InstanceInterfaces
{
    public interface ICharacter : ICreature
    {
        List<IItem> Items { get; set; }
        void InsertItem(IItem item);
        void DropItem(IItem item);
    }
}