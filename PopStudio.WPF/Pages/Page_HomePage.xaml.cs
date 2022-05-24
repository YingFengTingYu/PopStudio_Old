using System.Windows.Controls;
using System.Windows.Input;
using PopStudio.Language.Languages;

namespace PopStudio.WPF.Pages
{
    /// <summary>
    /// Page_HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class Page_HomePage : Page
    {
        void LoadFont()
        {
            Title = MAUIStr.Obj.HomePage_Title;
            label_begin.Text = MAUIStr.Obj.HomePage_Begin;
            label_function.Text = MAUIStr.Obj.HomePage_Function;
            label_agreement.Text = MAUIStr.Obj.HomePage_Agreement;
            label_ver.Text = string.Format(MAUIStr.Obj.HomePage_Version, Str.Obj.AppVersion);
            label_author_string.Text = MAUIStr.Obj.HomePage_Author_String;
            label_author.Text = MAUIStr.Obj.HomePage_Author;
            label_thanks_string.Text = MAUIStr.Obj.HomePage_Thanks_String;
            label_thanks.Text = MAUIStr.Obj.HomePage_Thanks;
            label_qqgroup_string.Text = MAUIStr.Obj.HomePage_QQGroup_String;
            label_qqgroup.Text = MAUIStr.Obj.HomePage_QQGroup;
            label_course_string.Text = MAUIStr.Obj.HomePage_Course_String;
            label_course.Text = MAUIStr.Obj.HomePage_Course;
            label_appnewnotice_string.Text = MAUIStr.Obj.HomePage_AppNewNotice_String;
            label_appnewnotice.Text = MAUIStr.Obj.HomePage_AppNewNotice;
        }

        public Page_HomePage()
        {
            InitializeComponent();
            LoadFont();
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Page_HomePage()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
		}

        private void label_course_MouseUp(object sender, MouseButtonEventArgs e)
        {
			System.Diagnostics.Process.Start("explorer.exe", MAUIStr.Obj.HomePage_Course);
		}
    }
}
