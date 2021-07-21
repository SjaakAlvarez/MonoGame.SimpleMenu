using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.SimpleMenu.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigurationAttribute : BaseConfigurationAttribute
    {       
        public Object[] Values { get; set; }      

        public ConfigurationAttribute(string name, Object[] values)
            :base(name)
        {          
            this.Values = values;           
        }
    }
}
