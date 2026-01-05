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
        public Action<ListBoxItem> ItemSelected;
        private int contentHeight = 0;
        private int minScroll => Math.Min(0, SourceRect.Height - contentHeight); // negative value if content is taller
        private int maxScroll = 0; // can't scroll past top
        public ListBox() : base()
        {
            Items.OnChildrenChanged += HandleDirty;

            Width = 200;
            Height = 200;

            _scrollRect = new Rectangle(100, 100, 100, 100);
        }

        public void AddItem(string text)
        {
            Items.Add(new ListBoxItem(text, this));
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();
        
            contentHeight = Items.Controls.Sum(item => item.Height + Padding) + Padding;

            _scrollRect = new Rectangle(
                X + Padding,
                Y + _scrollY,
                Width - (Padding * 2),
                contentHeight
            );

            layout.DoLayout(_scrollRect, Items.Controls.ToList<Control>(), Padding);
        }

        public override void Update()
        {
            base.Update();
        
            if(SourceRect.Contains(Input.MousePos) && Input.ScrollWheelDelta != 0)
            {
                _scrollY += (int)(Input.ScrollWheelDelta * 10);

                contentHeight = Items.Controls.Sum(item => item.Height + Padding) + Padding;

                _scrollY = Math.Clamp(_scrollY, minScroll, maxScroll);

                HandleDirty();
            }

            for (int i = 0; i < Items.Controls.Count; i++)
            {
                Items.Controls[i].IsActive = SourceRect.Contains(Items.Controls[i].SourceRect);
                Items.Controls[i].IsVisible = SourceRect.Contains(Items.Controls[i].SourceRect);                
                Items.Controls[i].Update();
            }
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            for (int i = 0; i < Items.Controls.Count; i++)
            {
                Items.Controls[i].Draw(_spritebatch);
            }
        }

    }



    public class ListBoxItem : Control
    {
        private ListBox _parent;
        public string Text;
        public SpriteFont Font = AssetLoader.DefaultFont;

        public ListBoxItem(string text, ListBox parent)
        {
            _parent = parent;

            Text = text;
            BackgroundColor = Color.Gray;
            BorderThickness = 1;
            Width = 100;
            Height = 30;
        }

        protected override void InternalMouseClick()
        {
            _parent.ItemSelected?.Invoke(this);
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
