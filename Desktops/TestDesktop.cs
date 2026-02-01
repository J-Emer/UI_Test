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
            ListBox _listBox = new ListBox
            {
                X = 100,
                Y = 100,
                Width = 300,
                Height = 400
            };
            Add(_listBox);

            _listBox.OnItemSelected += ItemSelected;


            for (int i = 0; i < 20; i++)
            {
                _listBox.Add($"Item: {i}");
            }


            Slider _slider = new Slider
            {
                X = 700,
                Y = 100,
                Width = 200,
                Height = 50
            };
            Add(_slider);

        }

        private void ItemSelected(ListBoxItem item)
        {
            Console.WriteLine($"Item Selected: {item.Text}");
        }
    }
}