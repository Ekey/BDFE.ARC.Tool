using System;

namespace BDFE.Unpacker
{
    class ArcCipher
    {
        private static UInt32[] iRandomInitialize(Byte[] lpBuffer)
        {
            UInt32 dwValueA = 0;
            UInt32 dwValueB = 0;

            UInt32[] lpRngBuffer = new UInt32[4];

            UInt32 dwTemp = BitConverter.ToUInt32(lpBuffer, 0);

            dwValueA = dwTemp ^ dwTemp << 13;
            lpRngBuffer[0] = dwTemp;

            dwValueB = dwValueA >> 17;
            dwValueA = dwValueB << 5 ^ dwValueA >> 17;
            lpRngBuffer[1] = dwValueA;

            dwValueA = dwValueA << 13 ^ dwValueB << 5;
            dwValueB = dwValueA >> 17;
            dwValueA = dwValueB << 5 ^ dwValueA >> 17;
            lpRngBuffer[2] = dwValueA;

            dwValueB = dwValueA << 13 ^ dwValueB << 5;
            lpRngBuffer[3] = (dwValueB >> 17) << 5 ^ dwValueB >> 17;

            return lpRngBuffer;
        }

        private static UInt32 iRandomGetValue(UInt32[] lpBuffer)
        {
            UInt32 dwResult;
            UInt32 dwValue;

            dwValue = lpBuffer[3];
            dwResult = lpBuffer[0] ^ lpBuffer[0] << 11;

            lpBuffer[0] = lpBuffer[1];
            lpBuffer[1] = lpBuffer[2];
            lpBuffer[2] = dwValue;

            dwResult = dwResult ^ dwResult >> 8 ^ dwValue ^ dwValue >> 19;
            lpBuffer[3] = dwResult;

            return dwResult;
        }

        public static UInt32 iGetSize(Byte[] lpBuffer)
        {
            UInt32 dwTempA;
            UInt32 dwTempB;

            UInt32 dwValueA;
            UInt32 dwValueB;

            var lpRngBuffer = iRandomInitialize(lpBuffer);

            dwTempA = BitConverter.ToUInt32(lpBuffer, 4);
            dwValueA = iRandomGetValue(lpRngBuffer);

            dwTempB = BitConverter.ToUInt32(lpBuffer, 8);
            dwValueB = iRandomGetValue(lpRngBuffer);

            if ((dwValueA ^ dwTempA) == (dwValueB ^ dwTempB))
            {
                return dwValueA ^ dwTempA;
            }

            return dwValueA;
        }

        public static Boolean iCheckSize(Byte[] lpBuffer)
        {
            UInt32 dwTempA;
            UInt32 dwTempB;

            UInt32 dwValueA;
            UInt32 dwValueB;

            var lpRngBuffer = iRandomInitialize(lpBuffer);

            dwTempA = BitConverter.ToUInt32(lpBuffer, 4);
            dwValueA = iRandomGetValue(lpRngBuffer);

            dwTempB = BitConverter.ToUInt32(lpBuffer, 8);
            dwValueB = iRandomGetValue(lpRngBuffer);

            return Convert.ToBoolean((dwValueA ^ dwTempA) == (dwValueB ^ dwTempB));
        }

        public static Byte[] iDecryptData(Byte[] lpBuffer)
        {
            UInt32 dwSize;

            UInt32 dwTempA;
            UInt32 dwTempB;

            var lpRngBuffer = iRandomInitialize(lpBuffer);

            dwTempA = BitConverter.ToUInt32(lpBuffer, 4);
            dwSize = iRandomGetValue(lpRngBuffer) ^ dwTempA;

            dwTempB = BitConverter.ToUInt32(lpBuffer, 8);

            if (dwSize != (iRandomGetValue(lpRngBuffer) ^ dwTempB))
            {
                return lpBuffer;
            }

            if (dwSize > 0)
            {
                UInt32 dwRngValue = 0;

                for (Int32 i = 0; i < dwSize; i++)
                {
                    if ((i & 3) == 0)
                    {
                        dwRngValue = iRandomGetValue(lpRngBuffer);
                    }

                    lpBuffer[i] = (Byte)(lpBuffer[i + 12] ^ (dwRngValue >> (8 * (i & 3))));
                }
            }

            return lpBuffer;
        }
    }
}
