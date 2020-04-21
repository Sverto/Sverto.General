using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Sverto.General.Drawing
{
    public static class DrawingHelper
    {

        public static PathGradientBrush CreateCircularGradientBrush(Color innerColor, Color outerColor, Rectangle rec, PointF? innerSize = null)
        {
            if (rec.Width <= 0)
                rec.Width = 1;
            if (rec.Height <= 0)
                rec.Height = 1;
            if (innerSize == null)
            {
                innerSize = new PointF(0.1f, 0.1f);
            }
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(rec);
            PathGradientBrush pgb = new PathGradientBrush(gp);
            pgb.CenterPoint = new PointF(Convert.ToSingle(rec.Width / 2), Convert.ToSingle(rec.Height / 2));
            pgb.SurroundColors = new Color[] { outerColor };
            pgb.CenterColor = innerColor;
            pgb.FocusScales = (PointF)innerSize;
            return pgb;
        }

    }
}
