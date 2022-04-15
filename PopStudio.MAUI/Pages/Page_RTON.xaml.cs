using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using PopStudio.GUILanguage.Languages;

namespace PopStudio.MAUI
{
	public partial class Page_RTON : ContentPage
	{
		public Page_RTON()
		{
			InitializeComponent();
            Title = MAUIStr.Obj.RTON_Title;
            label_introduction.Text = MAUIStr.Obj.RTON_Introduction;
            label_choosemode.Text = MAUIStr.Obj.Share_ChooseMode;
            label_mode1.Text = MAUIStr.Obj.RTON_Mode1;
            label_mode2.Text = MAUIStr.Obj.RTON_Mode2;
            text1.Text = MAUIStr.Obj.RTON_Choose1;
            text2.Text = MAUIStr.Obj.RTON_Choose2;
            text3.Text = MAUIStr.Obj.RTON_Choose3;
            button1.Text = MAUIStr.Obj.Share_Choose;
            button2.Text = MAUIStr.Obj.Share_Choose;
            button_run.Text = MAUIStr.Obj.Share_Run;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text4.Text = MAUIStr.Obj.Share_Waiting;
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("Simple RTON");
            CB_CMode.Items.Add("Encrypted RTON");
            CB_CMode.SelectedIndex = 0;
        }

		private void ToggleButton_Checked(object sender, EventArgs e)
		{
            if (((Switch)sender).IsToggled)
            {
                text1.Text = MAUIStr.Obj.RTON_Choose4;
                text2.Text = MAUIStr.Obj.RTON_Choose5;
                text3.Text = MAUIStr.Obj.RTON_Choose6;
                (textbox2.Text, textbox1.Text) = (textbox1.Text, textbox2.Text);
            }
            else
            {
                text1.Text = MAUIStr.Obj.RTON_Choose1;
                text2.Text = MAUIStr.Obj.RTON_Choose2;
                text3.Text = MAUIStr.Obj.RTON_Choose3;
                (textbox2.Text, textbox1.Text) = (textbox1.Text, textbox2.Text);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            text4.Text = MAUIStr.Obj.Share_Running;
            bool mode = TB_Mode.IsToggled;
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
                        throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
                    }
                    if (mode == true)
                    {
                        API.EncodeRTON(inFile, outFile, cmode);
                    }
                    else
                    {
                        API.DecodeRTON(inFile, outFile, cmode);
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
                Dispatcher.Dispatch(() =>
                {
                    if (err == null)
                    {
                        text4.Text = MAUIStr.Obj.Share_Finish;
                    }
                    else
                    {
                        text4.Text = string.Format(MAUIStr.Obj.Share_Wrong, err);
                    }
                    b.IsEnabled = true;
                });
            }))
            { IsBackground = true }.Start();
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