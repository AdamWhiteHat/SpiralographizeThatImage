using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralographizeThatImage
{
    public static class DebugHelper
    {
        public static bool IsDebugEnabled;

        static DebugHelper()
        {
            IsDebugEnabled = false;
            SetDebugTrue();
        }

        [Conditional("DEBUG")]
        private static void SetDebugTrue()
        {
            IsDebugEnabled = true;
        }
    }
}
