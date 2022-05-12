using PopStudio.GUI.Languages;

namespace PopStudio.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            fitem_homepage.Title = MAUIStr.Obj.HomePage_Title;
            fitem_package.Title = MAUIStr.Obj.Package_Title;
            fitem_atlas.Title = MAUIStr.Obj.Atlas_Title;
            fitem_texture.Title = MAUIStr.Obj.Texture_Title;
            fitem_reanim.Title = MAUIStr.Obj.Reanim_Title;
            fitem_particles.Title = MAUIStr.Obj.Particles_Title;
            fitem_trail.Title = MAUIStr.Obj.Trail_Title;
            fitem_pam.Title = MAUIStr.Obj.Pam_Title;
            fitem_rton.Title = MAUIStr.Obj.RTON_Title;
            fitem_compress.Title = MAUIStr.Obj.Compress_Title;
            fitem_luascript.Title = MAUIStr.Obj.LuaScript_Title;
            fitem_setting.Title = MAUIStr.Obj.Setting_Title;
            FlyoutBehavior = Permission.HiddenFlyout() ? FlyoutBehavior.Flyout : FlyoutBehavior.Locked;
            Permission.PlatformInit();
        }

        protected override bool OnBackButtonPressed()
        {
            Dispatcher.Dispatch(async () =>
            {
                if (await DisplayAlert(MAUIStr.Obj.Shell_ExitTitle, MAUIStr.Obj.Shell_ExitText, MAUIStr.Obj.Shell_OK, MAUIStr.Obj.Shell_Cancel)) Environment.Exit(0);
            });
            return true;
        }
    }
}