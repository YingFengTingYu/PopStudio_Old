using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PopStudio.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            string settingxml = "setting.xml";
            if (File.Exists(settingxml))
            {
                Setting.LoadFromXml(settingxml);
            }
        }
    }
}
