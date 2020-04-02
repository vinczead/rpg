namespace GameScript.Models.InstanceClasses
{
    public class EquipmentInstance : ItemInstance
    {
        public void Equip(CharacterInstance character)
        {
            Executer.ExecuteRunBlock(this, "WhenEquipped");
        }
    }
}