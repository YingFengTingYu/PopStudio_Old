using System.Windows;
using System.Windows.Controls;

namespace PopStudio.Platform
{
    /// <summary>
    /// Page_ActionSheet.xaml 的交互逻辑
    /// </summary>
    public partial class Page_ActionSheet : Page
    {
        public void SetInfo(string title, string cancel, string ok, params string[] items)
        {
            int l = items.Length;
            this.title.Text = title;
            this.items = items;
            okstring = ok;
            cancelstring = cancel;
            this.ok.Content = ok.Replace("\0", string.Empty);
            this.cancel.Content = cancel.Replace("\0", string.Empty);
            List<SingleItem> items2 = new List<SingleItem>();
            for (int i = 0; i < l; i++)
            {
                items2.Add(new SingleItem(items[i]));
            }
            list.ItemsSource = items2;
            list.SelectedIndex = 0;
        }

        public Page_ActionSheet()
        {
            InitializeComponent();
        }

        public string result;
        string[] items;
        string cancelstring;
        string okstring;
        public event Action Close;
        public string Result => result;

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex < 0 || items.Length <= 0)
            {
                result = okstring;
            }
            else
            {
                result = items[list.SelectedIndex];
            }
            Close?.Invoke();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            result = cancelstring;
            Close?.Invoke();
        }

        private class SingleItem
        {
            public string ItemName { get; set; }

            public SingleItem()
            {

            }

            public SingleItem(string ItemName)
            {
                this.ItemName = ItemName;
            }
        }
    }
}
