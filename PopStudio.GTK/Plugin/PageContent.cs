using Gtk;

namespace PopStudio.GTK.Plugin
{
    internal class PageContent : ScrolledWindow
    {
        Page l;

        public PageContent()
        {
            VscrollbarPolicy = PolicyType.Automatic;
            HscrollbarPolicy = PolicyType.Never;
        }

        public void SetPage(Page p)
        {
            if (l != null) Remove(l);
            l = p;
            l.Margin = 20;
            Add(l);
        }
    }
}
