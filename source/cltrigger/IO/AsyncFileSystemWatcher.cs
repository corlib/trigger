using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Corlib.IO {
    public sealed class AsyncFileSystemWatcher : IDisposable {
        readonly FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher ();
        readonly string _folder;
        readonly string _filter;
        Action<FileSystemChangeEventArgs> _eventHandler;

        private AsyncFileSystemWatcher (string folder, string filter) {
            _folder = folder;
            _filter = filter;

            _fileSystemWatcher.Created += OnCreated;
            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Renamed += OnRenamed;
            _fileSystemWatcher.Deleted += OnDeleted;
            _fileSystemWatcher.Error += OnError;
        }

        public static AsyncFileSystemWatcher Folder (string folder) {
            return new AsyncFileSystemWatcher (folder, null);
        }

        public static AsyncFileSystemWatcher File (string fileName) {
            return new AsyncFileSystemWatcher (
                Path.GetDirectoryName (fileName),
                Path.GetFileName (fileName));
        }

        public Task<FileSystemChangeEventArgs> WaitForExistanceAsync (CancellationToken cancellationToken) {
            var directoryInfo = new DirectoryInfo (_folder);
            return WaitForExistanceAsync (directoryInfo, _filter, cancellationToken);
        }

        public async Task<FileSystemChangeEventArgs> WaitForChangeAsync (FileSystemChangeType changeType, CancellationToken cancellationToken) {
            var directoryInfo = new DirectoryInfo (_folder);
            await WaitForExistanceAsync (directoryInfo.Parent, directoryInfo.Name, cancellationToken);

            return await WaitForChangeAsync (_folder, _filter, changeType, null, cancellationToken);
        }

        public void Dispose () {
            _fileSystemWatcher.Dispose ();
        }

        async Task<FileSystemChangeEventArgs> WaitForExistanceAsync (DirectoryInfo directoryInfo, string filter, CancellationToken cancellationToken) {
            var parent = directoryInfo.Parent;
            if (null != parent && !parent.Exists)
                await WaitForExistanceAsync (parent, directoryInfo.Name, cancellationToken);

            var changeType = FileSystemChangeType.Created;
            var folder = directoryInfo.FullName;
            Action<TaskCompletionSource<FileSystemChangeEventArgs>> onEnableRaisingEvents = taskCompletionSource => {
                if (Exists (folder, filter))
                    taskCompletionSource.TrySetResult (new FileSystemChangeEventArgs (changeType, folder, filter));
            };

            return Exists (folder, filter) ?
                new FileSystemChangeEventArgs (changeType, folder, filter) :
                await WaitForChangeAsync (folder, filter, changeType, onEnableRaisingEvents, cancellationToken);
        }

        void UpdateWatcher(string folder, string filter, NotifyFilters notifyFilters) {
            _fileSystemWatcher.Path = folder;
            _fileSystemWatcher.Filter = filter;
            _fileSystemWatcher.NotifyFilter = notifyFilters;
        }

        Task<FileSystemChangeEventArgs> WaitForChangeAsync (string folder, string filter, FileSystemChangeType changeType, Action<TaskCompletionSource<FileSystemChangeEventArgs>> onEnableRaisingEvents, CancellationToken cancellationToken) {
            var taskCompletionSource = new TaskCompletionSource<FileSystemChangeEventArgs> ();

            UpdateWatcher (
                folder,
                filter,
                changeType.GetNotifyFilters ());

            var registration = cancellationToken.Register (() => {
                _fileSystemWatcher.EnableRaisingEvents = false;
                taskCompletionSource.TrySetCanceled ();
            });

            _eventHandler = e => {
                _fileSystemWatcher.EnableRaisingEvents = false;
                taskCompletionSource.TrySetResult (e);
                registration.Dispose ();
            };

            _fileSystemWatcher.EnableRaisingEvents = true;

            if (null != onEnableRaisingEvents)
                onEnableRaisingEvents (taskCompletionSource);

            return taskCompletionSource.Task;
        }

        void OnCreated (object sender, FileSystemEventArgs e) {
            _eventHandler (new FileSystemChangeEventArgs (
                FileSystemChangeType.Created,
                e.FullPath,
                e.Name));
        }

        void OnChanged (object sender, FileSystemEventArgs e) {
            _eventHandler (new FileSystemChangeEventArgs (
                FileSystemChangeType.Changed,
                e.FullPath,
                e.Name));
        }

        void OnRenamed (object sender, RenamedEventArgs e) {
            _eventHandler (new FileSystemChangeEventArgs (
                FileSystemChangeType.Renamed,
                e.FullPath,
                e.Name));
        }

        void OnDeleted (object sender, FileSystemEventArgs e) {
            _eventHandler (new FileSystemChangeEventArgs (
                FileSystemChangeType.Deleted,
                e.FullPath,
                e.Name));
        }

        void OnError (object sender, ErrorEventArgs e) {
            _eventHandler (new FileSystemChangeEventArgs (
                FileSystemChangeType.Exception,
                e.GetException));
        }

        static bool Exists (string folder, string filter) {
            return System.IO.File.Exists (Path.Combine (folder, filter));
        }
    }
}