using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using PopStudio.Language.Languages;
using System.Diagnostics;

namespace PopStudio.Avalonia.Pages
{
    public partial class Control_Atlas : UserControl
    {
        public Control_Atlas()
        {
            InitializeComponent();
            LoadControl();
            LoadFont();
            CB_Mode.Items = new List<string>
            {
                "RESOURCES.XML(Rsb)",
                "resources.xml(Old)",
                "resources.xml(Ancient)",
                "plist(Free)",
                "atlasimagemap.dat",
                "xml(TV)",
                "RESOURCES.RTON(Rsb)"
            };
            CB_Mode.SelectedIndex = 0;
            CB_MaxWidth.Items = new List<string>
            {
                "256",
                "512",
                "1024",
                "2048",
                "4096",
                "8192"
            };
            CB_MaxWidth.SelectedIndex = 3;
            CB_MaxHeight.Items = new List<string>
            {
                "256",
                "512",
                "1024",
                "2048",
                "4096",
                "8192"
            };
            CB_MaxHeight.SelectedIndex = 3;
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Control_Atlas()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        void LoadControl()
        {
            label_introduction = this.Get<TextBlock>("label_introduction");
            label_choosemode = this.Get<TextBlock>("label_choosemode");
            label_mode1 = this.Get<TextBlock>("label_mode1");
            label_mode2 = this.Get<TextBlock>("label_mode2");
            text1 = this.Get<TextBlock>("text1");
            text2 = this.Get<TextBlock>("text2");
            text3 = this.Get<TextBlock>("text3");
            text4 = this.Get<TextBlock>("text4");
            text_mode = this.Get<TextBlock>("text_mode");
            text_maxwidth = this.Get<TextBlock>("text_maxwidth");
            text_maxheight = this.Get<TextBlock>("text_maxheight");
            button1 = this.Get<Button>("button1");
            button2 = this.Get<Button>("button2");
            button3 = this.Get<Button>("button3");
            button_run = this.Get<Button>("button_run");
            label_statue = this.Get<TextBlock>("label_statue");
            text5 = this.Get<TextBlock>("text5");
            TB_Mode = this.Get<ToggleSwitch>("TB_Mode");
            textbox1 = this.Get<TextBox>("textbox1");
            textbox2 = this.Get<TextBox>("textbox2");
            textbox3 = this.Get<TextBox>("textbox3");
            textbox4 = this.Get<TextBox>("textbox4");
            CB_Mode = this.Get<ComboBox>("CB_Mode");
            CB_MaxHeight = this.Get<ComboBox>("CB_MaxHeight");
            CB_MaxWidth = this.Get<ComboBox>("CB_MaxWidth");
            splice_size = this.Get<StackPanel>("splice_size");
        }

        void LoadFont()
        {
            label_introduction.Text = MAUIStr.Obj.Atlas_Introduction;
            label_choosemode.Text = MAUIStr.Obj.Share_ChooseMode;
            label_mode1.Text = MAUIStr.Obj.Atlas_Mode1;
            label_mode2.Text = MAUIStr.Obj.Atlas_Mode2;
            text1.Text = MAUIStr.Obj.Atlas_Choose1;
            text2.Text = MAUIStr.Obj.Atlas_Choose2;
            text3.Text = MAUIStr.Obj.Atlas_Choose3;
            text4.Text = MAUIStr.Obj.Atlas_Choose4;
            text_mode.Text = MAUIStr.Obj.Atlas_Format;
            text_maxwidth.Text = MAUIStr.Obj.Atlas_MaxWidth;
            text_maxheight.Text = MAUIStr.Obj.Atlas_MaxHeight;
            button1.Content = MAUIStr.Obj.Share_Choose;
            button2.Content = MAUIStr.Obj.Share_Choose;
            button3.Content = MAUIStr.Obj.Share_Choose;
            button_run.Content = MAUIStr.Obj.Share_Run;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text5.Text = MAUIStr.Obj.Share_Waiting;
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string val;
                if (TB_Mode.IsChecked == true)
                {
                    val = await new OpenFolderDialog().ShowAsync(MainWindow.Singleten);
                }
                else
                {
                    val = (await new OpenFileDialog { AllowMultiple = false }.ShowAsync(MainWindow.Singleten))?[0];

                }
                if (!string.IsNullOrEmpty(val)) textbox1.Text = val;
            }
            catch (Exception)
            {
            }
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string val;
                if (TB_Mode.IsChecked == true)
                {
                    val = await new SaveFileDialog().ShowAsync(MainWindow.Singleten);
                }
                else
                {
                    val = await new OpenFolderDialog().ShowAsync(MainWindow.Singleten);
                }
                if (!string.IsNullOrEmpty(val)) textbox2.Text = val;
            }
            catch (Exception)
            {
            }
        }

        private async void Button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string val = (await new OpenFileDialog { AllowMultiple = false }.ShowAsync(MainWindow.Singleten))?[0];
                if (!string.IsNullOrEmpty(val)) textbox3.Text = val;
            }
            catch (Exception)
            {
            }
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            text5.Text = MAUIStr.Obj.Share_Running;
            bool mode = TB_Mode.IsChecked == true;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            string infoFile = textbox3.Text;
            string ID = textbox4.Text;
            int cmode = CB_Mode.SelectedIndex;
            int MaxWidth = 256 << CB_MaxWidth.SelectedIndex;
            int MaxHeight = 256 << CB_MaxHeight.SelectedIndex;
            new Thread(new ThreadStart(() =>
            {
                string err = null;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {
                    if (mode)
                    {
                        if (!Directory.Exists(inFile))
                        {
                            throw new Exception(string.Format(MAUIStr.Obj.Share_FolderNotFound, inFile));
                        }
                        if (!API.SpliceImage(inFile, outFile, infoFile, ID, cmode, MaxWidth, MaxHeight))
                        {
                            err = MAUIStr.Obj.Atlas_NotFound2;
                        }
                    }
                    else
                    {
                        if (!File.Exists(inFile))
                        {
                            throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
                        }
                        if (!API.CutImage(inFile, outFile, infoFile, ID, cmode))
                        {
                            err = MAUIStr.Obj.Atlas_NotFound1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
                sw.Stop();
                decimal time = sw.ElapsedMilliseconds / 1000m;
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (err == null)
                    {
                        text5.Text = string.Format(MAUIStr.Obj.Share_Finish, time.ToString("F3"));
                    }
                    else
                    {
                        text5.Text = string.Format(MAUIStr.Obj.Share_Wrong, err);
                    }
                    b.IsEnabled = true;
                });
            }))
            { IsBackground = true }.Start();
        }

        private void Switch_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch s)
            {
                if (s.IsChecked == true)
                {
                    text1.Text = MAUIStr.Obj.Atlas_Choose5;
                    text2.Text = MAUIStr.Obj.Atlas_Choose6;
                    text4.Text = MAUIStr.Obj.Atlas_Choose7;
                    splice_size.IsVisible = true;
                }
                else
                {
                    text1.Text = MAUIStr.Obj.Atlas_Choose1;
                    text2.Text = MAUIStr.Obj.Atlas_Choose2;
                    text4.Text = MAUIStr.Obj.Atlas_Choose4;
                    splice_size.IsVisible = false;
                }
                (textbox1.Text, textbox2.Text) = (textbox2.Text, textbox1.Text);
            }
        }
    }
}
