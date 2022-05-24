using System.Windows.Controls;
using System.Windows.Forms;

namespace PopStudio.WPF
{
    internal static class PickFile
    {
        public static string ChooseOpenFile(this Page _)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ChooseSaveFile(this Page _)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ChooseFolder(this Page _)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
