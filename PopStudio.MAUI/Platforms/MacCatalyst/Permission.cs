namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        public static partial bool HiddenPermission() => true;
        public static partial bool HiddenFlyout() => false;
        static string macospath = "setting.xml";

        public static partial string GetSettingPath() => macospath;

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page) => Task.FromResult(true);
    }
}
