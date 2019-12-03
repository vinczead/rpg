using System;

namespace Rpg.Models
{
    public class PlayerBase : CharacterBase
    {
        public override Type InstanceType { get { return typeof(Player); } }
    }
}