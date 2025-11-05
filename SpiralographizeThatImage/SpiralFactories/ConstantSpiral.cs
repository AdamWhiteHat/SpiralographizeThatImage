using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SpiralographizeThatImage.Factories
{
    public class ConstantSpiral : ISpiralGeometryFactory<LineSegment>
    {
        public static LineSegment[] GetGeometryElements(Size size, int revolutionCount, float thickness)
        {
            int pointCount = (int)Math.Round((size.Width + size.Height) * (revolutionCount / 2d));

            LineSegment[] results = new LineSegment[pointCount];

            float angle, scale;
            for (int i = 0; i < pointCount; i++)
            {
                angle = ((float)i * 2f * (float)Math.PI / ((float)pointCount / (float)revolutionCount));
                scale = 1f - i / (float)pointCount;

                // 2 * Pi Radians = 360 degrees
                // Convert to X, Y coords
                PointF point = new PointF();
                point.X = (float)(size.Width / 2f * (1f + scale * Math.Cos(angle)));
                point.Y = (float)(size.Height / 2f * (1f + scale * Math.Sin(angle)));

                results[i] = new LineSegment(point, thickness);
            }

            return results;
        }
    }
}
