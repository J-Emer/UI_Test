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
            Window _leftWindow = new Window("Left Window")
            {
                Width = 400,
                Layout = new HorizontalLayout(),
                Dock = DockStyle.Left
            };

            Add(_leftWindow);


            _leftWindow.Children.Add(new DropDownbutton("Foo", new List<string>{"Bar", "Fizz", "Buzz", "Hello", "World"}));




            Window _rightWindow = new Window("Right Window")
            {
                Width = 400,
                Layout = new HorizontalLayout(),
                Dock = DockStyle.Right
            };

            Add(_rightWindow);



            Window _bottomWindow = new Window("Bottom Window")
            {
                Width = 400,
                Height = 400,
                Layout = new HorizontalLayout(),
                Dock = DockStyle.Bottom
            };

            Add(_bottomWindow);            
        }

    }
}