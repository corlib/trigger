using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Corlib.IO {
    public static class StreamReaderExtensions {
        public static async Task<string> ReadToEndAsync (this StreamReader streamReader, CancellationToken cancellationToken) {
            if (!cancellationToken.CanBeCanceled)
                return await streamReader.ReadToEndAsync ();

            var stringBuilder = new StringBuilder ();
            ulong characters = 0;
            while (!streamReader.EndOfStream && !cancellationToken.IsCancellationRequested) {
                var line = await streamReader.ReadLineAsync ();
                stringBuilder.AppendLine (line);

                if ((characters += (ulong)line.Length) > 1000) {
                    cancellationToken.ThrowIfCancellationRequested ();
                    characters = 0;
                }
            }

            return stringBuilder.ToString ();
        }
    }
}