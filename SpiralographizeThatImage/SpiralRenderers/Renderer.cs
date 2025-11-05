using System;
using System.Drawing;

namespace SpiralographizeThatImage.Renderers
{
    public class Renderer
    {
        public static Bitmap ABlankImage(Size size)
        {
            Bitmap result = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.Transparent);
            }
            return result;
        }

        public class ALine : ISpiralRenderer<LineSegment>
        {
            public static void DrawPoint(Graphics graphics, PointF location, float thickness)
            {
                graphics.DrawEllipse(PenCache.GetPen(Color.Black, thickness), new RectangleF(location.X, location.Y, thickness, thickness));
            }

            public static void DrawSpiral(Graphics graphics, LineSegment[] geometryElements, Color color)
            {
                Pen pen = null;
                PointF previousPoint = PointF.Empty;

                float currentThickness = -1f;

                foreach (LineSegment segment in geometryElements)
                {
                    PointF currentPoint = segment.Point;

                    if (currentThickness != segment.Thickness)
                    {
                        pen = PenCache.GetPen(color, segment.Thickness);
                        currentThickness = segment.Thickness;
                    }

                    if (previousPoint != PointF.Empty)
                    {
                        graphics.DrawLine(pen, previousPoint, currentPoint);
                    }

                    previousPoint = currentPoint;
                }
            }
        }

        public class APoint : ISpiralRenderer<PointF>
        {
            private static Pen _pen = null;

            public static void DrawSpiral(Graphics graphics, PointF[] geometryElements, Color color, float thickness = 1f)
            {
                if (_pen == null)
                {
                    _pen = new Pen(color, thickness);
                }

                SizeF size = new SizeF((float)Math.Ceiling(thickness), (float)Math.Ceiling(thickness));

                foreach (PointF currentPoint in geometryElements)
                {
                    graphics.DrawEllipse(_pen, currentPoint.X, currentPoint.Y, size.Width, size.Height);
                }
            }
        }
    }
}
