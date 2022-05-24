using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PopStudio.Platform
{
    internal static class Permission
    {
        public static void OpenUrl(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
            }
        }

        public static string GetSettingPath() => "setting.xml";
    }
}