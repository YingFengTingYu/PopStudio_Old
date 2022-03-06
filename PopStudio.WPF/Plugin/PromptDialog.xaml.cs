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
    public partial class PromptDialog : Page
    {
        public PromptDialog(string title, string message, string accept, string cancel, string initialValue)
        {
            InitializeComponent();
            this.title.Text = title;
            text.Text = message;
            anstext.Text = initialValue;
            ok.Content = accept;
            this.cancel.Content = cancel;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Singleten.Result = anstext.Text;
            MainWindow.Singleten.CloseDialog();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Singleten.Result = "";
            MainWindow.Singleten.CloseDialog();
        }
    }
}
