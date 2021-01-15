using System;
using System.Collections;
using System.Text;

namespace Bixet
{
    public enum Endian
    {
        BigEndian,
        SmallEndian
    }

    public class BixetReader
    {
        public const string Verion = "0.0.2";
        public const int maxBytesSize = 8;
        public const int maxBitsSize = 64;
        private readonly byte[] bytes;
        private readonly BitArray bits;
        public int BytesCount { get { return this.bytes.Length; } }
        public int BitsCount { get { return this.bits.Count; } }

        public BixetReader(byte[] bytes, int offset, int byteLength, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian)
        {
            if (bytes == null || offset < 0 || byteLength <= 0 || offset + byteLength > bytes.Length) throw new IndexOutOfRangeException("给定的参数异常");
            this.bytes = new byte[byteLength];
            Array.Copy(bytes, offset, this.bytes, 0, byteLength);
            if (byteEndian == Endian.SmallEndian)
            {
                this.ReverseByteEndian(this.bytes);
            }
            this.bits = new BitArray(this.bytes);
            if (bitEndian == Endian.SmallEndian)
            {
                this.ReverseBitEndian(this.bits);
            }
        }

        public BixetReader(byte[] bytes, int length, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian) : this(bytes, 0, length, byteEndian, bitEndian) { }

        public BixetReader(byte[] bytes, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian) : this(bytes, 0, bytes.Length, byteEndian, bitEndian) { }

        public byte this[int i]
        {
            get { return this.GetSingleByte(i); }
        }

        public static byte operator %(BixetReader df, int i)
        {
            return df.GetSingleBit(i);
        }

        private void ReverseByteEndian(byte[] bytes)
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

        private void ReverseBitEndian(BitArray bits)
        {
            bool tmp;
            for(int i = 0; i < BitsCount; i += 8)
            {

                for(int j = 0; j < 4; ++j)
                {
                    tmp = bits[i + j];
                    bits[i + j] = bits[i + 7 - j];
                    bits[i + 7 - j] = tmp;
                }
            }
        }

        private byte GetSingleByte(int beginIndex)
        {
            if (beginIndex >= this.BytesCount) throw new IndexOutOfRangeException("给定的参数异常");
            return this.bytes[beginIndex];
        }

        private byte GetSingleBit(int beginIndex)
        {
            if (beginIndex >= this.BitsCount) throw new IndexOutOfRangeException("给定的参数异常");
            return (byte)(this.bits[beginIndex] ? 1 : 0);
        }

        public byte[] GetRawBytes(uint beginIndex, uint length)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BytesCount) throw new IndexOutOfRangeException("给定的参数异常");
            byte[] res = new byte[length];
            Array.Copy(this.bytes, beginIndex, res, 0, length);
            return res;
        }

        public BitArray GetRawBits(int beginIndex, int length)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BitsCount) throw new IndexOutOfRangeException("给定的参数异常");
            BitArray res = new BitArray(length);
            for (int i = 0; i < length; ++i) res[i] = bits[beginIndex++];
            return res;
        }

        public BitArray GetRawBits(int byteIndex, int bitIndex, int length)
        {
            return this.GetRawBits(byteIndex * 8 + bitIndex, length);
        }

        public T ReadValueByByteIndex<T>(int beginIndex, int length, bool reverseBytes = false)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBytesSize || beginIndex + length > this.BytesCount) throw new IndexOutOfRangeException("给定的参数异常");
            uint maxLength = this.ByteLengthOfType(typeof(T));
            long res = 0;
            if (maxLength > 0)
            {
                if (length > maxLength) throw new IndexOutOfRangeException("目标类型可容纳字节数小于待转换字节数");
                if (reverseBytes) beginIndex += length - 1;
                for (int i = 0; i < length; ++i)
                {
                    res *= 256;
                    if (!reverseBytes) res += this[beginIndex++];
                    else res += this[beginIndex--];
                }
            }
            else throw new NotSupportedException("不支持转换为目标类型");
            return (T)Convert.ChangeType(res, typeof(T));
        }

        public string ReadStringByByteIndex(int beginIndex, int length, bool reverseBytes = false, Encoding encoding = null)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBytesSize || beginIndex + length > this.BytesCount) throw new IndexOutOfRangeException("给定的参数异常");
            if (encoding == null) encoding = Encoding.Default;
            if(!reverseBytes) return encoding.GetString(this.bytes, beginIndex, length);
            byte[] tmp = new byte[length];
            beginIndex += length - 1;
            for(int i = 0; i < length; ++i) tmp[i] = this[beginIndex--];
            return encoding.GetString(tmp);
        }

        public T ReadValueByBitIndex<T>(int beginIndex, int length, bool reverseBits = false)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBitsSize || beginIndex + length > this.BitsCount) throw new IndexOutOfRangeException("给定的参数异常");
            uint maxLength = this.BitLengthOfType(typeof(T));
            long res = 0;
            if (maxLength > 0)
            {
                if (length > maxLength) throw new IndexOutOfRangeException("目标类型可容纳比特数小于待转换比特数");
                if (reverseBits) beginIndex += length - 1;
                for (int i = 0; i < length; ++i)
                {
                    res *= 2;
                    if (!reverseBits) res += this % (beginIndex++);
                    else res += this % (beginIndex--);
                }
            }
            else throw new NotSupportedException("不支持转换为目标类型");
            return (T)Convert.ChangeType(res, typeof(T));
        }

        public T ReadValueByBitIndex<T>(int byteIndex, int bitIndex, int length, bool reverseBits = false)
        {
            return this.ReadValueByBitIndex<T>(8 * byteIndex + bitIndex, length, reverseBits);
        }

        public string ReadStringByBitIndex(int beginIndex, int length, bool reverseBits = false, Encoding encoding = null)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BitsCount) throw new IndexOutOfRangeException("给定的参数异常");
            if (length % 8 != 0) throw new FormatException("待转换比特不为整字节");
            if (encoding == null) encoding = Encoding.Default;
            BitArray rawBits = this.GetRawBits(beginIndex, length);
            byte[] buf = new byte[length / 8];
            if (!reverseBits)
            {
                int lo = 0;
                int hi = length - 1;
                while(lo < hi)
                {
                    bool tmp = rawBits[lo];
                    rawBits[lo++] = rawBits[hi];
                    rawBits[hi--] = tmp;
                }
            }
            rawBits.CopyTo(buf, 0);
            this.ReverseByteEndian(buf);
            return encoding.GetString(buf);
        }

        public string ReadStringByBitIndex(int byteIndex, int bitIndex, int length, bool reverseBits = false, Encoding encoding = null)
        {
            return this.ReadStringByBitIndex(byteIndex * 8 + bitIndex, length, reverseBits, encoding);
        }

        private uint BitLengthOfType(Type T)
        {
            if (T == typeof(bool)) return 1;
            else if(T == typeof(sbyte)) return 8;
            else if(T == typeof(byte)) return 8;
            else if(T == typeof(int)) return 16;
            else if(T == typeof(uint)) return 16;
            else if(T == typeof(short)) return 32;
            else if(T == typeof(ushort)) return 32;
            else if(T == typeof(long)) return 64;
            else if(T == typeof(ulong)) return 64;
            return 0;
        }

        private uint ByteLengthOfType(Type T)
        {
            return this.BitLengthOfType(T) / 8;
        }
    }
}
