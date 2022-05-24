using Avalonia.Controls;
using Avalonia.Threading;
using PopStudio.Avalonia;
using PopStudio.Platform;
using System.Text;

namespace PopStudio.Plugin
{
    internal static partial class API
    {
        public static TextBox box;
        public static bool FirstTime = true;

        public static partial void Print(params object[] os)
        {
            StringBuilder str = new StringBuilder();
            str.Append(FirstTime ? string.Empty : "\n");
            FirstTime = false;
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
            Dispatcher.UIThread.InvokeAsync(() => box.Text += str);
        }

        public static partial bool? Alert(string text, string title, bool ask)
        {
            bool? ans = null;
            bool a = false;
            if (ask)
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    ans = await PopupDialog.DisplayAlert(title, text, "OK\0", "Cancel\0");
                    a = true;
                });
            }
            else
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    await PopupDialog.DisplayAlert(title, text, "Cancel\0");
                    a = true;
                });
            }
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial string Prompt(string text, string title, string defaulttext)
        {
            string ans = string.Empty;
            bool a = false;
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                ans = await PopupDialog.DisplayPromptAsync(title, text, initialValue: defaulttext);
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial string Sheet(string title, params string[] items)
        {
            string ans = null;
            bool a = false;
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                ans = await PopupDialog.DisplayActionSheet(title, "Cancel\0", "OK\0", items);
                a = true;
            });
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
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    ans = await new OpenFolderDialog().ShowAsync(MainWindow.Singleten);
                }
                catch (Exception)
                {
                    ans = null;
                }
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial string ChooseOpenFile()
        {
            string ans = null;
            bool a = false;
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    ans = (await new OpenFileDialog().ShowAsync(MainWindow.Singleten))?[0];
                }
                catch (Exception)
                {
                    ans = null;
                }
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial string ChooseSaveFile()
        {
            string ans = null;
            bool a = false;
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    ans = await new SaveFileDialog().ShowAsync(MainWindow.Singleten);
                }
                catch (Exception)
                {
                    ans = null;
                }
                a = true;
            });
            while (!a) Thread.Sleep(200);
            return ans;
        }

        public static partial void OpenUrl(string url) => Permission.OpenUrl(url);
    }
}

