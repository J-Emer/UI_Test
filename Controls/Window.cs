using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{


    public class Window : Control
    {
        public Desktop Desktop;
        public int Z_Order { get; set; } = 0;

        // Docking
        private DockStyle _dock = DockStyle.None;

        public DockStyle Dock
        {
            get => _dock;
            set
            {
                if (_dock == value)
                    return;

                DockStyle previous = _dock;
                _dock = value;

                // Transition: Docked -> Undocked
                if (previous != DockStyle.None && _dock == DockStyle.None)
                {
                    Width = 600;
                    Height = 400;
                    EnableAllResizeHandles();
                }

                LayoutInvalidated?.Invoke(this);
            }
        }

        public int DockSize { get; set; } = 250;

        // Constants
        private const int HEADER_HEIGHT = 40;
        private const int GRAB_SIZE = 15;
        private const int MIN_WIDTH = 250;
        private const int MIN_HEIGHT = 80;

        // Header / font
        public SpriteFont Font { get; set; } = AssetLoader.DefaultFont;
        public string HeaderText { get; set; } = "Window";
        public Color FontColor { get; set; } = Color.White;
        public Color HeaderColor { get; set; } = new Color(45, 45, 48);
        public Vector2 TextPosition { get; set; }

        // Close button
        private Rectangle _closeButtonRect;
        private Color _closeHover = Color.Red;
        private Color _closeNormal = Color.Transparent;
        private Color _closeColor = Color.Transparent;
        public Action<Window> OnClose;

        // Drag / resize state
        private Rectangle _headerRect;
        private Rectangle _drawRect;
        private readonly Dictionary<ResizeDirection, Rectangle> _grabHandles = new();
        private WindowDragMode _dragMode = WindowDragMode.None;
        private ResizeDirection _resizeDir = ResizeDirection.None;
        private Vector2 _dragOffset;

        // Resize permissions (set by DockManager)
        internal HashSet<ResizeDirection> AllowedResizeDirections = new();

        // Tracks dock state during resize
        private bool _wasDockedOnResize = false;

        // Layout
        public int Padding { get; set; } = 5;
        private ChildCollection Children;
        public Layout Layout = new HorizontalLayout();
        private DropDownbutton _dockButton;
        public event Action<Window> LayoutInvalidated;


        public Window(string title)
        {
            Name = title;
            HeaderText = title;

            X = 100;
            Y = 100;
            Width = 300;
            Height = 400;

            _dockButton = new DropDownbutton("None", new List<string>{"Left", "Right", "Top", "Bottom", "Fill", "None"})
            {
                Width = 70,
                Height = 30,
                BackgroundColor = Color.Transparent,
                BorderThickness = 0,
                FontColor = Color.White
            };
            _dockButton.OnItemSelected += DockSelected;
            
            Children = new ChildCollection();
            Children.OnChildrenChanged += HandleDirty;
        }

        private void DockSelected(string obj)
        {
            Enum.TryParse(obj, out DockStyle newDockStyle);
            _dock = newDockStyle;
            Desktop.HandleLayout();
        }

        public void EnableResizeHandles(ResizeDirection allowed)
        {
            AllowedResizeDirections.Clear();
            if (allowed != ResizeDirection.None)
                AllowedResizeDirections.Add(allowed);
        }

        public void EnableAllResizeHandles()
        {
            AllowedResizeDirections.Clear();
        }

        public void SetBounds(Rectangle r)
        {
            X = r.X;
            Y = r.Y;
            Width = r.Width;
            Height = r.Height;
        }

        protected override void HandleDirty()
        {
            base.HandleDirty();

            if(_dockButton.Text != _dock.ToString())
            {
                _dockButton.Text = _dock.ToString();
            }

            _headerRect = new Rectangle(SourceRect.X, SourceRect.Y, SourceRect.Width, HEADER_HEIGHT);

            _drawRect = new Rectangle(
                SourceRect.X,
                SourceRect.Y + HEADER_HEIGHT,
                SourceRect.Width,
                SourceRect.Height - HEADER_HEIGHT
            );

            _closeButtonRect = new Rectangle(
                SourceRect.Right - HEADER_HEIGHT,
                SourceRect.Y,
                HEADER_HEIGHT,
                HEADER_HEIGHT
            );

            _dockButton.X = _closeButtonRect.X - _dockButton.Width;
            _dockButton.Y = _closeButtonRect.Y + Padding;

            BuildGrabHandles();

            Vector2 textSize = Font.MeasureString(HeaderText);
            TextPosition = new Vector2(
                SourceRect.X + 8,
                SourceRect.Y + (_headerRect.Height - textSize.Y) * 0.5f
            );

            Layout.DoLayout(_drawRect, Children.Controls, Padding);
        }

        private void BuildGrabHandles()
        {
            _grabHandles.Clear();

            int x = SourceRect.X;
            int y = SourceRect.Y;
            int w = SourceRect.Width;
            int h = SourceRect.Height;

            _grabHandles[ResizeDirection.Left]   = new Rectangle(x - GRAB_SIZE, y + GRAB_SIZE, GRAB_SIZE, h - GRAB_SIZE * 2);
            _grabHandles[ResizeDirection.Right]  = new Rectangle(x + w, y + GRAB_SIZE, GRAB_SIZE, h - GRAB_SIZE * 2);
            _grabHandles[ResizeDirection.Top]    = new Rectangle(x + GRAB_SIZE, y - GRAB_SIZE, w - GRAB_SIZE * 2, GRAB_SIZE);
            _grabHandles[ResizeDirection.Bottom] = new Rectangle(x + GRAB_SIZE, y + h, w - GRAB_SIZE * 2, GRAB_SIZE);
        }

        public override void Update()
        {
            base.Update();
            HandleWindowInteraction();

            _dockButton.Update();

            for (int i = 0; i < Children.Controls.Count; i++)
                Children.Controls[i].Update();
        }

        private void HandleWindowInteraction()
        {
            Vector2 mouse = Input.MousePos;

            _closeColor = _closeButtonRect.Contains(mouse) ? _closeHover : _closeNormal;

            if (_closeButtonRect.Contains(mouse) && Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                OnClose?.Invoke(this);
                return;
            }

            // Begin interaction
            if (Input.GetMouseButtonDown(Input.MouseButton.Left) && !_dockButton.SourceRect.Contains(Input.MousePos))
            {
                Desktop.SetFocusedWindow(this);

                // Move (undock immediately)
                if (_headerRect.Contains(mouse))
                {
                    _dragMode = WindowDragMode.Move;
                    _dragOffset = mouse - new Vector2(X, Y);

                    if (Dock != DockStyle.None)
                        Dock = DockStyle.None;

                    return;
                }

                // Resize (DO NOT undock)
                foreach (var kv in _grabHandles)
                {
                    if (AllowedResizeDirections.Count > 0 &&
                        !AllowedResizeDirections.Contains(kv.Key))
                        continue;

                    if (kv.Value.Contains(mouse))
                    {
                        _dragMode = WindowDragMode.Resize;
                        _resizeDir = kv.Key;
                        _wasDockedOnResize = Dock != DockStyle.None;
                        return;
                    }
                }
            }

            // Active drag
            if (Input.GetMouseButton(Input.MouseButton.Left))
            {
                if (_dragMode == WindowDragMode.Move)
                {
                    X = (int)(mouse.X - _dragOffset.X);
                    Y = (int)(mouse.Y - _dragOffset.Y);
                }
                else if (_dragMode == WindowDragMode.Resize)
                {
                    ResizeWindow(mouse);
                }
            }

            // End interaction
            if (Input.GetMouseButtonUp(Input.MouseButton.Left))
            {
                if (_dragMode == WindowDragMode.Resize && _wasDockedOnResize)
                {
                    DockSize = Dock switch
                    {
                        DockStyle.Left or DockStyle.Right => Width,
                        DockStyle.Top or DockStyle.Bottom => Height,
                        _ => DockSize
                    };

                    LayoutInvalidated?.Invoke(this);
                }

                _dragMode = WindowDragMode.None;
                _resizeDir = ResizeDirection.None;
                _wasDockedOnResize = false;
            }
        }

        private void ResizeWindow(Vector2 mouse)
        {
            Vector2 pos = new(X, Y);
            Vector2 size = new(Width, Height);

            if (_resizeDir.HasFlag(ResizeDirection.Left))
            {
                float newX = mouse.X;
                size.X += pos.X - newX;
                pos.X = newX;
            }

            if (_resizeDir.HasFlag(ResizeDirection.Right))
                size.X = mouse.X - pos.X;

            if (_resizeDir.HasFlag(ResizeDirection.Top))
            {
                float newY = mouse.Y;
                size.Y += pos.Y - newY;
                pos.Y = newY;
            }

            if (_resizeDir.HasFlag(ResizeDirection.Bottom))
                size.Y = mouse.Y - pos.Y;

            size.X = Math.Max(MIN_WIDTH, size.X);
            size.Y = Math.Max(MIN_HEIGHT, size.Y);

            X = (int)pos.X;
            Y = (int)pos.Y;
            Width = (int)size.X;
            Height = (int)size.Y;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            sb.Draw(Texture, _headerRect, HeaderColor);
            sb.DrawString(Font, HeaderText, TextPosition, FontColor);

            sb.Draw(Texture, _closeButtonRect, _closeColor);
            sb.DrawString(Font, "X", new Vector2(_closeButtonRect.X + 14, _closeButtonRect.Y + 9),Color.White);

            for (int i = Children.Controls.Count - 1; i >= 0; i--)
                Children.Controls[i].Draw(sb);

            _dockButton.Draw(sb);

        }
    
    
    
    
    
        //---children---//
        public List<Control> ChildControls()
        {
            return Children.Controls;
        }
        public void Add(Control _control)
        {
            Children.Add(_control);
        }
        public void Remove(Control _control)
        {
            Children.Remove(_control);
        }
        public Control Find(string _name)
        {
            return Children.Find(_name);
        }
        public T Find<T>(string _name) where T : Control
        {
            return (T)Children.Find(_name);
        }
    
    }
}
