using GameScript;

namespace GameScript.Models.InstanceClasses
{
    public abstract class ItemInstance : ThingInstance
    {
        public void PickUp(CharacterInstance character)
        {
            Room.RemoveThing(this);
            character.InsertItem(this);
            Executer.ExecuteRunBlock(this, "WhenPickedUp");
        }

        public void Drop(CharacterInstance character)
        {
            Position = character.Position;
            character.Room.InsertThing(this);
            Executer.ExecuteRunBlock(this, "WhenDropped");
        }
    }
}