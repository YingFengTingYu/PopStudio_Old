using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PopStudio.Platform
{
    internal partial class WPFAPI : YFAPI
    {
        RichTextBox box;

        public override void InternalLoadTextBox(object o)
        {
            if (o is RichTextBox b)
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
            box.Dispatcher.BeginInvoke(() => { Paragraph para = new(); para.Inlines.Add(new Run(str.ToString())); box.Document.Blocks.Add(para); });
        }

        public override bool? InternalAlert(string text, string title, bool ask)
        {
            bool? ans = null;
            bool a = false;
            if (ask)
            {
                box.Dispatcher.BeginInvoke(async () => { ans = await PopupDialog.DisplayAlert(title, text, "OK", "Cancel"); a = true; });
            }
            else
            {
                box.Dispatcher.BeginInvoke(async () => { await PopupDialog.DisplayAlert(title, text, "Cancel"); a = true; });
            }
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalPrompt(string text, string title, string defaulttext)
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.BeginInvoke(async () => { ans = await PopupDialog.DisplayPromptAsync(title, text, initialValue: defaulttext); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalSheet(string title, params string[] items)
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.BeginInvoke(async () => { ans = await PopupDialog.DisplayActionSheet(title, "Cancel\0", "OK\0", items); a = true; });
            while (!a) Thread.Sleep(200);
            if (ans == "OK\0" && ans == "Cancel\0")
            {
                return null;
            }
            return ans;
        }

        public override string InternalChooseFolder()
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.BeginInvoke(() =>
            {
                ans = WPF.PickFile.ChooseFolder(null);
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalChooseOpenFile()
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.BeginInvoke(() =>
            {
                ans = WPF.PickFile.ChooseOpenFile(null);
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalChooseSaveFile()
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.BeginInvoke(() =>
            {
                ans = WPF.PickFile.ChooseSaveFile(null);
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override void InternalOpenUrl(string url) => Permission.OpenUrl(url);
    }
}

