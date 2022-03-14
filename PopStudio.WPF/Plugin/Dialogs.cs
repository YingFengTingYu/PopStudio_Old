using System.Windows.Media.Imaging;

namespace PopStudio.WPF.Plugin
{
    internal static class Dialogs
    {
        public static async Task<string> DisplayActionSheet(string title, string cancel, string ok, params string[] items)
        {
            MainWindow.Singleten.OpenSheetDialog(title, cancel, ok, items);
            while (MainWindow.Singleten.Result == null)
            {
                await Task.Delay(100);
            }
            return (string)MainWindow.Singleten.Result;
        }

        public static async Task DisplayAlert(string title, string message, string cancel)
        {
            MainWindow.Singleten.OpenAlertDialog(title, message, cancel);
            while (MainWindow.Singleten.Result == null)
            {
                await Task.Delay(100);
            }
        }

        public static async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            MainWindow.Singleten.OpenAlertDialog(title, message, accept, cancel);
            while (MainWindow.Singleten.Result == null)
            {
                await Task.Delay(100);
            }
            return (bool)MainWindow.Singleten.Result;
        }

        public static async Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string initialValue = "")
        {
            MainWindow.Singleten.OpenPromptDialog(title, message, accept, cancel, initialValue);
            while (MainWindow.Singleten.Result == null)
            {
                await Task.Delay(100);
            }
            return (string)MainWindow.Singleten.Result;
        }

        public static async Task DisplayPicture(string title, BitmapImage img, string cancel = "OK", Action action = null, bool TouchLeave = false)
        {
            MainWindow.Singleten.OpenPictureDialog(title, img, cancel, action, TouchLeave);
            while (MainWindow.Singleten.Result == null)
            {
                await Task.Delay(100);
            }
        }
    }
}
