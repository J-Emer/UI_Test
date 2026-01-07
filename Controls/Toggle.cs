using UI.Util;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace UI.Controls
{
    public class Toggle : Control
    {
        private Texture2D _texture => AssetLoader.GetPixel();
        private Rectangle _toggleRect;
        private int padding = 5;
        public bool State{get;set;} = true;
        public Action<bool> OnStateChanged;
        public Color ToggleButtonActive{get;set;} = Color.Black;
        public Color ToggleButtonInActive{get;set;} = Color.DarkGray;
        private Color _toggleColor;

        public Toggle() : base()
        {
            Width = 100;
            Height = 50;
        }


        protected override void HandleDirty()
        {
            base.HandleDirty();

            int _width = Width / 2;
            int _x = 0;

            if(State)
            {
                _x = X + padding;
                _toggleColor = ToggleButtonActive;
            }
            else
            {
                _x = X + _width;
                _toggleColor = ToggleButtonInActive;
            }

            int _y = Y + padding;
            int _height = Height - (padding * 2);


            _toggleRect = new Rectangle(_x, _y, _width, _height);
        }

        public override void Update()
        {
            base.Update();

            if(_toggleRect.Contains(Input.MousePos) && Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                State = !State;
                OnStateChanged?.Invoke(State);
                HandleDirty();
            }
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            _spritebatch.Draw(_texture, _toggleRect, _toggleColor);
        }
    }
}