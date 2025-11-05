using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SpiralographizeThatImage.Factories
{
    public class CalibrationSpiral : ISpiralGeometryFactory<LineSegment>
    {
        public static LineSegment[] GetGeometryElements(Bitmap image, int revolutionCount)
        {
            int width = image.Width;
            int height = image.Height;

            float thickness = revolutionCount;

            List<LineSegment> results = new List<LineSegment>();

            float offset = revolutionCount * 20 + revolutionCount;
            float length = 10;

            float baseX = (float)(width / 2) - offset;
            float baseY = (float)(height / 2) - offset;

            PointF verticalLineStart = new PointF();
            verticalLineStart.X = baseX - length;
            verticalLineStart.Y = baseY + length;

            PointF verticalLineEnd = new PointF();
            verticalLineEnd.X = baseX - length;
            verticalLineEnd.Y = baseY;

            PointF horizontalLineEnd = new PointF();
            horizontalLineEnd.X = baseX;
            horizontalLineEnd.Y = baseY;

            float hypot = (float)Math.Sqrt((length * length) + (length * length));

            PointF diagonalLineEnd = new PointF();
            diagonalLineEnd.X = baseX + hypot;
            diagonalLineEnd.Y = baseY + hypot;

            results.Add(new LineSegment(verticalLineStart, thickness));
            results.Add(new LineSegment(verticalLineEnd, thickness));
            results.Add(new LineSegment(horizontalLineEnd, thickness));
            results.Add(new LineSegment(diagonalLineEnd, thickness));

            return results.ToArray();
        }
    }
}
