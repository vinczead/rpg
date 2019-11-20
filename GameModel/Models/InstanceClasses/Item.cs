namespace GameModel.Models
{
    public abstract class Item : Models.GameObject
    {
        public void PickUp(Character character)
        {
            Room.RemoveGameObject(this);
            character.InsertItem(this);
            //ScriptRunner.RunScriptBlock(null, this, RunBlockType.WhenPickedUp);
        }
    }
}