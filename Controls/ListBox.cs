using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class ListBox : Control
    {
        private ChildCollection<ListBoxItem> Items = new ChildCollection<ListBoxItem>();
        public int Padding{get;set;} = 0;
        public Action<ListBoxItem> OnItemSelected;
        public ListBoxItem SelectedItem{get; private set;}
        private Layout _layout = new RowLayout();
        private Rectangle _scrollRect = new Rectangle();
        private int _scrollY = 0;
        private int _contentHeight = 0;

        public ListBox() : base()
        {
            Width = 200;
            Height = 100;
            BackgroundColor = new Color(51, 52, 54);
            Items.OnChildrenChanged += AfterInvalidation;
        }

        public void Add(string text, object userdata = null)
        {
            Items.Add(new ListBoxItem(this, ItemCallBack, text, userdata));
        }
        private void ItemCallBack(ListBoxItem _item)
        {
            SelectedItem = _item;
            OnItemSelected?.Invoke(SelectedItem);
        }
        protected override void AfterInvalidation()
        {
            _scrollRect = new Rectangle(
                SourceRect.X,
                _scrollY + Y,
                SourceRect.Width,
                _contentHeight
            );


            _layout.DoLayout(_scrollRect, Items.Controls.ToList<Control>(), Padding);
        }

        public override void Update()
        {
            base.Update();

            if(Input.ScrollWheelDelta != 0)
            {
                _scrollY += (int)Input.ScrollWheelDelta * 5;

                _contentHeight = Items.Controls.Sum(item => item.Height + Padding) + Padding;
                int minScroll = Math.Min(0, SourceRect.Height - _contentHeight); // negative value if content is taller
                int maxScroll = 0; // can't scroll past top

                _scrollY = Math.Clamp(_scrollY, minScroll, maxScroll);
                _scrollY = Math.Clamp(_scrollY, minScroll, maxScroll);
 
                AfterInvalidation();
            }

            for (int i = 0; i < Items.Controls.Count; i++)
            {
                bool contains = SourceRect.Contains(Items.Controls[i].SourceRect);
                Items.Controls[i].IsVisible = contains;
                Items.Controls[i].IsActive = contains;
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

            //border for scrollrect for testing
            _spritebatch.Draw(Texture, new Rectangle(_scrollRect.Left, _scrollRect.Top, _scrollRect.Width, 2), Color.White);//top
            _spritebatch.Draw(Texture, new Rectangle(_scrollRect.Right - 2, _scrollRect.Top, 2, _scrollRect.Height), Color.White);//right
            _spritebatch.Draw(Texture, new Rectangle(_scrollRect.Left, _scrollRect.Bottom - 2, _scrollRect.Width, 2), Color.White);//bottom
            _spritebatch.Draw(Texture, new Rectangle(_scrollRect.Left, _scrollRect.Top, 2, _scrollRect.Height), Color.White);//left
                    
        }
    }

    public class ListBoxItem : Control
    {
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "Item";
        public Color FontColor{get;set;} = Color.White;
        private Vector2 _textPos = Vector2.Zero;
        public Color HighlightColor{get;set;} = new Color(19, 90, 161);
        public Color NormalColor{get;set;} = Color.Transparent;   

        private ListBox _parent;
        private Action<ListBoxItem> _clickCallBack;

        public ListBoxItem(ListBox parent, Action<ListBoxItem> callback, string text, object userdata) : base()
        {
            _parent = parent;
            _clickCallBack = callback;
            Text = text;
            UserData = userdata;
            Width = 150;
            Height = 30;
            BackgroundColor = NormalColor;
            BorderThickness = 0;
        }
        protected override void AfterInvalidation()
        {
            int x = X + 5;
            int y = Y + 5;
            _textPos = new Vector2(x, y);
        }
        protected override void InternalMouseClick()
        {
            _clickCallBack?.Invoke(this);
        }
        protected override void InternalMouseEnter()
        {
            BackgroundColor = HighlightColor;
        }
        protected override void InternalMouseExit()
        {
            BackgroundColor = NormalColor;
        }        
        public override void Draw(SpriteBatch _spritebatch)
        {
            if(!IsVisible){return;} //why does this work here, but in the Control it doesn't? IsActive works in Control
            base.Draw(_spritebatch);

            _spritebatch.DrawString(Font, Text, _textPos, FontColor);
        }
    }
}