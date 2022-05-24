using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PopStudio.Language.Languages;

namespace PopStudio.Avalonia.Pages
{
    public partial class Control_HomePage : UserControl
    {
        public Control_HomePage()
        {
            InitializeComponent();
            LoadControl();
            LoadFont();
            MAUIStr.OnLanguageChanged += LoadFont;
        }

        ~Control_HomePage()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        void LoadControl()
        {
            label_begin = this.Get<TextBlock>("label_begin");
            label_function = this.Get<TextBlock>("label_function");
            label_agreement = this.Get<TextBlock>("label_agreement");
            label_ver = this.Get<TextBlock>("label_ver");
            label_author_string = this.Get<TextBlock>("label_author_string");
            label_author = this.Get<TextBlock>("label_author");
            label_thanks_string = this.Get<TextBlock>("label_thanks_string");
            label_thanks = this.Get<TextBlock>("label_thanks");
            label_qqgroup_string = this.Get<TextBlock>("label_qqgroup_string");
            label_qqgroup = this.Get<TextBlock>("label_qqgroup");
            label_course_string = this.Get<TextBlock>("label_course_string");
            label_course = this.Get<TextBlock>("label_course");
            label_appnewnotice_string = this.Get<TextBlock>("label_appnewnotice_string");
            label_appnewnotice = this.Get<TextBlock>("label_appnewnotice");
        }

        void LoadFont()
        {
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
    }
}
