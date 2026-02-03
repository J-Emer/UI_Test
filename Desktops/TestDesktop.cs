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

            MenuStrip _menu = new MenuStrip();
            Add(_menu);

            _menu.Items.Add(new MenuItem("File", new List<string>()));

            for (int i = 0; i < 5; i++)
            {
                _menu.Items.Add(new MenuItem($"Item{i}", new List<string>{"Foo", "Bar", "Fizz", "Buzz"}));
            }



            Window _leftWindow = new Window("Left Window")
            {
                Width = 350,
                Height = 400,
                Dock = DockStyle.Left,
                BorderThickness = 0
            };
            Add(_leftWindow);

            for (int i = 0; i < 5; i++)
            {
                _leftWindow.Children.Add(new Button
                {
                    Text = $"Button: {i}"
                });
            }


            Window _rightWindow = new Window("Right Window")
            {
                X = 400,
                Y = 300,
                Width = 350,
                Height = 400,
                Layout = new HorizontalLayout(),
                Dock = DockStyle.Right,
                BorderThickness = 0            
            };
            Add(_rightWindow);

            _rightWindow.Children.Add(new TextBox
            {
                Width = 150,
                Height = 30,
                Text = "TextBox"
            });
            _rightWindow.Children.Add(new NumericTextBox
            {
                Width = 150,
                Height = 30,
                Text = "00"
            });            

            _rightWindow.Children.Add(new DropDownbutton("Selection", new List<string>{"Hello", "World", "Fizz", "Buzz"}));

            Window _bottomWindow = new Window("Bottom Window")
            {
                Width = 350,
                Height = 300,
                Dock = DockStyle.Bottom,
                Layout = new RowLayout()
            };
            Add(_bottomWindow);

            _bottomWindow.Children.Add(new Label
            {
                Text = "Hello World"
            });



        }
       
    }
}