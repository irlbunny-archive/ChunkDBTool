using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;
using System.IO.Compression;

namespace ChunkDBTool
{
    public class FChunkHeader
    {
        private const uint CHUNK_HEADER_MAGIC = 0xB1FE3AA2;

        public uint Version { get; private set; }
        public uint HeaderSize { get; private set; }
        public uint DataSizeCompressed { get; private set; }
        public FGuid Guid { get; private set; }
        public ulong RollingHash { get; private set; }
        public EChunkStorageFlags StoredAs { get; private set; }
        public byte[] SHAHash { get; private set; }
        public EChunkHashFlags HashType { get; private set; }

        public FChunkHeader(BinaryReader reader)
        {
            if (reader.ReadUInt32() != CHUNK_HEADER_MAGIC)
                throw new Exception("Incorrect chunk.");

            Version = reader.ReadUInt32();
            HeaderSize = reader.ReadUInt32();
            DataSizeCompressed = reader.ReadUInt32();
            Guid = new FGuid(reader);
            RollingHash = reader.ReadUInt64();
            StoredAs = (EChunkStorageFlags) reader.ReadByte();
            SHAHash = reader.ReadBytes(20);
            HashType = (EChunkHashFlags) reader.ReadByte();

            var guidstring = $"{Guid.A:X8}{Guid.B:X8}{Guid.C:X8}{Guid.D:X8}";
            var filePath = @"D:\Fortnite OT11 PC\Fortnite OT11 PC\Part2\"+guidstring;
            //if (stringGuid == "D6CA528149AEC4EA98D71B88A1666FA2")
            {
                Console.WriteLine($"Guid: {guidstring}, StoredAs: {StoredAs}");
                //File.AppendAllText(@"D:\Fortnite OT11 PC\Fortnite OT11 PC\OT5.txt", $"Guid: {guidstring}, StoredAs: {StoredAs}\n");
                //if (StoredAs == EChunkStorageFlags.None)
                //{
                    //Console.ReadKey();
                //}
                var data = reader.ReadBytes((int)DataSizeCompressed);
                var decompressedData = StoredAs == EChunkStorageFlags.None ? data : Decompress(data);

                //Console.WriteLine("pog");

                if (File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Append))
                    {
                        stream.Write(decompressedData, 0, decompressedData.Length);
                        stream.Flush();
                        stream.Close();
                    }
                }
                else
                    File.WriteAllBytes(filePath, decompressedData);

                //File.WriteAllBytes("test.bin", decompressedData, true);

                //Console.ReadKey();
            }
        }

        public static byte[] Decompress(byte[] data)
        {
            MemoryStream inflated = new MemoryStream();
            using (Stream inflater = new InflaterInputStream(
                new MemoryStream(data), new Inflater(false)))
            {
                int count = 0;
                byte[] deflated = new byte[4096];
                while ((count = inflater.Read(deflated, 0, deflated.Length)) != 0)
                {
                    inflated.Write(deflated, 0, count);
                }
                inflated.Seek(0, SeekOrigin.Begin);
            }
            byte[] content = new byte[inflated.Length];
            inflated.Read(content, 0, content.Length);
            return content;
        }
    }
}
