using System;
using System.IO;

namespace Corlib.IO {
    internal static class FileSystemChangeTypeExtensions {
        public static NotifyFilters GetNotifyFilters (this FileSystemChangeType changeType) {
            switch (changeType) {
                case FileSystemChangeType.Created:
                    return NotifyFilters.FileName | NotifyFilters.CreationTime;
                default:
                    throw new NotSupportedException ();
            }
        }
    }
}