namespace PopStudio.Constant
{
    internal static class Str
    {
        static ILanguage obj;

        public static ILanguage Obj => obj ?? LoadLanguage();

        static ILanguage LoadLanguage()
        {
            switch (Setting.AppLanguage)
            {
                case Language.ZHCN: return obj = new ZHCN();
                case Language.ENUS: return obj = new ENUS();
                default: throw new Exception("Language Not Supported!");
            }
        }

        public static void SetLanguage(Language language)
        {
            switch (language)
            {
                case Language.ZHCN: obj = new ZHCN(); break;
                case Language.ENUS: obj = new ENUS(); break;
                default: throw new Exception("Language Not Supported!");
            }
        }
    }
}