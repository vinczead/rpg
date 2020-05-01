using GameScript.Visitors;

namespace GameScript.Models.InstanceClasses
{
    public class EquipmentInstance : ItemInstance
    {
        public void Equip(CharacterInstance character)
        {
            ExecutionVisitor.ExecuteRunBlock(Region.GameModel, this, "Equipped");
        }
    }
}