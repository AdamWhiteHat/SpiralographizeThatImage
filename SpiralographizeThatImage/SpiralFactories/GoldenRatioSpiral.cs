using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SpiralographizeThatImage.Factories
{
    public class GoldenRatioSpiral : ISpiralGeometryFactory<Point>
    {
        public static double Phi = (1.0d + Math.Sqrt(5.0d)) / 2.0d;

        public static PointF[] GetGeometryElements(Size size, int pointQuantity, DrawOrder drawOrder)
        {
            PointF[] results = GetGeometryElements(size, pointQuantity);

            if (drawOrder == DrawOrder.BySpiralArm)
            {
                results = ReorderGoldenRatioPoints_BySpiralArms(results);
            }
            else if (drawOrder == DrawOrder.FromTopToBottom)
            {
                results = ReorderGoldenRatioPoints_ByTopToBottom(results);
            }

            return results;
        }

        public static PointF[] GetGeometryElements(Size size, int pointQuantity)
        {
            int n = pointQuantity - 1;
            List<PointF> results = new List<PointF>();
            do
            {
                results.Add(CalculatePointLocation(n, pointQuantity, size));
                n--;
            }
            while (n >= 0);

            return results.ToArray();
        }

        private static PointF CalculatePointLocation(double currentPoint, double totalPoints, Size drawingArea)
        {
            double angle = currentPoint * Phi * 2.0d * Math.PI;
            double scale = currentPoint / totalPoints;

            // 2 * Pi Radians = 360 degrees
            // Convert to X, Y coords
            PointF result = new PointF();
            result.X = (float)((double)drawingArea.Width / 2d * (1d + scale * Math.Cos(angle)));
            result.Y = (float)((double)drawingArea.Height / 2d * (1d + scale * Math.Sin(angle)));

            result.X = (float)drawingArea.Width - result.X;

            return result;
        }

        #region DrawOrder

        private static PointF[] ReorderGoldenRatioPoints_BySpiralArms(PointF[] points)
        {
            int n = 0;
            Dictionary<int, List<PointF>> bucketsDictionary = new Dictionary<int, List<PointF>>();
            foreach (PointF location in points)
            {
                int key = n % 34;
                if (!bucketsDictionary.ContainsKey(key))
                {
                    bucketsDictionary.Add(key, new List<PointF>());
                }

                List<PointF> bucket = bucketsDictionary[key];
                bucket.Add(location);
                //bucketsDictionary[key] = bucket;

                n++;
            }

            List<PointF> results = new List<PointF>();
            foreach (var kvp in bucketsDictionary)
            {
                results.AddRange(kvp.Value);
            }

            if (DebugHelper.IsDebugEnabled)
            {
                File.WriteAllLines(Points_SpiralArms_Filename, results.Select(p => $"{Math.Round(p.X, 3)},{Math.Round(p.Y, 3)}"));
                File.AppendAllText(Points_SpiralArms_Filename, Environment.NewLine);
            }

            return results.ToArray();
        }

        private static PointF[] ReorderGoldenRatioPoints_ByTopToBottom(PointF[] points)
        {
            List<PointF> results = points.OrderBy(pt => pt.Y).ThenBy(pt => pt.X).ToList();

            if (DebugHelper.IsDebugEnabled)
            {
                File.WriteAllLines(Points_TopToBottom_Filename, results.Select(p => $"{Math.Round(p.X, 3)},{Math.Round(p.Y, 3)}"));
                File.AppendAllText(Points_TopToBottom_Filename, Environment.NewLine);
            }

            return results.ToArray();
        }

        private static string Points_TopToBottom_Filename = "GoldenRatio_PointLocations_OrderBy-TopToBottom.txt";
        private static string Points_SpiralArms_Filename = "GoldenRatio_PointLocations_OrderBy-SpiralArms.txt";

        #endregion

        #region RenderingTimer (Animates the drawing)

        /*

        // if (!Timer.IsDisposed) { MessageBox.Show(" This control is still busy rendering the last request! Please wait until the control has finished drawing, then try again.", "Too soon!", MessageBoxButtons.OK, MessageBoxIcon.Warning); } else { Timer = new RenderingTimer(pictureBox, 33); if (drawingCompleteCallback != null)  { Timer.DrawingCompleted += new EventHandler((s, e) => drawingCompleteCallback.Invoke()); } Timer.Start(); } 

        public class RenderingTimer : IDisposable
        {
            public event EventHandler DrawingCompleted;
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

            protected virtual void RaiseDrawingCompleted(EventArgs e)
            {
                EventHandler handler = DrawingCompleted;
                if (handler != null)
                {
                    handler(this, e);
                }
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
                    PointLocations.Add(CalculatePointLocation(n, totalFrames, pictureBox.Size));
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

            public void Tick(object source, EventArgs e)
            {
                if (IsDisposed)
                {
                    return;
                }

                if (i >= totalFrames)
                {
                    timer.Stop();
                    _graphics.Flush();
                    RaiseDrawingCompleted(EventArgs.Empty);
                    Dispose();
                }
                else
                {
                    RenderFrame(PointLocations[i++]);
                }
            }

            private void RenderFrame(Point location)
            {
                _graphics.DrawEllipse(_pen, new RectangleF(location, _size));
                //_graphics.Flush();
            }
        }

        */

        #endregion

    }
}
