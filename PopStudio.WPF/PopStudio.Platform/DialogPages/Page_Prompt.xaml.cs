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

namespace PopStudio.Platform
{
    /// <summary>
    /// Page_Prompt.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Prompt : Page
    {
        public void SetInfo(string title, string message, string accept, string cancel, string initialValue)
        {
            this.title.Text = title;
            text.Text = message;
            anstext.Text = initialValue;
            ok.Content = accept.Replace("\0", string.Empty);
            this.cancel.Content = cancel.Replace("\0", string.Empty);
        }

        public Page_Prompt()
        {
            InitializeComponent();
        }

        string result;
        public string Result => result;
        public event Action Close;

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            result = anstext.Text;
            Close?.Invoke();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            result = null;
            Close?.Invoke();
        }
    }
}
