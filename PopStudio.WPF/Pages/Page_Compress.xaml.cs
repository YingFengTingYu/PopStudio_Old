using PopStudio.Language.Languages;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PopStudio.WPF.Pages
{
    /// <summary>
    /// Page_Compress.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Compress : Page
    {
        void LoadFont()
        {
            Title = MAUIStr.Obj.Compress_Title;
            label_introduction.Text = MAUIStr.Obj.Compress_Introduction;
            label_choosemode.Text = MAUIStr.Obj.Share_ChooseMode;
            label_mode1.Text = MAUIStr.Obj.Compress_Mode1;
            label_mode2.Text = MAUIStr.Obj.Compress_Mode2;
            LoadFont_Checked(TB_Mode.IsChecked == true);
            button1.Content = MAUIStr.Obj.Share_Choose;
            button2.Content = MAUIStr.Obj.Share_Choose;
            button_run.Content = MAUIStr.Obj.Share_Run;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text4.Text = MAUIStr.Obj.Share_Waiting;
        }

        public Page_Compress()
        {
            InitializeComponent();
            LoadFont();
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("Zlib");
            CB_CMode.Items.Add("Gzip");
            CB_CMode.Items.Add("Deflate");
            CB_CMode.Items.Add("Brotli");
            CB_CMode.Items.Add("Lzma");
            CB_CMode.Items.Add("Lz4");
            CB_CMode.Items.Add("Bzip2");
            CB_CMode.SelectedIndex = 0;
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Page_Compress()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        void LoadFont_Checked(bool v)
        {
            if (v)
            {
                text1.Text = MAUIStr.Obj.Compress_Choose4;
                text2.Text = MAUIStr.Obj.Compress_Choose5;
                text3.Text = MAUIStr.Obj.Compress_Choose6;
            }
            else
            {
                text1.Text = MAUIStr.Obj.Compress_Choose1;
                text2.Text = MAUIStr.Obj.Compress_Choose2;
                text3.Text = MAUIStr.Obj.Compress_Choose3;
            }
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            LoadFont_Checked(((ToggleButton)sender).IsChecked == true);
            (textbox1.Text, textbox2.Text) = (textbox2.Text, textbox1.Text);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            text4.Text = MAUIStr.Obj.Share_Running;
            bool mode = TB_Mode.IsChecked == true;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            int cmode = CB_CMode.SelectedIndex;
            new Thread(new ThreadStart(() =>
            {
                string err = null;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {
                    if (!File.Exists(inFile))
                    {
                        throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
                    }
                    if (mode)
                    {
                        API.Compress(inFile, outFile, cmode);
                    }
                    else
                    {
                        API.Decompress(inFile, outFile, cmode);
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                string val = this.ChooseOpenFile(); //Can't default this
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
                string val = this.ChooseSaveFile(); //Can't default this
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
