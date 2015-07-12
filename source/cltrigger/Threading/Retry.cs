using System;
using System.Threading;
using System.Threading.Tasks;

namespace Corlib.Threading {
    public static class Retry {

        public static async Task<T> Func<T>(Func<T> func, CancellationToken cancellationToken) {
            foreach (var delay in RetrySequence.Create ()) {
                try {
                    return func ();
                }
                catch {
                    //TODO: log
                }
                await Task.Delay (delay, cancellationToken);
            }
            throw new OperationCanceledException ();
        }

        public static async Task Action (Action action, CancellationToken cancellationToken) {
            Func<object> func = () => {
                action ();
                return null;
            };
            await Func (func, cancellationToken);
        }
    }
}