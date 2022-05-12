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
    /// Page_Atlas.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Atlas : Page
    {
        public Page_Atlas()
        {
            InitializeComponent();
            Title = MAUIStr.Obj.Atlas_Title;
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
            CB_Mode.Items.Clear();
            CB_Mode.Items.Add("RESOURCES.XML(Rsb)");
            CB_Mode.Items.Add("resources.xml(Old)");
            CB_Mode.Items.Add("resources.xml(Ancient)");
            CB_Mode.Items.Add("plist(Free)");
            CB_Mode.Items.Add("atlasimagemap.dat");
            CB_Mode.Items.Add("xml(TV)");
            CB_Mode.Items.Add("RESOURCES.RTON(Rsb)");
            CB_Mode.SelectedIndex = 0;
            CB_MaxWidth.Items.Clear();
            CB_MaxWidth.Items.Add("256");
            CB_MaxWidth.Items.Add("512");
            CB_MaxWidth.Items.Add("1024");
            CB_MaxWidth.Items.Add("2048");
            CB_MaxWidth.Items.Add("4096");
            CB_MaxWidth.Items.Add("8192");
            CB_MaxWidth.SelectedIndex = 3;
            CB_MaxHeight.Items.Clear();
            CB_MaxHeight.Items.Add("256");
            CB_MaxHeight.Items.Add("512");
            CB_MaxHeight.Items.Add("1024");
            CB_MaxHeight.Items.Add("2048");
            CB_MaxHeight.Items.Add("4096");
            CB_MaxHeight.Items.Add("8192");
            CB_MaxHeight.SelectedIndex = 3;
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            if (((ToggleButton)sender).IsChecked == true)
            {
                text1.Text = MAUIStr.Obj.Atlas_Choose5;
                text2.Text = MAUIStr.Obj.Atlas_Choose6;
                text4.Text = MAUIStr.Obj.Atlas_Choose7;
                splice_size.Visibility = Visibility.Visible;
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
            }
            else
            {
                text1.Text = MAUIStr.Obj.Atlas_Choose1;
                text2.Text = MAUIStr.Obj.Atlas_Choose2;
                text4.Text = MAUIStr.Obj.Atlas_Choose4;
                splice_size.Visibility = Visibility.Collapsed;
                string temp = textbox1.Text;
                textbox1.Text = textbox2.Text;
                textbox2.Text = temp;
            }
        }

        private void Button_Click(object sender, EventArgs e)
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
                Dispatcher.Invoke(() =>
                {
                    if (err == null)
                    {
                        text5.Text = MAUIStr.Obj.Share_Finish;
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                string val;
                if (TB_Mode.IsChecked == true)
                {
                    val = this.ChooseFolder(); //Can't default this
                }
                else
                {
                    val = this.ChooseOpenFile(); //Can't default this
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
                if (TB_Mode.IsChecked == true)
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

        private void Button3_Clicked(object sender, EventArgs e)
        {
            try
            {
                string val = this.ChooseOpenFile(); //Can't default this
                if (!string.IsNullOrEmpty(val)) textbox3.Text = val;
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
