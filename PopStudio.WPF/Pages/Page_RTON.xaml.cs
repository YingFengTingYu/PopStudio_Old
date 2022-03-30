using PopStudio.GUILanguage.Languages;
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
    /// Page_RTON.xaml 的交互逻辑
    /// </summary>
    public partial class Page_RTON : Page
    {
        public Page_RTON()
        {
            InitializeComponent();
            Title = MAUIStr.Obj.RTON_Title;
            label_introduction.Text = MAUIStr.Obj.RTON_Introduction;
            label_choosemode.Text = MAUIStr.Obj.Share_ChooseMode;
            label_mode1.Text = MAUIStr.Obj.RTON_Mode1;
            label_mode2.Text = MAUIStr.Obj.RTON_Mode2;
            text1.Text = MAUIStr.Obj.RTON_Choose1;
            text2.Text = MAUIStr.Obj.RTON_Choose2;
            text3.Text = MAUIStr.Obj.RTON_Choose3;
            button1.Content = MAUIStr.Obj.Share_Choose;
            button2.Content = MAUIStr.Obj.Share_Choose;
            button_run.Content = MAUIStr.Obj.Share_Run;
            label_statue.Text = MAUIStr.Obj.Share_RunStatue;
            text4.Text = MAUIStr.Obj.Share_Waiting;
            CB_CMode.Items.Clear();
            CB_CMode.Items.Add("Simple RTON");
            CB_CMode.Items.Add("Encrypted RTON");
            CB_CMode.SelectedIndex = 0;
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            if (((ToggleButton)sender).IsChecked == true)
            {
                text1.Text = MAUIStr.Obj.RTON_Choose4;
                text2.Text = MAUIStr.Obj.RTON_Choose5;
                text3.Text = MAUIStr.Obj.RTON_Choose6;
                (textbox2.Text, textbox1.Text) = (textbox1.Text, textbox2.Text);
            }
            else
            {
                text1.Text = MAUIStr.Obj.RTON_Choose1;
                text2.Text = MAUIStr.Obj.RTON_Choose2;
                text3.Text = MAUIStr.Obj.RTON_Choose3;
                (textbox2.Text, textbox1.Text) = (textbox1.Text, textbox2.Text);
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
            new Thread(new ThreadStart(() =>
            {
                string err = null;
                try
                {
                    if (!File.Exists(inFile))
                    {
                        throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
                    }
                    if (mode == true)
                    {
                        API.EncodeRTON(inFile, outFile, cmode);
                    }
                    else
                    {
                        API.DecodeRTON(inFile, outFile, cmode);
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
