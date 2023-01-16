using System;
using System.Text;
using System.Security.Cryptography;

namespace BDFE.Unpacker
{
    class ArcHash
    {
        public static String iGetHash(String m_String)
        {
            MD5CryptoServiceProvider TMD5 = new MD5CryptoServiceProvider();
            var lpHash = TMD5.ComputeHash(new ASCIIEncoding().GetBytes(m_String));

            return Utils.iGetStringFromBytes(lpHash);
        }
    }
}
