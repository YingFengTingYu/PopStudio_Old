using PopStudio.GUILanguage.Languages;

namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        public static partial bool HiddenPermission() => false;

        public static partial bool HiddenFlyout() => true;

        static readonly string androidpath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath + "/setting.xml";
        public static partial string GetSettingPath() => androidpath;

        public static partial async Task<bool> CheckAndRequestPermissionAsync(this ContentPage page)
        {
            ReadWriteStoragePermission readwritepermission = new ReadWriteStoragePermission();
            PermissionStatus status = await readwritepermission.CheckStatusAsync();
            bool HavePermission = true;
            if (status != PermissionStatus.Granted)
            {
                //if (await page.DisplayAlert(MAUIStr.Obj.Permission_Title, MAUIStr.Obj.Permission_Request1, MAUIStr.Obj.Permission_GoTo, MAUIStr.Obj.Permission_Cancel))
                //{
                    HavePermission = (await readwritepermission.RequestAsync()) == PermissionStatus.Granted;
                //}
                //else
                //{
                //    HavePermission = false;
                //}
            }
            if (!HavePermission) return false;
#pragma warning disable CA1416
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R && !Android.OS.Environment.IsExternalStorageManager)
            {
                if (await page.DisplayAlert(MAUIStr.Obj.Permission_Title, MAUIStr.Obj.Permission_Request2, MAUIStr.Obj.Permission_GoTo, MAUIStr.Obj.Permission_Cancel))
                {
                    var bb = new Android.Content.Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission);
                    bb.SetFlags(Android.Content.ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(bb);
                }
            }
#pragma warning restore CA1416
            return true;
        }
    }

    public class ReadWriteStoragePermission : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new (string androidPermission, bool isRuntime)[2]
        {
            (Android.Manifest.Permission.ReadExternalStorage, true),
            (Android.Manifest.Permission.WriteExternalStorage, true)
        };
    }
}
