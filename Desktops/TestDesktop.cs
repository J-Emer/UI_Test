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
            Window _testWindow = new Window("Test Window", this)
            {
                X = 100,
                Y = 100,
                Width = 400,
                Layout = new RowLayout(),
            };

            Add(_testWindow);

            Toggle _toggle = new Toggle
            {
                Width = 100,
                Height = 30
            };

            _testWindow.Children.Add(_toggle);

        }

    }
}