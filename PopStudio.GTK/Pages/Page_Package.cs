using Gtk;
using PopStudio.GUILanguage.Languages;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK.Pages
{
    internal class Page_Package : Plugin.Page
    {
        public Label label_introduction;
        public Label label_choosemode;
        public HBox grid_choosemode;
        public Label label_mode1;
        public Switch switchmode;
        public Label label_mode2;
        public Label label1;
        public HBox choose1;
        public Entry textbox1;
        public Button button1;
        public Label label2;
        public HBox choose2;
        public Entry textbox2;
        public Button button2;
        public Label label3;
        public ComboBox CB_CMode;
        public HBox grid_change1;
        public Label label_changeimage;
        public Switch switchchange1;
        public HBox grid_change2;
        public Label label_deleteimage;
        public Switch switchchange2;
        public Button button_run;
        public Label label_statue;
        public Label text4;

        public Page_Package()
        {
            label_introduction = CreateText(MAUIStr.Obj.Package_Introduction);
            label_choosemode = CreateTitle(MAUIStr.Obj.Share_ChooseMode);
            grid_choosemode = CreateMode(out label_mode1, out label_mode2, out switchmode);
            switchmode.Activate += ModeChange;
            switchmode.ButtonReleaseEvent += ModeChange;
            label1 = CreateTitle(MAUIStr.Obj.Package_Choose1);
            choose1 = CreateEntryAndButton(out textbox1, out button1);
            button1.Clicked += (s, e) =>
            {
                textbox1.Text = switchmode.Active ? PickFile.ChooseFolder() : PickFile.ChooseOpenFile();
            };
            label2 = CreateTitle(MAUIStr.Obj.Package_Choose2);
            choose2 = CreateEntryAndButton(out textbox2, out button2);
            button2.Clicked += (s, e) =>
            {
                textbox2.Text = switchmode.Active ? PickFile.ChooseSaveFile() : PickFile.ChooseFolder();
            };
            label3 = CreateTitle(MAUIStr.Obj.Package_Choose3);
            CB_CMode = CreateComboBox();
            grid_change1 = CreateHideChange(out label_changeimage, out switchchange1, MAUIStr.Obj.Package_ChangeImage);
            grid_change2 = CreateHideChange(out label_deleteimage, out switchchange2, MAUIStr.Obj.Package_DeleteImage);
            button_run = CreateButton(MAUIStr.Obj.Share_Run);
            button_run.Clicked += Do;
            label_statue = CreateTitle(MAUIStr.Obj.Share_RunStatue);
            text4 = CreateTitle(MAUIStr.Obj.Share_Waiting);
            PackStart(label_introduction, false, false, 5);
            PackStart(label_choosemode, false, false, 5);
            PackStart(grid_choosemode, false, false, 5);
            PackStart(label1, false, false, 5);
            PackStart(choose1, false, false, 5);
            PackStart(label2, false, false, 5);
            PackStart(choose2, false, false, 5);
            PackStart(label3, false, false, 5);
            PackStart(CB_CMode, false, false, 5);
            PackStart(grid_change1, false, false, 5);
            PackStart(grid_change2, false, false, 5);
            PackStart(button_run, false, false, 5);
            PackStart(label_statue, false, false, 5);
            PackStart(text4, false, false, 5);
        }

        public void ModeChange(bool v)
        {
            if (v)
            {
                label1.Text = MAUIStr.Obj.Package_Choose4;
                label2.Text = MAUIStr.Obj.Package_Choose5;
                label3.Text = MAUIStr.Obj.Package_Choose6;
                grid_change1.Visible = false;
                grid_change2.Visible = false;
            }
            else
            {
                label1.Text = MAUIStr.Obj.Package_Choose1;
                label2.Text = MAUIStr.Obj.Package_Choose2;
                label3.Text = MAUIStr.Obj.Package_Choose3;
                grid_change1.Visible = true;
                grid_change2.Visible = true;
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

        public void Do(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Sensitive = false;
            text4.Text = MAUIStr.Obj.Share_Running;
            bool mode = switchmode.Active;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            int pmode = CB_CMode.Active;
            bool c1 = switchchange1.Active;
            bool c2 = switchchange2.Active;
            new Thread(new ThreadStart(() =>
            {
                string err = null;
                try
                {
                    if (mode == true)
                    {
                        if (!Directory.Exists(inFile))
                        {
                            throw new Exception(string.Format(MAUIStr.Obj.Share_FolderNotFound, inFile));
                        }
                        API.Pack(inFile, outFile, pmode);
                    }
                    else
                    {
                        if (!File.Exists(inFile))
                        {
                            throw new Exception(string.Format(MAUIStr.Obj.Share_FileNotFound, inFile));
                        }
                        API.Unpack(inFile, outFile, pmode, c1, c2);
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
                        text4.Text = MAUIStr.Obj.Share_Finish;
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

        static HBox CreateHideChange(out Label l, out Switch s, string t)
        {
            l = CreateTitle(t);
            s = CreateSwitch();
            HBox ans = new HBox();
            ans.PackStart(l, false, false, 0);
            ans.PackEnd(s, false, false, 5);
            return ans;
        }

        static HBox CreateMode(out Label l1, out Label l2, out Switch s)
        {
            l1 = CreateTitle(MAUIStr.Obj.Package_Mode1);
            l2 = CreateTitle(MAUIStr.Obj.Package_Mode2);
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

        static ComboBox CreateComboBox()
        {
            ComboBox ans = new ComboBox(new string[] { "dz", "rsb", "pak", "arcv" });
            ans.Active = 0;
            ans.ModifyFont(GenFont(11));
            return ans;
        }
    }
}
