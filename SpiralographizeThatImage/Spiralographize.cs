using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SpiralographizeThatImage
{
	public static class Spiralographize
	{
		public static class ModulatingThickness
		{
			public static LineSegment[] GetLineSegments(Bitmap image, int revolutionCount)
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


		public static class ModulatingRadius
		{
			public static LineSegment[] GetLineSegments(Bitmap image, int revolutionCount)
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

					point = new PointF();
					point.X = (float)(width / 2 * (1 + scale * Math.Cos(angle)));
					point.Y = (float)(height / 2 * (1 + scale * Math.Sin(angle)));

					results[i] = new LineSegment(point, thickness);
				}

				return results;
			}
		}

		public static class ConstantSpiral
		{
			public static LineSegment[] GetLineSegments(Bitmap image, int revolutionCount)
			{
				int width = image.Width;
				int height = image.Height;

				float thickness = 1.0f;

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

					results[i] = new LineSegment(point, thickness);
				}

				return results;
			}
		}




		private static Dictionary<float, Pen> _penDictionary = null;

		public static void DrawSpiral(Graphics graphics, LineSegment[] lineSegments, Color color)
		{
			if (_penDictionary == null)
			{
				_penDictionary = new Dictionary<float, Pen>();
			}

			Pen pen = null;
			PointF previousPoint = PointF.Empty;
			foreach (LineSegment segment in lineSegments)
			{
				PointF currentPoint = segment.Point;

				if (_penDictionary.ContainsKey(segment.Thickness))
				{
					pen = _penDictionary[segment.Thickness];
				}
				else
				{
					pen = new Pen(color, segment.Thickness);
					_penDictionary.Add(segment.Thickness, pen);
				}

				if (previousPoint != PointF.Empty)
				{
					graphics.DrawLine(pen, previousPoint, currentPoint);
				}

				previousPoint = currentPoint;
			}
		}
	}
}
