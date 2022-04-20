namespace PopStudio.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            string settingxml = Permission.GetSettingPath();
            if (!File.Exists(settingxml))
            {
                Setting.SaveAsXml(settingxml);
            }
            Setting.LoadFromXml(settingxml);
            
            MainPage = new AppShell();
        }
    }
}