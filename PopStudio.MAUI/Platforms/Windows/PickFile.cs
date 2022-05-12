using Windows.Storage;
using Windows.Storage.Pickers;

namespace PopStudio.MAUI
{
    internal static partial class PickFile
    {
        public static partial async Task<string> ChooseOpenFile(this ContentPage page)
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add("*");
            IntPtr hwnd = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            StorageFile result = await filePicker.PickSingleFileAsync();
            return result?.Path;
        }

        public static partial async Task<string> ChooseSaveFile(this ContentPage page)
        {
            FileSavePicker filePicker = new FileSavePicker();
            IntPtr hwnd = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;
            filePicker.FileTypeChoices.Add(GUI.Languages.MAUIStr.Obj.PickFile_AllFiles, new List<string>() { ".", ".tex", ".txz", ".ptx", ".rton", ".compiled", ".reanim", ".trail", ".xml", ".xnb", ".json", ".png", ".jpg", ".gif", ".lzma", ".gz", ".lz4", ".dz", ".rsb", ".pak" });
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            StorageFile result = await filePicker.PickSaveFileAsync();
            return result?.Path;
        }


        public static partial async Task<string> ChooseFolder(this ContentPage page)
		{
            FolderPicker folderPicker = new FolderPicker();
            IntPtr hwnd = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
            StorageFolder result = await folderPicker.PickSingleFolderAsync();
            return result?.Path;
        }
	}
}
