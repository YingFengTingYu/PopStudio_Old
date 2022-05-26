using PopStudio.Platform;

namespace PopStudio.Image.Txz
{
    internal static class Txz
    {
        public static void Encode(string inFile, string outFile, int format)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (YFBitmap sKBitmap = YFBitmap.Create(inFile))
                {
                    TxzHead head = new TxzHead
                    {
                        width = (ushort)sKBitmap.Width,
                        height = (ushort)sKBitmap.Height
                    };
                    using (BinaryStream bs2 = new BinaryStream())
                    {
                        switch (format)
                        {
                            case 0:
                                head.format = TxzFormat.ABGR8888;
                                head.Write(bs);
                                Texture.ABGR8888.Write(bs2, sKBitmap);
                                break;
                            case 1:
                                head.format = TxzFormat.RGBA4444;
                                head.Write(bs);
                                Texture.RGBA4444.Write(bs2, sKBitmap);
                                break;
                            case 2:
                                head.format = TxzFormat.RGBA5551;
                                head.Write(bs);
                                Texture.RGBA5551.Write(bs2, sKBitmap);
                                break;
                            case 3:
                                head.format = TxzFormat.RGB565;
                                head.Write(bs);
                                Texture.RGB565.Write(bs2, sKBitmap);
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
                }
            }
        }

        public static void Encode(string inFile, string outFile, TxzFormat format, Endian _ = Endian.Null)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (YFBitmap sKBitmap = YFBitmap.Create(inFile))
                {
                    TxzHead head = new TxzHead
                    {
                        width = (ushort)sKBitmap.Width,
                        height = (ushort)sKBitmap.Height,
                        format = format
                    };
                    head.Write(bs);
                    using (BinaryStream bs2 = new BinaryStream())
                    {
                        switch (format)
                        {
                            case TxzFormat.ABGR8888:
                                Texture.ABGR8888.Write(bs2, sKBitmap);
                                break;
                            case TxzFormat.RGBA4444:
                                Texture.RGBA4444.Write(bs2, sKBitmap);
                                break;
                            case TxzFormat.RGBA5551:
                                Texture.RGBA5551.Write(bs2, sKBitmap);
                                break;
                            case TxzFormat.RGB565:
                                Texture.RGB565.Write(bs2, sKBitmap);
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
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream())
            {
                TxzHead head;
                using (BinaryStream bs_file = new BinaryStream(inFile, FileMode.Open))
                {
                    head = new TxzHead().Read(bs_file);
                    using (ZLibStream zLibStream = new ZLibStream(bs_file, CompressionMode.Decompress))
                    {
                        zLibStream.CopyTo(bs);
                    }
                }
                bs.Position = 0;
                switch (head.format)
                {
                    case TxzFormat.ABGR8888:
                        using (YFBitmap sKBitmap = Texture.ABGR8888.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TxzFormat.RGBA4444:
                        using (YFBitmap sKBitmap = Texture.RGBA4444.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TxzFormat.RGBA5551:
                        using (YFBitmap sKBitmap = Texture.RGBA5551.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case TxzFormat.RGB565:
                        using (YFBitmap sKBitmap = Texture.RGB565.Read(bs, head.width, head.height))
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