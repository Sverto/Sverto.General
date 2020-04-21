using System;
using System.Drawing;

namespace Sverto.General.Extensions
{
    public static class DrawingExtensions
    {

        // Point
        public static bool IsInRectangle(this Point p, Rectangle r)
        {
            return (p.X > r.X && p.Y > r.Y && p.X < r.X + r.Width && p.Y < r.Y + r.Height);
        }

        // Rectangle
        public static bool HasInside(this Rectangle r, Point p)
        {
            return IsInRectangle(p, r);
        }

        public static bool CollidesWith(this Rectangle r, Rectangle r2)
        {
            return r.IntersectsWith(r2);
        }

        // ContentAlignment
        public static StringAlignment ToStringAlignment(this ContentAlignment ca, HVAlignment returnAlignment = HVAlignment.Horizontal)
        {
            int lNum = Convert.ToInt32(Math.Log(Convert.ToDouble(ca), 2));
            if (returnAlignment == HVAlignment.Vertical)
            {
                return (StringAlignment)(lNum % 4);
            }
            else
            {
                return (StringAlignment)(lNum / 4);
            }
        }
        
        public static ContentAlignment ToContentAlignment(this StringAlignment hAlign, StringAlignment vAlign)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            return (ContentAlignment)Convert.ToInt32(Math.Pow(2, (int)hAlign) + (Math.Pow(16, (int)vAlign) - 1));
        }
    }

    public enum HVAlignment
    {
        Horizontal,
        Vertical
    }
}
