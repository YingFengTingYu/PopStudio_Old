namespace PopStudio.Image.Ptx
{
    internal enum PtxFormat
    {
        ARGB8888, //ABRG8888
        RGBA4444,
        RGB565,
        RGBA5551,
        DXT5 = 5,
        RGBA4444Block = 21,
        RGB565Block,
        RGBA5551Block,
        PVRTC4BPP = 30,
        PVRTC2BPP,
        ETC1,
        ETC2RGB,
        ETC2RGBA,
        BC1,
        BC2,
        BC3,
        ATITCRGB,
        ATITCRGBA,
        ETC1A8 = 147, //ETC1AIndex
        PVRTC4BPPA8,
        XRGB8888A8,
        ETC1AIndex
    }
}
