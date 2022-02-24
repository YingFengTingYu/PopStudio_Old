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
		}

		private void ToggleButton_Checked(object sender, EventArgs e)
		{
            if (((Switch)sender).IsToggled)
            {
                text1.Text = "请填写被编码的文件路径";
                text2.Text = "请填写编码生成文件存放路径";
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
            }
            else
            {
                text1.Text = "请填写被解码的文件路径";
                text2.Text = "请填写解码生成文件存放路径";
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
            }
		}

        private void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            text4.Text = "执行中......";
            bool? mode = TB_Mode.IsToggled;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
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
                        //Class.API.Json2RTON(inFile, outFile);
                    }
                    else
                    {
                        //Class.API.RTON2Json(inFile, outFile);
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
    }
}