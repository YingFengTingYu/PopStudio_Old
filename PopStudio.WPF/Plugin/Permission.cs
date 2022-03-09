namespace PopStudio.WPF
{
    internal static class Permission
    {
        public static string GetSettingPath() => "setting.xml";

        public static void Restart()
        {
            System.Windows.Forms.Application.Restart();
            Environment.Exit(0);
        }
    }
}