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

        async void load()
        {
            await Task.Delay(2500);
            MainPage = new AppShell();
        }
    }
}