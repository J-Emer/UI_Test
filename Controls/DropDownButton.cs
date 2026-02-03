using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class DropDownbutton : Button
    {
        protected ChildCollection<Button> Items = new ChildCollection<Button>();
        private Rectangle _panel;
        private Layout _layout = new RowLayout();
        private bool _showPanel = false;
        private int _padding = 5;
        public Action<string, Button> OnItemSelection;
        public Button SelectedButton{get; private set;}

        public DropDownbutton(string _text, List<string> _items) : base()
        {           
            Text = _text;
            Name = _text;

            foreach (var item in _items)
            {
                Button _b = new Button
                {
                    Text = item,
                    Name = item,
                    BorderThickness = 0,
                    Height = 20,
                    UserData = item
                };

                Items.Add(_b);

                _b.OnClick += ItemSelected;

                Width = 150;
                Height = 30;

                Items.OnChildrenChanged += AfterInvalidation;
            }
        }
        private void ItemSelected(Control control, Input.MouseButton button)
        {
            SelectedButton = (Button)control;
            Text = SelectedButton.Text;
            _showPanel = false;
            OnItemSelection?.Invoke(Text, SelectedButton);
        }
        protected override void AfterInvalidation()
        {
            base.AfterInvalidation();

            int _height = _padding;

            foreach (var item in Items.Controls)
            {
                _height += item.Height + _padding;
            }

            _panel = new Rectangle
            (
                X,
                Y + Height,
                Width,
                _height
            );

            _layout.DoLayout(_panel, Items.Controls.ToList<Control>(), _padding);
        }
        protected override void InternalMouseClick()
        {
            _showPanel = !_showPanel;
        }
        public override void Update()
        {
            base.Update();

            if(!_showPanel){return;}

            for (int i = 0; i < Items.Controls.Count; i++)
            {
                Items.Controls[i].Update();
            }
        }
        public override void Draw(SpriteBatch _spritebatch, GraphicsDeviceManager graphics)
        {
            base.Draw(_spritebatch, graphics);

            if(_showPanel)
            {
                _spritebatch.Draw(Texture, new Rectangle(_panel.Left, _panel.Top, _panel.Width, BorderThickness), BorderColor);//top
                _spritebatch.Draw(Texture, new Rectangle(_panel.Right - BorderThickness, _panel.Top, BorderThickness, _panel.Height), BorderColor);//right
                _spritebatch.Draw(Texture, new Rectangle(_panel.Left, _panel.Bottom - BorderThickness, _panel.Width, BorderThickness), BorderColor);//bottom
                _spritebatch.Draw(Texture, new Rectangle(_panel.Left, _panel.Top, BorderThickness, _panel.Height), BorderColor);//left

                for (int i = 0; i < Items.Controls.Count; i++)
                {
                    Items.Controls[i].Draw(_spritebatch, graphics);
                }
            }
        }
    }
}