namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        public static partial Language GetDefaultLanguage();

        public static partial string GetSettingPath();

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page);
    }
}
