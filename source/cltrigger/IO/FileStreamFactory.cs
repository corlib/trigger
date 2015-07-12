using Corlib.Threading;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Corlib.IO {
    public static class FileStreamFactory {
        public static async Task<FileStream> CreateAsync (
            string fileName,
            FileMode fileMode = FileMode.Open,
            FileAccess fileAccess = FileAccess.Read,
            FileShare fileShare = FileShare.Read,
            int bufferSize = 4096,
            bool useAsync = true,
            CancellationToken cancellationToken = default(CancellationToken)) {

            if (!File.Exists (fileName))
                using (var watcher = AsyncFileSystemWatcher.File (fileName))
                    await watcher.WaitForChangeAsync (FileSystemChangeType.Created, cancellationToken);

            return await Retry.Func (
                () => new FileStream (fileName, fileMode, fileAccess, fileShare, bufferSize, useAsync),
                cancellationToken);
        }
    }
}