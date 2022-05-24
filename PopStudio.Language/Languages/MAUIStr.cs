namespace PopStudio.Language.Languages
{
    internal class MAUIStr
    {
        static ILocalization obj;

        public static ILocalization Obj => obj ?? LoadLanguage();

        public static event Action OnLanguageChanged;

        public static ILocalization LoadLanguage()
        {
            bool eventCall = true;
            if (obj == null) eventCall = false;
            switch (Setting.AppLanguage)
            {
                case Constant.Language.ZHCN: obj = new MAUIZHCN(); break;
                case Constant.Language.ENUS: obj = new MAUIENUS(); break;
                default: throw new Exception("Language Not Supported!");
            }
            if (eventCall) OnLanguageChanged?.Invoke();
            return obj;
        }
    }
}
