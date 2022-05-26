using Gtk;
using PopStudio.Language.Languages;
using PopStudio.GTK.Pages;
using PopStudio.GTK.Plugin;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK
{
    internal class MainPage : Window
    {
        public static MainPage Singleten;

        bool topmost = false;

        public bool TopMost
        {
            get => topmost;
            set => KeepAbove = topmost = value;
        }

        static string _defaultwindowtitle = "PopStudio";
        static int _defaultwindowwidth = 900;
        static int _defaultwindowheight = 620;
        static int _defaultsidebarwidth = 210;
        static int _defaultsidebartitleheight = 70;
        static int _defaultpagetitleheight = 50;
        public static Gdk.Color _colorwhite = new Gdk.Color(0xFF, 0xFF, 0xFF);
        public static Gdk.Color _colorblack = new Gdk.Color(0, 0, 0);
        public static Gdk.Color _colorskyblue = new Gdk.Color(0x87, 0xCE, 0xEB);
        public static Gdk.Color _colorcornflowerblue = new Gdk.Color(0x64, 0x95, 0xED);

        #region bar
        public HBox root;
        public VBox sidebar;
        public Label sidebartitle;
        public Button button1;
        public Button button2;
        public Button button3;
        public Button button4;
        public Button button5;
        public Button button6;
        public Button button7;
        public Button button8;
        public Button button9;
        public Button button10;
        public Button button11;
        public Button button12;
        #endregion

        #region page
        public VBox pagecontent;
        public HBox hboxtitle;
        public EventBox boxtemp; //just for center title
        public Gtk.Image imagetemp; //just for center title
        public Label pagetitle;
        public EventBox boxtopmost;
        public Gtk.Image imagetopmost;
        public PageContent content;
        #endregion

        public MainPage() : base(_defaultwindowtitle)
        {
            Singleten = this;
            SetDefaultSize(_defaultwindowwidth, _defaultwindowheight);
            Icon = new Gdk.Pixbuf(ResourceAD.Icon);
            root = new HBox();
            Add(root);
            #region bar
            sidebar = new VBox();
            sidebar.ModifyBg(StateType.Normal, _colorskyblue); //87CEEB
            sidebar.WidthRequest = _defaultsidebarwidth;
            root.PackStart(sidebar, false, false, 0);
            sidebartitle = new Label("PopStudio");
            sidebartitle.HeightRequest = _defaultsidebartitleheight;
            sidebartitle.ModifyFont(GenFont(20));
            sidebartitle.ModifyFg(StateType.Normal, _colorwhite);
            sidebar.PackStart(sidebartitle, false, false, 0);
            button1 = CreateBarButton(MAUIStr.Obj.HomePage_Title);
            sidebar.PackStart(button1, false, false, 4);
            button1.Clicked += (s, e) => LoadHomePage();
            button2 = CreateBarButton(MAUIStr.Obj.Package_Title);
            sidebar.PackStart(button2, false, false, 4);
            button2.Clicked += (s, e) => LoadPackage();
            button11 = CreateBarButton(MAUIStr.Obj.Atlas_Title);
            sidebar.PackStart(button11, false, false, 4);
            button11.Clicked += (s, e) => LoadAtlas();
            button3 = CreateBarButton(MAUIStr.Obj.Texture_Title);
            sidebar.PackStart(button3, false, false, 4);
            button3.Clicked += (s, e) => LoadTexture();
            button4 = CreateBarButton(MAUIStr.Obj.Reanim_Title);
            sidebar.PackStart(button4, false, false, 4);
            button4.Clicked += (s, e) => LoadReanim();
            button5 = CreateBarButton(MAUIStr.Obj.Particles_Title);
            sidebar.PackStart(button5, false, false, 4);
            button5.Clicked += (s, e) => LoadParticles();
            button6 = CreateBarButton(MAUIStr.Obj.Trail_Title);
            sidebar.PackStart(button6, false, false, 4);
            button6.Clicked += (s, e) => LoadTrail();
            button12 = CreateBarButton(MAUIStr.Obj.Pam_Title);
            sidebar.PackStart(button12, false, false, 4);
            button12.Clicked += (s, e) => LoadPam();
            button7 = CreateBarButton(MAUIStr.Obj.RTON_Title);
            sidebar.PackStart(button7, false, false, 4);
            button7.Clicked += (s, e) => LoadRTON();
            button8 = CreateBarButton(MAUIStr.Obj.Compress_Title);
            sidebar.PackStart(button8, false, false, 4);
            button8.Clicked += (s, e) => LoadCompress();
            button9 = CreateBarButton(MAUIStr.Obj.LuaScript_Title);
            sidebar.PackStart(button9, false, false, 4);
            button9.Clicked += (s, e) => LoadLuaScript();
            button10 = CreateBarButton(MAUIStr.Obj.Setting_Title);
            sidebar.PackStart(button10, false, false, 4);
            button10.Clicked += (s, e) => LoadSetting();
            #endregion
            #region page
            pagecontent = new VBox();
            hboxtitle = new HBox();
            hboxtitle.ModifyBg(StateType.Normal, _colorcornflowerblue); //6495ED
            boxtemp = new EventBox();
            imagetemp = new Gtk.Image(new Gdk.Pixbuf(ResourceAD.empty).ScaleSimple(28, 28, Gdk.InterpType.Hyper));
            imagetemp.MarginLeft = 10;
            boxtemp.Add(imagetemp);
            hboxtitle.PackStart(boxtemp, false, false, 0);
            pagetitle = new Label();
            pagetitle.HeightRequest = _defaultpagetitleheight;
            pagetitle.ModifyFont(GenFont(17));
            pagetitle.ModifyFg(StateType.Normal, _colorwhite);
            hboxtitle.PackStart(pagetitle, true, true, 0);
            boxtopmost = new EventBox();
            boxtopmost.ButtonReleaseEvent += (s, e) => TopMost = !TopMost;
            imagetopmost = new Gtk.Image(new Gdk.Pixbuf(ResourceAD.topmost).ScaleSimple(28, 28, Gdk.InterpType.Hyper));
            imagetopmost.MarginRight = 10;
            boxtopmost.Add(imagetopmost);
            hboxtitle.PackStart(boxtopmost, false, false, 0);
            pagecontent.PackStart(hboxtitle, false, false, 0);
            content = new PageContent();
            pagecontent.PackStart(content, true, true, 0);
            root.PackStart(pagecontent, true, true, 0);
            #endregion
            if (Program.ShowScript(out string scriptFileName))
            {
                LoadLuaScript(scriptFileName);
            }
            else
            {
                LoadHomePage();
                if (Setting.OpenProgramAD)
                {
                    int randomNumber = new Random().Next(1, 4);
                    byte[] img;
                    string url;
                    switch (randomNumber)
                    {
                        case 1:
                            img = ResourceAD.ImageAD1;
                            url = ResourceAD.AD1;
                            break;
                        case 2:
                            img = ResourceAD.ImageAD2;
                            url = ResourceAD.AD2;
                            break;
                        case 3:
                            img = ResourceAD.ImageAD3;
                            url = ResourceAD.AD3;
                            break;
                        default:
                            return;
                    }
                    Dialogs.DisplayPicture(MAUIStr.Obj.AD_Title, img, MAUIStr.Obj.AD_Cancel, () => Permission.OpenUrl(url), true);
                }
            }
        }

        static Pango.FontDescription GenFont(int Size)
        {
#if MACOS
            Size += Size >> 1;
#endif
            return Pango.FontDescription.FromString($"Sans Not-Rotated {Size}");
        }

        static Button CreateBarButton(string title)
        {
            Button button = new Button(title);
            button.MarginLeft = 25;
            button.MarginRight = 25;
            button.ModifyFont(GenFont(11));
            return button;
        }

        Page_HomePage homePage = new Page_HomePage();
        Page_Package package = new Page_Package();
        Page_Texture texture = new Page_Texture();
        Page_Reanim reanim = new Page_Reanim();
        Page_Particles particles = new Page_Particles();
        Page_Trail trail = new Page_Trail();
        Page_RTON rton = new Page_RTON();
        Page_Compress compress = new Page_Compress();
        Page_LuaScript luaScript = new Page_LuaScript();
        Page_Setting setting = new Page_Setting();
        Page_Atlas atlas = new Page_Atlas();
        Page_Pam pam = new Page_Pam();

        public void LoadHomePage()
        {
            content.SetPage(homePage);
            pagetitle.Text = MAUIStr.Obj.HomePage_Title;
            ShowAll();
        }

        public void LoadPackage()
        {
            content.SetPage(package);
            pagetitle.Text = MAUIStr.Obj.Package_Title;
            ShowAll();
            package.ModeChange(package.switchmode.Active);
        }

        public void LoadAtlas()
        {
            content.SetPage(atlas);
            pagetitle.Text = MAUIStr.Obj.Atlas_Title;
            ShowAll();
            atlas.ModeChange(atlas.TB_Mode.Active);
        }

        public void LoadTexture()
        {
            content.SetPage(texture);
            pagetitle.Text = MAUIStr.Obj.Texture_Title;
            ShowAll();
            texture.ModeChange(texture.TB_Mode.Active);
        }

        public void LoadReanim()
        {
            content.SetPage(reanim);
            pagetitle.Text = MAUIStr.Obj.Reanim_Title;
            ShowAll();
        }

        public void LoadParticles()
        {
            content.SetPage(particles);
            pagetitle.Text = MAUIStr.Obj.Particles_Title;
            ShowAll();
        }

        public void LoadTrail()
        {
            content.SetPage(trail);
            pagetitle.Text = MAUIStr.Obj.Trail_Title;
            ShowAll();
        }

        public void LoadPam()
        {
            content.SetPage(pam);
            pagetitle.Text = MAUIStr.Obj.Pam_Title;
            ShowAll();
            pam.ModeChange(pam.TB_Mode.Active);
        }

        public void LoadRTON()
        {
            content.SetPage(rton);
            pagetitle.Text = MAUIStr.Obj.RTON_Title;
            ShowAll();
            rton.ModeChange(rton.TB_Mode.Active);
        }

        public void LoadCompress()
        {
            content.SetPage(compress);
            pagetitle.Text = MAUIStr.Obj.Compress_Title;
            ShowAll();
            compress.ModeChange(compress.TB_Mode.Active);
        }

        public void LoadLuaScript(string sc = null)
        {
            content.SetPage(luaScript);
            pagetitle.Text = MAUIStr.Obj.LuaScript_Title;
            ShowAll();
            if (sc != null)
            {
                luaScript.ShowScriptByFileName(sc);
                luaScript.RunScript();
            }
        }

        public void LoadSetting()
        {
            content.SetPage(setting);
            pagetitle.Text = MAUIStr.Obj.Setting_Title;
            ShowAll();
        }
    }
}