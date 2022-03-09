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
    /// Page_HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class Page_HomePage : Page
    {
        public Page_HomePage()
        {
            InitializeComponent();
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
