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
using PopStudio.WPF.Languages;

namespace PopStudio.WPF.Pages
{
    /// <summary>
    /// Page_Reanim.xaml 的交互逻辑
    /// </summary>
    public partial class Page_LuaScript : Page
    {
        public Page_LuaScript()
        {
            InitializeComponent();
            Title = MAUIStr.Obj.LuaScript_Title;
            label_introduction.Text = MAUIStr.Obj.LuaScript_Introduction;
            label_print.Text = MAUIStr.Obj.LuaScript_TracePrint;
            button_run.Content = MAUIStr.Obj.Share_Run;
            API.box = richtextbox2;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
        }

        private void button_run_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            richtextbox2.Document.Blocks.Clear();
            string script = new TextRange(richtextbox1.Document.ContentStart, richtextbox1.Document.ContentEnd).Text;
            new Thread(new ThreadStart(() =>
            {
                bool cg = true;
                try
                {
                    API.DoScript(script);
                }
                catch (Exception ex)
                {
                    cg = false;
                    API.Print(string.Format(MAUIStr.Obj.Share_Wrong, ex.Message));
                }
                if (cg)
                {
                    API.Print(MAUIStr.Obj.Share_Finish);
                }
                Dispatcher.BeginInvoke(() => { b.IsEnabled = true; });
            }))
            { IsBackground = true }.Start();
        }
    }
}
