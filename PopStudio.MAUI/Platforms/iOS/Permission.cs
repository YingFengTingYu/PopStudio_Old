namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        public static partial Language GetDefaultLanguage() => Language.ENUS;
        public static partial string GetSettingPath() => null;

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page) => null;
    }
}
