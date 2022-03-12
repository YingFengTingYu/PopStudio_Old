using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PopStudio.WPF.Languages;
using PopStudio.WPF.Pages;

namespace PopStudio.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Singleten
        public static MainWindow Singleten;
        #endregion

        #region Dialog

        public object Result { get; set; }

        public void OpenAlertDialog(string title, string message, string cancel)
        {
            Result = null;
            DialogControl.Content = new Frame
            {
                Content = new Plugin.AlertDialog(title, message, cancel)
            };
            DialogGrid.Visibility = Visibility.Visible;
        }

        public void OpenAlertDialog(string title, string message, string accept, string cancel)
        {
            Result = null;
            DialogControl.Content = new Frame
            {
                Content = new Plugin.AlertDialog(title, message, accept, cancel)
            };
            DialogGrid.Visibility = Visibility.Visible;
        }

        public void OpenPromptDialog(string title, string message, string accept, string cancel, string initialValue)
        {
            Result = null;
            DialogControl.Content = new Frame
            {
                Content = new Plugin.PromptDialog(title, message, accept, cancel, initialValue)
            };
            DialogGrid.Visibility = Visibility.Visible;
        }

        public void OpenPictureDialog(string title, BitmapImage img, string cancel, Action action, bool TouchLeave)
        {
            Result = null;
            DialogControl.Content = new Frame
            {
                Content = new Plugin.PictureDialog(title, img, cancel, action, TouchLeave)
            };
            DialogGrid.Visibility = Visibility.Visible;
        }

        public void OpenSheetDialog(string title, string cancel, string ok, params string[] items)
        {
            Result = null;
            DialogControl.Content = new Frame
            {
                Content = new Plugin.SheetDialog(title, cancel, ok, items)
            };
            DialogGrid.Visibility = Visibility.Visible;
        }

        public void CloseDialog()
        {
            DialogGrid.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Init
        public MainWindow()
        {
            InitializeComponent();
            button1.Content = MAUIStr.Obj.HomePage_Title;
            button2.Content = MAUIStr.Obj.Package_Title;
            button3.Content = MAUIStr.Obj.Texture_Title;
            button4.Content = MAUIStr.Obj.Reanim_Title;
            button5.Content = MAUIStr.Obj.Particles_Title;
            button6.Content = MAUIStr.Obj.Trail_Title;
            button7.Content = MAUIStr.Obj.RTON_Title;
            button8.Content = MAUIStr.Obj.Compress_Title;
            button9.Content = MAUIStr.Obj.LuaScript_Title;
            button10.Content = MAUIStr.Obj.Setting_Title;
            LoadHomePage();
            Singleten = this;
            if (Setting.OpenProgramAD)
            {
                int randomNumber = new Random().Next(1, 4);
                byte[] img;
                string url;
                switch (randomNumber)
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
                _ = Plugin.Dialogs.DisplayPicture(MAUIStr.Obj.AD_Title, GetImage(img), MAUIStr.Obj.AD_Cancel, () => System.Diagnostics.Process.Start("explorer.exe", url), true);
            }
        }

        BitmapImage GetImage(byte[] ary)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = new MemoryStream(ary);
            bmp.EndInit();
            return bmp;
        }
        #endregion

        #region Button

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LoadHomePage();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            LoadPackage();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            LoadTexture();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            LoadReanim();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            LoadParticles();
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            LoadTrail();
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            LoadRTON();
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            LoadCompress();
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            LoadLuaScript();
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            LoadSetting();
        }

        #endregion

        #region ShowPage

        Page_HomePage homePage = new Page_HomePage();
        Page_Package package = new Page_Package();
        Page_Texture texture = new Page_Texture();
        Page_Reanim reanim = new Page_Reanim();
        Page_Particles particles = new Page_Particles();
        Page_Trail trail = new Page_Trail();
        Page_RTON rton = new Page_RTON();
        Page_Compress compress = new Page_Compress();
        Page_LuaScript luaScript = new Page_LuaScript();
        Page_Setting setting = new Page_Setting();

        void ResetButton()
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
        }

        public void LoadHomePage()
        {
            ResetButton();
            button1.Background = Brushes.CornflowerBlue;
            button1.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = homePage
            };
            Label_Head.Content = MAUIStr.Obj.HomePage_Title;
        }

        public void LoadPackage()
        {
            ResetButton();
            button2.Background = Brushes.CornflowerBlue;
            button2.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = package
            };
            Label_Head.Content = MAUIStr.Obj.Package_Title;
        }

        public void LoadTexture()
        {
            ResetButton();
            button3.Background = Brushes.CornflowerBlue;
            button3.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = texture
            };
            Label_Head.Content = MAUIStr.Obj.Texture_Title;
        }

        public void LoadReanim()
        {
            ResetButton();
            button4.Background = Brushes.CornflowerBlue;
            button4.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = reanim
            };
            Label_Head.Content = MAUIStr.Obj.Reanim_Title;
        }

        public void LoadParticles()
        {
            ResetButton();
            button5.Background = Brushes.CornflowerBlue;
            button5.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = particles
            };
            Label_Head.Content = MAUIStr.Obj.Particles_Title;
        }

        public void LoadTrail()
        {
            ResetButton();
            button6.Background = Brushes.CornflowerBlue;
            button6.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = trail
            };
            Label_Head.Content = MAUIStr.Obj.Trail_Title;
        }

        public void LoadRTON()
        {
            ResetButton();
            button7.Background = Brushes.CornflowerBlue;
            button7.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = rton
            };
            Label_Head.Content = MAUIStr.Obj.RTON_Title;
        }

        public void LoadCompress()
        {
            ResetButton();
            button8.Background = Brushes.CornflowerBlue;
            button8.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = compress
            };
            Label_Head.Content = MAUIStr.Obj.Compress_Title;
        }

        public void LoadLuaScript()
        {
            ResetButton();
            button9.Background = Brushes.CornflowerBlue;
            button9.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = luaScript
            };
            Label_Head.Content = MAUIStr.Obj.LuaScript_Title;
        }

        public void LoadSetting()
        {
            ResetButton();
            button10.Background = Brushes.CornflowerBlue;
            button10.Foreground = Brushes.White;
            PageControl.Content = new Frame()
            {
                Content = setting
            };
            Label_Head.Content = MAUIStr.Obj.Setting_Title;
        }

        #endregion
    }
}
