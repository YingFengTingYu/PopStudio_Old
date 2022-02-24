using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace PopStudio.MAUI
{
	public partial class Page_HomePage : ContentPage
	{
		public Page_HomePage()
		{
			InitializeComponent();
            label_ver.Text = "版本号";// + Class.Const.APPVERSION;
            label_notice.Text = "";// Class.Const.APPNOTICE;
#if ANDROID //安卓权限检查，要求授予文件访问权限即可
            CheckAndRequestPermissionAsync();
		}

        public async void CheckAndRequestPermissionAsync()
        {
            ReadWriteStoragePermission a = new();
            var status = await a.CheckStatusAsync();
            bool HavePermission = true;
            if (status != PermissionStatus.Granted)
            {
                if (await DisplayAlert("权限申请", "在Android6及以上系统版本中，请授予程序存储权限，否则程序将无权读写文件！", "前往授权", "取消"))
                {
                    HavePermission = (await a.RequestAsync()) == PermissionStatus.Granted;
                }
                else
                {
                    HavePermission = false;
                }
            }
            if (!HavePermission) return;
            try
            {
                File.Create("/sdcard/REo1cUFQKTE220kiFEmtjh7U1Lr3oS8S");
                File.Delete("/sdcard/REo1cUFQKTE220kiFEmtjh7U1Lr3oS8S");
            }
            catch (Exception)
            {
                //累死了，这个方法很重要一定要记住，我可不想再费劲几个小时写这东西
                if (await DisplayAlert("权限申请", "在Android11及以上系统版本中，请授予程序所有文件访问权限，否则程序将只能读写程序内部文件夹中的文件！", "前往授权", "取消"))
                {
                    var bb = new Android.Content.Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission);
                    bb.SetFlags(Android.Content.ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(bb);
                }
            }
        }

        public class ReadWriteStoragePermission : Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new (string androidPermission, bool isRuntime)[2]
            {
                (Android.Manifest.Permission.ReadExternalStorage, true),
                (Android.Manifest.Permission.WriteExternalStorage, true)
            };
#endif
        }
    }
}