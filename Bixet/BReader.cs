using System;

namespace Bixet
{
    public class BReader
    {
        public const string verion = "0.4.0";
        public const int maxBytesSize = 8;
        public const int maxBitsSize = 64;
        private readonly byte[] bytes;
        private readonly System.Collections.BitArray bits;
        public int BytesCount { get { return this.bytes.Length; } }
        public int BitsCount { get { return this.bits.Count; } }

        public BReader(byte[] bytes, int offset, int length)
        {
            if (bytes == null || offset < 0 || length <= 0 || offset + length > bytes.Length) throw new ArgumentOutOfRangeException("给定的参数异常");
            this.bytes = new byte[length];
            Array.Copy(bytes, offset, this.bytes, 0, length);
            this.bits = new System.Collections.BitArray(this.bytes);
        }

        public BReader(byte[] bytes, int length) : this(bytes, 0, length) { }

        public BReader(byte[] bytes) : this(bytes, 0, bytes.Length) { }

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

        public byte[] GetRawBytes(int beginIndex, int length)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            byte[] res = new byte[length];
            Array.Copy(this.bytes, beginIndex, res, 0, length);
            return res;
        }

        public System.Collections.BitArray GetRawBits(int beginIndex, int length)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            System.Collections.BitArray res = new System.Collections.BitArray(length);
            for (int i = 0; i < length; ++i) res[i] = bits[beginIndex++];
            return res;
        }

        public System.Collections.BitArray GetRawBits(int byteIndex, int bitIndex, int length)
        {
            return this.GetRawBits(byteIndex * 8 + bitIndex, length);
        }

        public T ReadValueByByteIndex<T>(int beginIndex, int length, Endian byteEndian = Endian.BigEndian)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBytesSize || beginIndex + length > this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            uint maxLength = BUtil.ByteLengthOfType(typeof(T));
            long res = 0;
            if (maxLength > 0)
            {
                if (length > maxLength) throw new ArgumentOutOfRangeException("目标类型可容纳字节数小于待转换字节数");
                if (byteEndian == Endian.SmallEndian) beginIndex += length - 1;
                for (int i = 0; i < length; ++i)
                {
                    res <<= 8;
                    if (byteEndian == Endian.SmallEndian) res += this[beginIndex--];
                    else res += this[beginIndex++];
                }
            }
            else throw new NotSupportedException("不支持转换为目标类型");
            return (T)Convert.ChangeType(res, typeof(T));
        }

        public string ReadStringByByteIndex(int beginIndex, int length, Endian byteEndian = Endian.BigEndian, System.Text.Encoding encoding = null)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            if(byteEndian == Endian.BigEndian) return (encoding ?? System.Text.Encoding.Default).GetString(this.bytes, beginIndex, length);
            byte[] tmp = new byte[length];
            beginIndex += length - 1;
            for(int i = 0; i < length; ++i) tmp[i] = this[beginIndex--];
            return (encoding ?? System.Text.Encoding.Default).GetString(tmp);
        }

        public T ReadValueByBitIndex<T>(int beginIndex, int length, Endian bitEndian = Endian.SmallEndian)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBitsSize || beginIndex + length > this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            uint maxLength = BUtil.BitLengthOfType(typeof(T));
            long res = 0;
            if (maxLength > 0)
            {
                if (length > maxLength) throw new ArgumentOutOfRangeException("目标类型可容纳比特数小于待转换比特数");
                System.Collections.BitArray bits = this.GetRawBits(beginIndex, length);
                if (bitEndian == Endian.SmallEndian)
                {
                    BUtil.ReverseBitsOrder(bits);
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

        public T ReadValueByBitIndex<T>(int byteIndex, int bitIndex, int length, Endian bitEndian = Endian.SmallEndian)
        {
            return this.ReadValueByBitIndex<T>(8 * byteIndex + bitIndex, length, bitEndian);
        }

        public string ReadStringByBitIndex(int beginIndex, int length, Endian bitEndian = Endian.SmallEndian, System.Text.Encoding encoding = null)
        {
            if (beginIndex < 0 || length <= 0 || beginIndex + length > this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            if (length % 8 != 0) throw new FormatException("待转换比特不为整字节");
            System.Collections.BitArray rawBits = this.GetRawBits(beginIndex, length);
            byte[] buf = new byte[length / 8];
            if (bitEndian == Endian.BigEndian)
            {
                BUtil.ReverseBitsOrder(rawBits);
            }
            rawBits.CopyTo(buf, 0);
            return (encoding ?? System.Text.Encoding.Default).GetString(buf);
        }

        public string ReadStringByBitIndex(int byteIndex, int bitIndex, int length, Endian bitEndian = Endian.SmallEndian, System.Text.Encoding encoding = null)
        {
            return this.ReadStringByBitIndex(byteIndex * 8 + bitIndex, length, bitEndian, encoding);
        }

        
    }
}
