using System;
using System.IO;

namespace ChunkDBTool
{
    public class Program
    {
        private static void Main(string[] args)
        {
            using (var stream = File.OpenRead(@"D:\Fortnite OT11 PC\Fortnite OT11 PC\Fortnite-Windows.part2.chunkdb"))
            using (var reader = new BinaryReader(stream))
            {
                var header = new FChunkDatabase(reader);
            }
        }
    }
}