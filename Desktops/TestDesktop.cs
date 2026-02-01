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
        public TestDesktop(Game1 game, string fontName) : base(game, fontName){}

        public override void Load()
        {

            Control _control = new Control
            {
                X = 100,
                Y = 100,
                Width = 300,
                Height = 400
            };
            Add(_control);


            _control.OnClick += Clicked;

        }

        private void Clicked(Control control, Input.MouseButton button)
        {
            Console.WriteLine(button.ToString());
        }
    }
}