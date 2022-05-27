using PopStudio.Language.Languages;
using PopStudio.Platform;

namespace PopStudio.MAUI
{
	public partial class Page_RTON : ContentPage
	{
        void LoadFont()
        {
            Title = MAUIStr.Obj.RTON_Title;
            label_introduction.Text = MAUIStr.Obj.RTON_Introduction;
            label_choosemode.Text = MAUIStr.Obj.Share_ChooseMode;
            label_mode1.Text = MAUIStr.Obj.RTON_Mode1;
            label_mode2.Text = MAUIStr.Obj.RTON_Mode2;
            LoadFont_Checked(TB_Mode.IsToggled);
            button1.Text = MAUIStr.Obj.Share_Choose;
            button2.Text = MAUIStr.Obj.Share_Choose;
            button_run.Text = MAUIStr.Obj.Share_Run;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text4.Text = MAUIStr.Obj.Share_Waiting;
        }


        public Page_RTON()
		{
			InitializeComponent();
            LoadFont();
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("Simple RTON");
            CB_CMode.Items.Add("Encrypted RTON");
            CB_CMode.SelectedIndex = 0;
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Page_RTON()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        void LoadFont_Checked(bool v)
        {
            if (v)
            {
                text1.Text = MAUIStr.Obj.RTON_Choose4;
                text2.Text = MAUIStr.Obj.RTON_Choose5;
                text3.Text = MAUIStr.Obj.RTON_Choose6;
            }
            else
            {
                text1.Text = MAUIStr.Obj.RTON_Choose1;
                text2.Text = MAUIStr.Obj.RTON_Choose2;
                text3.Text = MAUIStr.Obj.RTON_Choose3;
            }
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
		{
            LoadFont_Checked(((Switch)sender).IsToggled);
            (textbox1.Text, textbox2.Text) = (textbox2.Text, textbox1.Text);
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
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                try
                {
                    if (!File.Exists(inFile))
                    {
                        throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
                    }
                    if (mode == true)
                    {
                        YFAPI.EncodeRTON(inFile, outFile, cmode);
                    }
                    else
                    {
                        YFAPI.DecodeRTON(inFile, outFile, cmode);
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
                sw.Stop();
                decimal time = sw.ElapsedMilliseconds / 1000m;
                Dispatcher.Dispatch(() =>
                {
                    if (err == null)
                    {
                        text4.Text = string.Format(MAUIStr.Obj.Share_Finish, time.ToString("F3"));
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