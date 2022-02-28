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
            label_ver.Text = "版本号3.2";
            label_notice.Text = "1.更新设置功能；\n2.制作了更精美的文件选取器。";
            this.CheckAndRequestPermissionAsync();
			string settingxml = Permission.GetSettingPath();
			if (!File.Exists(settingxml))
            {
				Dir.NewDir(settingxml, false);
				Setting.SaveAsXml(settingxml);
            }
			Setting.LoadFromXml(settingxml);
		}
    }
}