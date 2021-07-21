using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.SimpleMenu.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigurationActionAttribute : BaseConfigurationAttribute
    {        
        public ConfigurationActionAttribute(string name)
            :base(name)
        {
            
        }
    }
}
