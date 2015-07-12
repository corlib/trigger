using Corlib.Configuration;
using Corlib.IO;
using Corlib.Threading;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Corlib.Diagnostics;
using System;

namespace Corlib.Trigger {
    public static class TriggerProcess {

        public static async void StartAsync (CancellationToken cancellationToken) {
            var configuration = await ConfigLoader.LoadDefault (cancellationToken);

            Parallel.ForEach (configuration, triggerConfig =>
                WaitForTriggerFile (triggerConfig, cancellationToken));
        }

        static async void WaitForTriggerFile (TriggerConfig triggerConfig, CancellationToken cancellationToken) {
            try {
                await WaitForTriggerFileAsync (triggerConfig, cancellationToken);
            }
            catch (OperationCanceledException) {
                // expected
            }
        }

        static async Task WaitForTriggerFileAsync (TriggerConfig triggerConfig, CancellationToken cancellationToken) {
            using (var watcher = AsyncFileSystemWatcher.File (triggerConfig.Trigger)) {
                while (!cancellationToken.IsCancellationRequested) {

                    await watcher.WaitForExistanceAsync (cancellationToken);

                    switch (triggerConfig.Type) {
                        case TriggerType.File:
                            break;
                        default:
                            throw new NotSupportedException ();
                    }

                    var process = Process.Start (triggerConfig.Process.ToProcessStartInfo ());

                    await Retry.Action (() => File.Delete (triggerConfig.Trigger), cancellationToken);

                    await process.WaitForExitAsync (cancellationToken);
                }
            }
        }
    }
}