using PopStudio.Platform;

namespace PopStudio.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            YFBitmap.RegistPlatform<SkiaBitmap>();
            YFAPI.RegistPlatform<MAUIAPI>();
            YFFileListStream.RegistPlatform(YFRes.CompiledImageList);
            try
            {
                string settingxml = Permission.GetSettingPath();
                if (File.Exists(settingxml))
                {
                    Setting.LoadFromXml(settingxml);
                }
            }
            catch (Exception)
            {
            }
            MainPage = new AppShell();
        }
    }
}