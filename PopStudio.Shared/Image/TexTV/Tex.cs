using SkiaSharp;

namespace PopStudio.Image.TexTV
{
    internal static class Tex
    {
        public static void Encode(string inFile, string outFile, int format)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (SKBitmap sKBitmap = SKBitmap.Decode(inFile))
                {
                    TexHead head = new TexHead
                    {
                        width = sKBitmap.Width,
                        height = sKBitmap.Height
                    };
                    head.Write(bs);
                    using (BinaryStream bs2 = new BinaryStream())
                    {
                        switch (format)
                        {
                            case 1:
                                Texture.ARGB8888.Write(bs2, sKBitmap);
                                head.format =  TexFormat.ARGB8888;
                                break;
                            case 2:
                                Texture.ARGB4444.Write(bs2, sKBitmap);
                                head.format = TexFormat.ARGB4444;
                                break;
                            case 3:
                                Texture.ARGB1555.Write(bs2, sKBitmap);
                                head.format = TexFormat.ARGB1555;
                                break;
                            case 4:
                                Texture.RGB565.Write(bs2, sKBitmap);
                                head.format = TexFormat.RGB565;
                                break;
                            case 5:
                                Texture.ABGR8888.Write(bs2, sKBitmap);
                                head.format = TexFormat.ABGR8888;
                                break;
                            case 6:
                                Texture.RGBA4444.Write(bs2, sKBitmap);
                                head.format = TexFormat.RGBA4444;
                                break;
                            case 7:
                                Texture.RGBA5551.Write(bs2, sKBitmap);
                                head.format = TexFormat.RGBA5551;
                                break;
                            case 8:
                                Texture.XRGB8888.Write(bs2, sKBitmap);
                                head.format = TexFormat.XRGB8888;
                                break;
                            case 9:
                                Texture.LA88.Write(bs2, sKBitmap);
                                head.format = TexFormat.LA88;
                                break;
                            default:
                                throw new Exception(Str.Obj.UnknownFormat);
                        }
                        bs2.Position = 0;
                        using (ZLibStream zLibStream = new ZLibStream(bs, CompressionMode.Compress, true))
                        {
                            bs2.CopyTo(zLibStream);
                        }
                    }
                    head.zsize = (int)(bs.Length - 0x30);
                    bs.Position = 0;
                    head.Write(bs);
                }
            }
        }

        public static void Encode(string inFile, string outFile, TexFormat format, Endian _ = Endian.Null)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (SKBitmap sKBitmap = SKBitmap.Decode(inFile))
                {
                    TexHead head = new TexHead
                    {
                        width = sKBitmap.Width,
                        height = sKBitmap.Height,
                        format = format
                    };
                    head.Write(bs);
                    using (BinaryStream bs2 = new BinaryStream())
                    {
                        switch (format)
                        {
                            case TexFormat.ARGB8888:
                                Texture.ARGB8888.Write(bs2, sKBitmap);
                                break;
                            case TexFormat.ARGB4444:
                                Texture.ARGB4444.Write(bs2, sKBitmap);
                                break;
                            case TexFormat.ARGB1555:
                                Texture.ARGB1555.Write(bs2, sKBitmap);
                                break;
                            case TexFormat.RGB565:
                                Texture.RGB565.Write(bs2, sKBitmap);
                                break;
                            case TexFormat.ABGR8888:
                                Texture.ABGR8888.Write(bs2, sKBitmap);
                                break;
                            case TexFormat.RGBA4444:
                                Texture.RGBA4444.Write(bs2, sKBitmap);
                                break;
                            case TexFormat.RGBA5551:
                                Texture.RGBA5551.Write(bs2, sKBitmap);
                                break;
                            case TexFormat.XRGB8888:
                                Texture.XRGB8888.Write(bs2, sKBitmap);
                                break;
                            case TexFormat.LA88:
                                Texture.LA88.Write(bs2, sKBitmap);
                                break;
                            default:
                                throw new Exception(Str.Obj.UnknownFormat);
                        }
                        bs2.Position = 0;
                        using (ZLibStream zLibStream = new ZLibStream(bs, CompressionMode.Compress, true))
                        {
                            bs2.CopyTo(zLibStream);
                        }
                    }
                    head.zsize = (int)(bs.Length - 0x30);
                    bs.Position = 0;
                    head.Write(bs);
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream())
            {
                TexHead head;
                using (BinaryStream bs_file = new BinaryStream(inFile, FileMode.Open))
                {
                    head = new TexHead().Read(bs_file);
                    using (ZLibStream zLibStream = new ZLibStream(bs_file, CompressionMode.Decompress))
                    {
                        zLibStream.CopyTo(bs);
                    }
                }
                bs.Position = 0;
                switch (head.format)
                {
                    case TexFormat.ARGB8888:
                        using (SKBitmap sKBitmap = Texture.ARGB8888.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TexFormat.ARGB4444:
                        using (SKBitmap sKBitmap = Texture.ARGB4444.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TexFormat.ARGB1555:
                        using (SKBitmap sKBitmap = Texture.ARGB1555.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TexFormat.RGB565:
                        using (SKBitmap sKBitmap = Texture.RGB565.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TexFormat.ABGR8888:
                        using (SKBitmap sKBitmap = Texture.ABGR8888.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TexFormat.RGBA4444:
                        using (SKBitmap sKBitmap = Texture.RGBA4444.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TexFormat.RGBA5551:
                        using (SKBitmap sKBitmap = Texture.RGBA5551.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TexFormat.XRGB8888:
                        using (SKBitmap sKBitmap = Texture.XRGB8888.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TexFormat.LA88:
                        using (SKBitmap sKBitmap = Texture.LA88.Read(bs, head.width, head.height))
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
