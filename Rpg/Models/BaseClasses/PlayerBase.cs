using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public class PlayerBase : Models.CharacterBase
    {
        public override Type InstanceType { get { return typeof(Player); } }
    }
}