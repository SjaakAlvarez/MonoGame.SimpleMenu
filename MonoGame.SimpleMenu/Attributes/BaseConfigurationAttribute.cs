using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.SimpleMenu.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class BaseConfigurationAttribute : Attribute
    {
        public string Name { get; set; }             
      
        public BaseConfigurationAttribute(string name)
        {
            this.Name = name;                                
        }
    }
}
