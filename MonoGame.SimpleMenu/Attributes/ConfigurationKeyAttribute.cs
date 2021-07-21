using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.SimpleMenu.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigurationKeyAttribute : BaseConfigurationAttribute
    {         
        public ConfigurationKeyAttribute(string name)
            :base(name)
        {                
        }
    }
}
