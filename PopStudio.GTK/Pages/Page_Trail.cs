using Gtk;
using PopStudio.Language.Languages;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK.Pages
{
    internal class Page_Trail : Plugin.Page
    {
        public Label label_introduction;
        public Label text1;
        public HBox choose1;
        public Entry textbox1;
        public Button button1;
        public Label text2;
        public HBox choose2;
        public Entry textbox2;
        public Button button2;
        public Label text_in;
        public ComboBox CB_InMode;
        public Label text_out;
        public ComboBox CB_OutMode;
        public Button button_run;
        public Label label_statue;
        public Label text4;

        public Page_Trail()
        {
            label_introduction = CreateText(MAUIStr.Obj.Trail_Introduction);
            text1 = CreateTitle(MAUIStr.Obj.Trail_Choose1);
            choose1 = CreateEntryAndButton(out textbox1, out button1);
            button1.Clicked += (s, e) => textbox1.Text = PickFile.ChooseOpenFile();
            text2 = CreateTitle(MAUIStr.Obj.Trail_Choose2);
            choose2 = CreateEntryAndButton(out textbox2, out button2);
            button2.Clicked += (s, e) => textbox2.Text = PickFile.ChooseSaveFile();
            text_in = CreateTitle(MAUIStr.Obj.Trail_InFormat);
            CB_InMode = CreateComboBox(0, "PC_Compiled", "Phone32_Compiled", "Phone64_Compiled", "WP_Xnb", "GameConsole_Compiled", "TV_Compiled", "Studio_Json", "Raw_Xml");
            text_out = CreateTitle(MAUIStr.Obj.Trail_OutFormat);
            CB_OutMode = CreateComboBox(7, "PC_Compiled", "Phone32_Compiled", "Phone64_Compiled", "WP_Xnb", "GameConsole_Compiled", "TV_Compiled", "Studio_Json", "Raw_Xml");
            button_run = CreateButton(MAUIStr.Obj.Share_Run);
            button_run.Clicked += Button_Click;
            label_statue = CreateTitle(MAUIStr.Obj.Share_RunStatue);
            text4 = CreateTitle(MAUIStr.Obj.Share_Waiting);
            PackStart(label_introduction, false, false, 5);
            PackStart(text1, false, false, 5);
            PackStart(choose1, false, false, 5);
            PackStart(text2, false, false, 5);
            PackStart(choose2, false, false, 5);
            PackStart(text_in, false, false, 5);
            PackStart(CB_InMode, false, false, 5);
            PackStart(text_out, false, false, 5);
            PackStart(CB_OutMode, false, false, 5);
            PackStart(button_run, false, false, 5);
            PackStart(label_statue, false, false, 5);
            PackStart(text4, false, false, 5);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Sensitive = false;
            text4.Text = MAUIStr.Obj.Share_Running;
            string inFile = textbox1.Text;
            string outFile = textbox2.Text;
            int inmode = CB_InMode.Active;
            int outmode = CB_OutMode.Active;
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
                    API.Trail(inFile, outFile, inmode, outmode);
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

        static ComboBox CreateComboBox(int defaultIndex, params string[] s)
        {
            ComboBox ans = new ComboBox(s);
            ans.Active = defaultIndex;
            ans.ModifyFont(GenFont(11));
            return ans;
        }
    }
}