using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Util
{
    public static class ScissorStack
    {
        private static readonly Stack<Rectangle> _stack = new();
    
        public static void Push(GraphicsDevice device, Rectangle rect)
        {
            Rectangle current = device.ScissorRectangle;
            Rectangle next = Rectangle.Intersect(current, rect);
    
            _stack.Push(current);
            device.ScissorRectangle = next;
        }
    
        public static void Pop(GraphicsDevice device)
        {
            if (_stack.Count > 0)
                device.ScissorRectangle = _stack.Pop();
        }
    }

}