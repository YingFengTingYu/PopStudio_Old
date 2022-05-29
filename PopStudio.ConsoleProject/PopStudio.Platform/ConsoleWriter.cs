namespace PopStudio.Platform
{
    internal static class ConsoleWriter
    {
        public static void WriteLine(object o)
        {
            Console.WriteLine(o);
        }

        public static void WriteLine(string f, params object[] os)
        {
            Console.WriteLine(f, os);
        }

        public static void WriteErrorLine(object o)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(o);
            Console.ForegroundColor = currentForeColor;
        }

        public static void WriteErrorLine(string f, params object[] os)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(f, os);
            Console.ForegroundColor = currentForeColor;
        }

        public static void WriteWarningLine(object o)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(o);
            Console.ForegroundColor = currentForeColor;
        }

        public static void WriteWarningLine(string f, params object[] os)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(f, os);
            Console.ForegroundColor = currentForeColor;
        }

        public static void WriteSuccessLine(object o)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(o);
            Console.ForegroundColor = currentForeColor;
        }

        public static void WriteSuccessLine(string f, params object[] os)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(f, os);
            Console.ForegroundColor = currentForeColor;
        }
    }
}
