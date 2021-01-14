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

    public class DataFetcher
    {
        public const int maxBytesSize = 8;
        public const int maxBitsSize = 64;
        private readonly byte[] bytes;
        private readonly BitArray bits;
        public int BytesCount { get { return this.bytes.Length; } }
        public int BitsCount { get { return this.bits.Count; } }

        public DataFetcher(byte[] bytes, int offset, int byteLength, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian)
        {
            if (bytes == null || offset < 0 || byteLength <= 0 || offset + byteLength > bytes.Length) throw new IndexOutOfRangeException("给定的参数异常");
            this.bytes = new byte[byteLength];
            Array.Copy(bytes, offset, this.bytes, 0, byteLength);
            if (byteEndian == Endian.SmallEndian)
            {
                this.ReverseBytes();
            }
            this.bits = new BitArray(this.bytes);
            if (bitEndian == Endian.SmallEndian)
            {
                this.ReverseBits();
            }
        }

        public DataFetcher(byte[] bytes, int length, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian) : this(bytes, 0, length, byteEndian, bitEndian) { }

        public DataFetcher(byte[] bytes, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian) : this(bytes, 0, bytes.Length, byteEndian, bitEndian) { }

        public byte this[int i]
        {
            get { return this.GetSingleByte(i); }
        }

        public static byte operator %(DataFetcher df, int i)
        {
            return df.GetSingleBit(i);
        }

        private void ReverseBytes()
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

        private void ReverseBits()
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

        public T GetValueByByteIndex<T>(int beginIndex, int length, bool reverseBytes = false)
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

        public string GetStringByByteIndex(int beginIndex, int length, bool reverseBytes = false, Encoding encoding = null)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBytesSize || beginIndex + length > this.BytesCount) throw new IndexOutOfRangeException("给定的参数异常");
            if (encoding == null) encoding = Encoding.Default;
            if(!reverseBytes) return encoding.GetString(this.bytes, beginIndex, length);
            byte[] tmp = new byte[length];
            beginIndex += length - 1;
            for(int i = 0; i < length; ++i) tmp[i] = this[beginIndex--];
            return encoding.GetString(tmp);
        }

        public T GetValueByBitIndex<T>(int beginIndex, int length, bool reverseBits = false)
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

        public T GetValueByBitIndex<T>(int byteIndex, int bitIndex, int length, bool reverseBits = false)
        {
            return this.GetValueByBitIndex<T>(8 * byteIndex + bitIndex, length, reverseBits);
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
