using System;

namespace Sharpie
{
    internal class DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action) => _action = action;

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _action?.Invoke();
                }

                _disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose() => Dispose(true);
        #endregion
    }
}
