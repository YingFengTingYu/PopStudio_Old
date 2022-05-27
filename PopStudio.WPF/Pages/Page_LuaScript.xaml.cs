using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using PopStudio.Language.Languages;
using PopStudio.Platform;

namespace PopStudio.WPF.Pages
{
    /// <summary>
    /// Page_Reanim.xaml 的交互逻辑
    /// </summary>
    public partial class Page_LuaScript : Page
    {
        void LoadFont()
        {
            Title = MAUIStr.Obj.LuaScript_Title;
            label_introduction.Text = MAUIStr.Obj.LuaScript_Introduction;
            label_print.Text = MAUIStr.Obj.LuaScript_TracePrint;
            button_run.Content = MAUIStr.Obj.Share_Run;
        }

        public Page_LuaScript()
        {
            InitializeComponent();
            LoadFont();
            YFAPI.LoadTextBox(richtextbox2);
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Page_LuaScript()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
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
                    YFAPI.DoScript(script);
                }
                catch (Exception ex)
                {
                    cg = false;
                    YFAPI.Print(string.Format(MAUIStr.Obj.Share_Wrong, ex.Message));
                }
                if (cg)
                {
                    YFAPI.Print(MAUIStr.Obj.Share_Finish_NoTime);
                }
                Dispatcher.BeginInvoke(() => { b.IsEnabled = true; });
            }))
            { IsBackground = true }.Start();
        }
    }
}
