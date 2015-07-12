using System;

namespace Corlib.IO {
    public sealed class FileSystemChangeEventArgs : EventArgs {
        private readonly Func<Exception> _getException;

        public FileSystemChangeEventArgs (FileSystemChangeType type, Func<Exception> getException = null) {
            Type = type;
            _getException = getException ?? (() => null);
        }

        public FileSystemChangeEventArgs (FileSystemChangeType type, string fullPath, string name)
            : this (type) {
            FullPath = fullPath;
            Name = name;
        }

        public FileSystemChangeEventArgs (FileSystemChangeType type, string fullPath, string name, string oldFullPath, string oldName)
            : this (type, fullPath, name) {
            OldFullPath = oldFullPath;
            OldName = OldName;
        }

        /// <summary>Gets the type of event that occurred</summary>
        /// <returns>One of the FileSystemChangeType values that represents the kind of change detected in the file system.</returns>
        public FileSystemChangeType Type { get; private set; }

        /// <summary>Gets the fully qualifed path of the affected file or directory</summary>
        /// <returns>The path of the affected file or directory</returns>
        public string FullPath { get; private set; }

        /// <summary>Gets the name of the affected file or directory</summary>
        /// <returns>The name of the affected file or directory</returns>
        public string Name { get; private set; }

        /// <summary>Gets the previous fully qualified path of the affected file or directory</summary>
        /// <returns>The previous fully qualified path of the affected file or directory</returns>
        public string OldFullPath { get; private set; }

        /// <summary>Gets the old name of the affected file or directory</summary>
        /// <returns>The previous name of the affected file or directory</returns>
        public string OldName { get; private set; }

        public Exception Exception {
            get { return _getException (); }
        }
    }
}