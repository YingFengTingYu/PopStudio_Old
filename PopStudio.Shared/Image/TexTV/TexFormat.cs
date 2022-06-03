namespace PopStudio.Image.TexTV
{
    internal enum TexFormat
    {
        NONE,
        LUT8, //Invalid in game because "(unsigned int)(format - 2) > 8" will return directly
        ARGB8888,
        ARGB4444,
        ARGB1555,
        RGB565,
        ABGR8888,
        RGBA4444,
        RGBA5551,
        XRGB8888,
        LA88
    }
}
