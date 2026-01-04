using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public enum ButtonTextLayout{Left, Right, Center};

    public class Button : Control
    {
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "Button";
        public Color FontColor{get;set;} = Color.Black;
        private Vector2 _fontPos = Vector2.Zero;
        public Color NormalColor{get;set;} = Color.White;
        public Color HighlightColor{get;set;} = Color.Gray;
        public int ExpandAmount{get;set;} = 5;
        public ButtonTextLayout TextLayout{get;set;} = ButtonTextLayout.Center;


        public Button(string _text) : base()
        {
            Text = _text;
            Width = 150;
            Height = 30;
            BackgroundColor = NormalColor;
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();

            Vector2 _fontSize = Font.MeasureString(Text);
            Vector2 _centerFont = _fontSize * 0.5f;
            Vector2 _centerControl = new Vector2(SourceRect.Center.X, SourceRect.Center.Y);




            if(TextLayout == ButtonTextLayout.Left)
            {
                float xPos = X + 10f;
                float yPos = _centerControl.Y - _centerFont.Y;

                _fontPos = new Vector2(xPos, yPos);
            }

            if(TextLayout == ButtonTextLayout.Center)
            {
                float xPos = _centerControl.X - _centerFont.X;
                float yPos = _centerControl.Y - _centerFont.Y;

                _fontPos = new Vector2(xPos, yPos);
            }

            if(TextLayout == ButtonTextLayout.Right)
            {
                float xPos = SourceRect.Right - (_fontSize.X + 10);
                float yPos = _centerControl.Y - _centerFont.Y;

                _fontPos = new Vector2(xPos, yPos);
            }            

        }
        protected override void InternalMouseEnter()
        {
            BackgroundColor = HighlightColor;
            X -= ExpandAmount / 2;
            Y -= ExpandAmount / 2;

            Width += ExpandAmount;
            Height += ExpandAmount;

        }
        protected override void InternalMouseExit()
        {
            BackgroundColor = NormalColor;
            X += ExpandAmount / 2;
            Y += ExpandAmount / 2;

            Width -= ExpandAmount;
            Height -= ExpandAmount;
        }
        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            _spritebatch.DrawString(Font, Text, _fontPos, FontColor);
        }
    }
}