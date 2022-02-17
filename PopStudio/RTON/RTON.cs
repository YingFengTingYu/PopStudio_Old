using System.Text.Json;
using System.Text.Json.Nodes;

namespace PopStudio.RTON
{
    internal class RTON
    {
        public static string magic = "RTON";
        public static int version = 0x1;
        public static string EOF = "DONE";
        public static StringPool R0x90 = new StringPool();
        public static StringPool R0x92 = new StringPool();

        public static void Decode(string inFile, string outFile)
        {
            R0x90.Clear();
            R0x92.Clear();
            string tempFile = null;
            try
            {
                using (Stream stream = new FileStream(outFile, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        using (BinaryStream bs = BinaryStream.Open(inFile))
                        {
                            bs.IdString(magic);
                            bs.IdInt32(version);
                            ReadJObject(bs, sw);
                            bs.IdString(EOF);
                        }
                    }
                    //stream.Position = 0;
                    //using (StreamReader sr = new StreamReader(stream))
                    //{

                    //}
                }
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
            R0x90.Clear();
            R0x92.Clear();
        }

        public static void ReadJArray(BinaryStream bs, StreamWriter sw, string space = "")
        {
            string space2 = space + "    ";
            bs.IdByte(0xFD);
            int number = bs.ReadVarInt32();
            sw.Write('[');
            for (int i = 0; i < number; i++)
            {
                if (i != 0) sw.Write(',');
                sw.Write('\n');
                sw.Write(space2);
                byte type = bs.ReadByte();
                if (type == 0x00)
                {
                    sw.Write("false");
                }
                else if (type == 0x01)
                {
                    sw.Write("true");
                }
                else if (type == 0x08)
                {
                    sw.Write(bs.ReadSByte());
                }
                else if (type == 0x09)
                {
                    sw.Write((sbyte)0);
                }
                else if (type == 0x0A)
                {
                    sw.Write(bs.ReadByte());
                }
                else if (type == 0x0B)
                {
                    sw.Write((byte)0);
                }
                else if (type == 0x10)
                {
                    sw.Write(bs.ReadInt16());
                }
                else if (type == 0x11)
                {
                    sw.Write((short)0);
                }
                else if (type == 0x12)
                {
                    sw.Write(bs.ReadUInt16());
                }
                else if (type == 0x13)
                {
                    sw.Write((ushort)0);
                }
                else if (type == 0x20)
                {
                    sw.Write(bs.ReadInt32());
                }
                else if (type == 0x21)
                {
                    sw.Write(0);
                }
                else if (type == 0x22)
                {
                    sw.Write(bs.ReadFloat32());
                }
                else if (type == 0x23)
                {
                    sw.Write(0F);
                }
                else if (type == 0x24)
                {
                    sw.Write(bs.ReadVarInt32());
                }
                else if (type == 0x25)
                {
                    sw.Write(bs.ReadZigZag32());
                }
                else if (type == 0x26)
                {
                    sw.Write(bs.ReadUInt32());
                }
                else if (type == 0x27)
                {
                    sw.Write(0U);
                }
                else if (type == 0x28)
                {
                    sw.Write(bs.ReadUVarInt32());
                }
                else if (type == 0x40)
                {
                    sw.Write(bs.ReadInt64());
                }
                else if (type == 0x41)
                {
                    sw.Write(0L);
                }
                else if (type == 0x42)
                {
                    sw.Write(bs.ReadFloat64());
                }
                else if (type == 0x43)
                {
                    sw.Write(0D);
                }
                else if (type == 0x44)
                {
                    sw.Write(bs.ReadVarInt64());
                }
                else if (type == 0x45)
                {
                    sw.Write(bs.ReadZigZag64());
                }
                else if (type == 0x46)
                {
                    sw.Write(bs.ReadUInt64());
                }
                else if (type == 0x47)
                {
                    sw.Write(0UL);
                }
                else if (type == 0x48)
                {
                    sw.Write(bs.ReadUVarInt64());
                }
                else if (type == 0x81)
                {
                    sw.Write('"');
                    sw.Write(bs.ReadStringByVarInt32Head());
                    sw.Write('"');
                }
                else if (type == 0x82)
                {
                    bs.ReadVarInt32();
                    sw.Write('"');
                    sw.Write(bs.ReadStringByVarInt32Head());
                    sw.Write('"');
                }
                else if (type == 0x83)
                {
                    bs.IdByte(0x03);
                    bs.ReadVarInt32();
                    string value = bs.ReadStringByVarInt32Head();
                    bs.ReadVarInt32();
                    sw.Write('"');
                    sw.Write("RTID(" + bs.ReadStringByVarInt32Head() + "@" + value + ")");
                    sw.Write('"');
                }
                else if (type == 0x84)
                {
                    sw.Write("null");
                }
                else if (type == 0x85)
                {
                    ReadJObject(bs, sw, space2);
                }
                else if (type == 0x86)
                {
                    ReadJArray(bs, sw, space2);
                }
                else if (type == 0x90)
                {
                    string value = bs.ReadStringByVarInt32Head();
                    R0x90.ThrowInPool(value);
                    sw.Write('"');
                    sw.Write(value);
                    sw.Write('"');
                }
                else if (type == 0x91)
                {
                    sw.Write('"');
                    sw.Write(R0x90[bs.ReadVarInt32()].Value);
                    sw.Write('"');
                }
                else if (type == 0x92)
                {
                    bs.ReadVarInt32();
                    string value = bs.ReadStringByVarInt32Head();
                    R0x92.ThrowInPool(value);
                    sw.Write('"');
                    sw.Write(value);
                    sw.Write('"');
                }
                else if (type == 0x93)
                {
                    sw.Write('"');
                    sw.Write(R0x92[bs.ReadVarInt32()].Value);
                    sw.Write('"');
                }
                else
                {
                    throw new Exception(Str.Obj.DataMisMatch);
                }
            }
            bs.IdByte(0xFE);
            if (number != 0)
            {
                sw.Write('\n');
                sw.Write(space);
            }
            sw.Write(']');
        }

        public static void ReadJObject(BinaryStream bs, StreamWriter sw, string space = "")
        {
            string space2 = space + "    ";
            sw.Write('{');
            bool first = true;
            while (true)
            {
                //key
                byte type = bs.ReadByte();
                if (type == 0xFF)
                {
                    if (!first)
                    {
                        sw.Write('\n');
                        sw.Write(space);
                    }
                    sw.Write('}');
                    return;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sw.Write(',');
                }
                sw.Write('\n');
                sw.Write(space2);
                if (type == 0x81)
                {
                    sw.Write('"');
                    sw.Write(bs.ReadStringByVarInt32Head().Replace("\\", "\\\\").Replace("\"", "\\\""));
                    sw.Write('"');
                    sw.Write(':');
                }
                else if (type == 0x82)
                {
                    bs.ReadVarInt32();
                    sw.Write('"');
                    sw.Write(bs.ReadStringByVarInt32Head().Replace("\\", "\\\\").Replace("\"", "\\\""));
                    sw.Write('"');
                    sw.Write(':');
                }
                else if (type == 0x83)
                {
                    bs.IdByte(0x03);
                    bs.ReadVarInt32();
                    string key = bs.ReadStringByVarInt32Head();
                    bs.ReadVarInt32();
                    sw.Write('"');
                    sw.Write(("RTID(" + bs.ReadStringByVarInt32Head() + "@" + key + ")").Replace("\\", "\\\\").Replace("\"", "\\\""));
                    sw.Write('"');
                    sw.Write(':');
                }
                else if (type == 0x90)
                {
                    string key = bs.ReadStringByVarInt32Head().Replace("\\", "\\\\").Replace("\"", "\\\"");
                    R0x90.ThrowInPool(key);
                    sw.Write('"');
                    sw.Write(key);
                    sw.Write('"');
                    sw.Write(':');
                }
                else if (type == 0x91)
                {
                    sw.Write('"');
                    sw.Write(R0x90[bs.ReadVarInt32()].Value);
                    sw.Write('"');
                    sw.Write(':');
                }
                else if (type == 0x92)
                {
                    bs.ReadVarInt32();
                    string key = bs.ReadStringByVarInt32Head().Replace("\\", "\\\\").Replace("\"", "\\\"");
                    R0x92.ThrowInPool(key);
                    sw.Write('"');
                    sw.Write(key);
                    sw.Write('"');
                    sw.Write(':');
                }
                else if (type == 0x93)
                {
                    sw.Write('"');
                    sw.Write(R0x92[bs.ReadVarInt32()].Value);
                    sw.Write('"');
                    sw.Write(':');
                }
                else
                {
                    throw new Exception(Str.Obj.DataMisMatch);
                }
                //value
                type = bs.ReadByte();
                if (type == 0x00)
                {
                    sw.Write("false");
                }
                else if (type == 0x01)
                {
                    sw.Write("true");
                }
                else if (type == 0x08)
                {
                    sw.Write(bs.ReadSByte());
                }
                else if (type == 0x09)
                {
                    sw.Write((sbyte)0);
                }
                else if (type == 0x0A)
                {
                    sw.Write(bs.ReadByte());
                }
                else if (type == 0x0B)
                {
                    sw.Write((byte)0);
                }
                else if (type == 0x10)
                {
                    sw.Write(bs.ReadInt16());
                }
                else if (type == 0x11)
                {
                    sw.Write((short)0);
                }
                else if (type == 0x12)
                {
                    sw.Write(bs.ReadUInt16());
                }
                else if (type == 0x13)
                {
                    sw.Write((ushort)0);
                }
                else if (type == 0x20)
                {
                    sw.Write(bs.ReadInt32());
                }
                else if (type == 0x21)
                {
                    sw.Write(0);
                }
                else if (type == 0x22)
                {
                    sw.Write(bs.ReadFloat32());
                }
                else if (type == 0x23)
                {
                    sw.Write(0F);
                }
                else if (type == 0x24)
                {
                    sw.Write(bs.ReadVarInt32());
                }
                else if (type == 0x25)
                {
                    sw.Write(bs.ReadZigZag32());
                }
                else if (type == 0x26)
                {
                    sw.Write(bs.ReadUInt32());
                }
                else if (type == 0x27)
                {
                    sw.Write(0U);
                }
                else if (type == 0x28)
                {
                    sw.Write(bs.ReadUVarInt32());
                }
                else if (type == 0x40)
                {
                    sw.Write(bs.ReadInt64());
                }
                else if (type == 0x41)
                {
                    sw.Write(0L);
                }
                else if (type == 0x42)
                {
                    sw.Write(bs.ReadFloat64());
                }
                else if (type == 0x43)
                {
                    sw.Write(0D);
                }
                else if (type == 0x44)
                {
                    sw.Write(bs.ReadVarInt64());
                }
                else if (type == 0x45)
                {
                    sw.Write(bs.ReadZigZag64());
                }
                else if (type == 0x46)
                {
                    sw.Write(bs.ReadUInt64());
                }
                else if (type == 0x47)
                {
                    sw.Write(0UL);
                }
                else if (type == 0x48)
                {
                    sw.Write(bs.ReadUVarInt64());
                }
                else if (type == 0x81)
                {
                    sw.Write('"');
                    sw.Write(bs.ReadStringByVarInt32Head().Replace("\\", "\\\\").Replace("\"", "\\\""));
                    sw.Write('"');
                }
                else if (type == 0x82)
                {
                    bs.ReadVarInt32();
                    sw.Write('"');
                    sw.Write(bs.ReadStringByVarInt32Head().Replace("\\", "\\\\").Replace("\"", "\\\""));
                    sw.Write('"');
                }
                else if (type == 0x83)
                {
                    bs.IdByte(0x03);
                    bs.ReadVarInt32();
                    string value = bs.ReadStringByVarInt32Head();
                    bs.ReadVarInt32();
                    sw.Write('"');
                    sw.Write(("RTID(" + bs.ReadStringByVarInt32Head() + "@" + value + ")").Replace("\\", "\\\\").Replace("\"", "\\\""));
                    sw.Write('"');
                }
                else if (type == 0x84)
                {
                    sw.Write("null");
                }
                else if (type == 0x85)
                {
                    ReadJObject(bs, sw, space2);
                }
                else if (type == 0x86)
                {
                    ReadJArray(bs, sw, space2);
                }
                else if (type == 0x90)
                {
                    string value = bs.ReadStringByVarInt32Head().Replace("\\", "\\\\").Replace("\"", "\\\"");
                    R0x90.ThrowInPool(value);
                    sw.Write('"');
                    sw.Write(value);
                    sw.Write('"');
                }
                else if (type == 0x91)
                {
                    sw.Write('"');
                    sw.Write(R0x90[bs.ReadVarInt32()].Value);
                    sw.Write('"');
                }
                else if (type == 0x92)
                {
                    bs.ReadVarInt32();
                    string value = bs.ReadStringByVarInt32Head().Replace("\\", "\\\\").Replace("\"", "\\\"");
                    R0x92.ThrowInPool(value);
                    sw.Write('"');
                    sw.Write(value);
                    sw.Write('"');
                }
                else if (type == 0x93)
                {
                    sw.Write('"');
                    sw.Write(R0x92[bs.ReadVarInt32()].Value);
                    sw.Write('"');
                }
                else
                {
                    throw new Exception(Str.Obj.DataMisMatch);
                }
            }
        }

        public static void Encode(string inFile, string outFile)
        {
            R0x90.Clear();
            R0x92.Clear();
            string jsondata;
            using (StreamReader sr = new StreamReader(inFile))
            {
                jsondata = sr.ReadToEnd();
            }
            using JsonDocument json = JsonDocument.Parse(jsondata);
            JsonElement root = json.RootElement;
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                bs.WriteString(magic);
                bs.WriteInt32(version);
                WriteJObject(bs, root);
                bs.WriteString(EOF);
            }
            R0x90.Clear();
            R0x92.Clear();
        }

        public static bool IsASCII(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] > 127) return false;
            }
            return true;
        }

        public static void WriteJArray(BinaryStream bs, JsonElement json)
        {
            bs.WriteByte(0xFD);
            int n = json.GetArrayLength();
            bs.WriteVarInt32(n);
            for (int i = 0; i < n; i++)
            {
                JsonElement value = json[i];
                switch (value.ValueKind)
                {
                    case JsonValueKind.Object:
                        bs.WriteByte(0x85);
                        WriteJObject(bs, value);
                        break;
                    case JsonValueKind.Array:
                        bs.WriteByte(0x86);
                        WriteJArray(bs, value);
                        break;
                    case JsonValueKind.Undefined:
                    case JsonValueKind.Null:
                        bs.WriteByte(0x84);
                        break;
                    case JsonValueKind.True:
                        bs.WriteByte(0x01);
                        break;
                    case JsonValueKind.False:
                        bs.WriteByte(0x00);
                        break;
                    case JsonValueKind.String:
                        string str = value.GetString();
                        if (str.Length > 6 && str[..5] == "RTID(" && str[^1] == ')' && str.IndexOf('@') != -1)
                        {
                            //83rton
                            var ary = str[5..^1].Split('@');
                            bs.WriteByte(0x83);
                            bs.WriteByte(0x03);
                            bs.WriteVarInt32(ary[1].Length);
                            bs.WriteStringByVarInt32Head(ary[1]);
                            bs.WriteVarInt32(ary[0].Length);
                            bs.WriteStringByVarInt32Head(ary[0]);
                        }
                        else if (IsASCII(str))
                        {
                            //9091rton
                            if (R0x90.Exist(str))
                            {
                                //91
                                bs.WriteByte(0x91);
                                bs.WriteVarInt32(R0x90[str].Index);
                            }
                            else
                            {
                                //90
                                bs.WriteByte(0x90);
                                bs.WriteStringByVarInt32Head(str);
                                R0x90.ThrowInPool(str);
                            }
                        }
                        else
                        {
                            //9293rton
                            if (R0x92.Exist(str))
                            {
                                //93
                                bs.WriteByte(0x93);
                                bs.WriteVarInt32(R0x92[str].Index);
                            }
                            else
                            {
                                //92
                                bs.WriteByte(0x92);
                                bs.WriteVarInt32(str.Length);
                                bs.WriteStringByVarInt32Head(str);
                                R0x92.ThrowInPool(str);
                            }
                        }
                        break;
                    case JsonValueKind.Number:
                        if (value.GetRawText().IndexOf('.') > -1)
                        {
                            //is float number
                            double F64Val = value.GetDouble();
                            if (F64Val == 0.0)
                            {
                                bs.WriteByte(0x23);
                            }
                            else
                            {
                                float F32Val = value.GetSingle();
                                if (((double)F32Val) == F64Val)
                                {
                                    bs.WriteByte(0x22);
                                    bs.WriteFloat32(F32Val);
                                }
                                else
                                {
                                    bs.WriteByte(0x42);
                                    bs.WriteFloat64(F64Val);
                                }
                            }
                        }
                        else
                        {
                            //is varint number
                            long I64Val = value.GetInt64();
                            if (I64Val == 0)
                            {
                                bs.WriteByte(0x21);
                            }
                            else if (I64Val > 0)
                            {
                                if (I64Val <= int.MaxValue)
                                {
                                    //rton24
                                    bs.WriteByte(0x24);
                                    bs.WriteVarInt32((int)I64Val);
                                }
                                else
                                {
                                    //rton44
                                    bs.WriteByte(0x44);
                                    bs.WriteVarInt64(I64Val);
                                }
                            }
                            else
                            {
                                if (I64Val + (1 << 30) >= 0)
                                {
                                    bs.WriteByte(0x25);
                                    bs.WriteZigZag32((int)I64Val);
                                }
                                else
                                {
                                    bs.WriteByte(0x45);
                                    bs.WriteZigZag64(I64Val);
                                }
                            }
                        }
                        break;
                    default:
                        throw new Exception(Str.Obj.UnknownFormat);
                }
            }
            bs.WriteByte(0xFE);
        }

        public static void WriteJObject(BinaryStream bs, JsonElement json)
        {
            foreach (JsonProperty property in json.EnumerateObject())
            {
                //key
                string key = property.Name;
                if (key.Length > 6 && key[..5] == "RTID(" && key[^1] == ')' && key.IndexOf('@') != -1)
                {
                    //83rton
                    var ary = key[5..^1].Split('@');
                    bs.WriteByte(0x83);
                    bs.WriteByte(0x03);
                    bs.WriteVarInt32(ary[1].Length);
                    bs.WriteStringByVarInt32Head(ary[1]);
                    bs.WriteVarInt32(ary[0].Length);
                    bs.WriteStringByVarInt32Head(ary[0]);
                }
                else if (IsASCII(key))
                {
                    //9091rton
                    if (R0x90.Exist(key))
                    {
                        //91
                        bs.WriteByte(0x91);
                        bs.WriteVarInt32(R0x90[key].Index);
                    }
                    else
                    {
                        //90
                        bs.WriteByte(0x90);
                        bs.WriteStringByVarInt32Head(key);
                        R0x90.ThrowInPool(key);
                    }
                }
                else
                {
                    //9293rton
                    if (R0x92.Exist(key))
                    {
                        //93
                        bs.WriteByte(0x93);
                        bs.WriteVarInt32(R0x92[key].Index);
                    }
                    else
                    {
                        //92
                        bs.WriteByte(0x92);
                        bs.WriteVarInt32(key.Length);
                        bs.WriteStringByVarInt32Head(key);
                        R0x92.ThrowInPool(key);
                    }
                }
                JsonElement value = property.Value;
                switch (value.ValueKind)
                {
                    case JsonValueKind.Object:
                        bs.WriteByte(0x85);
                        WriteJObject(bs, value);
                        break;
                    case JsonValueKind.Array:
                        bs.WriteByte(0x86);
                        WriteJArray(bs, value);
                        break;
                    case JsonValueKind.Undefined:
                    case JsonValueKind.Null:
                        bs.WriteByte(0x84);
                        break;
                    case JsonValueKind.True:
                        bs.WriteByte(0x01);
                        break;
                    case JsonValueKind.False:
                        bs.WriteByte(0x00);
                        break;
                    case JsonValueKind.String:
                        string str = value.GetString();
                        if (str.Length > 6 && str[..5] == "RTID(" && str[^1] == ')' && str.IndexOf('@') != -1)
                        {
                            //83rton
                            var ary = str[5..^1].Split('@');
                            bs.WriteByte(0x83);
                            bs.WriteByte(0x03);
                            bs.WriteVarInt32(ary[1].Length);
                            bs.WriteStringByVarInt32Head(ary[1]);
                            bs.WriteVarInt32(ary[0].Length);
                            bs.WriteStringByVarInt32Head(ary[0]);
                        }
                        else if (IsASCII(str))
                        {
                            //9091rton
                            if (R0x90.Exist(str))
                            {
                                //91
                                bs.WriteByte(0x91);
                                bs.WriteVarInt32(R0x90[str].Index);
                            }
                            else
                            {
                                //90
                                bs.WriteByte(0x90);
                                bs.WriteStringByVarInt32Head(str);
                                R0x90.ThrowInPool(str);
                            }
                        }
                        else
                        {
                            //9293rton
                            if (R0x92.Exist(str))
                            {
                                //93
                                bs.WriteByte(0x93);
                                bs.WriteVarInt32(R0x92[str].Index);
                            }
                            else
                            {
                                //92
                                bs.WriteByte(0x92);
                                bs.WriteVarInt32(str.Length);
                                bs.WriteStringByVarInt32Head(str);
                                R0x92.ThrowInPool(str);
                            }
                        }
                        break;
                    case JsonValueKind.Number:
                        if (value.GetRawText().IndexOf('.') > -1)
                        {
                            //is float number
                            double F64Val = value.GetDouble();
                            if (F64Val == 0.0)
                            {
                                bs.WriteByte(0x23);
                            }
                            else
                            {
                                float F32Val = value.GetSingle();
                                if (((double)F32Val) == F64Val)
                                {
                                    bs.WriteByte(0x22);
                                    bs.WriteFloat32(F32Val);
                                }
                                else
                                {
                                    bs.WriteByte(0x42);
                                    bs.WriteFloat64(F64Val);
                                }
                            }
                        }
                        else
                        {
                            //is varint number
                            long I64Val = value.GetInt64();
                            if (I64Val == 0)
                            {
                                bs.WriteByte(0x21);
                            }
                            else if (I64Val > 0)
                            {
                                if (I64Val <= int.MaxValue)
                                {
                                    //rton24
                                    bs.WriteByte(0x24);
                                    bs.WriteVarInt32((int)I64Val);
                                }
                                else
                                {
                                    //rton44
                                    bs.WriteByte(0x44);
                                    bs.WriteVarInt64(I64Val);
                                }
                            }
                            else
                            {
                                if (I64Val + (1 << 30) >= 0)
                                {
                                    bs.WriteByte(0x25);
                                    bs.WriteZigZag32((int)I64Val);
                                }
                                else
                                {
                                    bs.WriteByte(0x45);
                                    bs.WriteZigZag64(I64Val);
                                }
                            }
                        }
                        break;
                    default:
                        throw new Exception(Str.Obj.UnknownFormat);
                }
            }
            bs.WriteByte(0xFF);
        }
    }
}
