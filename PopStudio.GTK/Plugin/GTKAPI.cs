using Gtk;
using PopStudio.GTK;
using PopStudio.GTK.Plugin;
using System.Text;

namespace PopStudio.Platform
{
    internal class GTKAPI : YFAPI
    {
        TextView box;

        public override void InternalLoadTextBox(object o)
        {
            if (o is TextView b)
            {
                box = b;
            }
        }

        public override void InternalPrint(params object[] os)
        {
            StringBuilder str = new StringBuilder();
            if (os.Length != 0)
            {
                string nil = "nil";
                for (int i = 0; i < os.Length; i++)
                {
                    str.Append((os[i]?.ToString()) ?? nil);
                    str.Append(' ');
                }
                str.Remove(str.Length - 1, 1);
            }
            str.Append('\n');
            Application.Invoke((s, e) => box.Buffer.Text += str);
        }

        public override bool? InternalAlert(string text, string title, bool ask)
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
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalPrompt(string text, string title, string defaulttext)
        {
            string ans = string.Empty;
            bool a = false;
            Application.Invoke((s, e) =>
            {
                ans = Dialogs.DisplayPromptAsync(title, text, initialValue: defaulttext);
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalSheet(string title, params string[] items)
        {
            string ans = null;
            bool a = false;
            Application.Invoke((s, e) =>
            {
                ans = Dialogs.DisplayActionSheet(title, "Cancel", "OK", items);
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalChooseFolder()
        {
            string ans = null;
            bool a = false;
            Application.Invoke((s, e) => { ans = PickFile.ChooseFolder(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalChooseOpenFile()
        {
            string ans = null;
            bool a = false;
            Application.Invoke((s, e) => { ans = PickFile.ChooseOpenFile(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalChooseSaveFile()
        {
            string ans = null;
            bool a = false;
            Application.Invoke((s, e) => { ans = PickFile.ChooseSaveFile(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override void InternalOpenUrl(string url) => Permission.OpenUrl(url);
    }
}

