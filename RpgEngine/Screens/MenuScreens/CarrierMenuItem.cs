using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Screens
{
    class CarrierMenuItem<T> : MenuItem
    {
        public T Data { get; set; }
        public CarrierMenuItem(string text, T data) : base(text)
        {
            Data = data;
        }

        public CarrierMenuItem(string text, string description, EventHandler<MenuItemSelectedEventArgs> selected, T data) : base(text, description, selected)
        {
            Data = data;
        }
    }
}
