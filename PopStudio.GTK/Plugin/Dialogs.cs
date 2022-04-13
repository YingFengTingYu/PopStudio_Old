using Gtk;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK.Plugin
{
    internal class Dialogs
    {
        static Pango.FontDescription GenFont(int Size)
        {
#if MACOS
            Size += Size >> 1;
#endif
            return Pango.FontDescription.FromString($"Sans Not-Rotated {Size}");
        }

        public static string DisplayActionSheet(string title, string cancel, string ok, params string[] items)
        {
            string ans = null;
            using (Dialog alert = new Dialog(title, MainPage.Singleten, DialogFlags.Modal, ok, 1, cancel, 2))
            {
                using (ScrolledWindow s = new ScrolledWindow())
                {
                    using (TreeView tree = new TreeView())
                    {
                        tree.HeadersVisible = false;
                        TreeViewColumn v1 = new TreeViewColumn();
                        CellRendererText v1cell = new CellRendererText();
                        v1cell.FontDesc = GenFont(11);
                        v1.PackStart(v1cell, true);
                        v1.AddAttribute(v1cell, "text", 0);
                        tree.AppendColumn(v1);
                        ListStore l = new ListStore(typeof(string));
                        for (int i = 0; i < items.Length; i++)
                        {
                            l.AppendValues(items[i]);
                        }
                        tree.Model = l;
                        s.Add(tree);
                        alert.ContentArea.PackStart(s, false, false, 0);
                        alert.Resizable = false;
                        s.WidthRequest = 350;
                        s.VscrollbarPolicy = PolicyType.Never;
                        s.HscrollbarPolicy = PolicyType.Automatic;
                        if (items.Length > 8)
                        {
                            s.HeightRequest = 200;
                            s.VscrollbarPolicy = PolicyType.Automatic;
                        }
                        alert.ShowAll();
                        if (alert.Run() == 1 && items.Length > 0)
                        {
                            TreeSelection t = tree.Selection;
                            t.GetSelected(out ITreeModel model, out TreeIter iter);
                            ans = (string)model.GetValue(iter, 0);
                        }
                        alert.Destroy();
                    }
                }
            }
            return ans;
        }

        public static void DisplayAlert(string title, string message, string cancel)
        {
            using (Dialog alert = new Dialog(title, MainPage.Singleten, DialogFlags.Modal, cancel, 1))
            {
                using (Label l = CreateText(message))
                {
                    l.MarginLeft = 20;
                    l.MarginRight = 20;
                    l.MarginTop = 20;
                    l.MarginBottom = 50;
                    l.MaxWidthChars = 40;
                    l.ModifyFont(GenFont(11));
                    alert.ContentArea.PackStart(l, false, false, 0);
                    alert.Resizable = false;
                    alert.ShowAll();
                    alert.Run();
                    alert.Destroy();
                }
            }
        }

        public static bool DisplayAlert(string title, string message, string accept, string cancel)
        {
            bool ans;
            using (Dialog alert = new Dialog(title, MainPage.Singleten, DialogFlags.Modal, accept, 1, cancel, 2))
            {
                using (Label l = CreateText(message))
                {
                    l.MarginLeft = 20;
                    l.MarginRight = 20;
                    l.MarginTop = 20;
                    l.MarginBottom = 50;
                    l.MaxWidthChars = 40;
                    l.ModifyFont(GenFont(11));
                    alert.WidthRequest = 350;
                    alert.ContentArea.PackStart(l, false, false, 0);
                    alert.Resizable = false;
                    alert.ShowAll();
                    ans = alert.Run() == 1;
                    alert.Destroy();
                }
            }
            return ans;
        }

        /// <summary>
        /// It's not async. I just want to keep the same name as the other projects.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="cancel"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static string DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string initialValue = "")
        {
            string ans = null;
            using (Dialog alert = new Dialog(title, MainPage.Singleten, DialogFlags.Modal, accept, 1, cancel, 2))
            {
                using (Label l = CreateText(message))
                {
                    l.MarginLeft = 20;
                    l.MarginRight = 20;
                    l.MarginTop = 20;
                    l.MaxWidthChars = 40;
                    l.ModifyFont(GenFont(11));
                    using (Entry entry = new Entry(initialValue))
                    {
                        entry.Margin = 20;
                        entry.ModifyFont(GenFont(11));
                        alert.WidthRequest = 350;
                        alert.ContentArea.PackStart(l, false, false, 0);
                        alert.ContentArea.PackStart(entry, false, false, 0);
                        alert.Resizable = false;
                        alert.ShowAll();
                        if (alert.Run() == 1) ans = entry.Text;
                        alert.Destroy();
                    }
                }
            }
            return ans;
        }

        public static void DisplayPicture(string title, byte[] img, string cancel = "OK", System.Action action = null, bool TouchLeave = false)
        {
            Gdk.Pixbuf image = new Gdk.Pixbuf(img);
            image = image.ScaleSimple(615, 435, Gdk.InterpType.Hyper);
            using (Dialog alert = new Dialog(title, MainPage.Singleten, DialogFlags.Modal, cancel, 1))
            {
                using (EventBox box = new EventBox())
                {
                    using (Gtk.Image l = new Gtk.Image(image))
                    {
                        box.Add(l);
                        alert.ContentArea.PackStart(box, false, false, 0);
                        l.Margin = 20;
                        box.ButtonReleaseEvent += (s, e) =>
                        {
                            if (action != null) action();
                            if (TouchLeave)
                            {
                                alert.Destroy();
                            }
                        };
                        alert.ShowAll();
                        alert.Run();
                        alert.Destroy();
                    }
                }
            }
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
