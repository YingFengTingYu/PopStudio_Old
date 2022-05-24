using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PopStudio.Platform
{
    public partial class Control_Prompt : UserControl
    {
        public void SetInfo(string title, string message, string accept, string cancel, string initialValue)
        {
            this.title.Text = title;
            text.Text = message;
            anstext.Text = initialValue;
            ok.Content = accept.Replace("\0", string.Empty);
            this.cancel.Content = cancel.Replace("\0", string.Empty);
        }

        public Control_Prompt()
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
            anstext = this.Get<TextBox>("anstext");
            ok = this.Get<Button>("ok");
            cancel = this.Get<Button>("cancel");
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
