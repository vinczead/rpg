using GameScript;

namespace GameScript.Models.InstanceClasses
{
    public abstract class ItemInstance : ThingInstance
    {
        public void PickUp(CharacterInstance character)
        {
            Region.RemoveThing(this);
            character.InsertItem(this);
            Executer.ExecuteRunBlock(this, "WhenPickedUp");
        }

        public void Drop(CharacterInstance character)
        {
            Position = character.Position;
            character.Region.InsertThing(this);
            Executer.ExecuteRunBlock(this, "WhenDropped");
        }
    }
}