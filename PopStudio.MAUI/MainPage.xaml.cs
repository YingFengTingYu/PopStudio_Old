using PopStudio.MAUI.Languages;

namespace PopStudio.MAUI
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            fitem_homepage.Title = MAUIStr.Obj.HomePage_Title;
            fitem_package.Title = MAUIStr.Obj.Package_Title;
            fitem_texture.Title = MAUIStr.Obj.Texture_Title;
            fitem_reanim.Title = MAUIStr.Obj.Reanim_Title;
            fitem_particles.Title = MAUIStr.Obj.Particles_Title;
            fitem_trail.Title = MAUIStr.Obj.Trail_Title;
            fitem_rton.Title = MAUIStr.Obj.RTON_Title;
            fitem_compress.Title = MAUIStr.Obj.Compress_Title;
            fitem_luascript.Title = MAUIStr.Obj.LuaScript_Title;
            fitem_setting.Title = MAUIStr.Obj.Setting_Title;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert(MAUIStr.Obj.Shell_ExitTitle, MAUIStr.Obj.Shell_ExitText, MAUIStr.Obj.Shell_OK, MAUIStr.Obj.Shell_Cancel)) Environment.Exit(0);
            });
            return true;
        }
    }
}