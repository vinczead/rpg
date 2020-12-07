using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Common.Models
{
    public class Visitor
    {
        protected bool success = false;
        public bool Success { get => success; }
        public virtual void Visit(ThingInstance thing) { Debugger.Log(0, "", "Nothing happens."); }
        public virtual void Visit(ItemInstance item) { Debugger.Log(0, "", "Nothing happens."); }
        public virtual void Visit(ConsumableInstance consumable) { Debugger.Log(0, "", "Nothing happens."); }
        public virtual void Visit(EquipmentInstance equipment) { Debugger.Log(0, "", "Nothing happens."); }
        public virtual void Visit(ActivatorInstance activator) { Debugger.Log(0, "", "Nothing happens."); }
        public virtual void Visit(CreatureInstance creature) { Debugger.Log(0, "", "Nothing happens."); }
        public virtual void Visit(CharacterInstance character) { Debugger.Log(0, "", "Nothing happens."); }
    }
}
