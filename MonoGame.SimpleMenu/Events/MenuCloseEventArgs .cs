using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.SimpleMenu.Events
{
    public class MenuCloseEventArgs : EventArgs
    {
        public object Configuration { get; set; }
        public bool Save { get; set; }

        public MenuCloseEventArgs(object configuration, bool save)
        {
            this.Save = save;
            this.Configuration = configuration;
        }
    }
}
