using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Controls;

namespace UI.Util
{
    public class DockManager
    {
        public void ApplyDocking(Rectangle bounds, List<DesktopControl> Windows)
        {
            Rectangle remaining = bounds;

            foreach (var ctrl in Windows)
            {
                if (ctrl is not DesktopControl dc || dc.Dock == DockStyle.None)
                    continue;

                switch (dc.Dock)
                {
                    case DockStyle.Left:
                        dc.X = remaining.Left;
                        dc.Y = remaining.Top;
                        dc.Width = dc.Width;
                        dc.Height = remaining.Height;
                        remaining.X += dc.Width;
                        remaining.Width -= dc.Width;
                        break;

                    case DockStyle.Right:
                        dc.Width = dc.Width;
                        dc.Height = remaining.Height;
                        dc.X = remaining.Right - dc.Width;
                        dc.Y = remaining.Top;
                        remaining.Width -= dc.Width;
                        break;

                    case DockStyle.Top:
                        dc.X = remaining.Left;
                        dc.Y = remaining.Top;
                        dc.Width = remaining.Width;
                        dc.Height = dc.Height;
                        remaining.Y += dc.Height;
                        remaining.Height -= dc.Height;
                        break;

                    case DockStyle.Bottom:
                        dc.Width = remaining.Width;
                        dc.Height = dc.Height;
                        dc.X = remaining.Left;
                        dc.Y = remaining.Bottom - dc.Height;
                        remaining.Height -= dc.Height;
                        break;

                    case DockStyle.Fill:
                        dc.X = remaining.Left;
                        dc.Y = remaining.Top;
                        dc.Width = remaining.Width;
                        dc.Height = remaining.Height;
                        remaining = Rectangle.Empty;
                        break;
                }
            }
        }        
    
        public void CheckDropZone(Rectangle bounds, DesktopControl control)
        {
            var zones = BuildDropZones(bounds);
            Point mouse = Input.MousePos.ToPoint();

            if (zones.Left.Contains(mouse))
                control.Dock = DockStyle.Left;
            else if (zones.Right.Contains(mouse))
                control.Dock = DockStyle.Right;
            else if (zones.Top.Contains(mouse))
                control.Dock = DockStyle.Top;
            else if (zones.Bottom.Contains(mouse))
                control.Dock = DockStyle.Bottom;
            else if (zones.Center.Contains(mouse))
                control.Dock = DockStyle.Fill;

        }

        public DockDropZones BuildDropZones(Rectangle bounds)
        {
            int w = bounds.Width;
            int h = bounds.Height;

            int leftW   = (int)(w * 0.15f);
            int rightW  = (int)(w * 0.15f);
            int topH    = (int)(h * 0.10f);
            int bottomH = (int)(h * 0.10f);

            DockDropZones zones;

            // LEFT
            zones.Left = new Rectangle(
                bounds.Left,
                bounds.Top,
                leftW,
                h
            );

            // RIGHT
            zones.Right = new Rectangle(
                bounds.Right - rightW,
                bounds.Top,
                rightW,
                h
            );

            // TOP
            zones.Top = new Rectangle(
                bounds.Left + leftW,
                bounds.Top,
                w - leftW - rightW,
                topH
            );

            // BOTTOM
            zones.Bottom = new Rectangle(
                bounds.Left + leftW,
                bounds.Bottom - bottomH,
                w - leftW - rightW,
                bottomH
            );

            // CENTER (remaining space)
            zones.Center = new Rectangle(
                bounds.Left + leftW,
                bounds.Top + topH,
                w - leftW - rightW,
                h - topH - bottomH
            );

            return zones;
        }

        public void DrawDropZones(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            DockDropZones zones = BuildDropZones(graphics.GraphicsDevice.Viewport.Bounds);

            spriteBatch.Draw(AssetLoader.GetPixel(), zones.Left,   Color.Yellow  * 0.5f);
            spriteBatch.Draw(AssetLoader.GetPixel(), zones.Right,  Color.Yellow  * 0.5f);
            spriteBatch.Draw(AssetLoader.GetPixel(), zones.Top,    Color.Yellow  * 0.5f);
            spriteBatch.Draw(AssetLoader.GetPixel(), zones.Bottom, Color.Yellow  * 0.5f);
            spriteBatch.Draw(AssetLoader.GetPixel(), zones.Center, Color.Yellow  * 0.5f);

        }
    }

    public struct DockDropZones
    {
        public Rectangle Left;
        public Rectangle Right;
        public Rectangle Top;
        public Rectangle Bottom;
        public Rectangle Center;
    }    
}