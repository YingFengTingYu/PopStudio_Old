using WindowsFileOpenPicker = Windows.Storage.Pickers.FileOpenPicker;
using P = Windows.Storage.Pickers;

namespace PopStudio.MAUI.Platforms.Windows
{
    public class OpenFilePicker
    {
        public async Task<string> PickFile()
        {
            var filePicker = new WindowsFileOpenPicker();
            filePicker.FileTypeFilter.Add("*");

            // Get the current window's HWND by passing in the Window object
            var hwnd = ((MauiWinUIWindow)App.Current.Windows[0].Handler.NativeView).WindowHandle;
            
            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            
            var result = await filePicker.PickSingleFileAsync();

            return result?.Path;
        }
    }
}
