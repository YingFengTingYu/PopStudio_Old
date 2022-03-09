using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using PopStudio.MAUI.Languages;

namespace PopStudio.MAUI
{
	public partial class Page_LuaScript : ContentPage
	{
		public Page_LuaScript()
		{
			InitializeComponent();
			Title = MAUIStr.Obj.LuaScript_Title;
			label_introduction.Text = MAUIStr.Obj.LuaScript_Introduction;
			label_print.Text = MAUIStr.Obj.LuaScript_TracePrint;
			button_run.Text = MAUIStr.Obj.Share_Run;
			API.box = this;
		}

        private void button_run_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            richtextbox2.Text = string.Empty;
            string script = richtextbox1.Text;
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
                Device.BeginInvokeOnMainThread(() => { b.IsEnabled = true; });
            }))
            { IsBackground = true }.Start();
        }

        public void Print(string str)
        {
            richtextbox2.Text += str + '\n';
        }
	}
}