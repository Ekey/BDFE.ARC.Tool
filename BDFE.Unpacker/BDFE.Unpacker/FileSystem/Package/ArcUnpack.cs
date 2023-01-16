using System;
using System.IO;
using System.Collections.Generic;

namespace BDFE.Unpacker
{
    class ArcUnpack
    {
        private static List<ArcEntry> m_EntryTable = new List<ArcEntry>();
        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            ArcHashList.iLoadProject();
            using (FileStream TArcStream = File.OpenRead(m_Archive))
            {
                var lpTable = TArcStream.ReadBytes(12);
                UInt32 dwSize = ArcCipher.iGetSize(lpTable);

                Array.Resize(ref lpTable, (Int32)dwSize + 12);
                var lpTemp = TArcStream.ReadBytes((Int32)dwSize);
                Array.Copy(lpTemp, 0, lpTable, 12, (Int32)dwSize);

                if (!ArcCipher.iCheckSize(lpTable))
                {
                    throw new InvalidDataException();
                }

                lpTable = ArcCipher.iDecryptData(lpTable);

                m_EntryTable.Clear();
                using (var TTableReader = new MemoryStream(lpTable))
                {
                    for (Int32 i = 0; i < TTableReader.Length / 24; i++)
                    {
                        var m_Entry = new ArcEntry();

                        m_Entry.m_NameHash = Utils.iGetStringFromBytes(TTableReader.ReadBytes(16));
                        m_Entry.dwOffset = TTableReader.ReadUInt32();
                        m_Entry.dwCompressedSize = TTableReader.ReadInt32();

                        m_EntryTable.Add(m_Entry);
                    }
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = ArcHashList.iGetNameFromHashList(m_Entry.m_NameHash);
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TArcStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                    var lpSrcBuffer = TArcStream.ReadBytes(m_Entry.dwCompressedSize);

                    if (!ArcCipher.iCheckSize(lpSrcBuffer))
                    {
                        throw new InvalidDataException();
                    }

                    lpSrcBuffer = ArcCipher.iDecryptData(lpSrcBuffer);

                    var lpDstBuffer = Zlib.iDecompress(lpSrcBuffer);

                    File.WriteAllBytes(m_FullPath, lpDstBuffer);
                }

                TArcStream.Dispose();
            }
        }
    }
}
