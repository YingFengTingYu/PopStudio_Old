namespace PopStudio.WPF
{
    internal static class Permission
    {
        public static Language GetDefaultLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.Name switch
            {
                "zh-CN" => Language.ZHCN,
                _ => Language.ENUS
            };
        }

        public static string GetSettingPath() => Path.GetFullPath(".\\setting.xml");
    }
}