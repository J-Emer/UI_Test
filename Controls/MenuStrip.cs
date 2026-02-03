using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Util;

namespace UI.Controls
{
    public class MenuStrip : DesktopControl
    {
        public ChildCollection<MenuItem> Items{get; private set;} = new ChildCollection<MenuItem>();
        private Layout layout = new VerticalLayout();


        public MenuStrip() : base()
        {
            Height = 30;
            Width = 200;
            Dock = DockStyle.Top;
            Items.OnChildrenChanged += AfterInvalidation;
            BackgroundColor = new Color(32, 35, 38);
            ZOrder = 0;
        }
        protected override void AfterInvalidation()
        {
            base.AfterInvalidation();
            layout.DoLayout(SourceRect, Items.Controls.ToList<Control>(), 0);
        }

        public override void Update()
        {
            base.Update();

            for (int i = 0; i < Items.Controls.Count; i++)
            {
                Items.Controls[i].Update();
            }
        }
        public override void Draw(SpriteBatch _spritebatch, GraphicsDeviceManager graphics)
        {
            base.Draw(_spritebatch, graphics);

            // ScissorStack.Push(graphics.GraphicsDevice, SourceRect);

            for (int i = 0; i < Items.Controls.Count; i++)
            {
                Items.Controls[i].Draw(_spritebatch, graphics);
            }            
            
            // ScissorStack.Pop(graphics.GraphicsDevice);
        }
    }
}