using PopStudio.GUI.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PopStudio.WPF.Pages
{
    /// <summary>
    /// Page_Texture.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Texture : Page
    {
        public Page_Texture()
        {
            InitializeComponent();
            Title = MAUIStr.Obj.Texture_Title;
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
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("PTX(rsb)");
            CB_CMode.Items.Add("cdat(Android,iOS)");
            CB_CMode.Items.Add("tex(iOS)");
            CB_CMode.Items.Add("txz(Android,iOS)");
            CB_CMode.Items.Add("tex(TV)");
            CB_CMode.Items.Add("ptx(Xbox360)");
            CB_CMode.Items.Add("ptx(PS3)");
            CB_CMode.Items.Add("ptx(PSV)");
            CB_CMode.Items.Add("xnb(Windows Phone)");
            CB_CMode.SelectedIndex = 0;
            CB_FMode.Items.Clear();
            CB_FMode.Items.Add("ARGB8888(0)");
            CB_FMode.Items.Add("ABGR8888(0)");
            CB_FMode.Items.Add("RGBA4444(1)");
            CB_FMode.Items.Add("RGB565(2)");
            CB_FMode.Items.Add("RGBA5551(3)");
            CB_FMode.Items.Add("RGBA4444_Block(21)");
            CB_FMode.Items.Add("RGB565_Block(22)");
            CB_FMode.Items.Add("RGBA5551_Block(23)");
            CB_FMode.Items.Add("XRGB8888_A8(149)");
            CB_FMode.Items.Add("ARGB8888(BE)(0)");
            CB_FMode.Items.Add("ARGB8888_Padding(BE)(0)");
            CB_FMode.Items.Add("DXT1_RGB(35)");
            CB_FMode.Items.Add("DXT3_RGBA(36)");
            CB_FMode.Items.Add("DXT5_RGBA(37)");
            CB_FMode.Items.Add("DXT5(5)");
            CB_FMode.Items.Add("DXT5(BE)(5)");
            CB_FMode.Items.Add("ETC1_RGB(32)");
            CB_FMode.Items.Add("ETC1_RGB_A8(147)");
            CB_FMode.Items.Add("ETC1_RGB_A_Palette(147)");
            CB_FMode.Items.Add("ETC1_RGB_A_Palette(150)");
            CB_FMode.Items.Add("PVRTC_4BPP_RGBA(30)");
            CB_FMode.Items.Add("PVRTC_4BPP_RGB_A8(148)");
            CB_FMode.SelectedIndex = 0;
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            if (((ToggleButton)sender).IsChecked == true)
            {
                text1.Text = MAUIStr.Obj.Texture_Choose4;
                text2.Text = MAUIStr.Obj.Texture_Choose5;
                text3.Text = MAUIStr.Obj.Texture_Choose6;
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
                SP_FMode.Visibility = Visibility.Visible;
            }
            else
            {
                text1.Text = MAUIStr.Obj.Texture_Choose1;
                text2.Text = MAUIStr.Obj.Texture_Choose2;
                text3.Text = MAUIStr.Obj.Texture_Choose3;
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
                SP_FMode.Visibility = Visibility.Collapsed;
            }
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
            int fmode = CB_FMode.SelectedIndex;
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
                Dispatcher.Invoke(() =>
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

        private void CB_CMode_Selected(object sender, EventArgs e)
        {
            int index = CB_CMode.SelectedIndex;
            CB_FMode.Items.Clear();
            if (index == 0)
            {
                CB_FMode.Items.Add("ARGB8888(0)");
                CB_FMode.Items.Add("ABGR8888(0)");
                CB_FMode.Items.Add("RGBA4444(1)");
                CB_FMode.Items.Add("RGB565(2)");
                CB_FMode.Items.Add("RGBA5551(3)");
                CB_FMode.Items.Add("RGBA4444_Block(21)");
                CB_FMode.Items.Add("RGB565_Block(22)");
                CB_FMode.Items.Add("RGBA5551_Block(23)");
                CB_FMode.Items.Add("XRGB8888_A8(149)");
                CB_FMode.Items.Add("ARGB8888(BE)(0)");
                CB_FMode.Items.Add("ARGB8888_Padding(BE)(0)");
                CB_FMode.Items.Add("DXT1_RGB(35)");
                CB_FMode.Items.Add("DXT3_RGBA(36)");
                CB_FMode.Items.Add("DXT5_RGBA(37)");
                CB_FMode.Items.Add("DXT5(5)");
                CB_FMode.Items.Add("DXT5(BE)(5)");
                CB_FMode.Items.Add("ETC1_RGB(32)");
                CB_FMode.Items.Add("ETC1_RGB_A8(147)");
                CB_FMode.Items.Add("ETC1_RGB_A_Palette(147)");
                CB_FMode.Items.Add("ETC1_RGB_A_Palette(150)");
                CB_FMode.Items.Add("PVRTC_4BPP_RGBA(30)");
                CB_FMode.Items.Add("PVRTC_4BPP_RGB_A8(148)");
            }
            else if (index == 1)
            {
                CB_FMode.Items.Add("Encrypt");
            }
            else if (index == 2 || index == 3)
            {
                CB_FMode.Items.Add("ABGR8888(1)");
                CB_FMode.Items.Add("RGBA4444(2)");
                CB_FMode.Items.Add("RGBA5551(3)");
                CB_FMode.Items.Add("RGB565(4)");
            }
            else if (index == 4)
            {
                CB_FMode.Items.Add("LUT8(1)(Invalid)");
                CB_FMode.Items.Add("ARGB8888(2)");
                CB_FMode.Items.Add("ARGB4444(3)");
                CB_FMode.Items.Add("ARGB1555(4)");
                CB_FMode.Items.Add("RGB565(5)");
                CB_FMode.Items.Add("ABGR8888(6)");
                CB_FMode.Items.Add("RGBA4444(7)");
                CB_FMode.Items.Add("RGBA5551(8)");
                CB_FMode.Items.Add("XRGB8888(9)");
                CB_FMode.Items.Add("LA88(10)");
            }
            else if (index == 5)
            {
                CB_FMode.Items.Add("DXT5_RGBA_Padding(BE)");
            }
            else if (index == 6)
            {
                CB_FMode.Items.Add("DXT5_RGBA");
            }
            else if (index == 7)
            {
                CB_FMode.Items.Add("DXT5_RGBA_Morton");
            }
            else if (index == 8)
            {
                CB_FMode.Items.Add("ABGR8888");
            }
            CB_FMode.SelectedIndex = 0;
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
