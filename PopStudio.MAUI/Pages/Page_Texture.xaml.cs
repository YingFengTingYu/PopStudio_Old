using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace PopStudio.MAUI
{
	public partial class Page_Texture : ContentPage
	{
		public Page_Texture()
		{
			InitializeComponent();
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("PTX（rsb）");
            CB_CMode.Items.Add("cdat（安卓和iOS）");
            CB_CMode.Items.Add("tex（iOS）");
            CB_CMode.Items.Add("txz（安卓和iOS）");
            CB_CMode.Items.Add("tex（TV）");
            CB_CMode.Items.Add("ptx（Xbox360）");
            CB_CMode.Items.Add("ptx（PS3）");
            CB_CMode.Items.Add("ptx（PSV）");
            CB_CMode.SelectedIndex = 0;
            CB_FMode.Items.Clear();
            CB_FMode.Items.Add("ARGB8888(0)");
            CB_FMode.Items.Add("ABGR8888(0)");
            CB_FMode.Items.Add("RGBA4444(1)");
            CB_FMode.Items.Add("RGB565(2)");
            CB_FMode.Items.Add("RGBA5551(3)");
            CB_FMode.Items.Add("RGBA4444_Block(21)");
            CB_FMode.Items.Add("RGB565_Block(22)");
            CB_FMode.Items.Add("RGBA5551_Block(23)");
            CB_FMode.Items.Add("XRGB8888_A8(149)");
            CB_FMode.Items.Add("ARGB8888大端序(0)");
            CB_FMode.Items.Add("ARGB8888_Padding大端序(0)");
            CB_FMode.Items.Add("DXT1_RGB(35)");
            CB_FMode.Items.Add("DXT3_RGBA(36)");
            CB_FMode.Items.Add("DXT5_RGBA(37)");
            CB_FMode.Items.Add("DXT5(5)");
            CB_FMode.Items.Add("DXT5大端序(5)");
            CB_FMode.Items.Add("ETC1_RGB(32)");
            CB_FMode.Items.Add("ETC1_RGB_A8(147)");
            CB_FMode.Items.Add("ETC1_RGB_A_Palette(147)");
            CB_FMode.Items.Add("ETC1_RGB_A_Palette(150)");
            CB_FMode.Items.Add("PVRTC_4BPP_RGBA(30)");
            CB_FMode.Items.Add("PVRTC_4BPP_RGB_A8(148)");
            CB_FMode.SelectedIndex = 0;
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            if (((Switch)sender).IsToggled)
            {
                text1.Text = "请填写被编码的png图像路径";
                text2.Text = "请填写编码所得特殊图像存放路径";
                text3.Text = "请选择编码模式";
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
                SP_FMode.IsVisible = true;
            }
            else
            {
                text1.Text = "请填写被解码的特殊图像路径";
                text2.Text = "请填写解码所得png图像存放路径";
                text3.Text = "请选择解码模式";
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
                SP_FMode.IsVisible = false;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            text4.Text = "执行中......";
            bool mode = TB_Mode.IsToggled;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            int cmode = CB_CMode.SelectedIndex;
            int fmode = CB_FMode.SelectedIndex;
            new Thread(new ThreadStart(() =>
            {
                string err = null;
                try
                {
                    if (!File.Exists(inFile))
                    {
                        throw new FileNotFoundException("文件" + inFile + "不存在！");
                    }
                    if (mode)
                    {
                        API.EncodeImage(inFile, outFile, cmode, fmode);
                    }
                    else
                    {
                        API.DecodeImage(inFile, outFile, cmode);
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

        private void CB_CMode_Selected(object sender, EventArgs e)
        {
            int index = CB_CMode.SelectedIndex;
            CB_FMode.Items.Clear();
            if (index == 0)
            {
                CB_FMode.Items.Add("ARGB8888(0)");
                CB_FMode.Items.Add("ABGR8888(0)");
                CB_FMode.Items.Add("RGBA4444(1)");
                CB_FMode.Items.Add("RGB565(2)");
                CB_FMode.Items.Add("RGBA5551(3)");
                CB_FMode.Items.Add("RGBA4444_Block(21)");
                CB_FMode.Items.Add("RGB565_Block(22)");
                CB_FMode.Items.Add("RGBA5551_Block(23)");
                CB_FMode.Items.Add("XRGB8888_A8(149)");
                CB_FMode.Items.Add("ARGB8888大端序(0)");
                CB_FMode.Items.Add("ARGB8888_Padding大端序(0)");
                CB_FMode.Items.Add("DXT1_RGB(35)");
                CB_FMode.Items.Add("DXT3_RGBA(36)");
                CB_FMode.Items.Add("DXT5_RGBA(37)");
                CB_FMode.Items.Add("DXT5(5)");
                CB_FMode.Items.Add("DXT5大端序(5)");
                CB_FMode.Items.Add("ETC1_RGB(32)");
                CB_FMode.Items.Add("ETC1_RGB_A8(147)");
                CB_FMode.Items.Add("ETC1_RGB_A_Palette(147)");
                CB_FMode.Items.Add("ETC1_RGB_A_Palette(150)");
                CB_FMode.Items.Add("PVRTC_4BPP_RGBA(30)");
                CB_FMode.Items.Add("PVRTC_4BPP_RGB_A8(148)");
            }
            else if (index == 1)
            {
                CB_FMode.Items.Add("Encrypt");
            }
            else if (index == 2 || index == 3)
            {
                CB_FMode.Items.Add("ABGR8888(1)");
                CB_FMode.Items.Add("RGBA4444(2)");
                CB_FMode.Items.Add("RGBA5551(3)");
                CB_FMode.Items.Add("RGB565(4)");
            }
            else if (index == 4)
            {
				CB_FMode.Items.Add("LUT8(1)(Invalid)");
				CB_FMode.Items.Add("ARGB8888(2)");
                CB_FMode.Items.Add("ARGB4444(3)");
                CB_FMode.Items.Add("ARGB1555(4)");
                CB_FMode.Items.Add("RGB565(5)");
                CB_FMode.Items.Add("ABGR8888(6)");
                CB_FMode.Items.Add("RGBA4444(7)");
                CB_FMode.Items.Add("RGBA5551(8)");
                CB_FMode.Items.Add("XRGB8888(9)");
                CB_FMode.Items.Add("LA88(10)");
            }
            else if (index == 5)
            {
                CB_FMode.Items.Add("DXT5_RGBA_Padding");
            }
            else if (index == 6)
            {
                CB_FMode.Items.Add("DXT5_RGBA");
            }
            else if (index == 7)
            {
                CB_FMode.Items.Add("DXT5_RGBA_Morton");
            }
            CB_FMode.SelectedIndex = 0;
        }

		private async void Button_Clicked(object sender, EventArgs e)
		{
			try
			{
				string val = await this.ChooseOpenFile(); //Can't default this
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
				string val = await this.ChooseSaveFile(); //Can't default this
                if (!string.IsNullOrEmpty(val)) textbox2.Text = val;
			}
			catch (Exception)
			{
			}
		}
	}
}