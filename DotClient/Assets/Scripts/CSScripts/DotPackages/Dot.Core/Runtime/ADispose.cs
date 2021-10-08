using System;

namespace DotEngine.Core
{
    public abstract class ADispose : IDisposable
    {
        private bool dispsed = false;

        ~ADispose()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(dispsed)
            {
                return;
            }
            if(disposing)
            {
                DisposeManagedResource();
            }
            DisposeManagedResource();
        }

        protected abstract void DisposeManagedResource();
        protected abstract void DisposeUnmanagedResource();
    }
}
