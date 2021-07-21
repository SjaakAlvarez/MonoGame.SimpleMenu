using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MonoGame.SimpleMenu.Attributes
{
    [AttributeUsage(AttributeTargets.Property,Inherited = true, AllowMultiple = false)]
    [ImmutableObject(true)]
    public class SpacerAttribute : Attribute
    {
        public string Name { get; set;}
        public SpacerAttribute(string name=null) { this.Name = name; }
    }
}
