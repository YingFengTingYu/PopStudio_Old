namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        public static partial void ModifyFullBar()
        {

        }
        public static partial bool HiddenPermission() => true;
        public static partial bool HiddenFlyout() => true;
        public static partial string GetSettingPath() => null;

        public static partial Task<bool> CheckAndRequestPermissionAsync(this ContentPage page) => null;
    }
}
