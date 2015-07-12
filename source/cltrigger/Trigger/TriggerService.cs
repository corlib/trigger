using Corlib.Threading;
using System;
using System.Threading;

namespace Corlib.Trigger {
    public sealed class TriggerService {
        readonly CancellationTokenSourceFactory _cancellationTokenSourceSource = new CancellationTokenSourceFactory ();

        public void Start () {
            TriggerProcess.StartAsync (CurrentCancellationToken);
        }

        public void Stop () {
            _cancellationTokenSourceSource.Cancel ();
        }

        public void Pause () {
            var cancellationToken = _cancellationTokenSourceSource.GetNextCancellationToken ();
        }

        public void Continue () {
            TriggerProcess.StartAsync (CurrentCancellationToken);
        }

        CancellationToken CurrentCancellationToken {
            get { return _cancellationTokenSourceSource.CurrentCancellationToken; }
        }
    }
}