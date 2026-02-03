using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Controls;
using UI.Util;

namespace UI.Widgets
{
    public class ColorPicker : Window
    {

        public int Red{get; private set;}
        public int Green{get; private set;}
        public int Blue{get; private set;}
        private ColorPanel _colorPanel;
        private Label _label;
        public Color Color => _colorPanel.BackgroundColor;
        public Action<Color> OnValueChanged;

        public ColorPicker(string name) : base(name)
        {
            Layout = new RowLayout();
            Padding = 5;

            _colorPanel = new ColorPanel
            {
                Width = 200,
                Height = 200
            };

            Children.Add(_colorPanel);

            Slider _rSlider = new Slider
            {
                ThumbColor = Color.Red
            };
            _rSlider.OnValueChanged += RChanged;
            Children.Add(_rSlider);

            Slider _gSlider = new Slider
            {
                ThumbColor = Color.Green
            };
            _gSlider.OnValueChanged += GChanged;  
            Children.Add(_gSlider);

            Slider _bSlider = new Slider
            {
                ThumbColor = Color.Blue
            };
            _bSlider.OnValueChanged += BChanged; 
            Children.Add(_bSlider);

            _label = new Label
            {
                Text = "",
                FontColor = Color.White
            };
            Children.Add(_label);

            Width = 300;
            Height = 350;                     
        }
        private void BChanged(float obj)
        {
            Blue = (int)(obj * 255);
            HandleColor();
        }
        private void GChanged(float obj)
        {
            Green = (int)(obj * 255);
            HandleColor();
        }
        private void RChanged(float obj)
        {
            Red = (int)(obj * 255);
            HandleColor();
        }
        private void HandleColor()
        {
            _colorPanel.BackgroundColor = new Color(Red, Green, Blue);
            _label.Text = $"R: {Red}, Green: {Green}, Blue: {Blue}";
            OnValueChanged?.Invoke(_colorPanel.BackgroundColor);
        }
        public void SetColor(Color _color)
        {
            Red = _color.R;
            Green = _color.G;
            Blue = _color.B;

            _colorPanel.BackgroundColor = new Color(Red, Green, Blue);
            Console.WriteLine($"{GetType().Name}.SetColor() //---todo: update slider positions when setting color---//");
        }
        protected override void AfterInvalidation()
        {
            base.AfterInvalidation();

            Layout.DoLayout(_bodyRect, Children.Controls, Padding);
        }
        public override void Update()
        {
            base.Update();

            for (int i = 0; i < Children.Controls.Count; i++)
            {
                Children.Controls[i].Update();
            }
        }
        public override void Draw(SpriteBatch _spritebatch, GraphicsDeviceManager graphics)
        {
            base.Draw(_spritebatch, graphics);

            ScissorStack.Push(graphics.GraphicsDevice, SourceRect);

            for (int i = 0; i < Children.Controls.Count; i++)
            {
                Children.Controls[i].Draw(_spritebatch, graphics);
            }            
        
            ScissorStack.Pop(graphics.GraphicsDevice);
        }
    
    
    }

    public class ColorPanel : Control
    {
        
    }

}