using Common.Script.Utility;
using Common.Script.Visitors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CharacterInstance : CreatureInstance
    {
        public List<ItemInstance> Items { get; set; } = new List<ItemInstance>();
        public Dictionary<string, string> Topics { get; set; } = new Dictionary<string, string>();

        public override void Update(GameTime gameTime)
        {
            if(State == State.Dead)
            {
                for (int i = Items.Count -1; i >= 0; i--)
                {
                    var item = Items[i];
                    item.Accept(new DropVisitor(this));
                }
            }
            base.Update(gameTime);
        }
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
