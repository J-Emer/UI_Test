using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class Label : Control
    {
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "Label";
        public Color FontColor{get;set;} = Color.Black;

        
        public Label() : base()
        {
            BackgroundColor = Color.Transparent;
            BorderThickness = 0;
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            //base.Draw(_spritebatch);

            _spritebatch.DrawString(Font, Text, new Vector2(X, Y), FontColor);
        }
    }
}