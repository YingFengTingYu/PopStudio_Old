using PopStudio.Platform;

namespace PopStudio.Image.TexTV
{
    internal static class Tex
    {
        public static void Encode(string inFile, string outFile, int format)
        {
            bool zlib = true;
            if (format >= 7)
            {
                format -= 7;
                zlib = false;
            }
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (YFBitmap sKBitmap = YFBitmap.Create(inFile))
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
                            case 0:
                                Texture.ARGB8888.Write(bs2, sKBitmap);
                                head.format = 2;
                                break;
                            case 1:
                                Texture.ARGB4444.Write(bs2, sKBitmap);
                                head.format = 3;
                                break;
                            case 2:
                                Texture.ARGB1555.Write(bs2, sKBitmap);
                                head.format = 4;
                                break;
                            case 3:
                                Texture.RGB565.Write(bs2, sKBitmap);
                                head.format = 5;
                                break;
                            case 4:
                                Texture.ABGR8888.Write(bs2, sKBitmap);
                                head.format = 6;
                                break;
                            case 5:
                                Texture.RGBA4444.Write(bs2, sKBitmap);
                                head.format = 7;
                                break;
                            case 6:
                                Texture.RGBA5551.Write(bs2, sKBitmap);
                                head.format = 8;
                                break;
                            default:
                                throw new Exception(Str.Obj.UnknownFormat);
                        }
                        bs2.Position = 0;
                        if (zlib)
                        {
                            using (ZLibStream zLibStream = new ZLibStream(bs, CompressionMode.Compress, true))
                            {
                                bs2.CopyTo(zLibStream);
                            }
                            head.zsize = (int)(bs.Length - 0x30);
                            head.flags |= 1u;
                        }
                        else
                        {
                            bs2.CopyTo(bs);
                            head.zsize = 0;
                            head.flags &= ~1u;
                        }
                    }
                    
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
                    if ((head.flags & 0b1) != 0)
                    {
                        using (ZLibStream zLibStream = new ZLibStream(bs_file, CompressionMode.Decompress))
                        {
                            zLibStream.CopyTo(bs);
                        }
                    }
                    else
                    {
                        bs_file.CopyTo(bs);
                    }
                }
                bs.Position = 0;
                switch (head.format)
                {
                    case 2:
                        using (YFBitmap sKBitmap = Texture.ARGB8888.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 3:
                        using (YFBitmap sKBitmap = Texture.ARGB4444.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 4:
                        using (YFBitmap sKBitmap = Texture.ARGB1555.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 5:
                        using (YFBitmap sKBitmap = Texture.RGB565.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 6:
                        using (YFBitmap sKBitmap = Texture.ABGR8888.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 7:
                        using (YFBitmap sKBitmap = Texture.RGBA4444.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 8:
                        using (YFBitmap sKBitmap = Texture.RGBA5551.Read(bs, head.width, head.height))
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
