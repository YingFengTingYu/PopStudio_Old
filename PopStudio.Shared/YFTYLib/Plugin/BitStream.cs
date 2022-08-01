using System;
using System.Collections.Generic;
using System.Text;

namespace PopStudio.Plugin
{
    internal class BitStream : IDisposable
    {
        public readonly Stream BaseStream;
        public bool LeaveOpen = false;
        int buffer;

        public bool isMemoryStream => BaseStream is MemoryStream;

        public long Length { get => BaseStream.Length; set => BaseStream.SetLength(value); }

        public long Position { get => BaseStream.Position; set => BaseStream.Position = value; }

        int BitPosition { get; set; }

        public BitStream(Stream stream)
        {
            BaseStream = stream;
            BitPosition = 0;
    }

        public BitStream(byte[] ary) : this(new MemoryStream(ary))
        {
        }

        public BitStream(string filePath, FileMode mode) : this(new FileStream(filePath, mode))
        {
        }

        public BitStream() : this(new MemoryStream())
        {
        }

        public static BitStream Create(string filePath)
        {
            return new BitStream(filePath, FileMode.Create);
        }

        public static BitStream Open(string filePath)
        {
            return new BitStream(filePath, FileMode.Open);
        }

        public int ReadBits(int bits)
        {
            int ans = 0;
            for (int i = bits - 1; i >= 0; i--)
            {
                ans |= ReadOneBit() << i;
            }
            return ans;
        }

        int ReadOneBit()
        {
            if (BitPosition == 0)
            {
                buffer = BaseStream.ReadByte();
                if (buffer == -1)
                {
                    throw new Exception(Str.Obj.EndOfFile);
                }
            }
            BitPosition = (BitPosition + 7) % 8;
            return (buffer >> BitPosition) & 0b1;
        }

        public virtual void Close()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (LeaveOpen)
                {
                    BaseStream.Flush();
                }
                else
                {
                    BaseStream.Close();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void CopyTo(Stream s)
        {
            BaseStream.CopyTo(s);
        }

        public static implicit operator Stream(BitStream a)
        {
            return a.BaseStream;
        }
    }
}
