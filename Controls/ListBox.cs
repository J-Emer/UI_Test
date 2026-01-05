using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class ListBox : Control
    {
        private ChildCollection<ListBoxItem> Items = new ChildCollection<ListBoxItem>();
        private int Padding = 5;
        private Layout layout = new RowLayout();
        private Rectangle _scrollRect;
        private int _scrollY = 0;


        public ListBox() : base()
        {
            Width = 200;
            Height = 200;

            _scrollY = Y + Padding;

            HandleDirty();
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();

            int sizeY = Padding;

            foreach (var item in Items.Controls)
            {
                sizeY += item.Height + Padding;

                item.IsActive = item.Y >= SourceRect.Y;
            }

            _scrollRect = new Rectangle(
                X + Padding,
                _scrollY + Y,
                Width - (Padding + Padding),
                sizeY
            );

            layout.DoLayout(_scrollRect, Items.Controls.ToList<Control>(), Padding);

            foreach (var item in Items.Controls)
            {
                item.IsActive = SourceRect.Contains(item.SourceRect);
                item.IsVisible = SourceRect.Contains(item.SourceRect);
            }            
        }

        public void AddItem(string _text)
        {
            Items.Add(new ListBoxItem(_text));
            HandleDirty();
        }

        public override void Update()
        {
            base.Update();

            if(SourceRect.Contains(Input.MousePos))
            {
                _scrollY += (int)(Input.ScrollWheelDelta * 5);
                HandleDirty();
            }

            // Clamp _scrollY so it can't scroll past the top or bottom
            int contentHeight = Items.Controls.Sum(item => item.Height + Padding) + Padding;
            int minScroll = Math.Min(0, SourceRect.Height - contentHeight); // negative value if content is taller
            int maxScroll = 0; // can't scroll past top

            _scrollY = Math.Clamp(_scrollY, minScroll, maxScroll);

            foreach (var item in Items.Controls)
            {
                item.Update();
            }
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            foreach (var item in Items.Controls)
            {
                item.Draw(_spritebatch);
            }
        }

    }



    public class ListBoxItem : Control
    {
        public string Text;
        public SpriteFont Font = AssetLoader.DefaultFont;

        public ListBoxItem(string text)
        {
            Text = text;
            BackgroundColor = Color.Gray;
            BorderThickness = 1;
            Width = 100;
            Height = 30;
        }

        protected override void InternalMouseClick()
        {
            Console.WriteLine(Text);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(IsVisible)
            {
                spriteBatch.DrawString(
                    Font,
                    Text,
                    new Vector2(SourceRect.X + 4, SourceRect.Y + 4),
                    Color.Black
                );
            }


        }
    }





}
