using System;
using System.IO;
using System.IO.Compression;

namespace BDFE.Unpacker
{
    class Zlib
    {
        public static Byte[] iDecompress(Byte[] lpBuffer)
        {
            Byte[] lpResult = null;
            try
            {
                using (MemoryStream TDstMemoryStream = new MemoryStream())
                {
                    using (MemoryStream TSrcMemoryStream = new MemoryStream(lpBuffer))
                    {
                        Int32 dwDecompressedSize = TSrcMemoryStream.ReadInt32();
                        using (DeflateStream TDeflateStream = new DeflateStream(TSrcMemoryStream, CompressionMode.Decompress))
                        {
                            TDeflateStream.CopyTo(TDstMemoryStream);
                            TDeflateStream.Dispose();
                        }
                    }
                    lpResult = TDstMemoryStream.ToArray();
                }
            }
            catch (Exception exception)
            {
                Array.Resize(ref lpBuffer, lpBuffer.Length - 12);

                return lpBuffer;
            }

            return lpResult;
        }
    }
}