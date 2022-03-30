// See https://aka.ms/new-console-template for more information
string settingxml = "setting.xml";
if (File.Exists(settingxml))
{
    PopStudio.Setting.LoadFromXml(settingxml);
}
Gtk.Application.Init();
var win = new PopStudio.GTK.MainPage();
win.DeleteEvent += (s, e) =>
{
    Gtk.Application.Quit();
};
win.ShowAll();
Gtk.Application.Run();