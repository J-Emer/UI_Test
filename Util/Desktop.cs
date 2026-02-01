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
        protected List<Control> Controls = new List<Control>();
        protected Rectangle _scissorsRect = new Rectangle();
        protected RasterizerState RasterizerState = new RasterizerState
                                                                        {
                                                                            ScissorTestEnable = true,
                                                                        };
        
        public Desktop(Game1 game, GraphicsDeviceManager graphics, string fontName)
        {
            Game = game;
            Graphics = graphics;
            game.Window.ClientSizeChanged += ClientSizedChanged;
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            AssetLoader.Init(Game.Content, Game.GraphicsDevice, fontName);
            _scissorsRect = new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            Graphics.GraphicsDevice.ScissorRectangle = _scissorsRect;
        }

        private void ClientSizedChanged(object sender, EventArgs e)
        {
            _scissorsRect = new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            Graphics.GraphicsDevice.ScissorRectangle = _scissorsRect;
        }

        public void Add(Control _control) => Controls.Add(_control);
        public void Remove(Control _control) => Controls.Remove(_control);
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

            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Draw(spriteBatch, Graphics);
            }              

            spriteBatch.End();

        }
        public virtual void Load(){}
        public virtual void Unload(){}
        public void Dispose()
        {

        }
    }
}