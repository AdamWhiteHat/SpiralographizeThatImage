using System;
using System.Drawing;

namespace SpiralographizeThatImage.Renderers
{
    public interface ISpiralRenderer<T>
    {
        public static void DrawSpiral<T>(Graphics graphics, T[] geometryElements, Color color) => throw new NotImplementedException();
    }
}
