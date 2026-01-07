using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Controls;

namespace UI.Util
{
    public abstract class Desktop : IDisposable
    {
        private ChildCollection<Window> Windows = new ChildCollection<Window>();
        public Game1 Game;
        public RasterizerState rasterizer;
        public Rectangle cacheRect;
        private SpriteBatch sb;

        private DockManager _dockManager;


        public Desktop(Game1 game)
        {
            Game = game;
            sb = new SpriteBatch(game.GraphicsDevice);

            rasterizer = new RasterizerState(){ScissorTestEnable = true};
            cacheRect = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);

            _dockManager = new DockManager(cacheRect);

            Game.Window.ClientSizeChanged += ViewPortChanged;
        }

        private void ViewPortChanged(object sender, EventArgs e)
        {
            cacheRect = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            _dockManager.SetDesktopBounds(cacheRect);
            HandleLayout();
        }

        public void HandleLayout()
        {
            _dockManager.Layout(Windows.Controls);
        }

        protected void Add(Window _window)
        {
            Windows.Add(_window);
            _window.OnClose += CloseWindow;
            _window.LayoutInvalidated += WindowLayoutInvalidated;
            _dockManager.Layout(Windows.Controls);
        }

        private void WindowLayoutInvalidated(Window window)
        {
            HandleLayout();
        }

        protected void Remove(Window _window)
        {
            Windows.Remove(_window);
            _window.LayoutInvalidated -= WindowLayoutInvalidated;
        }
        public Window GetWindow(string name)
        {
            return Windows.Find(name);
        }
        private void CloseWindow(Window window)
        {
            Windows.Remove(window);
        }

        public abstract void Load();

        public virtual void UnLoad()
        {
            Windows.Controls.Clear();
        }

        public void Update()
        {
            for (int i = 0; i < Windows.Controls.Count; i++)
            {
                Windows.Controls[i].Update();
            }
        }
        public void Draw()
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, rasterizer);
            
            for (int i = 0; i < Windows.Controls.Count; i++)
            {
                sb.GraphicsDevice.ScissorRectangle = Windows.Controls[i].SourceRect;
                Windows.Controls[i].Draw(sb);
                sb.GraphicsDevice.ScissorRectangle = cacheRect;
            }

            sb.End();
        }

        public void Dispose()
        {
            Windows.Controls.Clear();
        }
    }
}