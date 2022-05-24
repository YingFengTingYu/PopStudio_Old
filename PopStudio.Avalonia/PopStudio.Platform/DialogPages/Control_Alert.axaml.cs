using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PopStudio.Platform
{
    public partial class Control_Alert : UserControl
    {
        public void SetInfo(string title, string message, string cancel)
        {
            this.title.Text = title;
            text.Text = message;
            board1.IsVisible = false;
            board2.IsVisible = true;
            cancel2.Content = cancel.Replace("\0", string.Empty);
        }

        public void SetInfo(string title, string message, string accept, string cancel)
        {
            this.title.Text = title;
            text.Text = message;
            board1.IsVisible = true;
            board2.IsVisible = false;
            ok.Content = accept.Replace("\0", string.Empty);
            this.cancel.Content = cancel.Replace("\0", string.Empty);
        }

        public Control_Alert()
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
            text = this.Get<TextBlock>("text");
            board1 = this.Get<Grid>("board1");
            ok = this.Get<Button>("ok");
            cancel = this.Get<Button>("cancel");
            board2 = this.Get<Grid>("board2");
            cancel2 = this.Get<Button>("cancel2");
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
