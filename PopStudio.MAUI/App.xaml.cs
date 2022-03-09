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
                temp();
            }
            else
            {
                MainPage = new MainPage();
            }
        }

        async void temp()
        {
            await Task.Delay(3000);
            MainPage = new MainPage();
        }
    }
}