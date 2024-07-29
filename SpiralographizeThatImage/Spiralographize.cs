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
        public class ModulatingThickness : ISpiralGeometryFactory<LineSegment>
        {
            public static LineSegment[] GetGeometrySegments(Bitmap image, int revolutionCount)
            {
                int width = image.Width;
                int height = image.Height;

                float minThickness = 1.0f;
                float maxThickness = (((width + height) / 2.0f) / (revolutionCount * 2.0f));

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
            private static int Thickness = 1;
            private static float Phi = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;
            private static RenderingTimer Timer = new RenderingTimer();

            private static float DegreesToRadians(float rad)
            {
                return rad * ((float)Math.PI / 180.0f);
            }

            private static float RadiansToDegrees(float degrees)
            {
                return degrees * (180.0f / (float)Math.PI);
            }

            public static void Render(PictureBox pictureBox)
            {
                if (!Timer.IsDisposed)
                {
                    MessageBox.Show(" This control is still busy rendering the last request! Please wait until the control has finished drawing, then try again.", "Too soon!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Timer = new RenderingTimer(pictureBox);
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
                private Size _size;
                private int totalFrames;
                private PictureBox pictureBox;

                public List<Point> PointLocations;

                private System.Windows.Forms.Timer timer;

                public RenderingTimer()
                {
                    IsDisposed = true;
                    i = 0;
                    totalFrames = 0;
                    pictureBox = null;
                }

                public RenderingTimer(PictureBox control, float frameRateFPS = 120f)
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

                    _pen = PenCache.GetBlackPen(Thickness + 1);
                    _size = new Size(Thickness, Thickness);

                    PointLocations = new List<Point>();
                }

                public void Dispose()
                {
                    if (!IsDisposed)
                    {
                        IsDisposed = true;
                    }
                    if (timer != null)
                    {
                        if (timer.Enabled)
                        {
                            timer.Stop();
                        }
                        timer.Dispose();
                    }
                }

                public void Start()
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    int n = 0;
                    PointLocations = new List<Point>();
                    do
                    {
                        PointLocations.Add(CalculatePointLocation(n));
                        n++;
                    }
                    while (n < totalFrames);

                    i = 0;
                    timer.Start();
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

                    return result;
                }

                private void RenderFrame(Point location)
                {
                    Bitmap copy = new Bitmap(pictureBox.Image);
                    using (Graphics graphics = Graphics.FromImage(copy))
                    {
                        Rectangle rect = new Rectangle(location, _size);
                        graphics.DrawEllipse(_pen, rect);
                        graphics.Flush();
                    }
                    pictureBox.Image = copy;
                }
            }
        }


        public static class PenCache
        {
            private static Dictionary<float, Pen> _blackPenDictionary = null;
            private static Dictionary<Tuple<Color, float>, Pen> _penDictionary = null;

            static PenCache()
            {
                _blackPenDictionary = new Dictionary<float, Pen>();
                _penDictionary = new Dictionary<Tuple<Color, float>, Pen>(new ColorFloatTupleEqualityComparer());
            }

            public static Pen GetBlackPen(float thickness)
            {
                if (_blackPenDictionary == null)
                {
                    _blackPenDictionary = new Dictionary<float, Pen>();
                }

                Pen result = null;
                if (_blackPenDictionary.ContainsKey(thickness))
                {
                    result = _blackPenDictionary[thickness];
                }
                else
                {
                    result = new Pen(Color.Black, thickness);
                    _blackPenDictionary.Add(thickness, result);
                }
                return result;
            }

            public static Pen GetPen(Color color, float thickness)
            {
                if (color == Color.Black)
                {
                    return PenCache.GetBlackPen(thickness);
                }
                else
                {
                    Tuple<Color, float> key = new Tuple<Color, float>(color, thickness);

                    if (_penDictionary.ContainsKey(key))
                    {
                        return _penDictionary[key];
                    }
                    else
                    {
                        Pen value = new Pen(color, thickness);
                        _penDictionary.Add(key, value);
                        return value;
                    }
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
                    graphics.DrawEllipse(PenCache.GetBlackPen(thickness), new System.Drawing.Rectangle(new Point((int)Math.Round(location.X), (int)Math.Round(location.Y)), new Size(thickness, thickness)));
                }

                public static void DrawSpiral(Graphics graphics, LineSegment[] geometrySegments, Color color)
                {
                    Pen pen = null;
                    PointF previousPoint = PointF.Empty;
                    foreach (LineSegment segment in geometrySegments)
                    {
                        PointF currentPoint = segment.Point;

                        if (color == Color.Black)
                        {
                            pen = PenCache.GetBlackPen(segment.Thickness);
                        }
                        else
                        {
                            pen = new Pen(color, segment.Thickness);
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
