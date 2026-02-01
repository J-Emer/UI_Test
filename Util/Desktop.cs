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
        protected List<Control> Controls = new List<Control>();

        public Desktop(Game1 game, string fontName)
        {
            Game = game;
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            AssetLoader.Init(Game.Content, Game.GraphicsDevice, fontName);
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
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);

            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Draw(spriteBatch);
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