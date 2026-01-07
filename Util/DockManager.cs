using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UI.Controls;

namespace UI.Util
{
    public class DockManager
    {
        private Rectangle _desktopBounds;

        public DockManager(Rectangle desktopBounds)
        {
            _desktopBounds = desktopBounds;
        }

        public void SetDesktopBounds(Rectangle bounds)
        {
            _desktopBounds = bounds;
        }

        public void Layout(IReadOnlyList<Window> windows)
        {
            Rectangle remaining = _desktopBounds;

            // Order matters: edges first, center last
            foreach (var win in windows)
            {
                if (win.Dock == DockStyle.None)
                {
                    win.EnableAllResizeHandles();
                    continue;
                }

                switch (win.Dock)
                {
                    case DockStyle.Left:
                        ApplyDockLeft(win, ref remaining);
                        break;

                    case DockStyle.Right:
                        ApplyDockRight(win, ref remaining);
                        break;

                    case DockStyle.Top:
                        ApplyDockTop(win, ref remaining);
                        break;

                    case DockStyle.Bottom:
                        ApplyDockBottom(win, ref remaining);
                        break;
                }
            }

            // Center windows get whatever space is left
            foreach (var win in windows)
            {
                if (win.Dock == DockStyle.Fill)
                {
                    win.SetBounds(remaining);
                    win.EnableResizeHandles(ResizeDirection.None);
                }
            }
        }

        private void ApplyDockLeft(Window w, ref Rectangle remaining)
        {
            w.SetBounds(new Rectangle(
                remaining.X,
                remaining.Y,
                w.DockSize,
                remaining.Height
            ));

            w.EnableResizeHandles(ResizeDirection.Right);

            remaining.X += w.DockSize;
            remaining.Width -= w.DockSize;
        }

        private void ApplyDockRight(Window w, ref Rectangle remaining)
        {
            w.SetBounds(new Rectangle(
                remaining.Right - w.DockSize,
                remaining.Y,
                w.DockSize,
                remaining.Height
            ));

            w.EnableResizeHandles(ResizeDirection.Left);

            remaining.Width -= w.DockSize;
        }

        private void ApplyDockTop(Window w, ref Rectangle remaining)
        {
            w.SetBounds(new Rectangle(
                remaining.X,
                remaining.Y,
                remaining.Width,
                w.DockSize
            ));

            w.EnableResizeHandles(ResizeDirection.Bottom);

            remaining.Y += w.DockSize;
            remaining.Height -= w.DockSize;
        }

        private void ApplyDockBottom(Window w, ref Rectangle remaining)
        {
            w.SetBounds(new Rectangle(
                remaining.X,
                remaining.Bottom - w.DockSize,
                remaining.Width,
                w.DockSize
            ));

            w.EnableResizeHandles(ResizeDirection.Top);

            remaining.Height -= w.DockSize;
        }
    }
}
