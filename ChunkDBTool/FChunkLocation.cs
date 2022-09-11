using System.IO;

namespace ChunkDBTool
{
    public class FChunkLocation
    {
        public FGuid ChunkId { get; private set; }
        public ulong ByteStart { get; private set; }
        public int ByteSize { get; private set; }

        public FChunkLocation(BinaryReader reader)
        {
            ChunkId = new FGuid(reader);
            ByteStart = reader.ReadUInt64();
            ByteSize = reader.ReadInt32();
        }
    }
}
