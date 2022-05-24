using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PopStudio.Language.Languages;
using PopStudio.Platform;
using PopStudio.WPF.Pages;

namespace PopStudio.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Singleten;

        public MainWindow()
        {
            Singleten = this;
            InitializeComponent();
            LoadFont();
            LoadHomePage();
            MAUIStr.OnLanguageChanged += LoadFont;
            if (Setting.OpenProgramAD) ShowAD(new Random().Next(1, 4));
            Prepare();
        }

        public async void Prepare()
        {
            loadingbar.Value = 0;
            LoadingGrid.Opacity = 1;
            LoadingGrid.Visibility = Visibility.Visible;
            for (int i = 1; i <= 50; i++)
            {
                await Task.Delay(10);
                loadingbar.Value = i << 1;
            }
            await Task.Delay(200);
            for (int i = 25; i > 0; i--)
            {
                await Task.Delay(10);
                LoadingGrid.Opacity = (i << 2) / 100d;
            }
            LoadingGrid.Visibility = Visibility.Collapsed;
        }

        public async void ShowAD(int index)
        {
            byte[] img;
            string url;
            switch (index)
            {
                case 1:
                    img = ResourceAD.ImageAD1;
                    url = ResourceAD.AD1;
                    break;
                case 2:
                    img = ResourceAD.ImageAD2;
                    url = ResourceAD.AD2;
                    break;
                case 3:
                    img = ResourceAD.ImageAD3;
                    url = ResourceAD.AD3;
                    break;
                default:
                    return;
            }
            await PopupDialog.DisplayPicture(MAUIStr.Obj.AD_Title, img, MAUIStr.Obj.AD_Cancel, () => Permission.OpenUrl(url), true);
        }

        ~MainWindow()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        public void ShowDialog(Page page)
        {
            DialogControl.Content = new Frame
            {
                Content = page
            };
            DialogGrid.Visibility = Visibility.Visible;
        }

        public void CloseDialog()
        {
            DialogControl.Content = null;
            DialogGrid.Visibility = Visibility.Collapsed;
        }

        private void Image_TopMost_Tapped(object sender, MouseButtonEventArgs e) => Topmost = !Topmost;

        int CurrentPageIndex = -1;

        public void LoadFont()
        {
            button1.Content = MAUIStr.Obj.HomePage_Title;
            button2.Content = MAUIStr.Obj.Package_Title;
            button3.Content = MAUIStr.Obj.Atlas_Title;
            button4.Content = MAUIStr.Obj.Texture_Title;
            button5.Content = MAUIStr.Obj.Reanim_Title;
            button6.Content = MAUIStr.Obj.Particles_Title;
            button7.Content = MAUIStr.Obj.Trail_Title;
            button8.Content = MAUIStr.Obj.Pam_Title;
            button9.Content = MAUIStr.Obj.RTON_Title;
            button10.Content = MAUIStr.Obj.Compress_Title;
            button11.Content = MAUIStr.Obj.LuaScript_Title;
            button12.Content = MAUIStr.Obj.Setting_Title;
            switch (CurrentPageIndex)
            {
                case 0:
                    SetTitle(MAUIStr.Obj.HomePage_Title);
                    break;
                case 1:
                    SetTitle(MAUIStr.Obj.Package_Title);
                    break;
                case 2:
                    SetTitle(MAUIStr.Obj.Atlas_Title);
                    break;
                case 3:
                    SetTitle(MAUIStr.Obj.Texture_Title);
                    break;
                case 4:
                    SetTitle(MAUIStr.Obj.Reanim_Title);
                    break;
                case 5:
                    SetTitle(MAUIStr.Obj.Particles_Title);
                    break;
                case 6:
                    SetTitle(MAUIStr.Obj.Trail_Title);
                    break;
                case 7:
                    SetTitle(MAUIStr.Obj.Pam_Title);
                    break;
                case 8:
                    SetTitle(MAUIStr.Obj.RTON_Title);
                    break;
                case 9:
                    SetTitle(MAUIStr.Obj.Compress_Title);
                    break;
                case 10:
                    SetTitle(MAUIStr.Obj.LuaScript_Title);
                    break;
                case 11:
                    SetTitle(MAUIStr.Obj.Setting_Title);
                    break;
            }
        }

        void ResetShellButton()
        {
            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            button2.Background = Brushes.White;
            button2.Foreground = Brushes.Black;
            button3.Background = Brushes.White;
            button3.Foreground = Brushes.Black;
            button4.Background = Brushes.White;
            button4.Foreground = Brushes.Black;
            button5.Background = Brushes.White;
            button5.Foreground = Brushes.Black;
            button6.Background = Brushes.White;
            button6.Foreground = Brushes.Black;
            button7.Background = Brushes.White;
            button7.Foreground = Brushes.Black;
            button8.Background = Brushes.White;
            button8.Foreground = Brushes.Black;
            button9.Background = Brushes.White;
            button9.Foreground = Brushes.Black;
            button10.Background = Brushes.White;
            button10.Foreground = Brushes.Black;
            button11.Background = Brushes.White;
            button11.Foreground = Brushes.Black;
            button12.Background = Brushes.White;
            button12.Foreground = Brushes.Black;
        }

        void UpButton(Button b)
        {
            b.Background = Brushes.CornflowerBlue;
            b.Foreground = Brushes.White;
        }

        void LoadPage(Page u) => PageControl.Content = new Frame { Content = u };

        void SetTitle(string s) => Label_Head.Content = s;

        Page_HomePage homePage = new Page_HomePage();
        Page_Package package = new Page_Package();
        Page_Atlas atlas = new Page_Atlas();
        Page_Texture texture = new Page_Texture();
        Page_Reanim reanim = new Page_Reanim();
        Page_Particles particles = new Page_Particles();
        Page_Trail trail = new Page_Trail();
        Page_Pam pam = new Page_Pam();
        Page_RTON rton = new Page_RTON();
        Page_Compress compress = new Page_Compress();
        Page_LuaScript luaScript = new Page_LuaScript();
        Page_Setting setting = new Page_Setting();

        public void LoadHomePage()
        {
            ResetShellButton();
            UpButton(button1);
            LoadPage(homePage);
            SetTitle(MAUIStr.Obj.HomePage_Title);
            CurrentPageIndex = 0;
        }

        public void LoadPackage()
        {
            ResetShellButton();
            UpButton(button2);
            LoadPage(package);
            SetTitle(MAUIStr.Obj.Package_Title);
            CurrentPageIndex = 1;
        }

        public void LoadAtlas()
        {
            ResetShellButton();
            UpButton(button3);
            LoadPage(atlas);
            SetTitle(MAUIStr.Obj.Atlas_Title);
            CurrentPageIndex = 2;
        }

        public void LoadTexture()
        {
            ResetShellButton();
            UpButton(button4);
            LoadPage(texture);
            SetTitle(MAUIStr.Obj.Texture_Title);
            CurrentPageIndex = 3;
        }

        public void LoadReanim()
        {
            ResetShellButton();
            UpButton(button5);
            LoadPage(reanim);
            SetTitle(MAUIStr.Obj.Reanim_Title);
            CurrentPageIndex = 4;
        }

        public void LoadParticles()
        {
            ResetShellButton();
            UpButton(button6);
            LoadPage(particles);
            SetTitle(MAUIStr.Obj.Particles_Title);
            CurrentPageIndex = 5;
        }

        public void LoadTrail()
        {
            ResetShellButton();
            UpButton(button7);
            LoadPage(trail);
            SetTitle(MAUIStr.Obj.Trail_Title);
            CurrentPageIndex = 6;
        }

        public void LoadPam()
        {
            ResetShellButton();
            UpButton(button8);
            LoadPage(pam);
            SetTitle(MAUIStr.Obj.Pam_Title);
            CurrentPageIndex = 7;
        }

        public void LoadRTON()
        {
            ResetShellButton();
            UpButton(button9);
            LoadPage(rton);
            SetTitle(MAUIStr.Obj.RTON_Title);
            CurrentPageIndex = 8;
        }

        public void LoadCompress()
        {
            ResetShellButton();
            UpButton(button10);
            LoadPage(compress);
            SetTitle(MAUIStr.Obj.Compress_Title);
            CurrentPageIndex = 9;
        }

        public void LoadLuaScript()
        {
            ResetShellButton();
            UpButton(button11);
            LoadPage(luaScript);
            SetTitle(MAUIStr.Obj.LuaScript_Title);
            CurrentPageIndex = 10;
        }

        public void LoadSetting()
        {
            ResetShellButton();
            UpButton(button12);
            LoadPage(setting);
            SetTitle(MAUIStr.Obj.Setting_Title);
            CurrentPageIndex = 11;
        }

        private void Button1_Click(object sender, RoutedEventArgs e) => LoadHomePage();

        private void Button2_Click(object sender, RoutedEventArgs e) => LoadPackage();

        private void Button3_Click(object sender, RoutedEventArgs e) => LoadAtlas();

        private void Button4_Click(object sender, RoutedEventArgs e) => LoadTexture();

        private void Button5_Click(object sender, RoutedEventArgs e) => LoadReanim();

        private void Button6_Click(object sender, RoutedEventArgs e) => LoadParticles();

        private void Button7_Click(object sender, RoutedEventArgs e) => LoadTrail();

        private void Button8_Click(object sender, RoutedEventArgs e) => LoadPam();

        private void Button9_Click(object sender, RoutedEventArgs e) => LoadRTON();

        private void Button10_Click(object sender, RoutedEventArgs e) => LoadCompress();

        private void Button11_Click(object sender, RoutedEventArgs e) => LoadLuaScript();

        private void Button12_Click(object sender, RoutedEventArgs e) => LoadSetting();
    }
}
