using System;
using System.Collections.Generic;
using System.IO;

namespace ChunkDBTool
{
    public class FChunkDatabase
    {
        private const uint CHUNKDB_HEADER_MAGIC = 0xB1FE3AA3;

        public uint Version { get; private set; }
        public uint HeaderSize { get; private set; }
        public ulong DataSize { get; private set; }
        public int ChunkCount { get; private set; }

        public FChunkLocation[] Locations { get; private set; }
        public FChunkHeader[] Chunks { get; private set; }

        public FChunkDatabase(BinaryReader reader)
        {
            if (reader.ReadUInt32() != CHUNKDB_HEADER_MAGIC)
                throw new Exception("Incorrect chunkdb.");

            Version = reader.ReadUInt32();
            HeaderSize = reader.ReadUInt32();
            DataSize = reader.ReadUInt64();
            ChunkCount = reader.ReadInt32();

            //Console.WriteLine(ChunkCount);
            //Console.ReadKey();

            var locations = new List<FChunkLocation>();
            for (var i = 0; i < ChunkCount; i++)
                locations.Add(new FChunkLocation(reader));

            Locations = locations.ToArray();

            var chunks = new List<FChunkHeader>();
            for (var i = 0; i < Locations.Length; i++)
            {
                reader.BaseStream.Position = (long)Locations[i].ByteStart;
                chunks.Add(new FChunkHeader(reader));
            }

            Chunks = chunks.ToArray();
        }
    }
}
