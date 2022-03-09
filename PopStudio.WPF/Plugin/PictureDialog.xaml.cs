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
using System.Windows.Shapes;

namespace PopStudio.WPF.Plugin
{
    /// <summary>
    /// PictureDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PictureDialog : Page
    {
        public PictureDialog(string title, BitmapImage img, string cancel, Action action, bool TouchLeave)
        {
            InitializeComponent();
            this.title.Text = title;
            pic.Source = img;
            if (action != null)
            {
                pic.Cursor = Cursors.Hand;
                pic.MouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs args) => action());
            }
            if (TouchLeave)
            {
                pic.MouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs args) => cancel_Click(null, null));
            }
            this.cancel.Content = cancel;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Singleten.Result = true;
            MainWindow.Singleten.CloseDialog();
        }
    }
}
