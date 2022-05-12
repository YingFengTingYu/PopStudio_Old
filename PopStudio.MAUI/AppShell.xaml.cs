using PopStudio.GUI.Languages;

namespace PopStudio.MAUI
{
    public partial class AppShell : Shell
    {
        Page_HomePage homePage;
        public Page_HomePage HomePage => homePage ??= new Page_HomePage();

        public AppShell()
        {
            InitializeComponent();
            List<FlyoutPageItem> mItem = new List<FlyoutPageItem>();
            mItem.Add(new FlyoutPageItem
            {
                Index = 1,
                Title = fitem_homepage.Title = MAUIStr.Obj.HomePage_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 2,
                Title = fitem_package.Title = MAUIStr.Obj.Package_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 3,
                Title = fitem_atlas.Title = MAUIStr.Obj.Atlas_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 4,
                Title = fitem_texture.Title = MAUIStr.Obj.Texture_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 5,
                Title = fitem_reanim.Title = MAUIStr.Obj.Reanim_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 6,
                Title = fitem_particles.Title = MAUIStr.Obj.Particles_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 7,
                Title = fitem_trail.Title = MAUIStr.Obj.Trail_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 8,
                Title = fitem_pam.Title = MAUIStr.Obj.Pam_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 9,
                Title = fitem_rton.Title = MAUIStr.Obj.RTON_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 10,
                Title = fitem_compress.Title = MAUIStr.Obj.Compress_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 11,
                Title = fitem_luascript.Title = MAUIStr.Obj.LuaScript_Title
            });
            mItem.Add(new FlyoutPageItem
            {
                Index = 12,
                Title = fitem_setting.Title = MAUIStr.Obj.Setting_Title
            });
            collectionView.ItemsSource = mItem;
            collectionView.SelectedItem = mItem[0];
            //settingHomePage.Title = fitem_homepage.Title = MAUIStr.Obj.HomePage_Title;
            //settingPackage.Title = fitem_package.Title = MAUIStr.Obj.Package_Title;
            //settingAtlas.Title = fitem_atlas.Title = MAUIStr.Obj.Atlas_Title;
            //fitem_texture.Title = MAUIStr.Obj.Texture_Title;
            //fitem_reanim.Title = MAUIStr.Obj.Reanim_Title;
            //fitem_particles.Title = MAUIStr.Obj.Particles_Title;
            //fitem_trail.Title = MAUIStr.Obj.Trail_Title;
            //fitem_pam.Title = MAUIStr.Obj.Pam_Title;
            //fitem_rton.Title = MAUIStr.Obj.RTON_Title;
            //fitem_compress.Title = MAUIStr.Obj.Compress_Title;
            //fitem_luascript.Title = MAUIStr.Obj.LuaScript_Title;
            //fitem_setting.Title = MAUIStr.Obj.Setting_Title;
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

        private async void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is FlyoutPageItem item)
            {
                CurrentItem = item.Index switch
                {
                    2 => fitem_package,
                    3 => fitem_atlas,
                    4 => fitem_texture,
                    5 => fitem_reanim,
                    6 => fitem_particles,
                    7 => fitem_trail,
                    8 => fitem_pam,
                    9 => fitem_rton,
                    10 => fitem_compress,
                    11 => fitem_luascript,
                    12 => fitem_setting,
                    _ => fitem_homepage
                };
                await Task.Delay(200);
                FlyoutIsPresented = false;
            }
        }
    }

    public class FlyoutPageItem
    {
        public string Title { get; set; }
        public int Index { get; set; }
    }
}