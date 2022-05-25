namespace PopStudio.Platform
{
    internal static partial class Permission
    {
        public static partial void PlatformInit()
        {
        }
        public static partial bool HiddenPermission() => true;
        public static partial bool HiddenFlyout() => false;

        static readonly string macospath = $"{FileSystem.AppDataDirectory}/popstudio_setting.xml";

        public static partial string GetSettingPath() => macospath;

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page) => Task.FromResult(true);
    }
}
