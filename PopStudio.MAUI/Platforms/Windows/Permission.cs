namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        public static partial bool HiddenFlyout() => false;
        static string windowspath = Path.GetFullPath("D:\\setting.xml");

        public static partial string GetSettingPath() => windowspath;

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page) => Task.FromResult(true);
    }
}
