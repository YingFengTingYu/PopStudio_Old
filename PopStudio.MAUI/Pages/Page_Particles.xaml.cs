using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace PopStudio.MAUI
{
	public partial class Page_Particles : ContentPage
	{
		public Page_Particles()
		{
			InitializeComponent();
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("PC");
            CB_CMode.Items.Add("Phone32位");
            CB_CMode.Items.Add("安卓64位");
            CB_CMode.Items.Add("iOS64位");
            CB_CMode.Items.Add("WindowsPhone");
            CB_CMode.Items.Add("TV");
            CB_CMode.Items.Add("TV上古版本");
            CB_CMode.SelectedIndex = 0;
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            if (((Switch)sender).IsToggled)
            {
                text1.Text = "请填写被编码的文件路径";
                text2.Text = "请填写编码所得文件存放路径";
                text3.Text = "请选择编码模式";
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
            }
            else
            {
                text1.Text = "请填写被解码的文件路径";
                text2.Text = "请填写解码所得文件存放路径";
                text3.Text = "请选择解码模式";
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
            int cmode = CB_CMode.SelectedIndex;
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
                        //Class.API.ParticlesCompiled(inFile, outFile, cmode);
                    }
                    else
                    {
                        //Class.API.ParticlesDecompiled(inFile, outFile, cmode);
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