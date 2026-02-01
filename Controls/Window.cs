
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class Window : Control
    {
        public const int HeaderHeight = 30;
        public const int CloseBtnSize = 30;

        public Color HeaderBackColor = new Color(24, 25, 26);
        protected Rectangle _headerRect = new Rectangle();
        protected Button _closeButton;
        private bool _canDrag = false;
        private Vector2 _dragOffset = Vector2.Zero;
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "Window";
        public Color TextColor{get;set;} = Color.White;
        protected Vector2 TextPosition{get;set;} = Vector2.Zero;



        public Window(string name) : base()
        {
            _closeButton = new Button
            {
                Width = CloseBtnSize,
                Height = CloseBtnSize,
                BackgroundColor = Color.Transparent,
                NormalColor = Color.Transparent,
                HighlightColor = Color.Red,
                FontColor = Color.White,
                Text = "X",
                BorderThickness = 0
            };
            _closeButton.OnClick += Close;


            Name = name;
            Text = Name;
            Width = 400;
            Height = 400;
            BackgroundColor = new Color(51, 52, 54);
        }

        private void Close(Control control, Input.MouseButton button)
        {
            Console.WriteLine("---closing window");
        }

        protected override void AfterInvalidation()
        {
            _headerRect = new Rectangle(X, Y, Width, HeaderHeight);

            _closeButton.X = SourceRect.Right - CloseBtnSize;
            _closeButton.Y = Y;
            
            TextPosition = new Vector2(X + 5, Y + 5); //magic numbers = padding
        }

        public override void Update()
        {
            base.Update();

            _closeButton.Update();
            HandleDrag();
        }
        private void HandleDrag()
        {
            if(_headerRect.Contains(Input.MousePos) && Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                _canDrag = true;
                _dragOffset = Input.MousePos - new Vector2(X, Y);
            }

            if(_canDrag)
            {
                X = (int)(Input.MousePos.X - _dragOffset.X);
                Y = (int)(Input.MousePos.Y - _dragOffset.Y);
            }

            if(Input.GetMouseButtonUp(Input.MouseButton.Left))
            {
                _canDrag = false;
            }            
        }
        public override void Draw(SpriteBatch _spritebatch, GraphicsDeviceManager graphics)
        {
            base.Draw(_spritebatch, graphics);

            ScissorStack.Push(graphics.GraphicsDevice, SourceRect);

            _spritebatch.Draw(Texture, _headerRect, HeaderBackColor);
            _closeButton.Draw(_spritebatch, graphics);
            _spritebatch.DrawString(Font, Text, TextPosition, TextColor);

            ScissorStack.Pop(graphics.GraphicsDevice);
        }
    }
}