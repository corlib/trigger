using Corlib.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Corlib.Configuration {
    public static class ConfigLoader {
        public static async Task<IEnumerable<TriggerConfig>> LoadDefault (CancellationToken cancellationToken) {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine (baseDirectory, "cltrigger.json");
            return await LoadAsync (path, cancellationToken);
        }

        static async Task<IEnumerable<TriggerConfig>> LoadAsync (string fileName, CancellationToken cancellationToken) {
            string json;
            using (var fileStream = await FileStreamFactory.CreateAsync (fileName, cancellationToken: cancellationToken))
            using (var streamReader = new StreamReader (fileStream)) {
                json = await streamReader.ReadToEndAsync (cancellationToken);
            }
            return JsonConvert.DeserializeObject<IEnumerable<TriggerConfig>> (json);
        }
    }
}