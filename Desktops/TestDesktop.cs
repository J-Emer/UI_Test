using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Controls;
using UI.Util;
using UI.Widgets;

namespace UI.Desktops
{
    public class TestDesktop : Desktop
    {
        public TestDesktop(Game1 game) : base(game){}

        public override void Load()
        {
            Window _testWindow = new Window("Test Window")
            {
                X = 100,
                Y = 100,
                Layout = new RowLayout()  
            };

            Add(_testWindow);

            TextBox _textbox = new TextBox();
            _testWindow.Children.Add(_textbox);

            Slider _slider = new Slider
            {
                Value = 0.5f
            };
            _testWindow.Children.Add(_slider);

            _slider.OnValueChanged += SliderChanged;

            ColorPicker _picker = new ColorPicker();
            _picker.OnColorChanged += ColorChanged;
            _testWindow.Children.Add(_picker);
        }

        private void ColorChanged(Color color)
        {
            Console.WriteLine(color.ToString());
        }

        private void SliderChanged(float obj)
        {
           Console.WriteLine(obj.ToString());
        }
    }
}