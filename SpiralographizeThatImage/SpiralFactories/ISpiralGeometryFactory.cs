using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SpiralographizeThatImage.Factories
{
    public enum ImageSpirals
    {
        ByRadius,
        ByThickness
    }

    public enum DrawOrder
    {
        BySpiralArm,
        FromTopToBottom
    }

    public interface ISpiralGeometryFactory<T>
    {
        public static T[] GetGeometryElements<T>(Size size, int quantityParameter) => throw new NotImplementedException();
    }
}
