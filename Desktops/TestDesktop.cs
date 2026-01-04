using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Controls;
using UI.Util;

namespace UI.Desktops
{
    public class TestDesktop : Desktop
    {
        public TestDesktop(Game1 game) : base(game){}

        public override void Load()
        {
            Window _testWindow = new Window("Test Window");
            Windows.Add(_testWindow);


            Window _otherWindow = new Window("Other Window")
            {
                X = 600,
                Y = 100
            };
            Windows.Add(_otherWindow);

            TextBox _textbox = new TextBox
            {
                X = 650,
                Y = 150,
                Width = 500
            };
            _otherWindow.Children.Add(_textbox);
        }
    }
}