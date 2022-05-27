using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using PopStudio.Language.Languages;
using PopStudio.Platform;

namespace PopStudio.WPF.Pages
{
    /// <summary>
    /// Page_Package.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Package : Page
    {
		void LoadFont()
        {
            Title = MAUIStr.Obj.Package_Title;
            label_mode1.Text = MAUIStr.Obj.Package_Mode1;
            label_mode2.Text = MAUIStr.Obj.Package_Mode2;
            label_introduction.Text = MAUIStr.Obj.Package_Introduction;
            label_choosemode.Text = MAUIStr.Obj.Share_ChooseMode;
			LoadFont_Checked(switchmode.IsChecked == true);
            label_changeimage.Text = MAUIStr.Obj.Package_ChangeImage;
            label_deleteimage.Text = MAUIStr.Obj.Package_DeleteImage;
            button1.Content = MAUIStr.Obj.Share_Choose;
            button2.Content = MAUIStr.Obj.Share_Choose;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text4.Text = MAUIStr.Obj.Share_Waiting;
            button_run.Content = MAUIStr.Obj.Share_Run;
        }

        public Page_Package()
        {
            InitializeComponent();
			LoadFont();
            CB_CMode.Items.Clear();
			CB_CMode.Items.Add("dz");
			CB_CMode.Items.Add("rsb");
			CB_CMode.Items.Add("pak");
			CB_CMode.Items.Add("arcv");
			CB_CMode.SelectedIndex = 0;
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Page_Package()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        public void Do(object sender, EventArgs e)
		{
			Button b = (Button)sender;
			b.IsEnabled = false;
			text4.Text = MAUIStr.Obj.Share_Running;
			bool mode = switchmode.IsChecked == true;
			string inFile = textbox1.Text;
			string outFile = textbox2.Text;
			int pmode = CB_CMode.SelectedIndex;
			bool c1 = switchchange1.IsChecked == true;
			bool c2 = switchchange2.IsChecked == true;
			new Thread(new ThreadStart(() =>
			{
				string err = null;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
				{
					if (mode == true)
					{
                        if (!Directory.Exists(inFile))
						{
							throw new Exception(string.Format(MAUIStr.Obj.Share_FolderNotFound, inFile));
						}
						API.Pack(inFile, outFile, pmode);
					}
					else
					{
						if (!File.Exists(inFile))
						{
							throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
						}
						API.Unpack(inFile, outFile, pmode, c1, c2);
					}
				}
				catch (Exception ex)
				{
					err = ex.Message;
				}
                sw.Stop();
                decimal time = sw.ElapsedMilliseconds / 1000m;
                Dispatcher.BeginInvoke(() =>
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

		void LoadFont_Checked(bool v)
        {
            if (v)
            {
                label1.Text = MAUIStr.Obj.Package_Choose4;
                label2.Text = MAUIStr.Obj.Package_Choose5;
                label3.Text = MAUIStr.Obj.Package_Choose6;
            }
            else
            {
                label1.Text = MAUIStr.Obj.Package_Choose1;
                label2.Text = MAUIStr.Obj.Package_Choose2;
                label3.Text = MAUIStr.Obj.Package_Choose3;
            }
        }


        public void ModeChange(object sender, RoutedEventArgs e)
		{
			LoadFont_Checked(((ToggleButton)sender).IsChecked == true);
            if (((ToggleButton)sender).IsChecked == true)
			{
				change.Visibility = Visibility.Collapsed;
			}
			else
			{
				change.Visibility = Visibility.Visible;
			}
            (textbox1.Text, textbox2.Text) = (textbox2.Text, textbox1.Text);
        }

		private void Button_Clicked(object sender, EventArgs e)
		{
			try
			{
				string val;
				if (switchmode.IsChecked == true)
				{
					val = this.ChooseFolder(); //Can't default this
				}
				else
				{
					val = this.ChooseOpenFile();

				}
				if (!string.IsNullOrEmpty(val)) textbox1.Text = val;
			}
			catch (Exception)
			{
			}
		}

		private void Button2_Clicked(object sender, EventArgs e)
		{
			try
			{
				string val;
				if (switchmode.IsChecked == true)
				{
					val = this.ChooseSaveFile(); //Can't default this
				}
				else
				{
					val = this.ChooseFolder(); //Can't default this
				}
				if (!string.IsNullOrEmpty(val)) textbox2.Text = val;
			}
			catch (Exception)
			{
			}
		}

		private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
		}
	}
}