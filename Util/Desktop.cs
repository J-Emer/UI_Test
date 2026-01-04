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
        protected ChildCollection<Window> Windows{get; private set;} = new ChildCollection<Window>();
        public Game1 Game;
        public RasterizerState rasterizer;
        public Rectangle cacheRect;
        private SpriteBatch sb;


        public Desktop(Game1 game)
        {
            Game = game;
            sb = new SpriteBatch(game.GraphicsDevice);

            rasterizer = new RasterizerState(){ScissorTestEnable = true};
            cacheRect = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);

            Game.Window.ClientSizeChanged += ViewPortChanged;
        }

        private void ViewPortChanged(object sender, EventArgs e)
        {
            cacheRect = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
        }

        protected void Add(Window _window)
        {
            Windows.Add(_window);
            _window.OnClose += CloseWindow;
        }
        protected void Remove(Window _window)
        {
            Windows.Remove(_window);
            //todo: handle closing a window
        }
        private void CloseWindow(Window window)
        {
            Console.WriteLine("todo: handle closing a window in a safe way");
        }

        public abstract void Load();

        public virtual void UnLoad()
        {
            //todo: clear the windows in a safe way
            Windows.Controls.Clear();
        }

        public void Update()
        {
            foreach (var item in Windows.Controls)
            {
                item.Update();
            }
        }
        public void Draw()
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, rasterizer);

            foreach (var item in Windows.Controls)
            {
                sb.GraphicsDevice.ScissorRectangle = item.SourceRect;
                item.Draw(sb);
                sb.GraphicsDevice.ScissorRectangle = cacheRect;
            }

            sb.End();
        }

        public void Dispose()
        {
            //todo: close the windows in a safe way
            Windows.Controls.Clear();
        }
    }
}