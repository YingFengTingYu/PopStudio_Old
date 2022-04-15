using System.Diagnostics;

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

        public static void OpenUrl(string url) => Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
    }
}