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
        protected Game1 Game;
        protected SpriteBatch spriteBatch;
        protected GraphicsDeviceManager Graphics;
        protected List<DesktopControl> Controls = new List<DesktopControl>();
        protected Rectangle _scissorsRect = new Rectangle();
        protected RasterizerState RasterizerState = new RasterizerState
                                                                        {
                                                                            ScissorTestEnable = true,
                                                                        };
        private DockManager _dockManager;
        public bool IsDragging{get;set;} = false;


        public Desktop(Game1 game, GraphicsDeviceManager graphics, string fontName)
        {
            Game = game;
            Graphics = graphics;
            Game.Window.ClientSizeChanged += ClientSizedChanged;
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            AssetLoader.Init(Game.Content, Game.GraphicsDevice, fontName);
            _scissorsRect = new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            Graphics.GraphicsDevice.ScissorRectangle = _scissorsRect;
            _dockManager = new DockManager();
        }
        private void ClientSizedChanged(object sender, EventArgs e)
        {
            _scissorsRect = new Rectangle(0, 0, Graphics.GraphicsDevice.Viewport.Width, Graphics.GraphicsDevice.Viewport.Height);
            Graphics.GraphicsDevice.ScissorRectangle = _scissorsRect;
            HandleDock();
        }
        public void Add(DesktopControl _control)
        {
            Controls.Add(_control);
            _control.ParentDesktop = this;
            HandleDock();
        }
        public void Remove(DesktopControl _control)
        {
            Controls.Remove(_control);
            HandleDock();
        }
        private void HandleDock()
        {
            _dockManager.ApplyDocking(Graphics.GraphicsDevice.Viewport.Bounds, Controls);
        }
        public void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            Input.Update();        

            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Update();
            }    
        }
        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState);

            Graphics.GraphicsDevice.ScissorRectangle = _scissorsRect;
        
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                Controls[i].Draw(spriteBatch, Graphics);
            }
           
            if(IsDragging)
            {
                _dockManager.DrawDropZones(spriteBatch, Graphics);               
            }

            spriteBatch.End();

        }
        public void CheckDropZone(DesktopControl _control)
        {
            _dockManager.CheckDropZone(Graphics.GraphicsDevice.Viewport.Bounds, _control);
            HandleDock();
        }
        
        public virtual void Load(){}
        public virtual void Unload(){}
        public void Dispose()
        {

        }
    }
}