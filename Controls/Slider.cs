using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class Slider : Control
    {
        public float Min { get; set; } = 0f;
        public float Max { get; set; } = 1f;

        private float _value;
        public float Value
        {
            get => _value;
            set
            {
                float clamped = MathHelper.Clamp(value, Min, Max);
                if (_value != clamped)
                {
                    _value = clamped;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public Action<float> OnValueChanged;

        // Visuals
        public int TrackHeight { get; set; } = 4;
        public int ThumbWidth { get; set; } = 20;
        public int ThumbHeight { get; set; } = 20;

        public Color TrackColor { get; set; } = Color.DarkGray;
        public Color ThumbColor { get; set; } = Color.LightGray;

        private bool _dragging = false;

        public Slider()
        {
            Width = 100;
            Height = 30;
            BackgroundColor = Color.Transparent;
        }

        public override void Update()
        {
            base.Update();

            if (!IsActive)
                return;

            Rectangle thumbRect = GetThumbRect();

            // Start dragging
            if (Input.GetMouseButtonDown(Input.MouseButton.Left) &&
                thumbRect.Contains(Input.MousePos))
            {
                _dragging = true;
            }

            // Stop dragging
            if (Input.GetMouseButtonUp(Input.MouseButton.Left))
            {
                _dragging = false;
            }

            // Dragging logic
            if (_dragging)
            {
                float mouseX = Input.MousePos.X;
                float t = (mouseX - SourceRect.X) / (float)SourceRect.Width;
                Value = MathHelper.Lerp(Min, Max, MathHelper.Clamp(t, 0f, 1f));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible)
                return;

            // Track
            Rectangle trackRect = new Rectangle(
                SourceRect.X,
                SourceRect.Y + (SourceRect.Height - TrackHeight) / 2,
                SourceRect.Width,
                TrackHeight
            );

            spriteBatch.Draw(Texture, trackRect, TrackColor);

            // Thumb
            spriteBatch.Draw(Texture, GetThumbRect(), ThumbColor);

            // Optional border (reuse Control border logic)
            DrawBorder(spriteBatch);
        }

        private Rectangle GetThumbRect()
        {
            float t = (Value - Min) / (Max - Min);

            int trackStart = SourceRect.X + ThumbWidth / 2;
            int trackEnd   = SourceRect.Right - ThumbWidth / 2;
            int trackWidth = trackEnd - trackStart;

            int thumbX = (int)(trackStart + t * trackWidth) - ThumbWidth / 2;
            int thumbY = SourceRect.Y + (SourceRect.Height - ThumbHeight) / 2;

            return new Rectangle(thumbX, thumbY, ThumbWidth, ThumbHeight);
        }


        private void DrawBorder(SpriteBatch spriteBatch)
        {
            if (BorderThickness <= 0)
                return;

            spriteBatch.Draw(Texture, new Rectangle(SourceRect.Left, SourceRect.Top, SourceRect.Width, BorderThickness),BorderColor); // Top

            spriteBatch.Draw(Texture,
                new Rectangle(SourceRect.Left, SourceRect.Bottom - BorderThickness, SourceRect.Width, BorderThickness),
                BorderColor); // Bottom

            spriteBatch.Draw(Texture,
                new Rectangle(SourceRect.Left, SourceRect.Top, BorderThickness, SourceRect.Height),
                BorderColor); // Left

            spriteBatch.Draw(Texture,
                new Rectangle(SourceRect.Right - BorderThickness, SourceRect.Top, BorderThickness, SourceRect.Height),
                BorderColor); // Right
        }
    }
}
