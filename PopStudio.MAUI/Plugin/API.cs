using PopStudio.MAUI;

namespace PopStudio.Plugin
{
    internal static partial class API
    {
        public static Page_LuaScript box;

        public static partial void Print(params object[] os)
        {
            string str = string.Empty;
            if (os.Length != 0)
            {
                string nil = "nil";
                for (int i = 0; i < os.Length; i++)
                {
                    str += ((os[i]?.ToString()) ?? nil) + ' ';
                }
                str = str[0..^1];
            }
            box.Dispatcher.Dispatch(() => box.Print(str));
        }

        public static partial bool? Alert(string text, string title, bool ask)
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

        public static partial string Prompt(string text, string title, string defaulttext)
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.Dispatch(async () => { ans = await box.DisplayPromptAsync(title, text, initialValue: defaulttext); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial string Sheet(string title, params string[] items)
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

        public static partial string ChooseFolder()
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.Dispatch(async () => { ans = await box.ChooseFolder(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial string ChooseOpenFile()
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.Dispatch(async () => { ans = await box.ChooseOpenFile(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial string ChooseSaveFile()
        {
            string ans = null;
            bool a = false;
            box.Dispatcher.Dispatch(async () => { ans = await box.ChooseSaveFile(); a = true; });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial void OpenUrl(string url) => Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
    }
}

