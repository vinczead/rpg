using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Screens
{
    class MenuItemSelectedEventArgs : EventArgs
    {
        public MenuItem MenuItem { get; set; }

        public MenuItemSelectedEventArgs(MenuItem menuItem)
        {
            MenuItem = menuItem;
        }
    }
}
