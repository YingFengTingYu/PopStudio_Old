namespace PopStudio.Constant
{
    internal static class Str
    {
        public static ILanguage Obj = new ZHCN();

        public static void SetLanguage(Language language)
        {
            switch (language)
            {
                case Language.ZHCN: Obj = new ZHCN(); break;
                case Language.ENUS: Obj = new ENUS(); break;
                default: throw new Exception("Language Not Supported!");
            }
        }
    }
}