using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopStudio.Plugin
{
    internal static partial class API
    {
        public static partial void Print(params object[] os)
        {
            string str = string.Empty;
            if (os.Length != 0)
            {
                string nil = "nil";
                for (int i = 0; i < os.Length; i++)
                {
                    str += ((os[i]?.ToString()) ?? nil) + ' ';
                }
                str = str[0..^1];
            }
            Console.WriteLine(str);
        }

        public static partial bool? Alert(string text, string title, bool ask)
        {
            if (ask)
            {
                Console.WriteLine(title);
                Console.WriteLine(text);
                Console.WriteLine("Press y to accept and any other key to cancel...");
                char c = Console.ReadKey().KeyChar;
                if (c == 'y' || c == 'Y')
                {
                    return true;
                }
                return false;
            }
            else
            {
                Console.WriteLine(title);
                Console.WriteLine(text);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return null;
            }
        }

        public static partial string Prompt(string text, string title, string defaulttext)
        {
            Console.WriteLine(title);
            Console.WriteLine(text);
            return Console.ReadLine();
        }

        public static partial string Sheet(string title, params string[] items)
        {
            Console.WriteLine(title);
            int length = items.Length;
            Dictionary<string, string> tempdic = new Dictionary<string, string>();
            for (int i = 0; i < length; i++)
            {
                tempdic.Add(i.ToString(), items[i]);
                Console.WriteLine("Enter {0} to select option {1}", i, items[i]);
            }
            string info = Console.ReadLine();
            if (!tempdic.ContainsKey(info))
            {
                return null;
            }
            return tempdic[info];
        }

        public static partial string ChooseFolder()
        {
            Console.WriteLine("Please enter the path of folder...");
            return Console.ReadLine();
        }

        public static partial string ChooseOpenFile()
        {
            Console.WriteLine("Please enter the path of file to open...");
            return Console.ReadLine();
        }

        public static partial string ChooseSaveFile()
        {
            Console.WriteLine("Please enter the path of file to save...");
            return Console.ReadLine();
        }
    }
}

