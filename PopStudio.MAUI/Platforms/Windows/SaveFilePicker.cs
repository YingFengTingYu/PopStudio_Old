using WindowsFileSavePicker = Windows.Storage.Pickers.FileSavePicker;
using P = Windows.Storage.Pickers;

namespace PopStudio.MAUI.Platforms.Windows
{
    public class SaveFilePicker
    {
        public async Task<string> PickFile()
        {
            var filePicker = new WindowsFileSavePicker();

            // Get the current window's HWND by passing in the Window object
            var hwnd = ((MauiWinUIWindow)App.Current.Windows[0].Handler.NativeView).WindowHandle;
            filePicker.FileTypeChoices.Add("所有文件", new List<string>() { ".", ".tex", ".txz", ".ptx", ".rton", ".compiled", ".reanim", ".trail", ".xml", ".xnb", ".json", ".png", ".jpg", ".gif", ".lzma", ".gz", ".lz4", ".dz", ".rsb", ".pak" });
            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            
            var result = await filePicker.PickSaveFileAsync();

            return result?.Path;
        }
    }
}
