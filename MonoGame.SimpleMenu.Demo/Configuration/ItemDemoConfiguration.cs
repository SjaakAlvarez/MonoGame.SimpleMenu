using Microsoft.Xna.Framework.Input;
using MonoGame.SimpleMenu.Attributes;
using System;

namespace MonoGame.SimpleMenu.Demo.Configuration
{
    public class ItemDemoConfiguration
    {
        [Configuration("Integer", new Object[] { 3, 4, 5 })]
        [Order(0)] public int DemoInteger { get; set; } = 3;

        [Configuration("String", new Object[] { "Yes", "No", "Maybe" })]
        [Order(10)] public string DemoString { get; set; } = "Yes";

        [Configuration("Float", new Object[] { 0.1f, 0.25f, 0.5f, 1.0f })]
        [Order(12)] public float DemoFloat { get; set; } = 0.1f;

        [Configuration("Boolean", new Object[] { true, false })]
        [Order(14)] public bool DemoBoolean { get; set; } = true;

        [Spacer]
        [ConfigurationKey("Keyboard")]
        [Order(20)] public Keys DemoKeyboard { get; set; } = Keys.Left;        

        [Spacer]
        [ConfigurationSlider("Slider")]
        [Order(50)] public int DemoSlider { get; set; } = 10;
             
    }
}
