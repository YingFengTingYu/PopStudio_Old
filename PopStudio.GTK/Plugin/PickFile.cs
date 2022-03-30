using Gtk;

namespace PopStudio.GTK
{
    internal static class PickFile
    {
        public static string ChooseOpenFile()
        {
            string ans = null;
            using (FileChooserDialog f = new FileChooserDialog("Open File", MainPage.Singleten, FileChooserAction.Open, "OK", 1, "Cancel", 2))
            {
                if (f.Run() == 1) ans = f.Filename;
                f.Destroy();
            }
            return string.IsNullOrEmpty(ans) ? null : ans;
        }

        public static string ChooseSaveFile()
        {
            string ans = null;
            using (FileChooserDialog f = new FileChooserDialog("Save File", MainPage.Singleten, FileChooserAction.Save, "OK", 1, "Cancel", 2))
            {
                if (f.Run() == 1) ans = f.Filename;
                f.Destroy();
            }
            return string.IsNullOrEmpty(ans) ? null : ans;
        }

        public static string ChooseFolder()
        {
            string ans = null;
            using (FileChooserDialog f = new FileChooserDialog("Choose Folder", MainPage.Singleten, FileChooserAction.SelectFolder, "OK", 1, "Cancel", 2))
            {
                if (f.Run() == 1) ans = f.Filename;
                f.Destroy();
            }
            return string.IsNullOrEmpty(ans) ? null : ans;
        }
    }
}
