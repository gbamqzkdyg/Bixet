using System;
using System.Collections;
using System.Text;

namespace Bixet
{
    public class BixetWriter
    {
        public const string Verion = "0.0.1";
        public const int maxBytesSize = 8;
        public const int maxBitsSize = 64;
        private readonly BitArray bits;
        private readonly Endian byteEndian;
        private readonly Endian bitEndian;
        public int BytesCount { get { return this.bits.Count / 8; } }
        public int BitsCount { get { return this.bits.Count; } }

        public BixetWriter(int byteLength, Endian byteEndian = Endian.BigEndian, Endian bitEndian = Endian.SmallEndian)
        {
            if (byteLength <= 0) throw new ArgumentOutOfRangeException("给定的参数异常");
            this.bits = new BitArray(byteLength * 8);
            this.byteEndian = byteEndian;
            this.bitEndian = bitEndian;
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
            BitArray tmp = new BitArray(new byte[] { value });
            index *= 8;
            for(int i = 7; i >= 0; --i)
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
            BitArray ba = new BitArray(tmp);
            BixetUtil.ReverseBitEndian(ba);
            destIndex *= 8;
            length *= 8;
            for (int i = 0; i < length; ++i) this.bits[destIndex++] = ba[i];
        }

        public void SetRawBytes(int destIndex, byte[] bytes)
        {
            this.SetRawBytes(destIndex, bytes, 0, bytes.Length);
        }

        public void SetRawBits(int destIndex, BitArray bits, int offset, int length)
        {
            if (destIndex < 0 || offset < 0 || length <= 0 || destIndex + length > this.BitsCount || offset + length > bits.Count) throw new ArgumentOutOfRangeException("给定的参数异常");
            //destIndex += length - 1;
            for (int i = 0; i < length; ++i) this.bits[destIndex++] = bits[offset++];
        }

        public byte[] GetData()
        {
            byte[] res = new byte[this.BytesCount];
            BitArray bits = new BitArray(this.bits);
            BixetUtil.ReverseBitEndian(bits);
            bits.CopyTo(res, 0);
            return res;
        }
    }
}
