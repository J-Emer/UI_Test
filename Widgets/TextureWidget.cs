using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Controls;

namespace UI.Widgets
{
    public class TextureWidget : Control
    {
        private Label _label;
        public Texture2D DisplayTexture = null;
        public int TextureWidth = 100;
        public int TextureHeight = 100;
        public int Padding = 10;
       
        private Rectangle _textureRect = new Rectangle();

        public TextureWidget(string _title) : base()
        {
            _label = new Label
            {
                Text = _title
            };

            Width = 200;
            Height = 200;
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();

            int xPos = X + Padding;

            _label.X = X + Padding;
            _label.Y = Y + Padding;

            int yPos = _label.Y + 30 + Padding;

            _textureRect = new Rectangle(xPos, yPos, 100, 100);
        }

        public override void Update()
        {
            base.Update();

            _label.Update();
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            _label.Draw(_spritebatch);

            if(DisplayTexture != null)
            {
                _spritebatch.Draw(DisplayTexture, _textureRect, Color.White);                
            }
        }
    }
}