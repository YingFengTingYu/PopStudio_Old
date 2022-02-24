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
			CB_CMode.Items.Add("dz（安卓，黑莓）");
			CB_CMode.Items.Add("rsb（安卓，iOS，PS3，PS4，Xbox360）");
			CB_CMode.Items.Add("pak（Windows，MacOS，PS3，PSV，Xbox360）");
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
    }
}