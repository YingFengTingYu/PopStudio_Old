using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using PopStudio.MAUI.Languages;

namespace PopStudio.MAUI
{
	public partial class Page_Particles : ContentPage
	{
		public Page_Particles()
		{
			InitializeComponent();
            Title = MAUIStr.Obj.Particles_Title;
            label_introduction.Text = MAUIStr.Obj.Particles_Introduction;
            label_choosemode.Text = MAUIStr.Obj.Share_ChooseMode;
            label_mode1.Text = MAUIStr.Obj.Particles_Mode1;
            label_mode2.Text = MAUIStr.Obj.Particles_Mode2;
            text1.Text = MAUIStr.Obj.Particles_Choose1;
            text2.Text = MAUIStr.Obj.Particles_Choose2;
            text3.Text = MAUIStr.Obj.Particles_Choose3;
            button1.Text = MAUIStr.Obj.Share_Choose;
            button2.Text = MAUIStr.Obj.Share_Choose;
            button_run.Text = MAUIStr.Obj.Share_Run;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text4.Text = MAUIStr.Obj.Share_Waiting;
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("PC");
            CB_CMode.Items.Add("Phone32");
            CB_CMode.Items.Add("Phone64");
            CB_CMode.Items.Add("WP");
            CB_CMode.Items.Add("GameConsole");
            CB_CMode.Items.Add("TV");
            CB_CMode.SelectedIndex = 0;
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            if (((Switch)sender).IsToggled)
            {
                text1.Text = MAUIStr.Obj.Particles_Choose4;
                text2.Text = MAUIStr.Obj.Particles_Choose5;
                text3.Text = MAUIStr.Obj.Particles_Choose6;
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
            }
            else
            {
                text1.Text = MAUIStr.Obj.Particles_Choose1;
                text2.Text = MAUIStr.Obj.Particles_Choose2;
                text3.Text = MAUIStr.Obj.Particles_Choose3;
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
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
                    if (mode)
                    {
                        API.EncodeParticles(inFile, outFile, cmode);
                    }
                    else
                    {
                        API.DecodeParticles(inFile, outFile, cmode);
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