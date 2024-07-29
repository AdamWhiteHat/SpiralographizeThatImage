using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralographizeThatImage
{
    public static class ExtensionMethods
    {
        public static Bitmap MakeOpaque(this Bitmap source)
        {
            return (Bitmap)source.Clone(new RectangleF(0, 0, source.Width, source.Height), PixelFormat.Format32bppRgb);
        }
    }
}
