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
                            head.check = Texture.RGBA4444_Block.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGBA4444_Block;
                            break;
                        case 7:
                            head.check = Texture.RGB565_Block.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGB565_Block;
                            break;
                        case 8:
                            head.check = Texture.RGBA5551_Block.Write(bs, sKBitmap);
                            head.format = PtxFormat.RGBA5551_Block;
                            break;
                        case 9:
                            head.check = Texture.XRGB8888_A8.Write(bs, sKBitmap);
                            head.format = PtxFormat.XRGB8888_A8;
                            break;
                        case 10:
                            bs.Endian = Endian.Big;
                            head.check = Texture.ARGB8888.Write(bs, sKBitmap);
                            head.format = PtxFormat.ARGB8888;
                            break;
                        case 11:
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
                        case 12:
                            head.check = Texture.DXT1_RGB.Write(bs, sKBitmap);
                            head.format = PtxFormat.DXT1_RGB;
                            break;
                        case 13:
                            head.check = Texture.DXT3_RGBA.Write(bs, sKBitmap);
                            head.format = PtxFormat.DXT3_RGBA;
                            break;
                        case 14:
                            head.check = Texture.DXT5_RGBA.Write(bs, sKBitmap);
                            head.format = PtxFormat.DXT5_RGBA;
                            break;
                        case 15:
                            head.check = Texture.DXT5_RGBA.Write(bs, sKBitmap);
                            head.format = PtxFormat.DXT5;
                            break;
                        case 16:
                            head.check = Texture.DXT5_RGBA.Write(bs, sKBitmap);
                            head.format = PtxFormat.DXT5; //the texture is also small endian
                            bs.Endian = Endian.Big; //but the info is big endian
                            break;
                        case 17:
                            head.check = Texture.ETC1_RGB.Write(bs, sKBitmap);
                            head.format = PtxFormat.ETC1_RGB;
                            break;
                        case 18:
                            head.check = Texture.ETC1_RGB_A8.Write(bs, sKBitmap);
                            head.format = PtxFormat.ETC1_RGB_A8;
                            break;
                        case 19:
                            head.check = Texture.ETC1_RGB_A_Compress.Write(bs, sKBitmap, out head.alphaSize);
                            head.format = PtxFormat.ETC1_RGB_A8; //Also 147
                            head.alphaFormat = 0x64;
                            break;
                        case 20:
                            head.check = Texture.ETC1_RGB_A_Compress.Write(bs, sKBitmap, out head.alphaSize);
                            head.format = PtxFormat.ETC1_RGB_A_Compress;
                            head.alphaFormat = 0x64;
                            break;
                        default:
                            throw new Exception(Str.Obj.UnknownFormat);
                    }
                    bs.Position = off;
                    head.Write(bs);
                }
            }
        }

        public static void Encode(string inFile, string outFile, PtxFormat format, Endian encodeendian = Endian.Null, bool chinesemode = false)
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
                            head.check = Texture.DXT5_RGBA.Write(bs, sKBitmap);
                            bs.Endian = backendian;
                            break;
                        case PtxFormat.RGBA4444_Block:
                            head.check = Texture.RGBA4444_Block.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.RGB565_Block:
                            head.check = Texture.RGB565_Block.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.RGBA5551_Block:
                            head.check = Texture.RGBA5551_Block.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.ETC1_RGB:
                            head.check = Texture.ETC1_RGB.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.DXT1_RGB:
                            head.check = Texture.DXT1_RGB.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.DXT3_RGBA:
                            head.check = Texture.DXT3_RGBA.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.DXT5_RGBA:
                            head.check = Texture.DXT5_RGBA.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.ETC1_RGB_A8:
                            if (chinesemode)
                            {
                                head.check = Texture.ETC1_RGB_A_Compress.Write(bs, sKBitmap, out head.alphaSize);
                                head.alphaFormat = 0x64;
                            }
                            else
                            {
                                head.check = Texture.ETC1_RGB_A8.Write(bs, sKBitmap);
                            }
                            break;
                        case PtxFormat.XRGB8888_A8:
                            head.check = Texture.XRGB8888_A8.Write(bs, sKBitmap);
                            break;
                        case PtxFormat.ETC1_RGB_A_Compress:
                            head.check = Texture.ETC1_RGB_A_Compress.Write(bs, sKBitmap, out head.alphaSize);
                            head.alphaFormat = 0x64;
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
                        using (SKBitmap sKBitmap = Texture.DXT5_RGBA.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.RGBA4444_Block:
                        using (SKBitmap sKBitmap = Texture.RGBA4444_Block.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.RGB565_Block:
                        using (SKBitmap sKBitmap = Texture.RGB565_Block.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.RGBA5551_Block:
                        using (SKBitmap sKBitmap = Texture.RGBA5551_Block.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.PVRTC_4BPP_RGBA:
                        using (SKBitmap sKBitmap = Texture.PVRTC_4BPP_RGBA.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.PVRTC_2BPP_RGBA:
                        using (SKBitmap sKBitmap = Texture.PVRTC_2BPP_RGBA.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.ETC1_RGB:
                        using (SKBitmap sKBitmap = Texture.ETC1_RGB.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.DXT1_RGB:
                        using (SKBitmap sKBitmap = Texture.DXT1_RGB.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.DXT3_RGBA:
                        using (SKBitmap sKBitmap = Texture.DXT3_RGBA.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.DXT5_RGBA:
                        using (SKBitmap sKBitmap = Texture.DXT5_RGBA.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.ETC1_RGB_A8:
                        if (head.alphaFormat == 0x64)
                        {
                            using (SKBitmap sKBitmap = Texture.ETC1_RGB_A_Compress.Read(bs, head.width, head.height))
                            {
                                sKBitmap.Save(outFile);
                            }
                        }
                        else
                        {
                            using (SKBitmap sKBitmap = Texture.ETC1_RGB_A8.Read(bs, head.width, head.height))
                            {
                                sKBitmap.Save(outFile);
                            }
                        }
                        break;
                    case PtxFormat.PVRTC_4BPP_RGB_A8:
                        using (SKBitmap sKBitmap = Texture.PVRTC_4BPP_RGB_A8.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.XRGB8888_A8:
                        using (SKBitmap sKBitmap = Texture.XRGB8888_A8.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case PtxFormat.ETC1_RGB_A_Compress:
                        using (SKBitmap sKBitmap = Texture.ETC1_RGB_A_Compress.Read(bs, head.width, head.height))
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
