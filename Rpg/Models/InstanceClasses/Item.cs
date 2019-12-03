using GameModel.Models.InstanceInterfaces;

namespace Rpg.Models
{
    public abstract class Item : GameWorldObject, IItem
    {
        public void PickUp(ICharacter character)
        {
            Room.RemoveGameWorldObject(this);
            character.InsertItem(this);
        }
    }
}