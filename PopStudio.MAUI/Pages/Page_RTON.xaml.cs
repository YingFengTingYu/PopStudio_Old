using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace PopStudio.MAUI
{
	public partial class Page_RTON : ContentPage
	{
		public Page_RTON()
		{
			InitializeComponent();
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("普通RTON");
            CB_CMode.Items.Add("加密RTON");
            CB_CMode.SelectedIndex = 0;
        }

		private void ToggleButton_Checked(object sender, EventArgs e)
		{
            if (((Switch)sender).IsToggled)
            {
                text1.Text = "请填写被编码的文件路径";
                text2.Text = "请填写编码生成文件存放路径";
                text3.Text = "请选择编码模式";
                (textbox2.Text, textbox1.Text) = (textbox1.Text, textbox2.Text);
            }
            else
            {
                text1.Text = "请填写被解码的文件路径";
                text2.Text = "请填写解码生成文件存放路径";
                text3.Text = "请选择解码模式";
                (textbox2.Text, textbox1.Text) = (textbox1.Text, textbox2.Text);
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
            string key = textbox3.Text;
            new Thread(new ThreadStart(() =>
            {
                string err = null;
                try
                {
                    if (!File.Exists(inFile))
                    {
                        throw new FileNotFoundException("文件" + inFile + "不存在！");
                    }
                    if (mode == true)
                    {
                        API.EncodeRTON(inFile, outFile, cmode, key);
                    }
                    else
                    {
                        API.DecodeRTON(inFile, outFile, cmode, key);
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

        private void CB_CMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Picker)sender).SelectedIndex == 0)
            {
                keyitem.IsVisible = false;
            }
            else
            {
                keyitem.IsVisible = true;
            }
        }
    }
}