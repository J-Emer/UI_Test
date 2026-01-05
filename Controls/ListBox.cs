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
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();

            int sizeY = Padding;

            foreach (var item in Items.Controls)
            {
                sizeY += item.Height + Padding;
            }

            _scrollRect = new Rectangle(
                X + Padding,
                _scrollY + Y,
                Width - (Padding + Padding),
                sizeY
            );

            layout.DoLayout(_scrollRect, Items.Controls.ToList<Control>(), Padding);
        }

        public void AddItem(string _text)
        {
            Items.Add(new ListBoxItem(_text));
        }

        public override void Update()
        {
            base.Update();

            if(_scrollRect.Contains(Input.MousePos))
            {
                _scrollY += (int)(Input.ScrollWheelDelta * 5);
                HandleDirty();

                Console.WriteLine(_scrollY.ToString());
            }

            foreach (var item in Items.Controls)
            {
                if(item.IsActive)
                {
                    item.Update();
                }
            }
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            foreach (var item in Items.Controls)
            {
                if(item.IsActive)
                {
                    item.Draw(_spritebatch);
                }
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(
                Font,
                Text,
                new Vector2(SourceRect.X + 4, SourceRect.Y + 4),
                Color.Black
            );
        }
    }





}
