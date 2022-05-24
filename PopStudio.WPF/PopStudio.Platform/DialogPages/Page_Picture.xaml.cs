using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PopStudio.Platform
{
    /// <summary>
    /// Page_Picture.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Picture : Page
    {
        Action Tap;
        public event Action Close;
        bool touchLeave;
        MemoryStream ms;

        public void SetInfo(string title, byte[] img, string cancel, Action action, bool TouchLeave)
        {
            ms?.Dispose();
            this.title.Text = title;
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = (ms = new MemoryStream(img));
            bmp.EndInit();
            pic.Source = bmp;
            Tap = action;
            touchLeave = TouchLeave;
            if (action != null || TouchLeave)
            {
                pic.Cursor = Cursors.Hand;
            }
            else
            {
                pic.Cursor = Cursors.Arrow;
            }
            this.cancel.Content = cancel.Replace("\0", string.Empty);
        }

        private void Pic_Tapped(object sender, MouseButtonEventArgs e)
        {
            Tap?.Invoke();
            if (touchLeave) Close?.Invoke();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close?.Invoke();
        }

        public Page_Picture()
        {
            InitializeComponent();
            Close += () => ms?.Dispose();
        }
    }
}