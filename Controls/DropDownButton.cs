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
        
        private ChildCollection<Button> Items = new ChildCollection<Button>();
        public Texture2D PanelTexture{get;set;} = AssetLoader.GetPixel();
        public Color PanelBackgroundColor{get;set;} = Color.DarkGray;
        private Rectangle _panel;
        private Layout _layout = new RowLayout();
        private bool _showPanel = false;
        private int _padding = 5;
        public Action<string> OnItemSelected;


        public DropDownbutton(string _text, List<string> _items) : base(_text)
        {
            this.NormalColor = Color.Transparent;
            this.BackgroundColor = Color.Transparent;

            ExpandAmount = 0;
            
            foreach (var item in _items)
            {
                Button _b = new Button(item)
                {
                    BorderThickness = 0,
                    Height = 20,
                    UserData = item,
                    HighlightColor = this.HighlightColor,
                    NormalColor = Color.Transparent,
                    BackgroundColor = Color.Transparent
                };

                Items.Add(_b);

                _b.OnClick += ItemSelected;
            }
        }

        private void ItemSelected(Control control)
        {
            Button _b = (Button)control;
            Text = _b.Text;
            _showPanel = false;
            OnItemSelected?.Invoke(Text);
            Console.WriteLine($"Item Selected: {control.UserData}");
        }
        protected override void HandleDirty()
        {
            base.HandleDirty();

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

            for (int i = 0; i < Items.Controls.Count; i++)
            {
                Items.Controls[i].Update();
            }
        }
        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            if(_showPanel)
            {
                _spritebatch.Draw(PanelTexture, _panel, PanelBackgroundColor);
                _spritebatch.Draw(Texture, new Rectangle(_panel.Left, _panel.Top, _panel.Width, BorderThickness), BorderColor);//top
                _spritebatch.Draw(Texture, new Rectangle(_panel.Right - BorderThickness, _panel.Top, BorderThickness, _panel.Height), BorderColor);//right
                _spritebatch.Draw(Texture, new Rectangle(_panel.Left, _panel.Bottom - BorderThickness, _panel.Width, BorderThickness), BorderColor);//bottom
                _spritebatch.Draw(Texture, new Rectangle(_panel.Left, _panel.Top, BorderThickness, _panel.Height), BorderColor);//left

                for (int i = 0; i < Items.Controls.Count; i++)
                {
                    Items.Controls[i].Draw(_spritebatch);
                }
            }
        }
    }
}