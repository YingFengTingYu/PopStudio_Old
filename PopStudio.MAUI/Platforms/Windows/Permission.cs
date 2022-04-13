namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        public static partial bool HiddenFlyout() => false;
        static string windowspath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\setting.xml";

        public static partial string GetSettingPath() => windowspath;

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page) => Task.FromResult(true);
    }
}
