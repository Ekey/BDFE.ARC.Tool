using System;

namespace BDFE.Unpacker
{
    class ArcEntry
    {
        public String m_NameHash { get; set; } // MD5
        public UInt32 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
    }
}
