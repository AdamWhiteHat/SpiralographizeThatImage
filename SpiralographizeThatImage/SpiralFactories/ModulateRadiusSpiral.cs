using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SpiralographizeThatImage.Factories
{
    public class ModulateRadiusSpiral : ISpiralGeometryFactory<LineSegment>
    {
        public static LineSegment[] GetGeometryElements(Bitmap image, int revolutionCount)
        {
            int width = image.Width;
            int height = image.Height;

            float thickness = 1.0f;
            float intensityMultiplier = (((width + height) / 2.0f) / (revolutionCount * 2.0f)) * 33.33f;

            int pointCount = (width + height) * (revolutionCount / 2);

            LineSegment[] results = new LineSegment[pointCount];
            float angle, scale;
            for (int i = 0; i < pointCount; i++)
            {
                angle = (float)(i * 2 * Math.PI / (pointCount / revolutionCount));
                scale = 1 - (float)i / pointCount;

                PointF point = new PointF();
                point.X = (float)(width / 2 * (1 + scale * Math.Cos(angle)));
                point.Y = (float)(height / 2 * (1 + scale * Math.Sin(angle)));

                Color pixel = image.GetPixel((int)Math.Min(width - 1, Math.Round(point.X)), (int)Math.Min(height - 1, Math.Round(point.Y)));
                float intensity = (float)Math.Round((1.0f - pixel.GetBrightness()) * intensityMultiplier, 3);

                // Modulate the scale and angle and recalculate 
                //angle = (float)(((i * 2 * Math.PI) + intensity) / (pointCount / revolutionCount));
                scale = 1 - (float)(i + intensity) / pointCount;

                // 2 * Pi Radians = 360 degrees
                // Convert to X, Y coords
                point = new PointF();
                point.X = (float)(width / 2 * (1 + scale * Math.Cos(angle)));
                point.Y = (float)(height / 2 * (1 + scale * Math.Sin(angle)));

                results[i] = new LineSegment(point, thickness);
            }

            return results;
        }
    }
}
