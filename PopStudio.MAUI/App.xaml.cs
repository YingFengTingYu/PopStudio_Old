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
            if (Setting.OpenProgramAD)
            {
                MainPage = new Page_AD();
                load();
            }
            else
            {
                MainPage = new AppShell();
            }
            Permission.ModifyFullBar();
        }

        async void load()
        {
            await Task.Delay(2500);
            MainPage = new AppShell();
        }
    }
}