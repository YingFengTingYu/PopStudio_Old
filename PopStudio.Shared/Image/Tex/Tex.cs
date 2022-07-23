using PopStudio.Platform;

namespace PopStudio.Image.Tex
{
    internal static class Tex
    {
        public static void Encode(string inFile, string outFile, int format)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (YFBitmap sKBitmap = YFBitmap.Create(inFile))
                {
                    TexHead head = new TexHead
                    {
                        width = (ushort)sKBitmap.Width,
                        height = (ushort)sKBitmap.Height
                    };
                    switch (format)
                    {
                        case 0:
                            head.format = 1;
                            head.Write(bs);
                            Texture.ABGR8888.Write(bs, sKBitmap);
                            break;
                        case 1:
                            head.format = 2;
                            head.Write(bs);
                            Texture.RGBA4444.Write(bs, sKBitmap);
                            break;
                        case 2:
                            head.format = 3;
                            head.Write(bs);
                            Texture.RGBA5551.Write(bs, sKBitmap);
                            break;
                        case 3:
                            head.format = 4;
                            head.Write(bs);
                            Texture.RGB565.Write(bs, sKBitmap);
                            break;
                        default:
                            throw new Exception(Str.Obj.UnknownFormat);
                    }
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                TexHead head = new TexHead().Read(bs);
                switch (head.format)
                {
                    case 1:
                        using (YFBitmap sKBitmap = Texture.ABGR8888.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 2:
                        using (YFBitmap sKBitmap = Texture.RGBA4444.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 3:
                        using (YFBitmap sKBitmap = Texture.RGBA5551.Read(bs, head.width, head.height))
                        {
                            sKBitmap.Save(outFile);
                        }
                        break;
                    case 4:
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