
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class Window : DesktopControl
    {
        public const int HeaderHeight = 30;
        public const int CloseBtnSize = 30;

        public ChildCollection Children{get; private set;} = new ChildCollection();
        public Color HeaderBackColor = new Color(24, 25, 26);
        protected Rectangle _headerRect = new Rectangle();
        protected Rectangle _bodyRect = new Rectangle();
        protected Button _closeButton;
        private bool _canDrag = false;
        private Vector2 _dragOffset = Vector2.Zero;
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "Window";
        public Color TextColor{get;set;} = Color.White;
        protected Vector2 TextPosition{get;set;} = Vector2.Zero;
        public int Padding{get;set;} = 5;
        public Layout Layout{get;set;} = new RowLayout();


#region Resize
        private const int ResizeHandleSize = 32;
        private ResizeDir _resizeDir = ResizeDir.None;
        private Vector2 _resizeStartMouse;
        private Rectangle _resizeStartRect;
        public int MinWidth = 150;
        public int MinHeight = 100;
        public bool ShowGrabHandles{get;set;} = false;

#endregion


        public Window(string name) : base()
        {
            _closeButton = new Button
            {
                Width = CloseBtnSize,
                Height = CloseBtnSize,
                BackgroundColor = Color.Transparent,
                NormalColor = Color.Transparent,
                HighlightColor = Color.Red,
                FontColor = Color.White,
                Text = "X",
                BorderThickness = 0
            };
            _closeButton.OnClick += Close;


            Name = name;
            Text = Name;
            Width = 400;
            Height = 400;
            BackgroundColor = new Color(51, 52, 54);

            Children.OnChildrenChanged += AfterInvalidation;
        }
        private void Close(Control control, Input.MouseButton button)
        {
            ParentDesktop.Remove(this);
        }
        protected override void AfterInvalidation()
        {
            _headerRect = new Rectangle(X, Y, Width, HeaderHeight);

            _bodyRect = new Rectangle(
                X,
                Y + HeaderHeight,
                Width,
                Height - HeaderHeight
            );

            _closeButton.X = SourceRect.Right - CloseBtnSize;
            _closeButton.Y = Y;
            
            TextPosition = new Vector2(X + Padding, Y + Padding); 

            Layout.DoLayout(_bodyRect, Children.Controls, Padding);
        }
        public override void Update()
        {
            base.Update();

            _closeButton.Update();
            HandleDrag();
            HandleResize();

            for (int i = 0; i < Children.Controls.Count; i++)
            {
                Children.Controls[i].Update();
            }
        }
        private void HandleDrag()
        {
            if (Dock != DockStyle.None)
            {
                return;                
            }

            if (_resizeDir != ResizeDir.None)
            {
                return;
            }

            if(_headerRect.Contains(Input.MousePos) && Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                _canDrag = true;
                _dragOffset = Input.MousePos - new Vector2(X, Y);
                // ParentDesktop.IsDragging = true;
            }

            if(_canDrag)
            {
                X = (int)(Input.MousePos.X - _dragOffset.X);
                Y = (int)(Input.MousePos.Y - _dragOffset.Y);
            }

            if(Input.GetMouseButtonUp(Input.MouseButton.Left))
            {
                _canDrag = false;

                //---see if this control was dropped in a dock-drop-zone
                // ParentDesktop.CheckDropZone(this);
                // ParentDesktop.IsDragging = false;
            }            
        }
        private ResizeDir GetResizeDir(Point mouse)
        {
            ResizeDir dir = ResizeDir.None;
            Rectangle r = SourceRect;
            int s = ResizeHandleSize;

            // LEFT edge
            if (new Rectangle(r.Left - s, r.Top + HeaderHeight, s, r.Height).Contains(mouse))
                dir |= ResizeDir.Left;

            // RIGHT edge
            if (new Rectangle(r.Right, r.Top + HeaderHeight, s, r.Height).Contains(mouse))
                dir |= ResizeDir.Right;

            // BOTTOM edge
            if (new Rectangle(r.Left - s, r.Bottom, r.Width + (s * 2), s).Contains(mouse))
                dir |= ResizeDir.Bottom;

            return dir;
        }
        private void HandleResize()
        {
            if (Dock != DockStyle.None)
            {
                return;                
            }

            Point mouse = Input.MousePos.ToPoint();

            // Begin resize
            if (_resizeDir == ResizeDir.None &&
                Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                ResizeDir dir = GetResizeDir(mouse);

                // Don't resize when dragging header
                if (dir != ResizeDir.None && !_headerRect.Contains(mouse))
                {
                    _resizeDir = dir;
                    _resizeStartMouse = Input.MousePos;
                    _resizeStartRect = SourceRect;
                    return;
                }
            }

            // Perform resize
            if (_resizeDir != ResizeDir.None)
            {
                Vector2 delta = Input.MousePos - _resizeStartMouse;

                Rectangle r = _resizeStartRect;

                if (_resizeDir.HasFlag(ResizeDir.Left))
                {
                    r.X += (int)delta.X;
                    r.Width -= (int)delta.X;
                }

                if (_resizeDir.HasFlag(ResizeDir.Right))
                {
                    r.Width += (int)delta.X;
                }

                if (_resizeDir.HasFlag(ResizeDir.Top))
                {
                    r.Y += (int)delta.Y;
                    r.Height -= (int)delta.Y;
                }

                if (_resizeDir.HasFlag(ResizeDir.Bottom))
                {
                    r.Height += (int)delta.Y;
                }

                // Clamp
                r.Width = Math.Max(MinWidth, r.Width);
                r.Height = Math.Max(MinHeight, r.Height);

                X = r.X;
                Y = r.Y;
                Width = r.Width;
                Height = r.Height;

                if (Input.GetMouseButtonUp(Input.MouseButton.Left))
                {
                    _resizeDir = ResizeDir.None;
                }
            }
        }
        public override void Draw(SpriteBatch _spritebatch, GraphicsDeviceManager graphics)
        {
            base.Draw(_spritebatch, graphics);

            ScissorStack.Push(graphics.GraphicsDevice, SourceRect);

            _spritebatch.Draw(Texture, _headerRect, HeaderBackColor);
            _closeButton.Draw(_spritebatch, graphics);
            _spritebatch.DrawString(Font, Text, TextPosition, TextColor);

            //reversed loop so it draws from the bottom up
            for (int i = Children.Controls.Count - 1; i >= 0; i--)
            {
                Children.Controls[i].Draw(_spritebatch, graphics);
            }
            

            ScissorStack.Pop(graphics.GraphicsDevice);

            DrawResizeHandles(_spritebatch);
        }
        private void DrawResizeHandles(SpriteBatch sb)
        {
            if(!ShowGrabHandles){return;}

            Color c = Color.Yellow * 0.25f;
            Rectangle r = SourceRect;
            int s = ResizeHandleSize;

            // Left
            sb.Draw(Texture,new Rectangle(r.Left - s, r.Top + HeaderHeight, s, r.Height),c);

            // Right
            sb.Draw(Texture,new Rectangle(r.Right, r.Top + HeaderHeight, s, r.Height),c);

            // Bottom
            sb.Draw(Texture,new Rectangle(r.Left - s, r.Bottom, r.Width + (s * 2), s),c);
        }

    }
}