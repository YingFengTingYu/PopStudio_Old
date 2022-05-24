using System.Windows;

namespace PopStudio.WPF
{
    class Program
    {
        [STAThread]
        static void Main(params string[] args)
        {
            try
            {
                string settingxml = "setting.xml";
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
