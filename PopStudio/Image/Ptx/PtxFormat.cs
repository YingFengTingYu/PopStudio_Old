namespace PopStudio.Image.Ptx
{
    internal enum PtxFormat
    {
        ARGB8888, //ABRG8888
        RGBA4444,
        RGB565,
        RGBA5551,
        DXT5 = 5,
        RGBA4444_Block = 21,
        RGB565_Block,
        RGBA5551_Block,
        PVRTC4BPP_RGBA = 30,
        PVRTC2BPP_RGBA,
        ETC1_RGB,
        ETC2_RGB,
        ETC2_RGBA,
        DXT1_RGB,
        DXT3_RGBA,
        DXT5_RGBA,
        ATITC_RGB,
        ATITC_RGBA,
        ETC1_RGB_A8 = 147, //ETC1_RGB_A_Compress
        PVRTC4BPP_RGB_A8,
        XRGB8888_A8,
        ETC1_RGB_A_Compress
    }
}
