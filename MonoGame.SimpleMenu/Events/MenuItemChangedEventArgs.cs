using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.SimpleMenu.Events
{
    public class MenuItemChangedEventArgs : EventArgs
    {
        public string Name { get; set; }
        public Object Value { get; set; }

        public MenuItemChangedEventArgs(string name, Object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
