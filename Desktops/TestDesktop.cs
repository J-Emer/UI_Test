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
        public TestDesktop(Game1 game, GraphicsDeviceManager graphics, string fontName) : base(game, graphics, fontName){}

        public override void Load()
        {

            Window _testWindow = new Window("Test Window")
            {
                X = 100,
                Y = 100
            };
            Add(_testWindow);

        }
       
    }
}