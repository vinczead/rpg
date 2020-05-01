using GameScript;
using GameScript.Visitors;

namespace GameScript.Models.InstanceClasses
{
    public abstract class ItemInstance : ThingInstance
    {
        public void PickUp(CharacterInstance character)
        {
            Region.RemoveThing(this);
            character.InsertItem(this);
            ExecutionVisitor.ExecuteRunBlock(Region.GameModel, this, "PickedUp");
        }

        public void Drop(CharacterInstance character)
        {
            Position = character.Position;
            character.Region.InsertThing(this);
            ExecutionVisitor.ExecuteRunBlock(Region.GameModel, this, "Dropped");
        }
    }
}