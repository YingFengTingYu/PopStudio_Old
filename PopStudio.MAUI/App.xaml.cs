using PopStudio.Platform;

namespace PopStudio.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Bitmap.RegistPlatform<SkiaBitmap>();
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