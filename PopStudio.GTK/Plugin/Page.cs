using Gtk;

namespace PopStudio.GTK.Plugin
{
    internal class Page : VBox
    {
        protected static Pango.FontDescription GenFont(int Size)
        {
#if MACOS
            Size += Size >> 1;
#endif
            return Pango.FontDescription.FromString($"Sans Not-Rotated {Size}");
        }
    }
}
