using System;

namespace ChunkDBTool
{
    [Flags]
    public enum EChunkStorageFlags : byte
    {
        None = 0,
        Compressed = 1,
        Encrypted = 2
    }
}
