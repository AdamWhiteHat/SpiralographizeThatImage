using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SpiralographizeThatImage.Factories
{
    public class ModulateThicknessSpiral : ISpiralGeometryFactory<LineSegment>
    {
        public static LineSegment[] GetGeometryElements(Bitmap image, int revolutionCount, float maxThickness)
        {
            int width = image.Width;
            int height = image.Height;
            //float maxThickness = 2.0f;//(((width + height) / 2.0f) / (revolutionCount * 2.0f));

            int pointCount = (width + height) * (revolutionCount / 2);

            LineSegment[] results = new LineSegment[pointCount];

            float angle, scale, thickness;
            for (int i = 0; i < pointCount; i++)
            {
                angle = (float)(i * 2 * Math.PI / (pointCount / revolutionCount));
                scale = 1 - (float)i / pointCount;

                // 2 * Pi Radians = 360 degrees
                // Convert to X, Y coords
                PointF point = new PointF();
                point.X = (float)(width / 2 * (1 + scale * Math.Cos(angle)));
                point.Y = (float)(height / 2 * (1 + scale * Math.Sin(angle)));

                Color pixel = image.GetPixel((int)Math.Min(width - 1, Math.Round(point.X)), (int)Math.Min(height - 1, Math.Round(point.Y)));
                thickness = (float)Math.Round((1.0f - pixel.GetBrightness()) * maxThickness, 3);

                results[i] = new LineSegment(point, thickness);
            }

            return results;
        }
    }
}
