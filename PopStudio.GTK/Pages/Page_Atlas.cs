using Gtk;
using PopStudio.GUILanguage.Languages;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK.Pages
{
    internal class Page_Atlas : Plugin.Page
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
        public HBox choose3;
        public Entry textbox3;
        public Button button3;
        public Label text4;
        public Entry textbox4;
        public Label text_mode;
        public ComboBox CB_Mode;
        public VBox splice_size;
        public Label text_maxwidth;
        public ComboBox CB_MaxWidth;
        public Label text_maxheight;
        public ComboBox CB_MaxHeight;
        public Button button_run;
        public Label label_statue;
        public Label text5;

        public Page_Atlas()
        {
            label_introduction = CreateText(MAUIStr.Obj.Atlas_Introduction);
            label_choosemode = CreateTitle(MAUIStr.Obj.Share_ChooseMode);
            grid_choosemode = CreateMode(out label_mode1, out label_mode2, out TB_Mode);
            TB_Mode.Activate += ModeChange;
            TB_Mode.ButtonReleaseEvent += ModeChange;
            text1 = CreateTitle(MAUIStr.Obj.Atlas_Choose1);
            choose1 = CreateEntryAndButton(out textbox1, out button1);
            button1.Clicked += (s, e) => textbox1.Text = TB_Mode.Active ? PickFile.ChooseFolder() : PickFile.ChooseOpenFile();
            text2 = CreateTitle(MAUIStr.Obj.Atlas_Choose2);
            choose2 = CreateEntryAndButton(out textbox2, out button2);
            button2.Clicked += (s, e) => textbox2.Text = TB_Mode.Active ? PickFile.ChooseSaveFile() : PickFile.ChooseFolder();
            text3 = CreateTitle(MAUIStr.Obj.Atlas_Choose3);
            choose3 = CreateEntryAndButton(out textbox3, out button3);
            button3.Clicked += (s, e) => textbox3.Text = PickFile.ChooseOpenFile();
            text4 = CreateTitle(MAUIStr.Obj.Atlas_Choose4);
            textbox4 = CreateEntry();
            text_mode = CreateTitle(MAUIStr.Obj.Atlas_Format);
            CB_Mode = CreateComboBox("resources.xml(Rsb)", "resources.xml(Old)", "resources.xml(Ancient)", "plist(Free)", "atlasimagemap.dat", "xml(TV)");
            splice_size = CreateHidden(out text_maxwidth, out CB_MaxWidth, out text_maxheight, out CB_MaxHeight);
            splice_size.Visible = false;
            button_run = CreateButton(MAUIStr.Obj.Share_Run);
            button_run.Clicked += Button_Click;
            label_statue = CreateTitle(MAUIStr.Obj.Share_RunStatue);
            text5 = CreateTitle(MAUIStr.Obj.Share_Waiting);
            PackStart(label_introduction, false, false, 5);
            PackStart(label_choosemode, false, false, 5);
            PackStart(grid_choosemode, false, false, 5);
            PackStart(text1, false, false, 5);
            PackStart(choose1, false, false, 5);
            PackStart(text2, false, false, 5);
            PackStart(choose2, false, false, 5);
            PackStart(text3, false, false, 5);
            PackStart(choose3, false, false, 5);
            PackStart(text4, false, false, 5);
            PackStart(textbox4, false, false, 5);
            PackStart(text_mode, false, false, 5);
            PackStart(CB_Mode, false, false, 5);
            PackStart(splice_size, false, false, 0);
            PackStart(button_run, false, false, 5);
            PackStart(label_statue, false, false, 5);
            PackStart(text5, false, false, 5);
        }

        public void ModeChange(bool v)
        {
            if (v)
            {
                text1.Text = MAUIStr.Obj.Atlas_Choose5;
                text2.Text = MAUIStr.Obj.Atlas_Choose6;
                text4.Text = MAUIStr.Obj.Atlas_Choose7;
                splice_size.Visible = true;
            }
            else
            {
                text1.Text = MAUIStr.Obj.Atlas_Choose1;
                text2.Text = MAUIStr.Obj.Atlas_Choose2;
                text4.Text = MAUIStr.Obj.Atlas_Choose4;
                splice_size.Visible = false;
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
            text5.Text = MAUIStr.Obj.Share_Running;
            bool mode = TB_Mode.Active;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            string infoFile = textbox3.Text;
            string ID = textbox4.Text;
            int cmode = CB_Mode.Active;
            int MaxWidth = 256 << CB_MaxWidth.Active;
            int MaxHeight = 256 << CB_MaxHeight.Active;
            new Thread(new ThreadStart(() =>
            {
                string err = null;
                try
                {
                    if (mode)
                    {
                        if (!Directory.Exists(inFile))
                        {
                            throw new Exception(string.Format(MAUIStr.Obj.Share_FolderNotFound, inFile));
                        }
                        if (!API.SpliceImage(inFile, outFile, infoFile, ID, cmode, MaxWidth, MaxHeight))
                        {
                            err = MAUIStr.Obj.Atlas_NotFound2;
                        }
                    }
                    else
                    {
                        if (!File.Exists(inFile))
                        {
                            throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
                        }
                        if (!API.CutImage(inFile, outFile, infoFile, ID, cmode))
                        {
                            err = MAUIStr.Obj.Atlas_NotFound1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
                Application.Invoke((s, e) =>
                {
                    if (err == null)
                    {
                        text5.Text = MAUIStr.Obj.Share_Finish;
                    }
                    else
                    {
                        text5.Text = string.Format(MAUIStr.Obj.Share_Wrong, err);
                    }
                    b.Sensitive = true;
                });
            }))
            { IsBackground = true }.Start();
        }

        static VBox CreateHidden(out Label l1, out ComboBox c1, out Label l2, out ComboBox c2)
        {
            l1 = CreateTitle(MAUIStr.Obj.Atlas_MaxWidth);
            c1 = CreateComboBox(3, "256", "512", "1024", "2048", "4096", "8192");
            l2 = CreateTitle(MAUIStr.Obj.Atlas_MaxHeight);
            c2 = CreateComboBox(3, "256", "512", "1024", "2048", "4096", "8192");
            VBox ans = new VBox();
            ans.PackStart(l1, false, false, 5);
            ans.PackStart(c1, false, false, 5);
            ans.PackStart(l2, false, false, 5);
            ans.PackStart(c2, false, false, 5);
            return ans;
        }

        static Entry CreateEntry() => new Entry();

        static HBox CreateEntryAndButton(out Entry e, out Button b)
        {
            e = new Entry();
            b = CreateButton(MAUIStr.Obj.Share_Choose);
            HBox ans = new HBox();
            ans.PackStart(e, true, true, 0);
            ans.PackEnd(b, false, false, 5);
            return ans;
        }

        static HBox CreateMode(out Label l1, out Label l2, out Switch s)
        {
            l1 = CreateTitle(MAUIStr.Obj.Atlas_Mode1);
            l2 = CreateTitle(MAUIStr.Obj.Atlas_Mode2);
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

        static Button CreateButton(string t) => new Button(t);

        static ComboBox CreateComboBox(params string[] s)
        {
            ComboBox ans = new ComboBox(s);
            ans.Active = 0;
            return ans;
        }

        static ComboBox CreateComboBox(int defaultIndex, params string[] s)
        {
            ComboBox ans = new ComboBox(s);
            ans.Active = defaultIndex;
            return ans;
        }
    }
}
