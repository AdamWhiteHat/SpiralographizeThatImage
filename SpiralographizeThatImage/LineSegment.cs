using System;
using System.Drawing;

namespace SpiralographizeThatImage
{
	public class LineSegment
	{
		public PointF Point { get; private set; }
		public float Thickness { get; private set; }

		public LineSegment(PointF point, float thickness)
		{
			Point = point;
			Thickness = thickness;
		}
	}
}
