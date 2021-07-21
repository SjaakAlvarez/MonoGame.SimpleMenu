using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MonoGame.SimpleMenu.Attributes
{
    [AttributeUsage(AttributeTargets.Property,Inherited = true, AllowMultiple = false)]
    [ImmutableObject(true)]
    public class OrderAttribute : Attribute
    {
        private readonly int order;
        public int Order { get { return order; } }
        public OrderAttribute(int order) { this.order = order; }
    }
}
