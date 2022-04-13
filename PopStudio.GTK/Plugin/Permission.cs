using Gtk;
using System.Diagnostics;

namespace PopStudio.GTK
{
    internal static class Permission
    {
        public static void OpenUrl(string url)
        {
#if LINUX
            Process.Start("xdg-open", url);
#elif MACOS
            Process.Start("open", url);
#else
            Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
#endif
        }

        public static string GetSettingPath() => "setting.xml";

        public static void Restart()
        {
            Process.Start(Environment.ProcessPath);
            Environment.Exit(0);
        }
    }
}