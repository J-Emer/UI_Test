using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    internal enum ResizeDirection
    {
        None   = 0,
        Left   = 1 << 0,
        Right  = 1 << 1,
        Top    = 1 << 2,
        Bottom = 1 << 3
    }

    internal enum WindowDragMode
    {
        None,
        Move,
        Resize
    }

    public class Window : Control
    {
        public int Z_Order{get;set;} = 0;

        //dock
        public DockStyle Dock { get; set; } = DockStyle.None;
        public int DockSize { get; set; } = 200; 

        //const 
        private const int HEADER_HEIGHT = 40;
        private const int GRAB_SIZE = 15;
        private const int MIN_WIDTH = 100;
        private const int MIN_HEIGHT = 80;

        //font
        public SpriteFont Font { get; set; } = AssetLoader.DefaultFont;
        public string HeaderText { get; set; } = "Window";
        public Color FontColor { get; set; } = Color.White;
        public Vector2 TextPosition { get; set; } = Vector2.Zero;


        //close button
        private Rectangle _closeButtonRect;
        private Color _closeRectHoverColor = Color.Red;
        private Color _closeRectNormalColor = Color.Transparent;
        private Color _closeRectColor = Color.Transparent;
        public Action<Window> OnClose;


        //drag
        private Rectangle _headerRect;
        private Rectangle _drawRect; //this is the area (sourcerect - headerrect) that is avaliable for the children controls to be drawn
        private readonly Dictionary<ResizeDirection, Rectangle> _grabHandles = new Dictionary<ResizeDirection, Rectangle>();
        private WindowDragMode _dragMode = WindowDragMode.None;
        private ResizeDirection _resizeDir = ResizeDirection.None;
        private Vector2 _dragOffset;
        public Color HeaderColor { get; set; } = new Color(45, 45, 48);


        //padding / layout
        public int Padding{get;set;} = 5;
        public ChildCollection Children;
        public Layout Layout = new HorizontalLayout();

        private Desktop _parentDesktop;


        public Window(string _title, Desktop _parent) : base()
        {
            _parentDesktop = _parent;

            HeaderText = _title;
            Name = _title;

            X = 100;
            Y = 100;
            Width = 300;
            Height = 400;

            Children = new ChildCollection();
            Children.OnChildrenChanged += HandleDirty;
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();
            // Header
            _headerRect = new Rectangle(
                SourceRect.X,
                SourceRect.Y,
                SourceRect.Width,
                HEADER_HEIGHT
            );

            _drawRect = new Rectangle(
                SourceRect.X,
                SourceRect.Y + _headerRect.Height,
                SourceRect.Width,
                SourceRect.Height - _headerRect.Height
            );

            // Close button
            _closeButtonRect = new Rectangle(
                SourceRect.Right - HEADER_HEIGHT,
                SourceRect.Y,
                HEADER_HEIGHT,
                HEADER_HEIGHT
            );

            BuildGrabHandles();

            // Header text
            Vector2 textSize = Font.MeasureString(HeaderText);
            float x = SourceRect.X + 5; // small padding from left
            float y = SourceRect.Y + (_headerRect.Height - textSize.Y) * 0.5f;
            TextPosition = new Vector2(x, y);

            // Layout children AFTER window is finalized
            Layout.DoLayout(_drawRect, Children.Controls, Padding);

        }

        private void BuildGrabHandles()
        {
            _grabHandles.Clear();

            int x = SourceRect.X;
            int y = SourceRect.Y;
            int w = SourceRect.Width;
            int h = SourceRect.Height;

            // Edges
            _grabHandles[ResizeDirection.Left] = new Rectangle(x - GRAB_SIZE, y + GRAB_SIZE, GRAB_SIZE, h - GRAB_SIZE * 2);
            _grabHandles[ResizeDirection.Right] = new Rectangle(x + w, y + GRAB_SIZE, GRAB_SIZE, h - GRAB_SIZE * 2);
            _grabHandles[ResizeDirection.Top] = new Rectangle(x + GRAB_SIZE, y - GRAB_SIZE, w - GRAB_SIZE * 2, GRAB_SIZE);
            _grabHandles[ResizeDirection.Bottom] = new Rectangle(x + GRAB_SIZE, y + h, w - GRAB_SIZE * 2, GRAB_SIZE);

            // Corners
            _grabHandles[ResizeDirection.Top | ResizeDirection.Left] = new Rectangle(x - GRAB_SIZE, y - GRAB_SIZE, GRAB_SIZE, GRAB_SIZE);
            _grabHandles[ResizeDirection.Top | ResizeDirection.Right] = new Rectangle(x + w, y - GRAB_SIZE, GRAB_SIZE, GRAB_SIZE);
            _grabHandles[ResizeDirection.Bottom | ResizeDirection.Left] = new Rectangle(x - GRAB_SIZE, y + h, GRAB_SIZE, GRAB_SIZE);
            _grabHandles[ResizeDirection.Bottom | ResizeDirection.Right] = new Rectangle(x + w, y + h, GRAB_SIZE, GRAB_SIZE);
        }

        public override void Update()
        {
            base.Update();

            HandleWindowInteraction();

            for (int i = 0; i < Children.Controls.Count; i++)
            {
                Children.Controls[i].Update();
            }
        }

        private void HandleWindowInteraction()
        {
            Vector2 mouse = Input.MousePos;

            if(_closeButtonRect.Contains(mouse))
            {
                _closeRectColor = _closeRectHoverColor;
            }
            else
            {
                _closeRectColor = _closeRectNormalColor;
            }

            // Close button
            if (_closeButtonRect.Contains(mouse) && Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                OnClose?.Invoke(this);
                return;
            }

            // Begin interaction
            if (Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                if (_headerRect.Contains(mouse))
                {
                    _dragMode = WindowDragMode.Move;
                    _dragOffset = mouse - new Vector2(X, Y);
                    return;
                }

                foreach (var kv in _grabHandles)
                {
                    if (kv.Value.Contains(mouse))
                    {
                        _dragMode = WindowDragMode.Resize;
                        _resizeDir = kv.Key;
                        return;
                    }
                }
            }

            // Active interaction
            if (Input.GetMouseButton(Input.MouseButton.Left))
            {
                if (_dragMode == WindowDragMode.Move)
                {
                    X = (int)mouse.X - (int)_dragOffset.X;
                    Y = (int)mouse.Y - (int)_dragOffset.Y;
                }
                else if (_dragMode == WindowDragMode.Resize)
                {
                    ResizeWindow(mouse);
                }
            }

            // End interaction
            if (Input.GetMouseButtonUp(Input.MouseButton.Left))
            {
                _dragMode = WindowDragMode.None;
                _resizeDir = ResizeDirection.None;
            }
        }

        private void ResizeWindow(Vector2 mouse)
        {
            Vector2 pos = new Vector2(X, Y);
            Vector2 size = new Vector2(Width, Height);

            if (_resizeDir.HasFlag(ResizeDirection.Left))
            {
                float newX = mouse.X;
                float delta = pos.X - newX;
                pos.X = newX;
                size.X += delta;
            }

            if (_resizeDir.HasFlag(ResizeDirection.Right))
            {
                size.X = mouse.X - pos.X;
            }

            if (_resizeDir.HasFlag(ResizeDirection.Top))
            {
                float newY = mouse.Y;
                float delta = pos.Y - newY;
                pos.Y = newY;
                size.Y += delta;
            }

            if (_resizeDir.HasFlag(ResizeDirection.Bottom))
            {
                size.Y = mouse.Y - pos.Y;
            }

            size.X = Math.Max(MIN_WIDTH, size.X);
            size.Y = Math.Max(MIN_HEIGHT, size.Y);

            X = (int)pos.X;
            Y = (int)pos.Y;
            Width = (int)size.X;
            Height = (int)size.Y;
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);

            // Header
            _spritebatch.Draw(Texture, _headerRect, HeaderColor);
            _spritebatch.DrawString(Font, HeaderText, TextPosition, FontColor);
           
            // Close button
            _spritebatch.Draw(Texture, _closeButtonRect, _closeRectColor);
            _spritebatch.DrawString(Font, "X", new Vector2(_closeButtonRect.X + 15, _closeButtonRect.Y + 10), Color.White);

            //Children.Controls.ForEach(x => x.Draw(_spritebatch));

            for (int i = Children.Controls.Count - 1; i >= 0; i--)
            {
                Children.Controls[i].Draw(_spritebatch);
            }
        }

    }
}