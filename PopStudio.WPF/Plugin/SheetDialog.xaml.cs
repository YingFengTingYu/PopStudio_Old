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
    /// SheetDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SheetDialog : Page
    {
        string[] items;
        string cancelstring;
        string okstring;

        public SheetDialog(string title, string cancel, string ok, params string[] items)
        {
            InitializeComponent();
            int l = items.Length;
            Height = Math.Min(l * 30 + 105, 400);
            this.title.Text = title;
            this.items = items;
            okstring = ok;
            cancelstring = cancel;
            this.ok.Content = ok;
            this.cancel.Content = cancel;
            List<SingleItem> items2 = new List<SingleItem>();
            for (int i = 0; i < l; i++)
            {
                items2.Add(new SingleItem(items[i]));
            }
            list.ItemsSource = items2;
            list.SelectedIndex = 0;
            if (Height == 400) list.Height = 295;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex < 0 || items.Length <= 0)
            {
                MainWindow.Singleten.Result = okstring;
            }
            else
            {
                MainWindow.Singleten.Result = items[list.SelectedIndex];
            }
            MainWindow.Singleten.CloseDialog();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Singleten.Result = cancelstring;
            MainWindow.Singleten.CloseDialog();
        }

        class SingleItem
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
