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
            ListBox _listBox = new ListBox
            {
                X = 100,
                Y = 100,
                Width = 300,
                Height = 400
            };
            Add(_listBox);
            _listBox.OnItemSelected += ListBoxSelected;

            for (int i = 0; i < 20; i++)
            {
                _listBox.Add($"Item: {i}");
            }



            ListBox _otherListBox = new ListBox
            {
                X = 600,
                Y = 100,
                Width = 300,
                Height = 400
            };
            Add(_otherListBox);
            _otherListBox.OnItemSelected += OtherListBoxSelected;

            for (int i = 0; i < 20; i++)
            {
                _otherListBox.Add($"Item: {i}");
            }


        }

        private void ListBoxSelected(ListBoxItem item)
        {
            Console.WriteLine($"Item Selected: {item.Text}");
        }
        private void OtherListBoxSelected(ListBoxItem item)
        {
            Console.WriteLine($"Other Item Selected: {item.Text}");
        }        
    }
}