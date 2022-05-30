using Gtk;
using PopStudio.Language.Languages;
using PopStudio.Platform;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK.Pages
{
    internal class Page_Texture : Plugin.Page
    {
        public Label label_introduction;
        public Label label_choosemode;
        public HBox grid_choosemode;
        public Label label_mode1;
        public Switch TB_Mode;
        public Label label_mode2;
        public Label text1;
        public HBox choose1;
        public Entry textbox1;
        public Button button1;
        public Label text2;
        public HBox choose2;
        public Entry textbox2;
        public Button button2;
        public Label text3;
        public ComboBox CB_CMode;
        public VBox SP_FMode;
        public Label textN;
        public ComboBox CB_FMode;
        public Button button_run;
        public Label label_statue;
        public Label text4;

        static string[][] Mode2String =
        {
            new string[]
            {
                "ARGB8888(0)",
                "ABGR8888(0)",
                "RGBA4444(1)",
                "RGB565(2)",
                "RGBA5551(3)",
                "RGBA4444_Block(21)",
                "RGB565_Block(22)",
                "RGBA5551_Block(23)",
                "XRGB8888_A8(149)",
                "ARGB8888(BE)(0)",
                "ARGB8888_Padding(BE)(0)",
                "DXT1_RGB(35)",
                "DXT3_RGBA(36)",
                "DXT5_RGBA(37)",
                "DXT5_RGBA_MortonBlock(5)",
                "DXT5_RGBA(BE)(5)",
                "ETC1_RGB(32)",
                "ETC1_RGB_A8(147)",
                "ETC1_RGB_A_Palette(147)",
                "ETC1_RGB_A_Palette(150)",
                "PVRTC_4BPP_RGBA(30)",
                "PVRTC_4BPP_RGB_A8(148)"
            },
            new string[]
            {
                "Encrypt"
            },
            new string[]
            {
                "ABGR8888(1)",
                "RGBA4444(2)",
                "RGBA5551(3)",
                "RGB565(4)"
            },
            new string[]
            {
                "ABGR8888(1)",
                "RGBA4444(2)",
                "RGBA5551(3)",
                "RGB565(4)"
            },
            new string[]
            {
                "LUT8(1)(Invalid)",
                "ARGB8888(2)",
                "ARGB4444(3)",
                "ARGB1555(4)",
                "RGB565(5)",
                "ABGR8888(6)",
                "RGBA4444(7)",
                "RGBA5551(8)",
                "XRGB8888(9)",
                "LA88(10)"
            },
            new string[]
            {
                "DXT5_RGBA_Padding(BE)"
            },
            new string[]
            {
                "DXT5_RGBA"
            },
            new string[]
            {
                "DXT5_RGBA_Morton"
            },
            new string[]
            {
                "ABGR8888"
            }
        };

        public Page_Texture()
        {
            label_introduction = CreateText(MAUIStr.Obj.Texture_Introduction);
            label_choosemode = CreateTitle(MAUIStr.Obj.Share_ChooseMode);
            grid_choosemode = CreateMode(out label_mode1, out label_mode2, out TB_Mode);
            TB_Mode.Activate += ModeChange;
            TB_Mode.ButtonReleaseEvent += ModeChange;
            text1 = CreateTitle(MAUIStr.Obj.Texture_Choose1);
            choose1 = CreateEntryAndButton(out textbox1, out button1);
            button1.Clicked += (s, e) => textbox1.Text = PickFile.ChooseOpenFile();
            text2 = CreateTitle(MAUIStr.Obj.Texture_Choose2);
            choose2 = CreateEntryAndButton(out textbox2, out button2);
            button2.Clicked += (s, e) => textbox2.Text = PickFile.ChooseSaveFile();
            text3 = CreateTitle(MAUIStr.Obj.Texture_Choose3);
            CB_CMode = CreateComboBox("PTX(rsb)", "cdat(Android,iOS)", "tex(iOS)", "txz(Android,iOS)", "tex(TV)", "ptx(Xbox360)", "ptx(PS3)", "ptx(PSV)", "xnb(Windows Phone)");
            CB_CMode.Changed += (s, e) =>
            {
                ListStore ClearList = new ListStore(typeof(string), typeof(string));
                CB_FMode.Model = ClearList;
                string[] sary = Mode2String[((ComboBox)s).Active];
                for (int i = 0; i < sary.Length; i++)
                {
                    ClearList.AppendValues(sary[i]);
                }
                CB_FMode.Active = 0;
            };
            SP_FMode = CreateHidden(out textN, out CB_FMode);
            SP_FMode.Visible = false;
            button_run = CreateButton(MAUIStr.Obj.Share_Run);
            button_run.Clicked += Button_Click;
            label_statue = CreateTitle(MAUIStr.Obj.Share_RunStatue);
            text4 = CreateTitle(MAUIStr.Obj.Share_Waiting);
            PackStart(label_introduction, false, false, 5);
            PackStart(label_choosemode, false, false, 5);
            PackStart(grid_choosemode, false, false, 5);
            PackStart(text1, false, false, 5);
            PackStart(choose1, false, false, 5);
            PackStart(text2, false, false, 5);
            PackStart(choose2, false, false, 5);
            PackStart(text3, false, false, 5);
            PackStart(CB_CMode, false, false, 5);
            PackStart(SP_FMode, false, false, 0);
            PackStart(button_run, false, false, 5);
            PackStart(label_statue, false, false, 5);
            PackStart(text4, false, false, 5);
        }

        public void ModeChange(bool v)
        {
            if (v)
            {
                text1.Text = MAUIStr.Obj.Texture_Choose4;
                text2.Text = MAUIStr.Obj.Texture_Choose5;
                text3.Text = MAUIStr.Obj.Texture_Choose6;
                SP_FMode.Visible = true;
            }
            else
            {
                text1.Text = MAUIStr.Obj.Texture_Choose1;
                text2.Text = MAUIStr.Obj.Texture_Choose2;
                text3.Text = MAUIStr.Obj.Texture_Choose3;
                SP_FMode.Visible = false;
            }
        }

        public void ModeChange(object sender, EventArgs e)
        {
            ModeChange(!((Switch)sender).Active);
            //交换文本框内容
            string temp = textbox1.Text;
            textbox1.Text = textbox2.Text;
            textbox2.Text = temp;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Sensitive = false;
            text4.Text = MAUIStr.Obj.Share_Running;
            bool mode = TB_Mode.Active;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            int cmode = CB_CMode.Active;
            int fmode = CB_FMode.Active;
            new Thread(new ThreadStart(() =>
            {
                string err = null;
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                try
                {
                    if (!File.Exists(inFile))
                    {
                        throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
                    }
                    if (mode)
                    {
                        YFAPI.EncodeImage(inFile, outFile, cmode, fmode);
                    }
                    else
                    {
                        YFAPI.DecodeImage(inFile, outFile, cmode);
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
                sw.Stop();
                decimal time = sw.ElapsedMilliseconds / 1000m;
                Application.Invoke((s, e) =>
                {
                    if (err == null)
                    {
                        text4.Text = string.Format(MAUIStr.Obj.Share_Finish, time.ToString("F3"));
                    }
                    else
                    {
                        text4.Text = string.Format(MAUIStr.Obj.Share_Wrong, err);
                    }
                    b.Sensitive = true;
                });
            }))
            { IsBackground = true }.Start();
        }

        static VBox CreateHidden(out Label l, out ComboBox c)
        {
            l = CreateTitle(MAUIStr.Obj.Texture_Choose7);
            c = CreateComboBox(Mode2String[0]);
            VBox ans = new VBox();
            ans.PackStart(l, false, false, 5);
            ans.PackStart(c, false, false, 5);
            return ans;
        }

        static Entry CreateEntry()
        {
            Entry ans = new Entry();
            ans.ModifyFont(GenFont(11));
            return ans;
        }

        static HBox CreateEntryAndButton(out Entry e, out Button b)
        {
            e = CreateEntry();
            b = CreateButton(MAUIStr.Obj.Share_Choose);
            HBox ans = new HBox();
            ans.PackStart(e, true, true, 0);
            ans.PackEnd(b, false, false, 5);
            return ans;
        }

        static HBox CreateMode(out Label l1, out Label l2, out Switch s)
        {
            l1 = CreateTitle(MAUIStr.Obj.Texture_Mode1);
            l2 = CreateTitle(MAUIStr.Obj.Texture_Mode2);
            s = CreateSwitch();
            HBox ans = new HBox();
            ans.PackStart(l1, false, false, 0);
            ans.PackStart(s, false, false, 5);
            ans.PackStart(l2, false, false, 5);
            return ans;
        }

        static Switch CreateSwitch() => new Switch();

        static Label CreateTitle(string subtitle)
        {
            Label l = new Label(subtitle);
            l.Wrap = true;
            l.LineWrapMode = Pango.WrapMode.Char;
            l.ModifyFont(GenFont(14));
            l.Xalign = 0;
            return l;
        }

        static Label CreateText(string text)
        {
            Label l = new Label(text);
            l.LineWrap = true;
            l.LineWrapMode = Pango.WrapMode.Char;
            l.ModifyFont(GenFont(11));
            l.Xalign = 0;
            return l;
        }

        static Button CreateButton(string t)
        {
            Button ans = new Button(t);
            ans.ModifyFont(GenFont(11));
            return ans;
        }

        static ComboBox CreateComboBox(params string[] s)
        {
            ComboBox ans = new ComboBox(s);
            ans.Active = 0;
            ans.ModifyFont(GenFont(11));
            return ans;
        }
    }
}
