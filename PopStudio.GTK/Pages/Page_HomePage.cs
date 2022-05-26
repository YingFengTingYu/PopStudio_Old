using Gtk;
using PopStudio.Language.Languages;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK.Pages
{
    internal class Page_HomePage : Plugin.Page
    {
        Label label_title;
        Label label_begin;
        Label label_function;
        Label label_agreement;
        Label label_ver;
        Label label_author_string;
        Label label_author;
        Label label_thanks_string;
        Label label_thanks;
        Label label_qqgroup_string;
        Label label_qqgroup;
        Label label_course_string;
        Label label_course;
        Label label_appnewnotice_string;
        Label label_appnewnotice;

        public Page_HomePage()
        {
            label_title = CreateTitle("PopStudio");
            label_begin = CreateSubTitle(MAUIStr.Obj.HomePage_Begin);
            label_function = CreateText(MAUIStr.Obj.HomePage_Function);
            label_agreement = CreateText(MAUIStr.Obj.HomePage_Agreement);
            label_ver = CreateText(string.Format(MAUIStr.Obj.HomePage_Version, Str.Obj.AppVersion));
            label_author_string = CreateText(MAUIStr.Obj.HomePage_Author_String);
            label_author = CreateText(MAUIStr.Obj.HomePage_Author);
            label_thanks_string = CreateText(MAUIStr.Obj.HomePage_Thanks_String);
            label_thanks = CreateText(MAUIStr.Obj.HomePage_Thanks);
            label_qqgroup_string = CreateText(MAUIStr.Obj.HomePage_QQGroup_String);
            label_qqgroup = CreateText(MAUIStr.Obj.HomePage_QQGroup);
            label_course_string = CreateText(MAUIStr.Obj.HomePage_Course_String);
            label_course = CreateText(MAUIStr.Obj.HomePage_Course);
            label_appnewnotice_string = CreateText(MAUIStr.Obj.HomePage_AppNewNotice_String);
            label_appnewnotice = CreateText(MAUIStr.Obj.HomePage_AppNewNotice);
            PackStart(label_title, false, false, 5);
            PackStart(label_begin, false, false, 10);
            PackStart(label_function, false, false, 5);
            PackStart(label_agreement, false, false, 5);
            PackStart(label_ver, false, false, 5);
            PackStart(label_author_string, false, false, 5);
            PackStart(label_author, false, false, 5);
            PackStart(label_thanks_string, false, false, 5);
            PackStart(label_thanks, false, false, 5);
            PackStart(label_qqgroup_string, false, false, 5);
            PackStart(label_qqgroup, false, false, 5);
            PackStart(label_course_string, false, false, 5);
            PackStart(label_course, false, false, 5);
            PackStart(label_appnewnotice_string, false, false, 5);
            PackStart(label_appnewnotice, false, false, 5);
        }

        static Label CreateTitle(string title)
        {
            Label l = new Label(title);
            l.Wrap = true;
            l.ModifyFont(GenFont(20));
            return l;
        }

        static Label CreateSubTitle(string subtitle)
        {
            Label l = new Label(subtitle);
            l.Wrap = true;
            l.ModifyFont(GenFont(14));
            return l;
        }
        
        static Label CreateText(string text)
        {
            Label l = new Label(text);
            l.LineWrap = true;
            l.LineWrapMode = Pango.WrapMode.Char;
            l.ModifyFont(GenFont(11));
            l.Xalign = 0;
            return l;
        }
    }
}
