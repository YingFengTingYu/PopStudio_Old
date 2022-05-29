using PopStudio.Language.Languages;

namespace PopStudio.Platform
{
    internal static class ConsoleReader
    {
        static List<string> Buffer = new List<string>();

        public static void RegistArguments(params string[] args)
        {
            foreach (string str in args)
            {
                Buffer.Add(str);
            }
        }

        static string InternalReadString(string text1, string text2)
        {
            if (Buffer.Count > 0)
            {
                string o = Buffer[0];
                Buffer.RemoveAt(0);
                return o;
            }
            else
            {
                if (text2 != null) Console.WriteLine(text2);
                Console.WriteLine(text1);
                return Console.ReadLine();
            }
        }

        public static short ReadInt16(string text = null)
        {
            while (true)
            {
                try
                {
                    return Convert.ToInt16(InternalReadString(MAUIStr.Obj.Console_Reader_ReadInt16, text));
                }
                catch (Exception)
                {
                    Console.WriteLine(MAUIStr.Obj.Console_Reader_WrongInt16);
                }
            }
        }

        public static int ReadInt32(string text = null)
        {
            while (true)
            {
                try
                {
                    return Convert.ToInt32(InternalReadString(MAUIStr.Obj.Console_Reader_ReadInt32, text));
                }
                catch (Exception)
                {
                    Console.WriteLine(MAUIStr.Obj.Console_Reader_WrongInt32);
                }
            }
        }

        public static long ReadInt64(string text = null)
        {
            while (true)
            {
                try
                {
                    return Convert.ToInt64(InternalReadString(MAUIStr.Obj.Console_Reader_ReadInt64, text));
                }
                catch (Exception)
                {
                    Console.WriteLine(MAUIStr.Obj.Console_Reader_WrongInt64);
                }
            }
        }

        public static ushort ReadUInt16(string text = null)
        {
            while (true)
            {
                try
                {
                    return Convert.ToUInt16(InternalReadString(MAUIStr.Obj.Console_Reader_ReadUInt16, text));
                }
                catch (Exception)
                {
                    Console.WriteLine(MAUIStr.Obj.Console_Reader_WrongUInt16);
                }
            }
        }

        public static uint ReadUInt32(string text = null)
        {
            while (true)
            {
                try
                {
                    return Convert.ToUInt32(InternalReadString(MAUIStr.Obj.Console_Reader_ReadUInt32, text));
                }
                catch (Exception)
                {
                    Console.WriteLine(MAUIStr.Obj.Console_Reader_WrongUInt32);
                }
            }
        }

        public static ulong ReadUInt64(string text = null)
        {
            while (true)
            {
                try
                {
                    return Convert.ToUInt64(InternalReadString(MAUIStr.Obj.Console_Reader_ReadUInt64, text));
                }
                catch (Exception)
                {
                    Console.WriteLine(MAUIStr.Obj.Console_Reader_WrongUInt64);
                }
            }
        }

        public static bool ReadBoolean(string text = null)
        {
            return InternalReadString(MAUIStr.Obj.Console_Reader_ReadBoolean, text).ToUpper() == "T";
        }

        public static string ReadString(string text = null)
        {
            return InternalReadString(MAUIStr.Obj.Console_Reader_ReadString, text);
        }

        public static string ReadPath(string text = null)
        {
            string s = InternalReadString(MAUIStr.Obj.Console_Reader_ReadString, text);
            if (s.StartsWith('"') && s.Length >= 2) return s[1..^1];
            return s;
        }
    }
}
