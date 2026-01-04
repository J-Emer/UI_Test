using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI;
using UI.Controls;
using UI.Util;

public abstract class ContainerControl : Control
{
    protected void DrawChildrenClipped(SpriteBatch spriteBatch, IEnumerable<Control> children)
    {
        foreach (var child in children)
        {
            child.Draw(spriteBatch); 

            if(SourceRect.Intersects(child.SourceRect))
            {
                child.IsActive = true;
            }
            else
            {
                child.IsActive = false;   
            }

            if(SourceRect.Intersects(child.SourceRect))
            {
                child.IsVisible = true;
            }
            else
            {
                child.IsVisible = false;   
            }            
        }
    }
}
