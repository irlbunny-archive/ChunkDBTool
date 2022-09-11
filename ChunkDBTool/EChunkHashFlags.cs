using System;

namespace ChunkDBTool
{
    [Flags]
    public enum EChunkHashFlags : byte
    {
        None = 0,
        RollingPoly64 = 1,
        Sha1 = 2
    }
}
