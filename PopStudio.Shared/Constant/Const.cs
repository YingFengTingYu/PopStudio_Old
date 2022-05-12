namespace PopStudio.Constant
{
    internal class Const
    {
#if ANDROID
        public const string PATHSEPARATOR = @"/";
        public const string UNUSEDPATHSEPARATOR = @"\";
        public const string EMPTYPATHSEPARATOR = @" /";
        public const string EMPTYUNUSEDPATHSEPARATOR = @" \";
        public const string DOUBLEPATHSEPARATOR = @"//";
        public const string DOUBLEUNUSEDPATHSEPARATOR = @"\\";
        public const int SYSTEM = 0b00000010;
#elif LINUX
        public const string PATHSEPARATOR = @"/";
        public const string UNUSEDPATHSEPARATOR = @"\";
        public const string EMPTYPATHSEPARATOR = @" /";
        public const string EMPTYUNUSEDPATHSEPARATOR = @" \";
        public const string DOUBLEPATHSEPARATOR = @"//";
        public const string DOUBLEUNUSEDPATHSEPARATOR = @"\\";
        public const int SYSTEM = 0b00000100;
#elif MACOS
        public const string PATHSEPARATOR = @"/";
        public const string UNUSEDPATHSEPARATOR = @"\";
        public const string EMPTYPATHSEPARATOR = @" /";
        public const string EMPTYUNUSEDPATHSEPARATOR = @" \";
        public const string DOUBLEPATHSEPARATOR = @"//";
        public const string DOUBLEUNUSEDPATHSEPARATOR = @"\\";
        public const int SYSTEM = 0b00001000;
#elif ANDROIDCONSOLE
        public const string PATHSEPARATOR = @"/";
        public const string UNUSEDPATHSEPARATOR = @"\";
        public const string EMPTYPATHSEPARATOR = @" /";
        public const string EMPTYUNUSEDPATHSEPARATOR = @" \";
        public const string DOUBLEPATHSEPARATOR = @"//";
        public const string DOUBLEUNUSEDPATHSEPARATOR = @"\\";
        public const int SYSTEM = 0b10000010;
#elif LINUXCONSOLE
        public const string PATHSEPARATOR = @"/";
        public const string UNUSEDPATHSEPARATOR = @"\";
        public const string EMPTYPATHSEPARATOR = @" /";
        public const string EMPTYUNUSEDPATHSEPARATOR = @" \";
        public const string DOUBLEPATHSEPARATOR = @"//";
        public const string DOUBLEUNUSEDPATHSEPARATOR = @"\\";
        public const int SYSTEM = 0b10000100;
#elif WINDOWSCONSOLE
        public const string PATHSEPARATOR = @"\";
        public const string UNUSEDPATHSEPARATOR = @"/";
        public const string EMPTYPATHSEPARATOR = @" \";
        public const string EMPTYUNUSEDPATHSEPARATOR = @" /";
        public const string DOUBLEPATHSEPARATOR = @"\\";
        public const string DOUBLEUNUSEDPATHSEPARATOR = @"//";
        public const int SYSTEM = 0b10000001;
#elif MACOSCONSOLE
        public const string PATHSEPARATOR = @"/";
        public const string UNUSEDPATHSEPARATOR = @"\";
        public const string EMPTYPATHSEPARATOR = @" /";
        public const string EMPTYUNUSEDPATHSEPARATOR = @" \";
        public const string DOUBLEPATHSEPARATOR = @"//";
        public const string DOUBLEUNUSEDPATHSEPARATOR = @"\\";
        public const int SYSTEM = 0b10001000;
#else
        public const string PATHSEPARATOR = @"\";
        public const string UNUSEDPATHSEPARATOR = @"/";
        public const string EMPTYPATHSEPARATOR = @" \";
        public const string EMPTYUNUSEDPATHSEPARATOR = @" /";
        public const string DOUBLEPATHSEPARATOR = @"\\";
        public const string DOUBLEUNUSEDPATHSEPARATOR = @"//";
        public const int SYSTEM = 0b00000001;
#endif
        public const int RAINYVERSION = 15;
    }
}