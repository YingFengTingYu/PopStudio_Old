using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace PopStudio.RTON
{
    internal static class RTON
    {
        public static readonly string magic = "RTON";
        public static readonly int version = 0x1;
        public static readonly string EOF = "DONE";
        public static readonly StringPool R0x90 = new StringPool();
        public static readonly StringPool R0x92 = new StringPool();

        /// <summary>
        /// It's very slow while converting RESOURCES.RTON
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        public static void Decode(string inFile, string outFile)
        {
            R0x90.Clear();
            R0x92.Clear();
            using (FileStream stream = new FileStream(outFile, FileMode.Create))
            {
                using (Utf8JsonWriter sw = new Utf8JsonWriter(stream, new JsonWriterOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), Indented = true }))
                {
                    using (BinaryStream bs = BinaryStream.Open(inFile))
                    {
                        bs.IdString(magic);
                        bs.IdInt32(version);
                        ReadJObject(bs, sw);
                        bs.IdString(EOF);
                    }
                }
            }
            R0x90.Clear();
            R0x92.Clear();
        }

        public static void DecodeAndDecrypt(string inFile, string outFile)
        {
            R0x90.Clear();
            R0x92.Clear();
            //The key for Rijndael is the MD5 of the enterred key
            byte[] keybytes = Encoding.UTF8.GetBytes(BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Setting.RTONKey))).ToLower().Replace("-", ""));
            byte[] ivbytes = new byte[24];
            //The iv for Rijndael is part of the key for Rijndael
            Array.Copy(keybytes, 4, ivbytes, 0, 24);
            byte[] source;
            using (FileStream stream = new FileStream(outFile, FileMode.Create))
            {
                using (Utf8JsonWriter sw = new Utf8JsonWriter(stream, new JsonWriterOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), Indented = true }))
                {
                    using (BinaryStream bs = BinaryStream.Open(inFile))
                    {
                        bs.IdInt16(0x10);
                        source = bs.ReadBytes((int)(bs.Length - 2));
                    }
                    using (BinaryStream bs = new BinaryStream(RijndaelHelper.Decrypt(source, keybytes, ivbytes, new Org.BouncyCastle.Crypto.Paddings.ZeroBytePadding())))
                    {
                        bs.Position = 0;
                        bs.IdString(magic);
                        bs.IdInt32(version);
                        ReadJObject(bs, sw);
                        bs.IdString(EOF);
                    }
                }
            }
            R0x90.Clear();
            R0x92.Clear();
        }

        static void ReadJArray(BinaryStream bs, Utf8JsonWriter sw)
        {
            sw.WriteStartArray();
            string tempstring;
            bs.IdByte(0xFD);
            int number = bs.ReadVarInt32();
            for (int i = 0; i < number; i++)
            {
                byte type = bs.ReadByte();
                switch (type)
                {
                    case 0x0:
                        sw.WriteBooleanValue(false);
                        break;
                    case 0x1:
                        sw.WriteBooleanValue(true);
                        break;
                    case 0x8:
                        sw.WriteNumberValue(bs.ReadSByte());
                        break;
                    case 0x9:
                        sw.WriteNumberValue(0);
                        break;
                    case 0xA:
                        sw.WriteNumberValue(bs.ReadByte());
                        break;
                    case 0xB:
                        sw.WriteNumberValue(0);
                        break;
                    case 0x10:
                        sw.WriteNumberValue(bs.ReadInt16());
                        break;
                    case 0x11:
                        sw.WriteNumberValue(0);
                        break;
                    case 0x12:
                        sw.WriteNumberValue(bs.ReadUInt16());
                        break;
                    case 0x13:
                        sw.WriteNumberValue(0);
                        break;
                    case 0x20:
                        sw.WriteNumberValue(bs.ReadInt32());
                        break;
                    case 0x21:
                        sw.WriteNumberValue(0);
                        break;
                    case 0x22:
                        sw.WriteNumberValue(bs.ReadFloat32());
                        break;
                    case 0x23:
                        sw.WriteNumberValue(0F);
                        break;
                    case 0x24:
                        sw.WriteNumberValue(bs.ReadVarInt32());
                        break;
                    case 0x25:
                        sw.WriteNumberValue(bs.ReadZigZag32());
                        break;
                    case 0x26:
                        sw.WriteNumberValue(bs.ReadUInt32());
                        break;
                    case 0x27:
                        sw.WriteNumberValue(0U);
                        break;
                    case 0x28:
                        sw.WriteNumberValue(bs.ReadUVarInt32());
                        break;
                    case 0x40:
                        sw.WriteNumberValue(bs.ReadInt64());
                        break;
                    case 0x41:
                        sw.WriteNumberValue(0L);
                        break;
                    case 0x42:
                        sw.WriteNumberValue(bs.ReadFloat64());
                        break;
                    case 0x43:
                        sw.WriteNumberValue(0D);
                        break;
                    case 0x44:
                        sw.WriteNumberValue(bs.ReadVarInt64());
                        break;
                    case 0x45:
                        sw.WriteNumberValue(bs.ReadZigZag64());
                        break;
                    case 0x46:
                        sw.WriteNumberValue(bs.ReadUInt64());
                        break;
                    case 0x47:
                        sw.WriteNumberValue(0UL);
                        break;
                    case 0x48:
                        sw.WriteNumberValue(bs.ReadUVarInt64());
                        break;
                    case 0x81:
                        sw.WriteStringValue(bs.ReadStringByVarInt32Head());
                        break;
                    case 0x82:
                        bs.ReadVarInt32();
                        sw.WriteStringValue(bs.ReadStringByVarInt32Head());
                        break;
                    case 0x83:
                        bs.IdByte(0x03);
                        bs.ReadVarInt32();
                        tempstring = bs.ReadStringByVarInt32Head();
                        bs.ReadVarInt32();
                        sw.WriteStringValue($"RTID({bs.ReadStringByVarInt32Head()}@{tempstring})");
                        break;
                    case 0x84:
                        sw.WriteNullValue();
                        break;
                    case 0x85:
                        ReadJObject(bs, sw);
                        break;
                    case 0x86:
                        ReadJArray(bs, sw);
                        break;
                    case 0x90:
                        tempstring = bs.ReadStringByVarInt32Head();
                        R0x90.ThrowInPool(tempstring);
                        sw.WriteStringValue(tempstring);
                        break;
                    case 0x91:
                        sw.WriteStringValue(R0x90[bs.ReadVarInt32()].Value);
                        break;
                    case 0x92:
                        bs.ReadVarInt32();
                        tempstring = bs.ReadStringByVarInt32Head();
                        R0x92.ThrowInPool(tempstring);
                        sw.WriteStringValue(tempstring);
                        break;
                    case 0x93:
                        sw.WriteStringValue(R0x92[bs.ReadVarInt32()].Value);
                        break;
                    default:
                        throw new Exception(Str.Obj.TypeMisMatch);
                }
            }
            bs.IdByte(0xFE);
            sw.WriteEndArray();
        }

        static void ReadJObject(BinaryStream bs, Utf8JsonWriter sw)
        {
            sw.WriteStartObject();
            string tempstring;
            while (true)
            {
                //key
                byte type = bs.ReadByte();
                if (type == 0xFF)
                {
                    break;
                }
                switch (type)
                {
                    case 0x81:
                        sw.WritePropertyName(bs.ReadStringByVarInt32Head());
                        break;
                    case 0x82:
                        bs.ReadVarInt32();
                        sw.WritePropertyName(bs.ReadStringByVarInt32Head());
                        break;
                    case 0x83:
                        bs.IdByte(0x03);
                        bs.ReadVarInt32();
                        tempstring = bs.ReadStringByVarInt32Head();
                        bs.ReadVarInt32();
                        sw.WritePropertyName($"RTID({bs.ReadStringByVarInt32Head()}@{tempstring})");
                        break;
                    case 0x90:
                        tempstring = bs.ReadStringByVarInt32Head();
                        R0x90.ThrowInPool(tempstring);
                        sw.WritePropertyName(tempstring);
                        break;
                    case 0x91:
                        sw.WritePropertyName(R0x90[bs.ReadVarInt32()].Value);
                        break;
                    case 0x92:
                        bs.ReadVarInt32();
                        tempstring = bs.ReadStringByVarInt32Head();
                        R0x92.ThrowInPool(tempstring);
                        sw.WritePropertyName(tempstring);
                        break;
                    case 0x93:
                        sw.WritePropertyName(R0x92[bs.ReadVarInt32()].Value);
                        break;
                    default:
                        throw new Exception(Str.Obj.TypeMisMatch);
                }
                //value
                type = bs.ReadByte();
                switch (type)
                {
                    case 0x0:
                        sw.WriteBooleanValue(false);
                        break;
                    case 0x1:
                        sw.WriteBooleanValue(true);
                        break;
                    case 0x8:
                        sw.WriteNumberValue(bs.ReadSByte());
                        break;
                    case 0x9:
                        sw.WriteNumberValue(0);
                        break;
                    case 0xA:
                        sw.WriteNumberValue(bs.ReadByte());
                        break;
                    case 0xB:
                        sw.WriteNumberValue(0);
                        break;
                    case 0x10:
                        sw.WriteNumberValue(bs.ReadInt16());
                        break;
                    case 0x11:
                        sw.WriteNumberValue(0);
                        break;
                    case 0x12:
                        sw.WriteNumberValue(bs.ReadUInt16());
                        break;
                    case 0x13:
                        sw.WriteNumberValue(0);
                        break;
                    case 0x20:
                        sw.WriteNumberValue(bs.ReadInt32());
                        break;
                    case 0x21:
                        sw.WriteNumberValue(0);
                        break;
                    case 0x22:
                        sw.WriteNumberValue(bs.ReadFloat32());
                        break;
                    case 0x23:
                        sw.WriteNumberValue(0F);
                        break;
                    case 0x24:
                        sw.WriteNumberValue(bs.ReadVarInt32());
                        break;
                    case 0x25:
                        sw.WriteNumberValue(bs.ReadZigZag32());
                        break;
                    case 0x26:
                        sw.WriteNumberValue(bs.ReadUInt32());
                        break;
                    case 0x27:
                        sw.WriteNumberValue(0U);
                        break;
                    case 0x28:
                        sw.WriteNumberValue(bs.ReadUVarInt32());
                        break;
                    case 0x40:
                        sw.WriteNumberValue(bs.ReadInt64());
                        break;
                    case 0x41:
                        sw.WriteNumberValue(0L);
                        break;
                    case 0x42:
                        sw.WriteNumberValue(bs.ReadFloat64());
                        break;
                    case 0x43:
                        sw.WriteNumberValue(0D);
                        break;
                    case 0x44:
                        sw.WriteNumberValue(bs.ReadVarInt64());
                        break;
                    case 0x45:
                        sw.WriteNumberValue(bs.ReadZigZag64());
                        break;
                    case 0x46:
                        sw.WriteNumberValue(bs.ReadUInt64());
                        break;
                    case 0x47:
                        sw.WriteNumberValue(0UL);
                        break;
                    case 0x48:
                        sw.WriteNumberValue(bs.ReadUVarInt64());
                        break;
                    case 0x81:
                        sw.WriteStringValue(bs.ReadStringByVarInt32Head());
                        break;
                    case 0x82:
                        bs.ReadVarInt32();
                        sw.WriteStringValue(bs.ReadStringByVarInt32Head());
                        break;
                    case 0x83:
                        bs.IdByte(0x03);
                        bs.ReadVarInt32();
                        tempstring = bs.ReadStringByVarInt32Head();
                        bs.ReadVarInt32();
                        sw.WriteStringValue($"RTID({bs.ReadStringByVarInt32Head()}@{tempstring})");
                        break;
                    case 0x84:
                        sw.WriteNullValue();
                        break;
                    case 0x85:
                        ReadJObject(bs, sw);
                        break;
                    case 0x86:
                        ReadJArray(bs, sw);
                        break;
                    case 0x90:
                        tempstring = bs.ReadStringByVarInt32Head();
                        R0x90.ThrowInPool(tempstring);
                        sw.WriteStringValue(tempstring);
                        break;
                    case 0x91:
                        sw.WriteStringValue(R0x90[bs.ReadVarInt32()].Value);
                        break;
                    case 0x92:
                        bs.ReadVarInt32();
                        tempstring = bs.ReadStringByVarInt32Head();
                        R0x92.ThrowInPool(tempstring);
                        sw.WriteStringValue(tempstring);
                        break;
                    case 0x93:
                        sw.WriteStringValue(R0x92[bs.ReadVarInt32()].Value);
                        break;
                    default:
                        throw new Exception(Str.Obj.TypeMisMatch);
                }
            }
            sw.WriteEndObject();
        }

        public static void EncodeAndEncrypt(string inFile, string outFile)
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
            byte[] source;
            using (BinaryStream bs = new BinaryStream())
            {
                bs.WriteString(magic);
                bs.WriteInt32(version);
                WriteJObject(bs, root);
                bs.WriteString(EOF);
                bs.Position = 0;
                source = bs.ReadBytes((int)bs.Length);
            }
            //The key for Rijndael is the MD5 of the enterred key
            byte[] keybytes = Encoding.UTF8.GetBytes(BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Setting.RTONKey))).ToLower().Replace("-", ""));
            byte[] ivbytes = new byte[24];
            //The iv for Rijndael is part of the key for Rijndael
            Array.Copy(keybytes, 4, ivbytes, 0, 24);
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                bs.WriteInt16(0x10);
                bs.WriteBytes(RijndaelHelper.Encrypt(source, keybytes, ivbytes, new Org.BouncyCastle.Crypto.Paddings.ZeroBytePadding()));
            }
            R0x90.Clear();
            R0x92.Clear();
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

        static bool IsASCII(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] > 127) return false;
            }
            return true;
        }

        static void WriteJArray(BinaryStream bs, JsonElement json)
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

        static void WriteJObject(BinaryStream bs, JsonElement json)
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
