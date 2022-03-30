using Gtk;
using PopStudio.GUILanguage.Languages;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK.Pages
{
    internal class Page_LuaScript : Plugin.Page
    {
        public Label label_introduction;
        public ScrolledWindow richtextbox1_window;
        public TextView richtextbox1;
        public Table button_parent;
        public Button button_run;
        public Label label_print;
        public ScrolledWindow richtextbox2_window;
        public TextView richtextbox2;

        public Page_LuaScript()
        {
            label_introduction = CreateText(MAUIStr.Obj.LuaScript_Introduction);
            richtextbox1_window = CreateRichTextBox(out richtextbox1);
            richtextbox1_window.HeightRequest = 250;
            button_parent = CreateTButton(out button_run);
            button_run.Clicked += button_run_Click;
            label_print = CreateText(MAUIStr.Obj.LuaScript_TracePrint);
            richtextbox2_window = CreateRichTextBox(out richtextbox2);
            richtextbox2_window.HeightRequest = 140;
            PackStart(label_introduction, false, false, 5);
            PackStart(richtextbox1_window, false, false, 5);
            PackStart(button_parent, false, false, 5);
            PackStart(label_print, false, false, 5);
            PackStart(richtextbox2_window, false, false, 5);
            API.box = richtextbox2;
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Sensitive = false;
            richtextbox2.Buffer.Text = string.Empty;
            API.FirstTime = true;
            string script = richtextbox1.Buffer.Text;
            new Thread(new ThreadStart(() =>
            {
                bool cg = true;
                try
                {
                    API.DoScript(script);
                }
                catch (Exception ex)
                {
                    cg = false;
                    API.Print(string.Format(MAUIStr.Obj.Share_Wrong, ex.Message));
                }
                if (cg)
                {
                    API.Print(MAUIStr.Obj.Share_Finish);
                }
                Application.Invoke((s, e) => { b.Sensitive = true; });
            }))
            { IsBackground = true }.Start();
        }

        static Table CreateTButton(out Button b)
        {
            b = new Button(MAUIStr.Obj.Share_Run);
            Table ans = new Table(1, 5, true);
            ans.Attach(b, 1, 4, 0, 1);
            return ans;
        }

        static ScrolledWindow CreateRichTextBox(out TextView t)
        {
            t = new TextView();
            ScrolledWindow ans = new ScrolledWindow();
            ans.Add(t);
            return ans;
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
    }
}
