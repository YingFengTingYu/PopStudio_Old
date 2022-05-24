using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using PopStudio.Language.Languages;
using System.Diagnostics;

namespace PopStudio.Avalonia.Pages
{
    public partial class Control_Trail : UserControl
    {
        public Control_Trail()
        {
            InitializeComponent();
            LoadControl();
            LoadFont();
            CB_InMode.Items = new List<string>
            {
                "PC_Compiled",
                "Phone32_Compiled",
                "Phone64_Compiled",
                "WP_Xnb",
                "GameConsole_Compiled",
                "TV_Compiled",
                "Studio_Json",
                "Raw_Xml"
            };
            CB_InMode.SelectedIndex = 0;
            CB_OutMode.Items = new List<string>
            {
                "PC_Compiled",
                "Phone32_Compiled",
                "Phone64_Compiled",
                "WP_Xnb",
                "GameConsole_Compiled",
                "TV_Compiled",
                "Studio_Json",
                "Raw_Xml"
            };
            CB_OutMode.SelectedIndex = 7;
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Control_Trail()
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
            text1 = this.Get<TextBlock>("text1");
            text2 = this.Get<TextBlock>("text2");
            text_in = this.Get<TextBlock>("text_in");
            text_out = this.Get<TextBlock>("text_out");
            button1 = this.Get<Button>("button1");
            button2 = this.Get<Button>("button2");
            button_run = this.Get<Button>("button_run");
            label_statue = this.Get<TextBlock>("label_statue");
            text4 = this.Get<TextBlock>("text4");
            textbox1 = this.Get<TextBox>("textbox1");
            textbox2 = this.Get<TextBox>("textbox2");
            CB_InMode = this.Get<ComboBox>("CB_InMode");
            CB_OutMode = this.Get<ComboBox>("CB_OutMode");
        }

        void LoadFont()
        {
            label_introduction.Text = MAUIStr.Obj.Trail_Introduction;
            text1.Text = MAUIStr.Obj.Trail_Choose1;
            text2.Text = MAUIStr.Obj.Trail_Choose2;
            text_in.Text = MAUIStr.Obj.Trail_InFormat;
            text_out.Text = MAUIStr.Obj.Trail_OutFormat;
            button1.Content = MAUIStr.Obj.Share_Choose;
            button2.Content = MAUIStr.Obj.Share_Choose;
            button_run.Content = MAUIStr.Obj.Share_Run;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text4.Text = MAUIStr.Obj.Share_Waiting;
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string val = (await new OpenFileDialog { AllowMultiple = false }.ShowAsync(MainWindow.Singleten))?[0];
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
                string val = await new SaveFileDialog().ShowAsync(MainWindow.Singleten);
                if (!string.IsNullOrEmpty(val)) textbox2.Text = val;
            }
            catch (Exception)
            {
            }
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            text4.Text = MAUIStr.Obj.Share_Running;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            int inmode = CB_InMode.SelectedIndex;
            int outmode = CB_OutMode.SelectedIndex;
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
                    API.Trail(inFile, outFile, inmode, outmode);
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
    }
}
