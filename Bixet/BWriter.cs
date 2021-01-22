using System;

namespace Bixet
{
    public class BWriter
    {
        public const string verion = "0.4.3";
        public const int maxBytesSize = 8;
        public const int maxBitsSize = 64;
        private readonly System.Collections.BitArray bits;
        public int BytesCount { get { return this.bits.Count / 8; } }
        public int BitsCount { get { return this.bits.Count; } }

        public BWriter(int byteLength)
        {
            if (byteLength <= 0) throw new ArgumentOutOfRangeException("给定的参数异常");
            this.bits = new System.Collections.BitArray(byteLength * 8);
        }

        public byte this[int i]
        {
            set => this.SetSingleByte(i, value);
        }

        public byte this[int i, int j]
        {
            set => this.SetSingleBit(i * 8 + j, value);
        }

        private void SetSingleByte(int index, byte value)
        {
            if(index >= this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            System.Collections.BitArray tmp = new System.Collections.BitArray(new byte[] { value });
            index *= 8;
            for(int i = 0; i < 8; ++i)
            {
                this.bits[index++] = tmp[i];
            }
        }

        private void SetSingleBit(int index, byte value)
        {
            if (index >= this.BitsCount || value > 1) throw new ArgumentOutOfRangeException("给定的参数异常");
            this.bits[index] = value != 0;
        }

        public void SetRawBytes(int destIndex, byte[] bytes, uint offset, int length)
        {
            if(destIndex < 0 || length <= 0 || destIndex + length > this.BytesCount || offset + length > bytes.Length) throw new ArgumentOutOfRangeException("给定的参数异常");
            byte[] tmp = new byte[length];
            Array.Copy(bytes, offset, tmp, 0, length);
            System.Collections.BitArray ba = new System.Collections.BitArray(tmp);
            destIndex *= 8;
            length *= 8;
            for (int i = 0; i < length; ++i) this.bits[destIndex++] = ba[i];
        }

        public void SetRawBytes(int destIndex, byte[] bytes)
        {
            this.SetRawBytes(destIndex, bytes, 0, bytes.Length);
        }

        public void SetRawBits(int destIndex, System.Collections.BitArray bits, int offset, int length)
        {
            if (destIndex < 0 || offset < 0 || length <= 0 || destIndex + length > this.BitsCount || offset + length > bits.Count) throw new ArgumentOutOfRangeException("给定的参数异常");
            for (int i = 0; i < length; ++i) this.bits[destIndex++] = bits[offset++];
        }

        public void SetRawBits(int destIndex, System.Collections.BitArray bits)
        {
            this.SetRawBits(destIndex, bits, 0, bits.Count);
        }

        public void SetRawBits(int byteIndex, int bitIndex, System.Collections.BitArray bits, int offset, int length)
        {
            this.SetRawBits(byteIndex * 8 + bitIndex, bits, offset, length);
        }

        public void SetRawBits(int byteIndex, int bitIndex, System.Collections.BitArray bits)
        {
            this.SetRawBits(byteIndex * 8 + bitIndex, bits, 0, bits.Count);
        }

        public void WriteValueByByteIndex<T>(int beginIndex, T value, int length, Endian byteEndian = Endian.BigEndian)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBytesSize || beginIndex + length > this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            uint maxLength = BUtil.ByteLengthOfType(typeof(T));
            if (maxLength > 0)
            {
                if (length > maxLength) throw new ArgumentOutOfRangeException("目标类型可容纳字节数小于待转换字节数");
                ulong l = (ulong)Convert.ChangeType(value, typeof(ulong));
                if(byteEndian == Endian.BigEndian)
                {
                    beginIndex += length - 1;
                }
                for(int i = 0; i < length; ++i)
                {
                    byte b = (byte)(l % 256);
                    if (byteEndian == Endian.BigEndian)
                    {
                        this[beginIndex--] = b;
                    }
                    else
                    {
                        this[beginIndex++] = b;
                    }
                    l >>= 8;
                }
                
            }
            else throw new NotSupportedException("不支持转换为目标类型");
        }

        public void WriteValueByByteIndex<T>(int beginIndex, T value, Endian byteEndian = Endian.BigEndian)
        {
            this.WriteValueByByteIndex<T>(beginIndex, value, (int)BUtil.ByteLengthOfType(typeof(T)), byteEndian);
        }

        public void WriteValueByBitIndex<T>(int beginIndex, T value, int length, Endian bitEndian = Endian.SmallEndian)
        {
            if (beginIndex < 0 || length <= 0 || length > maxBitsSize || beginIndex + length > this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            uint maxLength = BUtil.BitLengthOfType(typeof(T));
            if (maxLength > 0)
            {
                if (length > maxLength) throw new ArgumentOutOfRangeException("目标类型可容纳比特数小于待转换比特数");
                ulong l = (ulong)Convert.ChangeType(value, typeof(ulong));
                if (bitEndian == Endian.BigEndian)
                {
                    beginIndex += length - 1;
                }
                for (int i = 0; i < length; ++i)
                {
                    bool b = l % 2 == 1;
                    if (bitEndian == Endian.BigEndian)
                    {
                        this.bits[beginIndex--] = b;
                    }
                    else
                    {
                        this.bits[beginIndex++] = b;
                    }
                    l >>= 1;
                }

            }
            else throw new NotSupportedException("不支持转换为目标类型");
        }

        public void WriteValueByBitIndex<T>(int byteIndex, int bitIndex, T value, int length)
        {
            this.WriteValueByBitIndex<T>(byteIndex * 8 + bitIndex, value, length);
        }

        public void WriteStringByByteIndex(int beginIndex, string s, int length, Endian byteEndian = Endian.BigEndian, System.Text.Encoding encoding = null)
        {
            if (beginIndex < 0 || s == null || length <= 0 || beginIndex + length > this.BytesCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            byte[] bytes = (encoding ?? System.Text.Encoding.Default).GetBytes(s);
            if(bytes.Length < length) throw new ArgumentOutOfRangeException("给定的参数异常");
            if (byteEndian == Endian.BigEndian) this.SetRawBytes(beginIndex, bytes, 0, length);
            else
            {
                beginIndex += length - 1;
                for (int i = 0; i < length; ++i) this[beginIndex--] = bytes[i];
            }   
        }

        public void WriteStringByBitIndex(int beginIndex, string s, int length, Endian bitEndian = Endian.SmallEndian, System.Text.Encoding encoding = null)
        {
            if (beginIndex < 0 || s == null || length <= 0 || beginIndex + length > this.BitsCount) throw new ArgumentOutOfRangeException("给定的参数异常");
            if (length % 8 != 0) throw new FormatException("待转换比特不为整字节");
            System.Collections.BitArray bits = new System.Collections.BitArray((encoding ?? System.Text.Encoding.Default).GetBytes(s));
            if (bits.Count < length) throw new ArgumentOutOfRangeException("给定的参数异常");
            if (bitEndian == Endian.SmallEndian) this.SetRawBits(beginIndex, bits, 0, length);
            else
            {
                beginIndex += length - 1;
                for (int i = 0; i < length; ++i) this.bits[beginIndex--] = bits[i];
            }
        }

        public void WriteStringByBitIndex(int byteIndex, int bitIndex, string s, int length, Endian bitEndian = Endian.SmallEndian, System.Text.Encoding encoding = null)
        {
            this.WriteStringByBitIndex(byteIndex * 8 + bitIndex, s, length, bitEndian, encoding);
        }

        public byte[] GetData()
        {
            byte[] res = new byte[this.BytesCount];
            this.bits.CopyTo(res, 0);
            return res;
        }
    }
}
