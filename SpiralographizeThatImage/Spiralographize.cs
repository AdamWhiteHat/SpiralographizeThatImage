using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualBasic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Security.Policy;
using System.IO;
using System.Xml;
using System.Diagnostics.CodeAnalysis;
using System.Buffers.Text;

namespace SpiralographizeThatImage
{
    public interface ISpiralGeometryFactory<T>
    {
        public static T[] GetGeometrySegments<T>(Size size, int revolutionCount) => throw new NotImplementedException();
    }

    public interface ISpiralRenderer<T>
    {
        public static void DrawSpiral<T>(Graphics graphics, T[] geometrySegments, Color color) => throw new NotImplementedException();
    }

    public static class Spiralographize
    {
        public enum ImageSpirals
        {
            ByRadius,
            ByThickness
        }

        public static class Debug
        {
            public class Calibration : ISpiralGeometryFactory<LineSegment>
            {
                public static LineSegment[] GetGeometrySegments(Bitmap image, int revolutionCount)
                {
                    int width = image.Width;
                    int height = image.Height;

                    float thickness = revolutionCount;

                    List<LineSegment> results = new List<LineSegment>();

                    float offset = revolutionCount*20 + revolutionCount;
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

        public class ModulatingThickness : ISpiralGeometryFactory<LineSegment>
        {
            public static LineSegment[] GetGeometrySegments(Bitmap image, int revolutionCount)
            {
                int width = image.Width;
                int height = image.Height;

                float minThickness = 1.0f;
                float maxThickness = (((width + height) / 2.0f) / (revolutionCount * 1.5f));

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


        public class ModulatingRadius : ISpiralGeometryFactory<LineSegment>
        {
            public static LineSegment[] GetGeometrySegments(Bitmap image, int revolutionCount)
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

        public class ConstantSpiral : ISpiralGeometryFactory<LineSegment>
        {
            public static LineSegment[] GetGeometrySegments(Size size, int revolutionCount)
            {
                float thickness = 1.0f;

                int pointCount = (size.Width + size.Height) * (revolutionCount / 2);

                LineSegment[] results = new LineSegment[pointCount];

                float angle, scale;
                for (int i = 0; i < pointCount; i++)
                {
                    angle = (float)(i * 2 * Math.PI / (pointCount / revolutionCount));
                    scale = 1 - (float)i / pointCount;

                    // 2 * Pi Radians = 360 degrees
                    // Convert to X, Y coords
                    PointF point = new PointF();
                    point.X = (float)(size.Width / 2 * (1 + scale * Math.Cos(angle)));
                    point.Y = (float)(size.Height / 2 * (1 + scale * Math.Sin(angle)));

                    results[i] = new LineSegment(point, thickness);
                }

                return results;
            }
        }

        public static class GoldenRatioSpiral// : ISpiralGeometryFactory<PointF>
        {
            public enum DrawOrder
            {
                BySpiralArm,
                FromTopToBottom
            }

            private static float Thickness = 2.5f;
            private static float Phi = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;
            private static RenderingTimer Timer = new RenderingTimer();
            private static DrawOrder drawingOrder = DrawOrder.BySpiralArm;

            private static float DegreesToRadians(float rad)
            {
                return rad * ((float)Math.PI / 180.0f);
            }

            private static float RadiansToDegrees(float degrees)
            {
                return degrees * (180.0f / (float)Math.PI);
            }

            public static void Render(PictureBox pictureBox, DrawOrder drawOrder)
            {
                drawingOrder = drawOrder;
                if (!Timer.IsDisposed)
                {
                    MessageBox.Show(" This control is still busy rendering the last request! Please wait until the control has finished drawing, then try again.", "Too soon!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Timer = new RenderingTimer(pictureBox, 33);
                    Timer.Start();
                }
            }

            public class RenderingTimer : IDisposable
            {
                public bool IsDisposed
                {
                    get { return Interlocked.Read(ref _isDisposed) == 1; }
                    private set
                    {
                        long newValue = value ? 1 : 0;
                        Interlocked.Or(ref _isDisposed, newValue);
                    }
                }
                private long _isDisposed = 0;

                private int i;
                private Pen _pen;
                private SizeF _size;
                private int totalFrames;
                private PictureBox pictureBox;
                private Graphics _graphics;

                public List<Point> PointLocations;

                private System.Windows.Forms.Timer timer;

                public RenderingTimer()
                {
                    IsDisposed = true;
                    i = 0;
                    totalFrames = 0;
                    pictureBox = null;
                }

                public RenderingTimer(PictureBox control, float frameRateFPS = 520f)
                {
                    IsDisposed = false;

                    i = 0;
                    pictureBox = control;

                    int revolutionCount = ((pictureBox.Width + pictureBox.Height) / 2) / 10;
                    totalFrames = 10 * revolutionCount;

                    double timerInterval = Math.Max(1f, Math.Round(1000f / frameRateFPS));
                    timer = new System.Windows.Forms.Timer()
                    {
                        Interval = (int)timerInterval,
                        Enabled = false
                    };
                    timer.Tick += Tick;

                    _pen = PenCache.GetPen(Color.Black, Thickness + 1);
                    _size = new SizeF(Thickness, Thickness);

                    PointLocations = new List<Point>();

                    _graphics = pictureBox.CreateGraphics();
                    //Graphics graphics = Graphics.FromImage(pictureBox.Image))         
                }

                public void Dispose()
                {
                    if (!IsDisposed)
                    {
                        IsDisposed = true;

                        if (timer != null)
                        {
                            if (timer.Enabled)
                            {
                                timer.Stop();
                            }
                            timer.Dispose();
                        }

                        _graphics.Dispose();
                        _pen.Dispose();
                        pictureBox = null;
                    }
                }

                public void Start()
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    int n = totalFrames - 1;
                    PointLocations = new List<Point>();
                    do
                    {
                        PointLocations.Add(CalculatePointLocation(n));
                        n--;
                    }
                    while (n >= 0);

                    if (drawingOrder == DrawOrder.BySpiralArm)
                    {
                        PointLocations = ReorderGoldenRatioPoints_BySpiralArms(PointLocations);
                    }
                    else if (drawingOrder == DrawOrder.FromTopToBottom)
                    {
                        PointLocations = ReorderGoldenRatioPoints_ByTopToBottom(PointLocations);
                    }

                    i = 0;
                    timer.Start();
                }

                private static string GoldenRatioPointFile_TopToBottom = "GoldenRatio_PointLocations_OrderBy-TopToBottom.txt";
                private static string GoldenRatioPointFile_SpiralArms = "GoldenRatio_PointLocations_OrderBy-SpiralArms.txt";
                private List<Point> ReorderGoldenRatioPoints_BySpiralArms(List<Point> points)
                {
                    int n = 0;
                    Dictionary<int, List<Point>> bucketsDictionary = new Dictionary<int, List<Point>>();
                    foreach (Point location in points)
                    {
                        int key = n % 34;
                        if (!bucketsDictionary.ContainsKey(key))
                        {
                            bucketsDictionary.Add(key, new List<Point>());
                        }

                        List<Point> bucket = bucketsDictionary[key];
                        bucket.Add(location);
                        //bucketsDictionary[key] = bucket;

                        n++;
                    }

                    File.WriteAllText(GoldenRatioPointFile_SpiralArms, "");
                    List<Point> results = new List<Point>();
                    foreach (var kvp in bucketsDictionary)
                    {
                        results.AddRange(kvp.Value);
                        File.AppendAllText(GoldenRatioPointFile_SpiralArms, string.Join(Environment.NewLine, kvp.Value.Select(p => $"{p.X},{p.Y}")));
                        File.AppendAllText(GoldenRatioPointFile_SpiralArms, Environment.NewLine + Environment.NewLine);
                    }

                    return results;
                }

                private List<Point> ReorderGoldenRatioPoints_ByTopToBottom(List<Point> points)
                {
                    List<Point> results = points.OrderBy(pt => pt.Y).ThenBy(pt => pt.X).ToList();

                    File.WriteAllLines(GoldenRatioPointFile_TopToBottom, results.Select(p => $"{p.X},{p.Y}"));
                    return results;
                }

                public void Tick(object source, EventArgs e)
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    if (i >= totalFrames)
                    {
                        timer.Stop();
                        Dispose();
                    }
                    else
                    {
                        RenderFrame(PointLocations[i]);
                        i++;
                    }
                }

                private Point CalculatePointLocation(float currentFrame)
                {
                    float angle = currentFrame * Phi * 2.0f * (float)Math.PI;
                    float scale = currentFrame / totalFrames;

                    // 2 * Pi Radians = 360 degrees
                    // Convert to X, Y coords
                    Point result = new Point();
                    result.X = (int)Math.Round((float)pictureBox.Width / 2f * (1f + scale * (float)Math.Cos(angle)));
                    result.Y = (int)Math.Round((float)pictureBox.Height / 2f * (1f + scale * (float)Math.Sin(angle)));

                    result.X = pictureBox.Width - result.X;

                    return result;
                }

                private void RenderFrame(Point location)
                {
                    RectangleF rect = new RectangleF(location, _size);
                    _graphics.DrawEllipse(_pen, rect);
                    _graphics.Flush();
                }
            }
        }


        public static class PenCache
        {
            private static Dictionary<Tuple<Color, float>, Pen> _penDictionary = null;

            static PenCache()
            {
                _penDictionary = new Dictionary<Tuple<Color, float>, Pen>(new ColorFloatTupleEqualityComparer());
            }

            public static Pen GetPen(Color color, float thickness)
            {
                Tuple<Color, float> key = new Tuple<Color, float>(color, thickness);

                if (_penDictionary.ContainsKey(key))
                {
                    return _penDictionary[key];
                }
                else
                {
                    Pen result = new Pen(color, thickness);
                    result.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    result.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    result.MiterLimit = thickness;
                    result.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

                    _penDictionary.Add(key, result);
                    return result;
                }

            }

            public class ColorFloatTupleEqualityComparer : IEqualityComparer<Tuple<Color, float>>
            {
                public bool Equals(Tuple<Color, float>? x, Tuple<Color, float>? y)
                {
                    if (x == null)
                    {
                        if (y == null) { return true; }
                        return false;
                    }
                    else if (y == null) { return false; }
                    if (x.Item1 != y.Item1) { return false; }
                    if (x.Item2 != y.Item2) { return false; }
                    return true;
                }

                public int GetHashCode([DisallowNull] Tuple<Color, float> obj)
                {
                    return obj.GetHashCode();
                }
            }
        }


        public class Renderer
        {
            public static Bitmap ABlankImage(int width, int height)
            {
                Bitmap result = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(result))
                {
                    g.Clear(Color.Transparent);
                }
                return result;
            }

            public class ALine : ISpiralRenderer<LineSegment>
            {
                public static void DrawPoint(Graphics graphics, PointF location, int thickness)
                {
                    graphics.DrawEllipse(PenCache.GetPen(Color.Black, thickness), new System.Drawing.Rectangle(new Point((int)Math.Round(location.X), (int)Math.Round(location.Y)), new Size(thickness, thickness)));
                }

                public static void DrawSpiral(Graphics graphics, LineSegment[] geometrySegments, Color color)
                {
                    Pen pen = null;
                    PointF previousPoint = PointF.Empty;

                    float currentThickness = -1f;

                    foreach (LineSegment segment in geometrySegments)
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

                public static void DrawSpiral(Graphics graphics, PointF[] geometrySegments, Color color, float thickness = 1)
                {
                    if (_pen == null)
                    {
                        _pen = new Pen(color, thickness);
                    }

                    int thicc = (int)Math.Round(thickness);
                    Size pointSize = new Size(thicc, thicc);
                    foreach (PointF currentPoint in geometrySegments)
                    {
                        graphics.DrawEllipse(_pen, new System.Drawing.Rectangle(new Point((int)Math.Round(currentPoint.X), (int)Math.Round(currentPoint.Y)), pointSize));
                    }
                }
            }
        }
    }
}
