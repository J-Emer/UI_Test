using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class Label : Control
    {
        public string Text{get;set;} = "Label";
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public Color FontColor{get;set;} = Color.White;

        public Label() : base()
        {
            BackgroundColor = Color.Transparent;
            FontColor = Color.White;
            BorderThickness = 0;
        }

        public override void Draw(SpriteBatch _spritebatch, GraphicsDeviceManager graphics)
        {
            base.Draw(_spritebatch, graphics);

            _spritebatch.DrawString(Font, Text, new Vector2(X, Y), FontColor);
        }
    }
}