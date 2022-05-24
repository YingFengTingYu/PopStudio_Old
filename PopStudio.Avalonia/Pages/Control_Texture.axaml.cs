using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using PopStudio.Language.Languages;
using System.Diagnostics;

namespace PopStudio.Avalonia.Pages
{
    public partial class Control_Texture : UserControl
    {
        public Control_Texture()
        {
            InitializeComponent();
            LoadControl();
            LoadFont();
            CB_CMode.Items = new List<string>
            {
                "PTX(rsb)",
                "cdat(Android,iOS)",
                "tex(iOS)",
                "txz(Android,iOS)",
                "tex(TV)",
                "ptx(Xbox360)",
                "ptx(PS3)",
                "ptx(PSV)",
                "xnb(Windows Phone)"
            };
            CB_CMode.SelectedIndex = 0;
            CB_FMode.Items = new List<string>
            {
                "ARGB8888(0)",
                "ABGR8888(0)",
                "RGBA4444(1)",
                "RGB565(2)",
                "RGBA5551(3)",
                "RGBA4444_Block(21)",
                "RGB565_Block(22)",
                "RGBA5551_Block(23)",
                "XRGB8888_A8(149)",
                "ARGB8888(BE)(0)",
                "ARGB8888_Padding(BE)(0)",
                "DXT1_RGB(35)",
                "DXT3_RGBA(36)",
                "DXT5_RGBA(37)",
                "DXT5(5)",
                "DXT5(BE)(5)",
                "ETC1_RGB(32)",
                "ETC1_RGB_A8(147)",
                "ETC1_RGB_A_Palette(147)",
                "ETC1_RGB_A_Palette(150)",
                "PVRTC_4BPP_RGBA(30)",
                "PVRTC_4BPP_RGB_A8(148)"
            };
            CB_FMode.SelectedIndex = 0;
            CB_CMode.SelectionChanged += CB_CMode_Selected;
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Control_Texture()
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
            textN = this.Get<TextBlock>("textN");
            button1 = this.Get<Button>("button1");
            button2 = this.Get<Button>("button2");
            button_run = this.Get<Button>("button_run");
            label_statue = this.Get<TextBlock>("label_statue");
            text4 = this.Get<TextBlock>("text4");
            textbox1 = this.Get<TextBox>("textbox1");
            textbox2 = this.Get<TextBox>("textbox2");
            TB_Mode = this.Get<ToggleSwitch>("TB_Mode");
            CB_CMode = this.Get<ComboBox>("CB_CMode");
            CB_FMode = this.Get<ComboBox>("CB_FMode");
            SP_FMode = this.Get<StackPanel>("SP_FMode");
        }

        void LoadFont()
        {
            label_introduction.Text = MAUIStr.Obj.Texture_Introduction;
            label_choosemode.Text = MAUIStr.Obj.Share_ChooseMode;
            label_mode1.Text = MAUIStr.Obj.Texture_Mode1;
            label_mode2.Text = MAUIStr.Obj.Texture_Mode2;
            text1.Text = MAUIStr.Obj.Texture_Choose1;
            text2.Text = MAUIStr.Obj.Texture_Choose2;
            text3.Text = MAUIStr.Obj.Texture_Choose3;
            textN.Text = MAUIStr.Obj.Texture_Choose7;
            button1.Content = MAUIStr.Obj.Share_Choose;
            button2.Content = MAUIStr.Obj.Share_Choose;
            button_run.Content = MAUIStr.Obj.Share_Run;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text4.Text = MAUIStr.Obj.Share_Waiting;
        }

        private void Switch_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch s)
            {
                if (s.IsChecked == true)
                {
                    text1.Text = MAUIStr.Obj.Texture_Choose4;
                    text2.Text = MAUIStr.Obj.Texture_Choose5;
                    text3.Text = MAUIStr.Obj.Texture_Choose6;
                    SP_FMode.IsVisible = true;
                }
                else
                {
                    text1.Text = MAUIStr.Obj.Texture_Choose1;
                    text2.Text = MAUIStr.Obj.Texture_Choose2;
                    text3.Text = MAUIStr.Obj.Texture_Choose3;
                    SP_FMode.IsVisible = false;
                }
                (textbox1.Text, textbox2.Text) = (textbox2.Text, textbox1.Text);
            }
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
            bool mode = TB_Mode.IsChecked == true;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            int cmode = CB_CMode.SelectedIndex;
            int fmode = CB_FMode.SelectedIndex;
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
                        API.EncodeImage(inFile, outFile, cmode, fmode);
                    }
                    else
                    {
                        API.DecodeImage(inFile, outFile, cmode);
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

        private void CB_CMode_Selected(object sender, SelectionChangedEventArgs e)
        {
            int index = CB_CMode.SelectedIndex;
            List<string> FMode_Items = new List<string>();
            switch (index)
            {
                case 0:
                    FMode_Items.Add("ARGB8888(0)");
                    FMode_Items.Add("ABGR8888(0)");
                    FMode_Items.Add("RGBA4444(1)");
                    FMode_Items.Add("RGB565(2)");
                    FMode_Items.Add("RGBA5551(3)");
                    FMode_Items.Add("RGBA4444_Block(21)");
                    FMode_Items.Add("RGB565_Block(22)");
                    FMode_Items.Add("RGBA5551_Block(23)");
                    FMode_Items.Add("XRGB8888_A8(149)");
                    FMode_Items.Add("ARGB8888(BE)(0)");
                    FMode_Items.Add("ARGB8888_Padding(BE)(0)");
                    FMode_Items.Add("DXT1_RGB(35)");
                    FMode_Items.Add("DXT3_RGBA(36)");
                    FMode_Items.Add("DXT5_RGBA(37)");
                    FMode_Items.Add("DXT5(5)");
                    FMode_Items.Add("DXT5(BE)(5)");
                    FMode_Items.Add("ETC1_RGB(32)");
                    FMode_Items.Add("ETC1_RGB_A8(147)");
                    FMode_Items.Add("ETC1_RGB_A_Palette(147)");
                    FMode_Items.Add("ETC1_RGB_A_Palette(150)");
                    FMode_Items.Add("PVRTC_4BPP_RGBA(30)");
                    FMode_Items.Add("PVRTC_4BPP_RGB_A8(148)");
                    break;
                case 1:
                    FMode_Items.Add("Encrypt");
                    break;
                case 2:
                case 3:
                    FMode_Items.Add("ABGR8888(1)");
                    FMode_Items.Add("RGBA4444(2)");
                    FMode_Items.Add("RGBA5551(3)");
                    FMode_Items.Add("RGB565(4)");
                    break;
                case 4:
                    FMode_Items.Add("LUT8(1)(Invalid)");
                    FMode_Items.Add("ARGB8888(2)");
                    FMode_Items.Add("ARGB4444(3)");
                    FMode_Items.Add("ARGB1555(4)");
                    FMode_Items.Add("RGB565(5)");
                    FMode_Items.Add("ABGR8888(6)");
                    FMode_Items.Add("RGBA4444(7)");
                    FMode_Items.Add("RGBA5551(8)");
                    FMode_Items.Add("XRGB8888(9)");
                    FMode_Items.Add("LA88(10)");
                    break;
                case 5:
                    FMode_Items.Add("DXT5_RGBA_Padding(BE)");
                    break;
                case 6:
                    FMode_Items.Add("DXT5_RGBA");
                    break;
                case 7:
                    FMode_Items.Add("DXT5_RGBA_Morton");
                    break;
                case 8:
                    FMode_Items.Add("ABGR8888");
                    break;
            }
            CB_FMode.Items = FMode_Items;
            CB_FMode.SelectedIndex = 0;
        }
    }
}
