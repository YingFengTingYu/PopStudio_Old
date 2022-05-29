using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PopStudio.Platform
{
    internal static class Permission
    {
        public static void OpenUrl(string url)
        {
#if LINUXCONSOLE
            Process.Start("xdg-open", url);
#elif MACOSCONSOLE
            Process.Start("open", url);
#else
            Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
#endif
        }

        static string InternalPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "setting.xml");

        public static string GetSettingPath() => InternalPath;
    }
}