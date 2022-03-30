namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        public static partial bool HiddenFlyout();
        public static partial string GetSettingPath();

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page);
    }
}
