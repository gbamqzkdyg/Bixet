using System;
using System.Collections;

namespace Bixet
{
    public static class BixetUtil
    {
        public static void ReverseByteEndian(byte[] bytes)
        {
            int begin = 0;
            int end = bytes.Length - 1;
            byte tmp;
            while (begin < end)
            {
                tmp = bytes[begin];
                bytes[begin++] = bytes[end];
                bytes[end--] = tmp;
            }
        }

        public static void ReverseBitEndian(BitArray bits)
        {
            if(bits.Count % 8 != 0) throw new FormatException("待转换比特不为整字节");
            bool tmp;
            for (int i = 0; i < bits.Count; i += 8)
            {
                for (int j = 0; j < 4; ++j)
                {
                    tmp = bits[i + j];
                    bits[i + j] = bits[i + 7 - j];
                    bits[i + 7 - j] = tmp;
                }
            }
        }

        public static void ReverseBits(BitArray bits)
        {
            int hi = bits.Count - 1;
            int lo = 0;
            bool bit;
            while(lo < hi)
            {
                bit = bits[hi];
                bits[hi--] = bits[lo];
                bits[lo++] = bit;
            }
        }
    }
}
