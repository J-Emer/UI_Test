using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class Button : Control
    {
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "Button";
        public Color FontColor{get;set;} = Color.White;
        private Vector2 _textPos = Vector2.Zero;
        public Color HighlightColor{get;set;} = new Color(19, 90, 161);
        public Color NormalColor{get;set;} = new Color(26, 27, 28);


        public Button() : base()
        {
            BackgroundColor = NormalColor;
            Width = 150;
            Height = 30;
        }
        protected override void AfterInvalidation()
        {
            Vector2 _halfFontSize = Font.MeasureString(Text) * 0.5f;
            _textPos = Center - _halfFontSize;
        }
        protected override void InternalMouseEnter()
        {
            BackgroundColor = HighlightColor;
        }
        protected override void InternalMouseExit()
        {
            BackgroundColor = NormalColor;
        }
        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            _spritebatch.DrawString(Font, Text, _textPos, FontColor);
        }

        //control BG: 51, 52, 54        
    }
}