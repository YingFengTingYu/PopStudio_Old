namespace PopStudio.Platform
{
    internal static partial class Permission
    {
        public static partial void PlatformInit();
        public static partial bool HiddenPermission();
        public static partial bool HiddenFlyout();
        public static partial string GetSettingPath();

        public static partial Task<bool> CheckPermissionAsync();

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page);

        public static Task<bool> OpenUrl(string url) => Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
    }
}