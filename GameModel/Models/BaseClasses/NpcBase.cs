using System;

namespace GameModel.Models
{
    public class NpcBase : CharacterBase
    {
        public override Type InstanceType { get { return typeof(Npc); } }
    }
}