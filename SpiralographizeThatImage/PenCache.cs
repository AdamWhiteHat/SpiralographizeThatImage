using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralographizeThatImage
{
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
                result.StartCap = System.Drawing.Drawing2D.LineCap.Square;
                result.EndCap = System.Drawing.Drawing2D.LineCap.Square;
                result.MiterLimit = thickness;
                result.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                result.LineJoin = System.Drawing.Drawing2D.LineJoin.Miter;

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
                return HashCode.Combine(obj.Item1.GetHashCode(), obj.Item2.GetHashCode());
            }
        }
    }
}
