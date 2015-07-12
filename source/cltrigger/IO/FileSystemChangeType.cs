namespace Corlib.IO {
    public enum FileSystemChangeType : byte {
        None,
        Created,
        Renamed,
        Changed,
        Deleted,
        Exception
    }
}