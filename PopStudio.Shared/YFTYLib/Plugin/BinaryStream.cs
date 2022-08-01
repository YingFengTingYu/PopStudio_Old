using System.Text;

namespace PopStudio.Plugin
{
    /// <summary>
    /// 用于提供对流的二进制读写操作（你可以很轻松地用BinaryStream去创建其他流）
    ///  | 
    /// Used for read and write the Stream (You can easily use BinaryStream to create other stream)
    /// </summary>
    internal class BinaryStream : IDisposable
    {
        //2022.2.14 fix the bug of read int64 and uint64
        //字段
        //Fields
        public Encoding Encode = Encoding.UTF8;
        public Endian Endian = Endian.Small;
        public Endian StringEndian = Endian.Big;
        public Stream BaseStream;
        public bool LeaveOpen = false;
        byte[] m_buffer;

        //属性
        //Properties
        public bool isMemoryStream => BaseStream is MemoryStream;

        public long Length { get => BaseStream.Length; set => BaseStream.SetLength(value); }

        public long Position { get => BaseStream.Position; set => BaseStream.Position = value; }

        //构造
        //Constructors

        /// <summary>
        /// 使用Stream流来创建BinaryHelper
        ///  | 
        /// Use a Stream to create a BinaryHelper
        /// </summary>
        /// <param name="stream">BinaryHelper的基础流 | The Base Stream of BinaryHelper</param>
        public BinaryStream(Stream stream)
        {
            BaseStream = stream;
            m_buffer = new byte[16];
        }

        /// <summary>
        /// 使用字节数组来创建MemoryStream，然后创建基于这个MemoryStream的BinaryHelper
        ///  | 
        /// Use byte[] to create a MemoryStream, and then create a BinaryHelper based on it.
        /// </summary>
        /// <param name="ary">用于创建MemoryStream的字节数组 | The byte[] to create MemoryStream</param>
        public BinaryStream(byte[] ary) : this(new MemoryStream(ary))
        {
        }

        /// <summary>
        /// 通过文件位置和载入模式来创建FileStream，然后创建基于这个FileStream的BinaryHelper
        ///  | 
        /// Create a FileStream by file path, and then create a BinaryHelper based on it.
        /// </summary>
        /// <param name="filePath">文件位置 | The path of the file</param>
        /// <param name="mode">访问方式 | The method of FileStream</param>
        public BinaryStream(string filePath, FileMode mode) : this(new FileStream(filePath, mode))
        {
        }

        /// <summary>
        /// 创建一个MemoryStream，然后创建基于这个MemoryStream的BinaryHelper
        ///  | 
        /// Create a MemoryStream and then create a BinaryHelper based on it.
        /// </summary>
        public BinaryStream() : this(new MemoryStream())
        {
        }

        public static BinaryStream Create(string filePath)
        {
            return new BinaryStream(filePath, FileMode.Create);
        }

        public static BinaryStream Open(string filePath)
        {
            return new BinaryStream(filePath, FileMode.Open);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        //校验数据
        public void IdByte(byte h)
        {
            if (ReadUInt8() != h) throw new Exception(Str.Obj.DataMisMatch);
        }

        public void IdUInt8(byte h, Endian _ = Endian.Null)
        {
            if (ReadUInt8() != h) throw new Exception(Str.Obj.DataMisMatch);
        }

        public void IdInt16(short h, Endian endian = Endian.Null)
        {
            if (ReadInt16(endian) != h) throw new Exception(Str.Obj.DataMisMatch);
        }

        public void IdUInt16(ushort h, Endian endian = Endian.Null)
        {
            if (ReadUInt16(endian) != h) throw new Exception(Str.Obj.DataMisMatch);
        }

        public void IdInt32(int h, Endian endian = Endian.Null)
        {
            if (ReadInt32(endian) != h) throw new Exception(Str.Obj.DataMisMatch);
        }

        public void IdUInt32(uint h, Endian endian = Endian.Null)
        {
            if (ReadUInt32(endian) != h) throw new Exception(Str.Obj.DataMisMatch);
        }

        public void IdBytes(byte[] h, Endian _ = Endian.Null)
        {
            int length = h.Length;
            byte[] r = ReadBytes(length);
            for (int i = 0; i < length; i++) if (r[i] != h[i]) throw new Exception(Str.Obj.DataMisMatch);
        }

        public void IdString(string h, Endian endian = Endian.Null)
        {
            if (ReadString(h.Length, endian) != h) throw new Exception(Str.Obj.DataMisMatch);
        }

        public byte PeekByte(Endian _ = Endian.Null)
        {
            byte ans = ReadUInt8();
            Position--;
            return ans;
        }

        public ushort PeekUInt16(Endian endian = Endian.Null)
        {
            ushort ans = ReadUInt16(endian);
            Position -= 2;
            return ans;
        }

        public int PeekInt32(Endian endian = Endian.Null)
        {
            int ans = ReadInt32(endian);
            Position -= 4;
            return ans;
        }

        public string PeekString(int count, Endian endian = Endian.Null)
        {
            string ans = ReadString(count, endian);
            Position -= count;
            return ans;
        }

        //读取数据
        /// <summary>
        /// 读取有符号字节
        ///  | 
        /// Read signed byte
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public sbyte ReadSByte(Endian _ = Endian.Null)
        {
            FillBuffer(1);
            return (sbyte)m_buffer[0];
        }

        /// <summary>
        /// 读取有符号字节
        ///  | 
        /// Read signed byte
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public sbyte ReadInt8(Endian _ = Endian.Null)
        {
            FillBuffer(1);
            return (sbyte)m_buffer[0];
        }

        /// <summary>
        /// 读取16位整数
        ///  | 
        /// Read 16 bits integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public short ReadShort(Endian endian = Endian.Null) => ReadInt16(endian);

        /// <summary>
        /// 读取16位整数
        ///  | 
        /// Read 16 bits integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public short ReadInt16(Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            FillBuffer(2);
            if (endian == Endian.Big)
            {
                return (short)(m_buffer[1] | (m_buffer[0] << 8));
            }
            return (short)(m_buffer[0] | (m_buffer[1] << 8));
        }

        /// <summary>
        /// 读取24位整数
        ///  | 
        /// Read 24 bits integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public int ReadThreeByte(Endian endian = Endian.Null) => ReadInt24(endian);

        /// <summary>
        /// 读取24位整数
        ///  | 
        /// Read 24 bits integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public int ReadInt24(Endian endian = Endian.Null)
        {
            uint input = ReadUInt24(endian);
            if ((input & 0x800000) != 0) input |= 0xff000000;
            return (int)input;
        }

        /// <summary>
        /// 读取32位整数
        ///  | 
        /// Read 32 bits integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public int ReadInt(Endian endian = Endian.Null) => ReadInt32(endian);

        /// <summary>
        /// 读取32位整数
        ///  | 
        /// Read 32 bits integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public int ReadInt32(Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            FillBuffer(4);
            if (endian == Endian.Big)
            {
                return m_buffer[3] | (m_buffer[2] << 8) | (m_buffer[1] << 16) | (m_buffer[0] << 24);
            }
            return m_buffer[0] | (m_buffer[1] << 8) | (m_buffer[2] << 16) | (m_buffer[3] << 24);
        }

        /// <summary>
        /// 读取64位整数
        ///  | 
        /// Read 64 bits integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public long ReadLong(Endian endian = Endian.Null) => ReadInt64(endian);

        /// <summary>
        /// 读取64位整数
        ///  | 
        /// Read 64 bits integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public long ReadInt64(Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            FillBuffer(8);
            if (endian == Endian.Big)
            {
                return (long)((((ulong)(uint)(m_buffer[3] | (m_buffer[2] << 8) | (m_buffer[1] << 16) | (m_buffer[0] << 24))) << 32) | ((uint)(m_buffer[7] | (m_buffer[6] << 8) | (m_buffer[5] << 16) | (m_buffer[4] << 24))));
            }
            return (long)((((ulong)(uint)(m_buffer[4] | (m_buffer[5] << 8) | (m_buffer[6] << 16) | (m_buffer[7] << 24))) << 32) | ((uint)(m_buffer[0] | (m_buffer[1] << 8) | (m_buffer[2] << 16) | (m_buffer[3] << 24))));
        }

        /// <summary>
        /// 读取字节
        ///  | 
        /// Read byte
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public byte ReadByte(Endian _ = Endian.Null)
        {
            FillBuffer(1);
            return m_buffer[0];
        }

        /// <summary>
        /// 读取字节
        ///  | 
        /// Read byte
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public byte ReadUInt8(Endian _ = Endian.Null)
        {
            FillBuffer(1);
            return m_buffer[0];
        }

        /// <summary>
        /// 读取16位无符号整数
        ///  | 
        /// Read 16 bits unsigned integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public ushort ReadUShort(Endian endian = Endian.Null) => ReadUInt16(endian);

        /// <summary>
        /// 读取16位无符号整数
        ///  | 
        /// Read 16 bits unsigned integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public ushort ReadUInt16(Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            FillBuffer(2);
            if (endian == Endian.Big)
            {
                return (ushort)(m_buffer[1] | (m_buffer[0] << 8));
            }
            return (ushort)(m_buffer[0] | (m_buffer[1] << 8));
        }

        /// <summary>
        /// 读取24位无符号整数
        ///  | 
        /// Read 24 bits unsigned integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public uint ReadUThreeByte(Endian endian = Endian.Null) => ReadUInt24(endian);

        /// <summary>
        /// 读取24位无符号整数
        ///  | 
        /// Read 24 bits unsigned integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public uint ReadUInt24(Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            FillBuffer(3);
            if (endian == Endian.Big)
            {
                return (uint)(m_buffer[2] | (m_buffer[1] << 8) | (m_buffer[0] << 16));
            }
            return (uint)(m_buffer[0] | (m_buffer[1] << 8) | (m_buffer[2] << 16));
        }

        /// <summary>
        /// 读取32位无符号整数
        ///  | 
        /// Read 32 bits unsigned integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public uint ReadUInt(Endian endian = Endian.Null) => ReadUInt32(endian);

        /// <summary>
        /// 读取32位无符号整数
        ///  | 
        /// Read 32 bits unsigned integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public uint ReadUInt32(Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            FillBuffer(4);
            if (endian == Endian.Big)
            {
                return (uint)(m_buffer[3] | (m_buffer[2] << 8) | (m_buffer[1] << 16) | (m_buffer[0] << 24));
            }
            return (uint)(m_buffer[0] | (m_buffer[1] << 8) | (m_buffer[2] << 16) | (m_buffer[3] << 24));
        }

        /// <summary>
        /// 读取64位无符号整数
        ///  | 
        /// Read 64 bits unsigned integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public ulong ReadULong(Endian endian = Endian.Null) => ReadUInt64(endian);

        /// <summary>
        /// 读取64位无符号整数
        ///  | 
        /// Read 64 bits unsigned integer
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public ulong ReadUInt64(Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            FillBuffer(8);
            if (endian == Endian.Big)
            {
                return (((ulong)(uint)(m_buffer[3] | (m_buffer[2] << 8) | (m_buffer[1] << 16) | (m_buffer[0] << 24))) << 32) | ((uint)(m_buffer[7] | (m_buffer[6] << 8) | (m_buffer[5] << 16) | (m_buffer[4] << 24)));
            }
            return (((ulong)(uint)(m_buffer[4] | (m_buffer[5] << 8) | (m_buffer[6] << 16) | (m_buffer[7] << 24))) << 32) | ((uint)(m_buffer[0] | (m_buffer[1] << 8) | (m_buffer[2] << 16) | (m_buffer[3] << 24)));
        }

        /// <summary>
        /// 读取VarInt并转为32位整数
        ///  | 
        /// Read varint and turn it to 32 bits integer
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public int ReadVarInt32(Endian _ = Endian.Null)
        {
            int num = 0;
            int num2 = 0;
            byte b;
            do
            {
                if (num2 == 35)
                {
                    throw new Exception(Str.Obj.VarIntTooBig);
                }
                b = ReadUInt8();
                num |= (b & 0x7F) << num2;
                num2 += 7;
            }
            while ((b & 0x80) != 0);
            return num;
        }

        /// <summary>
        /// 读取VarInt并转为64位整数
        ///  | 
        /// Read varint and turn it to 64 bits integer
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public long ReadVarInt64(Endian _ = Endian.Null)
        {
            long num = 0;
            int num2 = 0;
            byte b;
            do
            {
                if (num2 == 70)
                {
                    throw new FormatException(Str.Obj.VarIntTooBig);
                }
                b = ReadUInt8();
                num |= ((long)(b & 0x7F)) << num2;
                num2 += 7;
            }
            while ((b & 0x80) != 0);
            return num;
        }

        /// <summary>
        /// 读取VarInt并转为32位无符号整数
        ///  | 
        /// Read varint and turn it to 32 bits unsigned integer
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public uint ReadUVarInt32(Endian _ = Endian.Null)
        {
            return (uint)ReadVarInt32();
        }

        /// <summary>
        /// 读取VarInt并转为64位整数
        ///  | 
        /// Read varint and turn it to 64 bits unsigned integer
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public ulong ReadUVarInt64(Endian _ = Endian.Null)
        {
            return (ulong)ReadVarInt64();
        }

        /// <summary>
        /// 读取ZigZag编码的VarInt并转为32位整数
        ///  | 
        /// Read varint (ZigZag Code) and turn it to 32 bits integer
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public int ReadZigZag32(Endian _ = Endian.Null)
        {
            uint n = (uint)ReadVarInt32();
            return (((int)(n << 31)) >> 31) ^ ((int)(n >> 1));
        }

        /// <summary>
        /// 读取ZigZag编码的VarInt并转为64位整数
        ///  | 
        /// Read varint (ZigZag Code) and turn it to 64 bits integer
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public long ReadZigZag64(Endian _ = Endian.Null)
        {
            ulong n = (ulong)ReadVarInt64();
            return ((long)(n >> 1)) ^ (-(long)(n & 0b1));
        }

        /// <summary>
        /// 读取32位浮点数
        ///  | 
        /// Read 32 bits floating-point number
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public float ReadSingle(Endian endian = Endian.Null) => ReadFloat32(endian);

        /// <summary>
        /// 读取32位浮点数
        ///  | 
        /// Read 32 bits floating-point number
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public float ReadFloat(Endian endian = Endian.Null) => ReadFloat32(endian);

        /// <summary>
        /// 读取32位浮点数
        ///  | 
        /// Read 32 bits floating-point number
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public unsafe float ReadFloat32(Endian endian = Endian.Null)
        {
            uint num = ReadUInt32(endian);
            return *(float*)&num;
        }

        /// <summary>
        /// 读取64位浮点数
        ///  | 
        /// Read 64 bits floating-point number
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public double ReadDouble(Endian endian = Endian.Null) => ReadFloat64(endian);

        /// <summary>
        /// 读取64位浮点数
        ///  | 
        /// Read 64 bits floating-point number
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public unsafe double ReadFloat64(Endian endian = Endian.Null)
        {
            ulong num = ReadUInt64(endian);
            return *(double*)&num;
        }

        /// <summary>
        /// 读取1字节布尔值
        ///  | 
        /// Read one byte boolean value
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public bool ReadBool(Endian _ = Endian.Null) => ReadBoolean();

        /// <summary>
        /// 读取1字节布尔值
        ///  | 
        /// Read one byte boolean value
        /// </summary>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public bool ReadBoolean(Endian _ = Endian.Null)
        {
            FillBuffer(1);
            return m_buffer[0] != 0;
        }

        /// <summary>
        /// 读取2字节Unicode字符
        ///  | 
        /// Read two byte Unicode Char
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public char ReadChar(Endian endian = Endian.Null)
        {
            return (char)ReadUInt16(endian);
        }

        /// <summary>
        /// 读取指定长度字节数组
        ///  | 
        /// Read byte[] by the length you set
        /// </summary>
        /// <param name="count">字节数组长度 | The length of byte[]</param>
        /// <param name="_">大端序或小端序（无效） | Big endian or small endian (invalid)</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public byte[] ReadBytes(int count, Endian _ = Endian.Null)
        {
            byte[] array = new byte[count];
            BaseStream.Read(array, 0, count);
            return array;
        }

        /// <summary>
        /// 读取指定长度字符串
        ///  | 
        /// Read string by the length you set
        /// </summary>
        /// <param name="count">字符串对应字节数组长度 | The length of byte[] for the string</param>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public string ReadString(int count, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = ReadBytes(count);
            if (endian == Endian.Small) Array.Reverse(ary);
            return Encode.GetString(ary);
        }

        /// <summary>
        /// 根据一个位于字符串前方的字节的字符串长度值读取字符串
        ///  | 
        /// Read string by the length (type is byte) before this string
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public string ReadStringByUInt8Head(Endian endian = Endian.Null)
        {
            return ReadString(ReadUInt8(), endian);
        }

        /// <summary>
        /// 根据一个位于字符串前方的16位无符号整数的字符串长度值读取字符串
        ///  | 
        /// Read string by the length (type is 16 bits unsigned integer) before this string
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public string ReadStringByUInt16Head(Endian endian = Endian.Null)
        {
            return ReadString(ReadUInt16(), endian);
        }

        public string ReadStringByInt16Head(Endian endian = Endian.Null)
        {
            return ReadString(ReadInt16(), endian);
        }

        /// <summary>
        /// 根据一个位于字符串前方的32位整数的字符串长度值读取字符串
        ///  | 
        /// Read string by the length (type is 32 bits integer) before this string
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public string ReadStringByInt32Head(Endian endian = Endian.Null)
        {
            return ReadString(ReadInt32(), endian);
        }

        /// <summary>
        /// 根据一个位于字符串前方的VarInt（随后被转为32位整数）的字符串长度值读取字符串
        ///  | 
        /// Read string by the length (type is VarInt and then turn to 32 bits integer) before this string
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public string ReadStringByVarInt32Head(Endian endian = Endian.Null)
        {
            return ReadString(ReadVarInt32(), endian);
        }

        /// <summary>
        /// 读取字符串直到读取到0x00为止
        ///  | 
        /// Read string until 0x00 has been read
        /// </summary>
        /// <param name="endian">大端序或小端序 | Big endian or small endian</param>
        /// <returns>读取到的内容 | Value which you read</returns>
        public string ReadStringByEmpty(Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = StringEndian;
            List<byte> bytes = new();
            byte tp;
            while (true)
            {
                if ((tp = ReadUInt8()) == 0)
                {
                    break;
                }
                bytes.Add(tp);
            }
            byte[] b = bytes.ToArray();
            if (endian == Endian.Small) Array.Reverse(b);
            return Encode.GetString(b);
        }

        public string GetStringByEmpty(long offset, Endian endian = Endian.Null)
        {
            long bak = BaseStream.Position;
            BaseStream.Position = offset;
            string ans = ReadStringByEmpty(endian);
            BaseStream.Position = bak;
            return ans;
        }

        //缓冲区操作
        /// <summary>
        /// 填充缓冲区
        ///  | 
        /// Fill the buffer
        /// </summary>
        /// <param name="numBytes">字节数 | The number of bytes</param>
        /// <exception cref="Exception">文件结尾 | End of file</exception>
        void FillBuffer(int numBytes)
        {
            int num = 0;
            int num2;
            if (numBytes == 1)
            {
                num2 = BaseStream.ReadByte();
                if (num2 == -1)
                {
                    throw new Exception(Constant.Str.Obj.EndOfFile);
                }
                m_buffer[0] = (byte)num2;
                return;
            }
            do
            {
                num2 = BaseStream.Read(m_buffer, num, numBytes - num);
                if (num2 == 0)
                {
                    throw new Exception(Str.Obj.EndOfFile);
                }
                num += num2;
            }
            while (num < numBytes);
        }

        //写入数据
        public void WriteSByte(sbyte value, Endian _ = Endian.Null)
        {
            BaseStream.WriteByte((byte)value);
        }

        public void WriteInt8(sbyte value, Endian _ = Endian.Null)
        {
            BaseStream.WriteByte((byte)value);
        }

        public void WriteShort(short value, Endian endian = Endian.Null) => WriteInt16(value, endian);

        public void WriteInt16(short value, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[1] = (byte)value;
                m_buffer[0] = (byte)(value >> 8);
            }
            else
            {
                m_buffer[0] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
            }
            BaseStream.Write(m_buffer, 0, 2);
        }

        public void WriteThreeByte(int value, Endian endian = Endian.Null) => WriteInt24(value, endian);

        public void WriteInt24(int value, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[2] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
                m_buffer[0] = (byte)(value >> 16);
            }
            else
            {
                m_buffer[0] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
                m_buffer[2] = (byte)(value >> 16);
            }
            BaseStream.Write(m_buffer, 0, 3);
        }

        public void WriteInt(int value, Endian endian = Endian.Null) => WriteInt32(value, endian);

        public void WriteInt32(int value, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[3] = (byte)value;
                m_buffer[2] = (byte)(value >> 8);
                m_buffer[1] = (byte)(value >> 16);
                m_buffer[0] = (byte)(value >> 24);
            }
            else
            {
                m_buffer[0] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
                m_buffer[2] = (byte)(value >> 16);
                m_buffer[3] = (byte)(value >> 24);
            }
            BaseStream.Write(m_buffer, 0, 4);
        }

        public void WriteLong(long value, Endian endian = Endian.Null) => WriteInt64(value, endian);

        public void WriteInt64(long value, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[7] = (byte)value;
                m_buffer[6] = (byte)(value >> 8);
                m_buffer[5] = (byte)(value >> 16);
                m_buffer[4] = (byte)(value >> 24);
                m_buffer[3] = (byte)(value >> 32);
                m_buffer[2] = (byte)(value >> 40);
                m_buffer[1] = (byte)(value >> 48);
                m_buffer[0] = (byte)(value >> 56);
            }
            else
            {
                m_buffer[0] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
                m_buffer[2] = (byte)(value >> 16);
                m_buffer[3] = (byte)(value >> 24);
                m_buffer[4] = (byte)(value >> 32);
                m_buffer[5] = (byte)(value >> 40);
                m_buffer[6] = (byte)(value >> 48);
                m_buffer[7] = (byte)(value >> 56);
            }
            BaseStream.Write(m_buffer, 0, 8);
        }

        public void WriteByte(byte value, Endian _ = Endian.Null)
        {
            BaseStream.WriteByte(value);
        }

        public void WriteUInt8(byte value, Endian _ = Endian.Null)
        {
            BaseStream.WriteByte(value);
        }

        public void WriteUShort(ushort value, Endian endian = Endian.Null) => WriteUInt16(value, endian);

        public void WriteUInt16(ushort value, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[1] = (byte)value;
                m_buffer[0] = (byte)(value >> 8);
            }
            else
            {
                m_buffer[0] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
            }
            BaseStream.Write(m_buffer, 0, 2);
        }

        public void WriteUThreeByte(uint value, Endian endian = Endian.Null) => WriteUInt24(value, endian);

        public void WriteUInt24(uint value, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[2] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
                m_buffer[0] = (byte)(value >> 16);
            }
            else
            {
                m_buffer[0] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
                m_buffer[2] = (byte)(value >> 16);
            }
            BaseStream.Write(m_buffer, 0, 3);
        }

        public void WriteUInt(uint value, Endian endian = Endian.Null) => WriteUInt32(value, endian);

        public void WriteUInt32(uint value, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[3] = (byte)value;
                m_buffer[2] = (byte)(value >> 8);
                m_buffer[1] = (byte)(value >> 16);
                m_buffer[0] = (byte)(value >> 24);
            }
            else
            {
                m_buffer[0] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
                m_buffer[2] = (byte)(value >> 16);
                m_buffer[3] = (byte)(value >> 24);
            }
            BaseStream.Write(m_buffer, 0, 4);
        }

        public void WriteULong(ulong value, Endian endian = Endian.Null) => WriteUInt64(value, endian);

        public void WriteUInt64(ulong value, Endian endian = Endian.Null)
        {
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[7] = (byte)value;
                m_buffer[6] = (byte)(value >> 8);
                m_buffer[5] = (byte)(value >> 16);
                m_buffer[4] = (byte)(value >> 24);
                m_buffer[3] = (byte)(value >> 32);
                m_buffer[2] = (byte)(value >> 40);
                m_buffer[1] = (byte)(value >> 48);
                m_buffer[0] = (byte)(value >> 56);
            }
            else
            {
                m_buffer[0] = (byte)value;
                m_buffer[1] = (byte)(value >> 8);
                m_buffer[2] = (byte)(value >> 16);
                m_buffer[3] = (byte)(value >> 24);
                m_buffer[4] = (byte)(value >> 32);
                m_buffer[5] = (byte)(value >> 40);
                m_buffer[6] = (byte)(value >> 48);
                m_buffer[7] = (byte)(value >> 56);
            }
            BaseStream.Write(m_buffer, 0, 8);
        }

        public void WriteVarInt32(int value, Endian _ = Endian.Null)
        {
            uint num;
            for (num = (uint)value; num >= 128; num >>= 7)
            {
                WriteByte((byte)(num | 0x80));
            }
            WriteByte((byte)num);
        }

        public void WriteVarInt64(long value, Endian _ = Endian.Null)
        {
            ulong num;
            for (num = (ulong)value; num >= 128; num >>= 7)
            {
                WriteByte((byte)(num | 0x80));
            }
            WriteByte((byte)num);
        }

        public void WriteUVarInt32(uint value, Endian _ = Endian.Null)
        {
            uint num;
            for (num = value; num >= 128; num >>= 7)
            {
                WriteByte((byte)(num | 0x80));
            }
            WriteByte((byte)num);
        }

        public void WriteUVarInt64(ulong value, Endian _ = Endian.Null)
        {
            ulong num;
            for (num = value; num >= 128; num >>= 7)
            {
                WriteByte((byte)(num | 0x80));
            }
            WriteByte((byte)num);
        }

        public void WriteZigZag32(int value, Endian _ = Endian.Null)
        {
            WriteVarInt32((value << 1) ^ (value >> 31));
        }

        public void WriteZigZag64(long value, Endian _ = Endian.Null)
        {
            WriteVarInt64((value << 1) ^ (value >> 63));
        }

        public void WriteSingle(float value, Endian endian = Endian.Null) => WriteFloat32(value, endian);

        public void WriteFloat(float value, Endian endian = Endian.Null) => WriteFloat32(value, endian);

        public unsafe void WriteFloat32(float value, Endian endian = Endian.Null)
        {
            uint num = *(uint*)&value;
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[3] = (byte)num;
                m_buffer[2] = (byte)(num >> 8);
                m_buffer[1] = (byte)(num >> 16);
                m_buffer[0] = (byte)(num >> 24);
            }
            else
            {
                m_buffer[0] = (byte)num;
                m_buffer[1] = (byte)(num >> 8);
                m_buffer[2] = (byte)(num >> 16);
                m_buffer[3] = (byte)(num >> 24);
            }
            BaseStream.Write(m_buffer, 0, 4);
        }

        public void WriteDouble(double value, Endian endian = Endian.Null) => WriteFloat64(value, endian);

        public unsafe void WriteFloat64(double value, Endian endian = Endian.Null)
        {
            ulong num = (ulong)*(long*)&value;
            if (endian == Endian.Null) endian = Endian;
            if (endian == Endian.Big)
            {
                m_buffer[7] = (byte)num;
                m_buffer[6] = (byte)(num >> 8);
                m_buffer[5] = (byte)(num >> 16);
                m_buffer[4] = (byte)(num >> 24);
                m_buffer[3] = (byte)(num >> 32);
                m_buffer[2] = (byte)(num >> 40);
                m_buffer[1] = (byte)(num >> 48);
                m_buffer[0] = (byte)(num >> 56);
            }
            else
            {
                m_buffer[0] = (byte)num;
                m_buffer[1] = (byte)(num >> 8);
                m_buffer[2] = (byte)(num >> 16);
                m_buffer[3] = (byte)(num >> 24);
                m_buffer[4] = (byte)(num >> 32);
                m_buffer[5] = (byte)(num >> 40);
                m_buffer[6] = (byte)(num >> 48);
                m_buffer[7] = (byte)(num >> 56);
            }
            BaseStream.Write(m_buffer, 0, 8);
        }

        public void WriteBool(bool value, Endian _ = Endian.Null) => WriteBoolean(value);

        public void WriteBoolean(bool value, Endian _ = Endian.Null)
        {
            m_buffer[0] = (byte)(value ? 1u : 0u);
            BaseStream.Write(m_buffer, 0, 1);
        }

        public void WriteChar(char value, Endian endian = Endian.Null)
        {
            WriteUInt16(value, endian);
        }

        public void WriteBytes(byte[] value, Endian _ = Endian.Null)
        {
            BaseStream.Write(value, 0, value.Length);
        }

        public void WriteString(string value, Endian endian = Endian.Null)
        {
            if (value == null) return;
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = Encode.GetBytes(value);
            if (endian == Endian.Small) Array.Reverse(ary);
            BaseStream.Write(ary, 0, ary.Length);
        }

        public void WriteString(string value, int length, Endian endian = Endian.Null)
        {
            if (value == null)
            {
                BaseStream.Write(new byte[length], 0, length);
                return;
            }
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = Encode.GetBytes(value);
            if (endian == Endian.Small) Array.Reverse(ary);
            byte[] t = new byte[length];
            if (ary.Length >= length)
            {
                Array.Copy(ary, 0, t, 0, length);
            }
            else
            {
                Array.Copy(ary, 0, t, 0, ary.Length);
            }
            BaseStream.Write(t, 0, length);
        }

        public void WriteStringByUInt8Head(string value, Endian endian = Endian.Null)
        {
            if (value == null)
            {
                WriteUInt8(0);
                return;
            }
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = Encode.GetBytes(value);
            if (endian == Endian.Small) Array.Reverse(ary);
            WriteUInt8((byte)ary.Length);
            BaseStream.Write(ary, 0, ary.Length);
        }

        public void WriteStringByUInt16Head(string value, Endian endian = Endian.Null)
        {
            if (value == null)
            {
                WriteUInt16(0);
                return;
            }
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = Encode.GetBytes(value);
            if (endian == Endian.Small) Array.Reverse(ary);
            WriteUInt16((ushort)ary.Length);
            BaseStream.Write(ary, 0, ary.Length);
        }

        public void WriteStringByInt16Head(string value, Endian endian = Endian.Null)
        {
            if (value == null)
            {
                WriteInt16(0);
                return;
            }
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = Encode.GetBytes(value);
            if (endian == Endian.Small) Array.Reverse(ary);
            WriteInt16((short)ary.Length);
            BaseStream.Write(ary, 0, ary.Length);
        }

        public void WriteStringByInt32Head(string value, Endian endian = Endian.Null)
        {
            if (value == null)
            {
                WriteInt32(0);
                return;
            }
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = Encode.GetBytes(value);
            if (endian == Endian.Small) Array.Reverse(ary);
            WriteInt32(ary.Length);
            BaseStream.Write(ary, 0, ary.Length);
        }

        public void WriteStringByVarInt32Head(string value, Endian endian = Endian.Null)
        {
            if (value == null)
            {
                WriteVarInt32(0);
                return;
            }
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = Encode.GetBytes(value);
            if (endian == Endian.Small) Array.Reverse(ary);
            WriteVarInt32(ary.Length);
            BaseStream.Write(ary, 0, ary.Length);
        }

        public void WriteStringByEmpty(string value, Endian endian = Endian.Null)
        {
            if (value == null)
            {
                WriteUInt8(0);
                return;
            }
            if (endian == Endian.Null) endian = StringEndian;
            byte[] ary = Encode.GetBytes(value);
            if (endian == Endian.Small) Array.Reverse(ary);
            BaseStream.Write(ary, 0, ary.Length);
            WriteUInt8(0);
        }

        //关闭及释放
        /// <summary>
        /// 关闭流
        ///  | 
        /// Close the stream
        /// </summary>
        public virtual void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// 释放流
        ///  | 
        /// Dispose the stream
        /// </summary>
        /// <param name="disposing">是否需要释放 | If need disposing</param>
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

        /// <summary>
        /// 释放流
        ///  | 
        /// Dispose the stream
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        public void CopyTo(Stream s)
        {
            byte[] array = new byte[81920];
            int count;
            while ((count = Read(array, 0, array.Length)) != 0)
            {
                s.Write(array, 0, count);
            }
        }

        public void CopyTo(Stream s, long Length)
        {
            byte[] array = new byte[81920];
            int count;
            int length2 = array.Length;
            long times = Length / length2;
            for (long i = 0; i < times; i++)
            {
                count = Read(array, 0, length2);
                if (count == 0) return;
                s.Write(array, 0, count);
            }
            length2 = (int)(Length % length2);
            if (length2 != 0)
            {
                Read(array, 0, length2);
                s.Write(array, 0, length2);
            }
        }

        public void CopyTo(Stream s, byte[] array)
        {
            array ??= new byte[81920];
            int count;
            while ((count = Read(array, 0, array.Length)) != 0)
            {
                s.Write(array, 0, count);
            }
        }

        public void CopyTo(Stream s, long Length, byte[] array)
        {
            array ??= new byte[81920];
            int count;
            int length2 = array.Length;
            long times = Length / length2;
            for (long i = 0; i < times; i++)
            {
                count = Read(array, 0, length2);
                if (count == 0) return;
                s.Write(array, 0, count);
            }
            length2 = (int)(Length % length2);
            if (length2 != 0)
            {
                Read(array, 0, length2);
                s.Write(array, 0, length2);
            }
        }

        public static implicit operator Stream(BinaryStream a)
        {
            return a.BaseStream;
        }
    }
}