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
                ProcessConfig (triggerConfig, cancellationToken));
        }

        static async void ProcessConfig (TriggerConfig triggerConfig, CancellationToken cancellationToken) {
            try {
                switch (triggerConfig.GetTriggerType ()) {
                    case TriggerType.File:
                        await WaitForTriggerFileAsync (triggerConfig, cancellationToken);
                        break;
                    default:
                        break;
                }
            }
            catch (OperationCanceledException) {
                // expected
            }
        }

        static async Task WaitForTriggerFileAsync (TriggerConfig triggerConfig, CancellationToken cancellationToken) {
            var triggerFile = triggerConfig.TriggerFile;
            using (var watcher = AsyncFileSystemWatcher.File (triggerFile)) {
                while (!cancellationToken.IsCancellationRequested) {

                    await watcher.WaitForExistanceAsync (cancellationToken);

                    //TODO: temporary
                    if (ActionType.Process != triggerConfig.GetActionType ())
                        return;

                    var process = Process.Start (triggerConfig.ProcessAction.ToProcessStartInfo ());

                    await DeleteAsync (triggerFile, cancellationToken);

                    await process.WaitForExitAsync (cancellationToken);
                }
            }
        }

        static Task DeleteAsync (string fileName, CancellationToken cancellationToken) {
            return Retry.Action (() => {
                //TODO: warn if delete is not successful
                if (!File.Exists (fileName))
                    return;

                File.Delete (fileName);
            }, cancellationToken);
        }
    }
}