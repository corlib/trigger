using System.Threading;

namespace Corlib.Threading {
    public sealed class CancellationTokenSourceFactory {

        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource ();
        CancellationTokenSource _current = new CancellationTokenSource ();

        public CancellationTokenSourceFactory () {
            GetNextCancellationToken ();
        }

        public CancellationToken CurrentCancellationToken {
            get {
                lock (_cancellationTokenSource)
                    return _current.Token;
            }
        }

        public CancellationToken GetNextCancellationToken () {
            Token.ThrowIfCancellationRequested ();

            lock (_cancellationTokenSource) {
                Token.ThrowIfCancellationRequested ();

                _current.Cancel ();

                var result = CancellationTokenSource.CreateLinkedTokenSource (Token);
                _current = result;
                return result.Token;
            }
        }

        public void Cancel () {
            _cancellationTokenSource.Cancel ();
        }

        private CancellationToken Token {
            get {
                return _cancellationTokenSource.Token;
            }
        }
    }
}