using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UI.Controls;

namespace UI.Util
{
    public class DockManager
    {
        private readonly Rectangle _desktopBounds;

        public DockManager(Rectangle desktopBounds)
        {
            _desktopBounds = desktopBounds;
        }

        public void Layout(IReadOnlyList<Window> windows)
        {
            Rectangle remaining = _desktopBounds;

            foreach (var win in windows)
            {
                if (win.Dock == DockStyle.None)
                    continue;

                switch (win.Dock)
                {
                    case DockStyle.Left:
                        ApplyLeft(win, ref remaining);
                        break;

                    case DockStyle.Right:
                        ApplyRight(win, ref remaining);
                        break;

                    case DockStyle.Top:
                        ApplyTop(win, ref remaining);
                        break;

                    case DockStyle.Bottom:
                        ApplyBottom(win, ref remaining);
                        break;

                    case DockStyle.Fill:
                        ApplyFill(win, remaining);
                        break;
                }
            }
        }

        private void ApplyLeft(Window win, ref Rectangle remaining)
        {
            win.X = remaining.Left;
            win.Y = remaining.Top;
            win.Width = win.DockSize;
            win.Height = remaining.Height;

            remaining.X += win.DockSize;
            remaining.Width -= win.DockSize;
        }

        private void ApplyRight(Window win, ref Rectangle remaining)
        {
            win.X = remaining.Right - win.DockSize;
            win.Y = remaining.Top;
            win.Width = win.DockSize;
            win.Height = remaining.Height;

            remaining.Width -= win.DockSize;
        }

        private void ApplyTop(Window win, ref Rectangle remaining)
        {
            win.X = remaining.Left;
            win.Y = remaining.Top;
            win.Width = remaining.Width;
            win.Height = win.DockSize;

            remaining.Y += win.DockSize;
            remaining.Height -= win.DockSize;
        }

        private void ApplyBottom(Window win, ref Rectangle remaining)
        {
            win.X = remaining.Left;
            win.Y = remaining.Bottom - win.DockSize;
            win.Width = remaining.Width;
            win.Height = win.DockSize;

            remaining.Height -= win.DockSize;
        }

        private void ApplyFill(Window win, Rectangle remaining)
        {
            win.X = remaining.X;
            win.Y = remaining.Y;
            win.Width = remaining.Width;
            win.Height = remaining.Height;
        }
    }
}
