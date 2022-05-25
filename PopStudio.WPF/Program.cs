using PopStudio.Platform;

namespace PopStudio.WPF
{
    class Program
    {
        [STAThread]
        static void Main(params string[] args)
        {
            Bitmap.RegistPlatform<GDIBitmap>();
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
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
