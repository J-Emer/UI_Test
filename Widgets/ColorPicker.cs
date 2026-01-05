using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Controls;
using UI.Util;

namespace UI.Widgets
{
    public class ColorPicker : Control
    {
        private ChildCollection Children = new ChildCollection();
        private Layout Layout = new RowLayout();
        private int Padding = 5;
        private float r = 0;
        private float g = 0;
        private float b = 0;
        public Color Color{get; private set;} = Color.White;
        public Action<Color> OnColorChanged;
        private ColorDisplay _display;

        public ColorPicker() : base()
        {
            _display = new ColorDisplay();
            Slider rSlider = new Slider
            {
                ThumbColor = Color.Red,
                Value = 0.5f
            };
            rSlider.OnValueChanged += RValueChanged;


            Slider gSlider = new Slider
            {
                ThumbColor = Color.Green,
                Value = 0.5f
            };
            gSlider.OnValueChanged += GValueChanged;

            Slider bSlider = new Slider
            {
                ThumbColor = Color.Blue,
                Value = 0.5f
            };
            bSlider.OnValueChanged += BValueChanged;


            Children.Add(_display);
            Children.Add(rSlider);
            Children.Add(gSlider);
            Children.Add(bSlider);

            Width = 200;
            Height = 200;
        }

        private void BValueChanged(float obj)
        {
            b = obj;
           DoColor();
        }

        private void GValueChanged(float obj)
        {
            g = obj;
           DoColor();
        }

        private void RValueChanged(float obj)
        {
           r = obj;
           DoColor();
        }
        private void DoColor()
        {
            Color = new Color(r,g,b);
            _display.BackgroundColor = Color;
            OnColorChanged?.Invoke(Color);
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();

            Layout.DoLayout(SourceRect, Children.Controls, Padding);
        }

        public override void Update()
        {
            base.Update();

            Children.Controls.ForEach(x => x.Update());
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            Children.Controls.ForEach(x => x.Draw(_spritebatch));
        }
    }

    internal class ColorDisplay : Control
    {
        public ColorDisplay() : base()
        {
            Width = 100;
            Height = 100;
        }
    }
}