using System.Windows;
using System.Windows.Controls;

namespace PopStudio.Platform
{
    /// <summary>
    /// Page_Alert.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Alert : Page
    {
        public void SetInfo(string title, string message, string cancel)
        {
            this.title.Text = title;
            text.Text = message;
            board1.Visibility = Visibility.Hidden;
            board2.Visibility = Visibility.Visible;
            cancel2.Content = cancel.Replace("\0", string.Empty);
        }

        public void SetInfo(string title, string message, string accept, string cancel)
        {
            this.title.Text = title;
            text.Text = message;
            board1.Visibility = Visibility.Visible;
            board2.Visibility = Visibility.Hidden;
            ok.Content = accept.Replace("\0", string.Empty);
            this.cancel.Content = cancel.Replace("\0", string.Empty);
        }

        public Page_Alert()
        {
            InitializeComponent();
        }

        bool result;
        public bool Result => result;
        public event Action Close;

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            result = true;
            Close?.Invoke();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            Close?.Invoke();
        }
    }
}
