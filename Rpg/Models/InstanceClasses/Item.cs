using GameModel.Models.InstanceInterfaces;
using GameScript;

namespace Rpg.Models
{
    public abstract class Item : GameWorldObject, IItem
    {
        public void PickUp(ICharacter character)
        {
            Room.RemoveGameWorldObject(this);
            character.InsertItem(this);
            Executer.ExecuteRunBlock(this, "WhenPickedUp");
        }
    }
}