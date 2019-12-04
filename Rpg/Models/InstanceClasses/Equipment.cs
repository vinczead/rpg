using GameModel.Models.InstanceInterfaces;
using GameScript;

namespace Rpg.Models
{
    public class Equipment : Item, IEquipment
    {
        public void Equip(ICharacter character)
        {
            Executer.ExecuteRunBlock(this, "WhenEquipped");
        }
    }
}