using System;
using System.Drawing;

namespace ABCsystem.Window
{
    public static class WindowClampHelper
    {
        public static Rectangle Clamp(Rectangle window, Rectangle bounds)
        {
            int w = Math.Min(window.Width, bounds.Width);
            int h = Math.Min(window.Height, bounds.Height);

            int x = window.X;
            int y = window.Y;

            if (x < bounds.Left) x = bounds.Left;
            if (y < bounds.Top) y = bounds.Top;
            if (x + w > bounds.Right) x = bounds.Right - w;
            if (y + h > bounds.Bottom) y = bounds.Bottom - h;

            return new Rectangle(x, y, w, h);
        }
    }
}
