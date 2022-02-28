namespace PopStudio.MAUI
{
    internal static partial class Permission
    {
        static string androidpath = "/sdcard/PopStudio/setting.xml";

        public static partial string GetSettingPath() => androidpath;

        public static partial async Task<bool> CheckAndRequestPermissionAsync(this ContentPage page)
        {
            ReadWriteStoragePermission readwritepermission = new ReadWriteStoragePermission();
            PermissionStatus status = await readwritepermission.CheckStatusAsync();
            bool HavePermission = true;
            if (status != PermissionStatus.Granted)
            {
                if (await page.DisplayAlert("权限申请", "在Android6及以上系统版本中，请授予程序存储权限，否则程序将无权读写文件！", "前往授权", "取消"))
                {
                    HavePermission = (await readwritepermission.RequestAsync()) == PermissionStatus.Granted;
                }
                else
                {
                    HavePermission = false;
                }
            }
            if (!HavePermission) return false;
            try
            {
                File.Create("/sdcard/REo1cUFQKTE220kiFEmtjh7U1Lr3oS8S");
                File.Delete("/sdcard/REo1cUFQKTE220kiFEmtjh7U1Lr3oS8S");
            }
            catch (Exception)
            {
                if (await page.DisplayAlert("权限申请", "在Android11及以上系统版本中，请授予程序所有文件访问权限，否则程序将只能读写程序内部文件夹中的文件！", "前往授权", "取消"))
                {
                    var bb = new Android.Content.Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission);
                    bb.SetFlags(Android.Content.ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(bb);
                }
            }
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
