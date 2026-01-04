using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;


namespace UI.Controls
{
    public class ListView : ContainerControl
    {
        private ChildCollection Children;
        public int Padding{get;set;} = 5;

        public int ChildWidth{get;set;} = 100;
        public int ChildHeight{get;set;} = 100;


        public int TextPadding { get; set; } = 10; // space between rect and text
        public int RowPadding  { get; set; } = 5;  // extra space between rows
        public int FontHeight  { get; set; } = 16; // or pull from font.LineSpacing
        public Action<ListViewItem> OnItemSelected;
        public ListViewItem SelectedItem{get;private set;} = null;

        public ListView() : base()
        {
            Children = new ChildCollection();
            Children.OnChildrenChanged += HandleDirty;
            
            foreach (var item in Children.Controls)
            {
                Console.WriteLine(item.SourceRect.ToString());
            }
        }
        protected override void HandleDirty()
        {
            base.HandleDirty();

            if (Children.Controls == null || Children.Controls.Count == 0)
                return;

            int availableWidth = SourceRect.Width - Padding * 2;

            int columns = Math.Max(1, (availableWidth + Padding) / (ChildWidth + Padding));

            int rowHeight =
                ChildHeight +
                TextPadding +
                FontHeight +
                Padding;

            for (int i = 0; i < Children.Controls.Count; i++)
            {
                int col = i % columns;
                int row = i / columns;

                int x = SourceRect.X + Padding + col * (ChildWidth + Padding);
                int y = SourceRect.Y + Padding + row * rowHeight;

                var child = Children.Controls[i];

                child.X = x;
                child.Y = y;
                child.Width  = ChildWidth;
                child.Height = ChildHeight;
            }
        }
        public override void Update()
        {
            base.Update();

            Children.Controls.ForEach(x => x.Update());
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawChildrenClipped(spriteBatch, Children.Controls);
        }

        internal void ListItemClicked(ListViewItem _item)
        {
            SelectedItem = _item;
            OnItemSelected?.Invoke(SelectedItem);
        }
        public void AddItem(string text)
        {
            ListViewItem _item = new ListViewItem(this, text, ListItemClicked)
            {
                Width = ChildWidth,
                Height = ChildHeight
            };
            Children.Add(_item);
        }
        public void AddItem(string text, Texture2D texture)
        {
            ListViewItem _item = new ListViewItem(this, text, ListItemClicked)
            {
                Texture = texture,
                Width = ChildWidth,
                Height = ChildHeight                
            };
            Children.Add(_item);
        }
        public void AddItem(string text, Texture2D texture, object userdata)
        {
            ListViewItem _item = new ListViewItem(this, text, ListItemClicked)
            {
                Texture = texture,
                UserData = userdata,
                Width = ChildWidth,
                Height = ChildHeight                
            };
            Children.Add(_item);
        }

    }


    public class ListViewItem : Control
    {
        private ListView Parent;
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "ListViewItem";
        public Color FontColor{get;set;} = Color.Black;
        private Vector2 _fontPos = Vector2.Zero;
        public Color BorderNormalColor{get;set;} = Color.Transparent;
        public Color BorderHighlightColor{get;set;} = Color.Black;
        private int ExpandAmount = 5;
        private Action<ListViewItem> OnItemClicked;


        public ListViewItem(ListView _parent, string _text, Action<ListViewItem> onItemClicked) : base()
        {
            Parent = _parent;
            Text = _text;
            Width = 100;
            Height = 100;
            BorderColor = BorderNormalColor;
            BorderThickness = 3;
            OnItemClicked = onItemClicked;
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();

            float xPos = SourceRect.Left;
            float yPos = SourceRect.Bottom + 10;

            _fontPos = new Vector2(xPos, yPos);
        }
        protected override void InternalMouseClick()
        {
            OnItemClicked?.Invoke(this);
        }
        protected override void InternalMouseEnter()
        {
            BorderColor = BorderHighlightColor;
            
            X -= ExpandAmount / 2;
            Y -= ExpandAmount / 2;

            Width += ExpandAmount;
            Height += ExpandAmount;
        }
        protected override void InternalMouseExit()
        {
            BorderColor = BorderNormalColor;

            X += ExpandAmount / 2;
            Y += ExpandAmount / 2;

            Width -= ExpandAmount;
            Height -= ExpandAmount;            
        }
        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            _spritebatch.DrawString(Font, Text, _fontPos, FontColor);
        }
    }


}