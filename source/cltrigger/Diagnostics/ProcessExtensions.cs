using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Corlib.Diagnostics {
    public static class ProcessExtensions {
        public static Task<int> WaitForExitAsync (this Process process, CancellationToken cancellationToken) {
            var taskCompletionSource = new TaskCompletionSource<int> ();

            var registration = cancellationToken.Register (
                () => taskCompletionSource.TrySetCanceled ());

            process.Exited += (s, e) => {
                taskCompletionSource.TrySetResult (process.ExitCode);
                registration.Dispose ();
            };

            process.EnableRaisingEvents = true;

            return taskCompletionSource.Task;
        }
    }
}