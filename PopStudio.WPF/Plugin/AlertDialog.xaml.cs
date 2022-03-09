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
    /// AlertDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AlertDialog : Page
    {
        public AlertDialog(string title, string message, string cancel)
        {
            InitializeComponent();
            this.title.Text = title;
            text.Text = message;
            board1.Visibility = Visibility.Hidden;
            cancel2.Content = cancel;
        }

        public AlertDialog(string title, string message, string accept, string cancel)
        {
            InitializeComponent();
            this.title.Text = title;
            text.Text = message;
            board2.Visibility = Visibility.Hidden;
            ok.Content = accept;
            this.cancel.Content = cancel;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Singleten.Result = true;
            MainWindow.Singleten.CloseDialog();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Singleten.Result = false;
            MainWindow.Singleten.CloseDialog();
        }
    }
}
