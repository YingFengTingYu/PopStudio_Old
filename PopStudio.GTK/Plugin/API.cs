using Gtk;
using PopStudio.GTK;
using PopStudio.GTK.Plugin;

namespace PopStudio.Plugin
{
    internal static partial class API
    {
        public static TextView box;
        public static bool FirstTime = true;

        public static partial void Print(params object[] os)
        {
            string str = FirstTime ? string.Empty : "\n";
            FirstTime = false;
            if (os.Length != 0)
            {
                string nil = "nil";
                for (int i = 0; i < os.Length; i++)
                {
                    str += ((os[i]?.ToString()) ?? nil) + ' ';
                }
                str = str[0..^1];
            }
            Application.Invoke((s, e) => box.Buffer.Text += str);
        }

        public static partial bool? Alert(string text, string title, bool ask)
        {
            bool? ans = null;
            bool a = false;
            if (ask)
            {
                Application.Invoke((s, e) =>
                {
                    ans = Dialogs.DisplayAlert(title, text, "OK", "Cancel");
                    a = true;
                });
            }
            else
            {
                Application.Invoke((s, e) =>
                {
                    Dialogs.DisplayAlert(title, text, "Cancel");
                    a = true;
                });
            }
            while (!a) Task.Delay(200);
            return ans;
        }

        public static partial string Prompt(string text, string title, string defaulttext)
        {
            string ans = string.Empty;
            bool a = false;
            Application.Invoke((s, e) =>
            {
                ans = Dialogs.DisplayPromptAsync(title, text, initialValue: defaulttext);
                a = true;
            });
            while (!a) Task.Delay(200);
            return ans;
        }

        public static partial string Sheet(string title, params string[] items)
        {
            string ans = null;
            bool a = false;
            Application.Invoke((s, e) =>
            {
                ans = Dialogs.DisplayActionSheet(title, "Cancel", "OK", items);
                a = true;
            });
            while (!a) Task.Delay(200);
            return ans;
        }

        public static partial string ChooseFolder()
        {
            string ans = null;
            bool a = false;
            Application.Invoke((s, e) => { ans = PickFile.ChooseFolder(); a = true; });
            while (!a) Task.Delay(200);
            return ans;
        }

        public static partial string ChooseOpenFile()
        {
            string ans = null;
            bool a = false;
            Application.Invoke((s, e) => { ans = PickFile.ChooseOpenFile(); a = true; });
            while (!a) Task.Delay(200);
            return ans;
        }

        public static partial string ChooseSaveFile()
        {
            string ans = null;
            bool a = false;
            Application.Invoke((s, e) => { ans = PickFile.ChooseSaveFile(); a = true; });
            while (!a) Task.Delay(200);
            return ans;
        }
    }
}

