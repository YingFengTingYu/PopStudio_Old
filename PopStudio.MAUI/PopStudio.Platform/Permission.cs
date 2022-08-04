using Android.Content.Res;
using Microsoft.Maui.Platform;
using PopStudio.Language.Languages;

namespace PopStudio.Platform
{
    internal static class Permission
    {
        public static void PlatformInit()
        {
            #region Switch
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("BlueTrack", (h, v) =>
            {
                h.PlatformView.TrackTintList = ColorStateList.ValueOf(Color.FromUint(0xFFB9B9B9).ToPlatform());
            });
            #endregion
            #region Underline
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("BlueUnderline", (h, v) =>
            {
                h.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.CornflowerBlue.ToPlatform());
            });
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("BlueUnderline", (h, v) =>
            {
                h.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.CornflowerBlue.ToPlatform());
            });
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("BlueUnderline", (h, v) =>
            {
                h.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.CornflowerBlue.ToPlatform());
            });
            #endregion
        }

        private static readonly string androidpath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath + "/setting.xml";
        
        public static string GetSettingPath() => androidpath;

        public static async Task<bool> CheckPermissionAsync()
        {
            ReadWriteStoragePermission readwritepermission = new ReadWriteStoragePermission();
            PermissionStatus status = await readwritepermission.CheckStatusAsync();
            if (status != PermissionStatus.Granted) return false;
#pragma warning disable CA1416
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R && !Android.OS.Environment.IsExternalStorageManager) return false;
#pragma warning restore CA1416
            return true;
        }

        public static async Task<bool> CheckAndRequestPermissionAsync(this ContentPage page)
        {
            ReadWriteStoragePermission readwritepermission = new ReadWriteStoragePermission();
            PermissionStatus status = await readwritepermission.CheckStatusAsync();
            bool HavePermission = true;
            if (status != PermissionStatus.Granted)
            {
                HavePermission = (await readwritepermission.RequestAsync()) == PermissionStatus.Granted;
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

        private class ReadWriteStoragePermission : Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new (string androidPermission, bool isRuntime)[2]
            {
                (Android.Manifest.Permission.ReadExternalStorage, true),
                (Android.Manifest.Permission.WriteExternalStorage, true)
            };
        }

        public static Task<bool> OpenUrl(string url) => Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
    }
}
