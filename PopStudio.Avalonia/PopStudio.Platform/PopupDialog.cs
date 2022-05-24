using Avalonia.Controls;
using PopStudio.Avalonia;

namespace PopStudio.Platform
{
    internal static class PopupDialog
    {
        public static Task<string> DisplayActionSheet(string title, string cancel, string ok, params string[] items)
        {
            MainWindow CurrentActivity = MainWindow.Singleten;
            Control_ActionSheet actionSheet = new Control_ActionSheet();
            actionSheet.SetInfo(title, cancel, ok, items);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            actionSheet.Close += () =>
            {
                CurrentActivity.CloseDialog();
                tcs.SetResult(actionSheet.Result);
            };
            CurrentActivity.ShowDialog(actionSheet);
            return tcs.Task;
        }

        public static Task DisplayAlert(string title, string message, string cancel)
        {
            MainWindow CurrentActivity = MainWindow.Singleten;
            Control_Alert alert = new Control_Alert();
            alert.SetInfo(title, message, cancel);
            TaskCompletionSource tcs = new TaskCompletionSource();
            alert.Close += () =>
            {
                CurrentActivity.CloseDialog();
                tcs.SetResult();
            };
            CurrentActivity.ShowDialog(alert);
            return tcs.Task;
        }

        public static Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            MainWindow CurrentActivity = MainWindow.Singleten;
            Control_Alert alert = new Control_Alert();
            alert.SetInfo(title, message, accept, cancel);
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            alert.Close += () =>
            {
                CurrentActivity.CloseDialog();
                tcs.SetResult(alert.Result);
            };
            CurrentActivity.ShowDialog(alert);
            return tcs.Task;
        }

        public static Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string initialValue = "")
        {
            MainWindow CurrentActivity = MainWindow.Singleten;
            Control_Prompt prompt = new Control_Prompt();
            prompt.SetInfo(title, message, accept, cancel, initialValue);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            prompt.Close += () =>
            {
                CurrentActivity.CloseDialog();
                tcs.SetResult(prompt.Result);
            };
            CurrentActivity.ShowDialog(prompt);
            return tcs.Task;
        }

        public static Task DisplayPicture(string title, byte[] img, string cancel = "OK", Action action = null, bool TouchLeave = false)
        {
            MainWindow CurrentActivity = MainWindow.Singleten;
            Control_Picture picture = new Control_Picture();
            picture.SetInfo(title, img, cancel, action, TouchLeave);
            TaskCompletionSource tcs = new TaskCompletionSource();
            picture.Close += () =>
            {
                CurrentActivity.CloseDialog();
                tcs.SetResult();
            };
            CurrentActivity.ShowDialog(picture);
            return tcs.Task;
        }
    }
}
