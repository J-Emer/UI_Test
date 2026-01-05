using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Controls;

namespace UI.Widgets
{
    public class Vector2Control : Control
    {
        private Label _headerLable;
        private Label _xLable;
        private Label _yLable;
        private NumericTextBox _xTextBox;
        private NumericTextBox _yTextBox;
        private int Padding = 5;
        private List<Control> Children = new List<Control>();

        public Vector2Control(string label, Vector2 vector) : base()
        {
            Width = 200;
            Height = 75;

            _headerLable = new Label{Text = label};
            _xLable = new Label{Text = "X:"};
            _yLable = new Label{Text = "Y:"};
            _xTextBox = new NumericTextBox{Text = vector.X.ToString()};
            _yTextBox = new NumericTextBox{Text = vector.Y.ToString()};


            Children.Add(_headerLable);
            Children.Add(_xLable);
            Children.Add(_yLable);
            Children.Add(_xTextBox);
            Children.Add(_yTextBox);
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();

            int xPos = X + Padding;
            int yPos = Y + Padding;

            _headerLable.X = xPos;
            _headerLable.Y = yPos;

            yPos += 30 + Padding;

            int _xLableWidth = (int)_xLable.Font.MeasureString(_xLable.Text).X + Padding; //+ (Padding * 5); 
            int _yLableWidth = (int)_yLable.Font.MeasureString(_xLable.Text).X + Padding; //+ (Padding * 5); 

            _xLable.X = xPos;
            _xLable.Y = yPos + 5;

            xPos += _xLableWidth;

            _xTextBox.X = xPos;
            _xTextBox.Y = yPos;

            xPos += _xTextBox.Width + Padding;

            _yLable.X = xPos;
            _yLable.Y = yPos + 5;

            xPos += _yLableWidth;

            _yTextBox.X = xPos;
            _yTextBox.Y = yPos;
        }

        public override void Update()
        {
            base.Update();

            _headerLable.Update();

            _xLable.Update();
            _yLable.Update();

            _xTextBox.Update();
            _yTextBox.Update();
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            _headerLable.Draw(_spritebatch);

            _xLable.Draw(_spritebatch);
            _yLable.Draw(_spritebatch);

            _xTextBox.Draw(_spritebatch);
            _yTextBox.Draw(_spritebatch);          
        }

    }
}