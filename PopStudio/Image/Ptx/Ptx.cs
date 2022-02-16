using SkiaSharp;

namespace PopStudio.Image.Ptx
{
    internal static class Ptx
    {
        public static bool abgrmode = false;
        static Endian endian = Endian.Small;
        public static bool fullwidth = false;

        public static void Encode(string inFile, string outFile, int format)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (SKBitmap sKBitmap = SKBitmap.Decode(inFile))
                {
                    long off = bs.Position;
                    PtxHead head = new PtxHead
                    {
                        width = sKBitmap.Width,
                        height = sKBitmap.Height,
                    };
                    head.Write(bs);
                    switch (format)
                    {
                        case 1:
                            head.check = Texture.ARGB8888.Write(bs, sKBitmap);
                            head.format = PtxFormat.ARGB8888;
                            break;
                        case 2:
                            head.check = Texture.ABGR8888.Write(bs, sKBitmap);
                            head.format = PtxFormat.ARGB8888;
                            break;
                        case 3:
                            head.check = Texture.RGBA4444.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGBA4444;
                            break;
                        case 4:
                            head.check = Texture.RGB565.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGB565;
                            break;
                        case 5:
                            head.check = Texture.RGBA5551.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGBA5551;
                            break;
                        case 6:
                            head.check = Texture.RGBA4444Block.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGBA4444Block;
                            break;
                        case 7:
                            head.check = Texture.RGB565Block.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGB565Block;
                            break;
                        case 8:
                            head.check = Texture.RGBA5551Block.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGBA5551Block;
                            break;
                        case 9:
                            bs.Endian = Endian.Big;
                            head.check = Texture.ARGB8888.Write(bs, sKBitmap);
                            head.format = PtxFormat.ARGB8888;
                            break;
                        case 10:
                            bs.Endian = Endian.Big;
                            if (head.width % 64 == 0)
                            {
                                head.check = Texture.ARGB8888.Write(bs, sKBitmap);
                            }
                            else
                            {
                                using (SKBitmap image2 = new SKBitmap(head.width / 64 * 64 + 64, head.height))
                                {
                                    using (SKCanvas canvas = new SKCanvas(image2))
                                    {
                                        SKRect t = new SKRect(0, 0, head.width, head.height);
                                        canvas.DrawBitmap(sKBitmap, t, t);
                                    }
                                    head.check = Texture.ARGB8888.Write(bs, image2);
                                }
                            }
                            head.format = PtxFormat.ARGB8888;
                            break;
                        case 11:
                            head.check = Texture.DXT1.Write(bs, sKBitmap);
                            head.format = PtxFormat.BC1;
                            break;
                        case 12:
                            head.check = Texture.DXT3.Write(bs, sKBitmap);
                            head.format = PtxFormat.BC2;
                            break;
                        case 13:
                            head.check = Texture.DXT5.Write(bs, sKBitmap);
                            head.format = PtxFormat.BC3;
                            break;
                        case 14:
                            head.check = Texture.DXT5.Write(bs, sKBitmap);
                            head.format = PtxFormat.DXT5;
                            break;
                        case 15:
                            head.check = Texture.DXT5.Write(bs, sKBitmap);
                            head.format = PtxFormat.DXT5; //the texture is also small endian
                            bs.Endian = Endian.Big; //but the info is big endian
                            break;
                        default:
                            throw new Exception(Str.Obj.UnknownFormat);
                    }
                    bs.Position = off;
                    head.Write(bs);
                }
            }
        }

        public static void Encode(string inFile, string outFile, PtxFormat format, Endian encodeendian = Endian.Null)
        {
            if (encodeendian == Endian.Null) encodeendian = endian;
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                bs.Endian = encodeendian;
                using (SKBitmap sKBitmap = SKBitmap.Decode(inFile))
                {
                    long off = bs.Position;
                    PtxHead head = new PtxHead
                    {
                        width = sKBitmap.Width,
                        height = sKBitmap.Height,
                        format = format
                    };
                    head.Write(bs);
                    switch (format)
                    {
                        case PtxFormat.ARGB8888:
                            if (abgrmode)
                            {
                                head.check = Texture.ABGR8888.Write(bs, sKBitmap);
                            }
                            else if (fullwidth && (head.width % 64 != 0))
                            {
                                using (SKBitmap image2 = new SKBitmap(head.width / 64 * 64 + 64, head.height))
                                {
                                    using (SKCanvas canvas = new SKCanvas(image2))
                                    {
                                        SKRect t = new SKRect(0, 0, head.width, head.height);
                                        canvas.DrawBitmap(sKBitmap, t, t);
                                    }
                                    head.check = Texture.ARGB8888.Write(bs, image2);
                                }
                            }
                            else
                            {
                                head.check = Texture.ARGB8888.Write(bs, sKBitmap);
                            }
                            break;
                        case PtxFormat.RGBA4444:
                            head.check = Texture.RGBA4444.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.RGB565:
                            head.check = Texture.RGB565.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.RGBA5551:
                            head.check = Texture.RGBA5551.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.DXT5:
                            Endian backendian = bs.Endian;
                            bs.Endian = Endian.Small;
                            head.check = Texture.DXT5.Write(bs, sKBitmap);
                            bs.Endian = backendian;
                            break;
                        case PtxFormat.RGBA4444Block:
                            head.check = Texture.RGBA4444Block.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.RGB565Block:
                            head.check = Texture.RGB565Block.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.RGBA5551Block:
                            head.check = Texture.RGBA5551Block.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.BC1:
                            head.check = Texture.DXT1.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.BC2:
                            head.check = Texture.DXT3.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.BC3:
                            head.check = Texture.DXT5.Write(bs, sKBitmap);
                            break;
                        default:
                            throw new Exception(Str.Obj.UnknownFormat);
                    }
                    bs.Position = off;
                    head.Write(bs);
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = BinaryStream.Open(inFile))
            {
                PtxHead head = new PtxHead().Read(bs);
                switch (head.format)
                {
                    case PtxFormat.ARGB8888:
                        {
                            if (abgrmode)
                            {
                                using (SKBitmap sKBitmap = Texture.ABGR8888.Read(bs, head.width, head.height))
                                {
                                    sKBitmap.Save(outFile);
                                }
                            }
                            else if ((head.width << 2) == head.check)
                            {
                                using (SKBitmap sKBitmap = Texture.ARGB8888.Read(bs, head.width, head.height))
                                {
                                    sKBitmap.Save(outFile);
                                }
                            }
                            else
                            {
                                using (SKBitmap sKBitmap = Texture.ARGB8888.Read(bs, head.check >> 2, head.height))
                                {
                                    using (SKBitmap image2 = new SKBitmap(head.width, head.height))
                                    {
                                        using (SKCanvas canvas = new SKCanvas(image2))
                                        {
                                            canvas.DrawBitmap(sKBitmap, new SKRect(0, 0, head.check >> 2, head.height));
                                        }
                                        image2.Save(outFile);
                                    }
                                }
                            }
                        }
                        break;
                    case PtxFormat.RGBA4444:
                        using (SKBitmap sKBitmap = Texture.RGBA4444.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.RGB565:
                        using (SKBitmap sKBitmap = Texture.RGB565.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.RGBA5551:
                        using (SKBitmap sKBitmap = Texture.RGBA5551.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.DXT5:
                        bs.Endian = Endian.Small;
                        using (SKBitmap sKBitmap = Texture.DXT5.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.RGBA4444Block:
                        using (SKBitmap sKBitmap = Texture.RGBA4444Block.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.RGB565Block:
                        using (SKBitmap sKBitmap = Texture.RGB565Block.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.RGBA5551Block:
                        using (SKBitmap sKBitmap = Texture.RGBA5551Block.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.PVRTC4BPP:
                        using (SKBitmap sKBitmap = Texture.PVRTC4BPP.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.PVRTC2BPP:
                        using (SKBitmap sKBitmap = Texture.PVRTC2BPP.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.ETC1:
                        using (SKBitmap sKBitmap = Texture.ETC1.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.BC1:
                        using (SKBitmap sKBitmap = Texture.DXT1.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.BC2:
                        using (SKBitmap sKBitmap = Texture.DXT3.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.BC3:
                        using (SKBitmap sKBitmap = Texture.DXT5.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.ETC1A8:
                        if (head.alphaFormat == 0x64)
                        {
                            using (SKBitmap sKBitmap = Texture.ETC1AIndex.Read(bs, head.width, head.height))
                            {
                                sKBitmap.Save(outFile);
                            }
                        }
                        else
                        {
                            using (SKBitmap sKBitmap = Texture.ETC1A8.Read(bs, head.width, head.height))
                            {
                                sKBitmap.Save(outFile);
                            }
                        }
                        break;
                    case PtxFormat.PVRTC4BPPA8:
                        using (SKBitmap sKBitmap = Texture.PVRTC4BPPA8.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.XRGB8888A8:
                        using (SKBitmap sKBitmap = Texture.XRGB8888A8.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.ETC1AIndex:
                        using (SKBitmap sKBitmap = Texture.ETC1AIndex.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    default:
                        throw new Exception(Str.Obj.UnknownFormat);
                }
            }
        }
    }
}
