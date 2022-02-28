namespace PopStudio.MAUI
{
    internal static partial class PickFile
    {
        public static partial Task<string> ChooseOpenFile(this ContentPage page);

        public static partial Task<string> ChooseSaveFile(this ContentPage page);

        public static partial Task<string> ChooseFolder(this ContentPage page);
    }
}
