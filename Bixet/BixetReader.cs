using System;
using System.Collections;
using System.Text;

namespace Bixet
{
    public class BixetReader
    {
        public const string Verion = "0.2.1";
        public const int maxBytesSize = 8;
        public const int maxBitsSize = 64;
        private readonly Endian byteEndian;
        private readonly Endian bitEndian;
        private readonly byte[] bytes;
        private readonly BitArray bits;
        public int BytesCount { get { return this.bytes.Length; } }
        public int BitsCount { get { return this.bits.Count; } }

        public BixetReader(byte[] bytes, int offset, int byteLength, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian)
        {
            if (bytes == null || offset < 0 || byteLength <= 0 || offset + byteLength > bytes.Length) throw new ArgumentOutOfRangeException("给定的参数异常");
            this.byteEndian = byteEndian;
            this.bitEndian = bitEndian;
            this.bytes = new byte[byteLength];
            Array.Copy(bytes, offset, this.bytes, 0, byteLength);
            this.bits = new BitArray(this.bytes);
        }

        public BixetReader(byte[] bytes, int length, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian) : this(bytes, 0, length, byteEndian, bitEndian) { }

        public BixetReader(byte[] bytes, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian) : this(bytes, 0, bytes.Length, byteEndian, bitEndian) { }

        public byte this[int i]
        {
            get => this.GetSingleByte(i);
        }

        public byte this[int i, int j]
        {
            get => this.GetSingleBit(i * 8 + j);
        }

        private byte GetSingleByte(int index)
        {
            if (index < 0 || index >= this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            return this.bytes[index];
        }

        private byte GetSingleBit(int index)
        {
            if (index < 0 || index >= this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            return (byte)(this.bits[index] ? 1 : 0);
        }

        public byte[] GetRawBytes(uint beginIndex, uint length)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            byte[] res = new byte[length];
            Array.Copy(this.bytes, beginIndex, res, 0, length);
            return res;
        }

        public BitArray GetRawBits(int beginIndex, int length)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            BitArray res = new BitArray(length);
            for (int i = 0; i < length; ++i) res[i] = bits[beginIndex++];
            return res;
        }

        public BitArray GetRawBits(int byteIndex, int bitIndex, int length)
        {
            return this.GetRawBits(byteIndex * 8 + bitIndex, length);
        }

        public T ReadValueByByteIndex<T>(int beginIndex, int length)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBytesSize || beginIndex + length > this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            uint maxLength = BixetUtil.ByteLengthOfType(typeof(T));
            long res = 0;
            if (maxLength > 0)
            {
                if (length > maxLength) throw new ArgumentOutOfRangeException("目标类型可容纳字节数小于待转换字节数");
                if (this.byteEndian == Endian.SmallEndian) beginIndex += length - 1;
                for (int i = 0; i < length; ++i)
                {
                    res <<= 8;
                    if (this.byteEndian == Endian.SmallEndian) res += this[beginIndex--];
                    else res += this[beginIndex++];
                }
            }
            else throw new NotSupportedException("不支持转换为目标类型");
            return (T)Convert.ChangeType(res, typeof(T));
        }

        public string ReadStringByByteIndex(int beginIndex, int length, Encoding encoding = null)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            if(this.byteEndian == Endian.BigEndian) return (encoding ?? Encoding.Default).GetString(this.bytes, beginIndex, length);
            byte[] tmp = new byte[length];
            beginIndex += length - 1;
            for(int i = 0; i < length; ++i) tmp[i] = this[beginIndex--];
            return (encoding ?? Encoding.Default).GetString(tmp);
        }

        public T ReadValueByBitIndex<T>(int beginIndex, int length)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBitsSize || beginIndex + length > this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            uint maxLength = BixetUtil.BitLengthOfType(typeof(T));
            long res = 0;
            if (maxLength > 0)
            {
                if (length > maxLength) throw new ArgumentOutOfRangeException("目标类型可容纳比特数小于待转换比特数");
                BitArray bits = this.GetRawBits(beginIndex, length);
                if (this.bitEndian == Endian.SmallEndian)
                {
                    BixetUtil.ReverseBits(bits);
                }
                for (int i = 0; i < length; ++i)
                {
                    res <<= 1;
                    res += bits[i] ? 1 : 0;
                }
            }
            else throw new NotSupportedException("不支持转换为目标类型");
            return (T)Convert.ChangeType(res, typeof(T));
        }

        public T ReadValueByBitIndex<T>(int byteIndex, int bitIndex, int length)
        {
            return this.ReadValueByBitIndex<T>(8 * byteIndex + bitIndex, length);
        }

        public string ReadStringByBitIndex(int beginIndex, int length, Encoding encoding = null)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            if (length % 8 != 0) throw new FormatException("待转换比特不为整字节");
            BitArray rawBits = this.GetRawBits(beginIndex, length);
            byte[] buf = new byte[length / 8];
            if (this.bitEndian == Endian.BigEndian)
            {
                BixetUtil.ReverseBits(rawBits);
            }
            rawBits.CopyTo(buf, 0);
            if (this.byteEndian == Endian.SmallEndian)
            {
                BixetUtil.ReverseByteEndian(buf);
            }
            return (encoding ?? Encoding.Default).GetString(buf);
        }

        public string ReadStringByBitIndex(int byteIndex, int bitIndex, int length, Encoding encoding = null)
        {
            return this.ReadStringByBitIndex(byteIndex * 8 + bitIndex, length, encoding);
        }

        
    }
}
