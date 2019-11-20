using System;

namespace GameModel.Models
{
    public class PlayerBase : Models.CharacterBase
    {
        public override Type InstanceType { get { return typeof(Player); } }
    }
}