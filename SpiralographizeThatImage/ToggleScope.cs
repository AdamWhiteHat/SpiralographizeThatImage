using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralographizeThatImage
{
    public class ToggleScope : IDisposable
    {
        public bool IsDisposed { get; private set; } = true;

        private bool _setState;
        private Action<bool> _toggleAction = null;

        public ToggleScope(Action<bool> toggleAction, bool setState = true)
        {
            IsDisposed = false;
            _setState = setState;
            _toggleAction = toggleAction;

            _toggleAction.Invoke(_setState);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                try { }
                finally
                {
                    IsDisposed = true;
                    _toggleAction.Invoke(!_setState);
                }
            }
        }
    }
}
