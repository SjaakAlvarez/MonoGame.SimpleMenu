using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.SimpleMenu.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigurationSliderAttribute : BaseConfigurationAttribute
    {         
        public ConfigurationSliderAttribute(string name)
            :base(name)
        {                
        }
    }
}
