using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace PopStudio.MAUI
{
	public partial class Page_Package : ContentPage
	{
		public Page_Package()
		{
			InitializeComponent();
			CB_CMode.Items.Clear();
			CB_CMode.Items.Add("dz");
			CB_CMode.Items.Add("rsb");
			CB_CMode.Items.Add("pak");
			CB_CMode.SelectedIndex = 0;
		}

		public void Do(object sender, EventArgs e)
        {
			Button b = (Button)sender;
			b.IsEnabled = false;
			bool mode = switchmode.IsToggled;
			string inFile = textbox1.Text;
			string outFile = textbox2.Text;
			int pmode = CB_CMode.SelectedIndex;
			bool c1 = switchchange1.IsToggled;
			bool c2 = switchchange2.IsToggled;
			new Thread(new ThreadStart(() =>
			{
				string err = null;
				try
				{
					if (mode == true)
					{
						if (!Directory.Exists(inFile))
						{
							throw new FileNotFoundException("文件夹" + inFile + "不存在！");
						}
						API.Pack(inFile, outFile, pmode);
					}
					else
					{
						if (!File.Exists(inFile))
						{
							throw new FileNotFoundException("文件" + inFile + "不存在！");
						}
						API.Unpack(inFile, outFile, pmode, c1, c2);
					}
				}
				catch (Exception ex)
				{
					err = ex.Message;
				}
				Device.BeginInvokeOnMainThread(() =>
				{
					if (err == null)
					{
						text4.Text = "执行完成";
					}
					else
					{
						text4.Text = "执行异常：" + err;
					}
					b.IsEnabled = true;
				});
			}))
			{ IsBackground = true }.Start();
        }

		public void ModeChange(object sender, ToggledEventArgs e)
        {
			if (((Switch)sender).IsToggled)
            {
				label1.Text = "请填写被打包的文件夹路径";
				label2.Text = "请填写打包生成文件路径";
				label3.Text = "请选择打包模式";
				change.IsVisible = false;
			}
            else
            {
				label1.Text = "请填写被解包的文件路径";
				label2.Text = "请填写解包生成文件夹路径";
				label3.Text = "请选择解包模式";
				change.IsVisible = true;
			}
			//交换文本框内容
			string temp = textbox1.Text;
			textbox1.Text = textbox2.Text;
			textbox2.Text = temp;
		}

		public async Task<string> ChooseFolder()
        {
#if ANDROID
			string file = "/sdcard";
			string createnew = "新建文件夹\0";
			string back = "返回上级目录\0";
			string ok = "确定\0";
			string choosenow = "选择当前目录\0";
			string cancel = "取消\0";
			while (true)
            {
				string[] rawary = Directory.GetDirectories(file);
				Array.Sort(rawary);
				string[] showary;
				if (file.Length <= 7)
                {
					showary = new string[rawary.Length + 1];
					showary[0] = createnew;
					for (int i = 0; i < rawary.Length; i++)
					{
						showary[i + 1] = Path.GetFileName(rawary[i]);
					}
				}
                else
                {
					showary = new string[rawary.Length + 2];
					showary[0] = createnew;
					showary[1] = back;
					for (int i = 0; i < rawary.Length; i++)
					{
						showary[i + 2] = Path.GetFileName(rawary[i]);
					}
				}
				string ans = await DisplayActionSheet(file + Const.PATHSEPARATOR, choosenow, cancel, showary);
				if (string.IsNullOrEmpty(ans) || ans == cancel)
                {
					return null;
                }
				else if (ans == choosenow)
                {
					return file;
                }
				else if (ans == createnew)
                {
					string newname = await DisplayPromptAsync("新建文件夹", "请输入文件夹名", ok, cancel, initialValue:"新建文件夹");
					if (!string.IsNullOrEmpty(newname))
                    {
                        try
                        {
							Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
                        }
						catch (Exception)
                        {
							await DisplayAlert("创建错误", "新建文件夹失败", ok, cancel);
                        }
                    }
                }
				else if (ans == back)
                {
					if (file.Length > 7) file = Path.GetDirectoryName(file);
				}
                else
                {
					file += Const.PATHSEPARATOR + ans;
				}
            }
#elif WINDOWS
return await new PopStudio.MAUI.Platforms.Windows.FolderPicker().PickFolder();
#else
return null;
#endif
		}

		public async Task<string> ChooseOpenFile()
		{
#if ANDROID
			string file = "/sdcard";
			string createnew = "新建文件夹\0";
			string back = "返回上级目录\0";
			string ok = "确定\0";
			string choosenow = "返回\0";
			string cancel = "取消\0";
			while (true)
			{
				string[] rawary = Directory.GetDirectories(file);
				string[] rawary2 = Directory.GetFiles(file);
				Array.Sort(rawary);
				Array.Sort(rawary2);
				string[] showary;
				int ary1 = rawary.Length;
				int ary2 = rawary2.Length;
				if (file.Length <= 7)
				{
					showary = new string[ary1 + ary2 + 1];
					showary[0] = createnew;
					for (int i = 0; i < ary1; i++)
					{
						showary[i + 1] = Path.GetFileName(rawary[i]);
					}
					for (int i = 0; i < ary2; i++)
					{
						showary[i + ary1 + 1] = Path.GetFileName(rawary2[i]);
					}
				}
				else
				{
					showary = new string[ary1 + rawary2.Length + 2];
					showary[0] = createnew;
					showary[1] = back;
					for (int i = 0; i < ary1; i++)
					{
						showary[i + 2] = Path.GetFileName(rawary[i]);
					}
					for (int i = 0; i < ary2; i++)
					{
						showary[i + ary1 + 2] = Path.GetFileName(rawary2[i]);
					}
				}
				string ans = await DisplayActionSheet(file + Const.PATHSEPARATOR, choosenow, cancel, showary);
				if (string.IsNullOrEmpty(ans) || ans == cancel || ans == choosenow)
				{
					return null;
				}
				else if (ans == createnew)
				{
					string newname = await DisplayPromptAsync("新建文件夹", "请输入文件夹名", ok, cancel, initialValue: "新建文件夹");
					if (!string.IsNullOrEmpty(newname))
					{
						try
						{
							Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
						}
						catch (Exception)
						{
							await DisplayAlert("创建错误", "新建文件夹失败", ok, cancel);
						}
					}
				}
				else if (ans == back)
				{
					if (file.Length > 7) file = Path.GetDirectoryName(file);
				}
				else
				{
					file += Const.PATHSEPARATOR + ans;
					if (File.Exists(file))
					{
						return file;
					}
				}
			}
#elif WINDOWS
			return await new PopStudio.MAUI.Platforms.Windows.OpenFilePicker().PickFile();
#else
return null;
#endif
		}

		public async Task<string> ChooseSaveFile()
		{
#if ANDROID
			string file = "/sdcard";
			string createnew = "新建文件夹\0";
			string back = "返回上级目录\0";
			string ok = "确定\0";
			string choosenow = "保存到此目录\0";
			string cancel = "取消\0";
			while (true)
			{
				string[] rawary = Directory.GetDirectories(file);
				string[] rawary2 = Directory.GetFiles(file);
				Array.Sort(rawary);
				Array.Sort(rawary2);
				string[] showary;
				int ary1 = rawary.Length;
				int ary2 = rawary2.Length;
				if (file.Length <= 7)
				{
					showary = new string[ary1 + ary2 + 1];
					showary[0] = createnew;
					for (int i = 0; i < ary1; i++)
					{
						showary[i + 1] = Path.GetFileName(rawary[i]);
					}
					for (int i = 0; i < ary2; i++)
					{
						showary[i + ary1 + 1] = Path.GetFileName(rawary2[i]);
					}
				}
				else
				{
					showary = new string[ary1 + rawary2.Length + 2];
					showary[0] = createnew;
					showary[1] = back;
					for (int i = 0; i < ary1; i++)
					{
						showary[i + 2] = Path.GetFileName(rawary[i]);
					}
					for (int i = 0; i < ary2; i++)
					{
						showary[i + ary1 + 2] = Path.GetFileName(rawary2[i]);
					}
				}
				string ans = await DisplayActionSheet(file + Const.PATHSEPARATOR, choosenow, cancel, showary);
				if (string.IsNullOrEmpty(ans) || ans == cancel)
				{
					return null;
				}
				else if (ans == choosenow)
				{
					string val = await DisplayPromptAsync("保存文件", "请输入文件名", "确定", "取消");
					if (string.IsNullOrEmpty(val)) return null;
					return file + Const.PATHSEPARATOR + val;
				}
				else if (ans == createnew)
				{
					string newname = await DisplayPromptAsync("新建文件夹", "请输入文件夹名", ok, cancel, initialValue: "新建文件夹");
					if (!string.IsNullOrEmpty(newname))
					{
						try
						{
							Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
						}
						catch (Exception)
						{
							await DisplayAlert("创建错误", "新建文件夹失败", ok, cancel);
						}
					}
				}
				else if (ans == back)
				{
					if (file.Length > 7) file = Path.GetDirectoryName(file);
				}
				else
				{
					file += Const.PATHSEPARATOR + ans;
					if (File.Exists(file))
					{
						return file;
					}
				}
			}
#elif WINDOWS
			return await new PopStudio.MAUI.Platforms.Windows.SaveFilePicker().PickFile();
#else
return null;
#endif
		}

		private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
				string val;
				if (switchmode.IsToggled)
                {
					val = await ChooseFolder();
				}
                else
                {
					val = await ChooseOpenFile();

				}
				if (!string.IsNullOrEmpty(val)) textbox1.Text = val;
			}
			catch (Exception)
            {
            }
		}

		private async void Button2_Clicked(object sender, EventArgs e)
		{
			try
			{
				string val;
				if (switchmode.IsToggled)
				{
					val = await ChooseSaveFile();
				}
				else
				{
					val = await ChooseFolder();
				}
				if (!string.IsNullOrEmpty(val)) textbox2.Text = val;
			}
			catch (Exception)
			{
			}
		}
	}
}