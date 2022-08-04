using PopStudio.Language.Languages;
using PopStudio.Platform;

namespace PopStudio.MAUI
{
	public partial class Page_HomePage : ContentPage
	{
        void LoadFont()
        {
            Title = MAUIStr.Obj.HomePage_Title;
            label_begin.Text = MAUIStr.Obj.HomePage_Begin;
            label_function.Text = MAUIStr.Obj.HomePage_Function;
            label_agreement.Text = MAUIStr.Obj.HomePage_Agreement;
            label_permission.Text = MAUIStr.Obj.HomePage_Permission;
            button_permission.Text = MAUIStr.Obj.HomePage_PermissionAsk;
            button_close.Text = MAUIStr.Obj.Permission_Cancel;
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
            ShowDialog();
        }

        ~Page_HomePage()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        private async void ShowDialog()
        {
			if (!await Permission.CheckPermissionAsync())
            {
                AndroidPermission.IsVisible = true;
            }
		}

        int GetRandomNumber(int m, int n)
        {
            return 2;
        }

        private async void button_permission_Clicked(object sender, EventArgs e)
        {
            await this.CheckAndRequestPermissionAsync();
        }

        private void button_close_Clicked(object sender, EventArgs e)
        {
            AndroidPermission.IsVisible = false;
        }
    }
}