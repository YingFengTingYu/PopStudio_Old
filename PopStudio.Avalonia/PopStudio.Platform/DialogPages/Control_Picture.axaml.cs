using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using Imaging = Avalonia.Media.Imaging;
using Avalonia.Interactivity;

namespace PopStudio.Platform
{
    public partial class Control_Picture : UserControl
    {
        Action Tap;
        public event Action Close;
        bool touchLeave;

        public void SetInfo(string title, byte[] img, string cancel, Action action, bool TouchLeave)
        {
            this.title.Text = title;
            using (MemoryStream ms = new MemoryStream(img))
            {
                pic.Source = new Imaging.Bitmap(ms);
            }
            Tap = action;
            touchLeave = TouchLeave;
            if (action != null || TouchLeave)
            {
                pic.Cursor = new Cursor(StandardCursorType.Hand);
            }
            else
            {
                pic.Cursor = new Cursor(StandardCursorType.Arrow);
            }
            this.cancel.Content = cancel.Replace("\0", string.Empty);
        }

        private void Pic_Tapped(object sender, RoutedEventArgs e)
        {
            Tap?.Invoke();
            if (touchLeave) Close?.Invoke();
        }

        public Control_Picture()
        {
            InitializeComponent();
            LoadControl();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        void LoadControl()
        {
            title = this.Get<TextBlock>("title");
            pic = this.Get<global::Avalonia.Controls.Image>("pic");
            cancel = this.Get<Button>("cancel");
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close?.Invoke();
        }
    }
}
