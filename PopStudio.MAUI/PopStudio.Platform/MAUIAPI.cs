using PopStudio.MAUI;
using System.Text;

namespace PopStudio.Platform
{
    internal class MAUIAPI : API
    {
        Page_LuaScript box;

        public override void InternalLoadTextBox(object o)
        {
            if (o is Page_LuaScript b)
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
            box.Dispatcher.Dispatch(() => box.Print(str.ToString()));
        }

        public override bool? InternalAlert(string text, string title, bool ask)
        {
            bool? ans = null;
            bool a = false;
            if (ask)
            {
                box.Dispatcher.Dispatch(async () => { ans = await box.DisplayAlert(title, text, "OK", "Cancel"); a = true; });
            }
            else
            {
                box.Dispatcher.Dispatch(async () => { await box.DisplayAlert(title, text, "Cancel"); a = true; });
            }
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalPrompt(string text, string title, string defaulttext)
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.Dispatch(async () => { ans = await box.DisplayPromptAsync(title, text, initialValue: defaulttext); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalSheet(string title, params string[] items)
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.Dispatch(async () => { ans = await box.DisplayActionSheet(title, "Cancel\0", "OK\0", items); a = true; });
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
            box.Dispatcher.Dispatch(async () => { ans = await box.ChooseFolder(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalChooseOpenFile()
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.Dispatch(async () => { ans = await box.ChooseOpenFile(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override string InternalChooseSaveFile()
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.Dispatch(async () => { ans = await box.ChooseSaveFile(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public override void InternalOpenUrl(string url) => Permission.OpenUrl(url);
    }
}
