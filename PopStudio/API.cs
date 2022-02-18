namespace PopStudio.Plugin
{
    internal class API
    {
        //Write for lua(coming soon)
        public static BinaryStream GetStream(string path, int mode)
        {
            switch (mode)
            {
                case 0: return new BinaryStream(path, FileMode.Open);
                case 1: return new BinaryStream(path, FileMode.Create);
                case 2: return new BinaryStream(path, FileMode.OpenOrCreate);
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static BinaryStream GetStream(byte[] bytes)
        {
            return new BinaryStream(bytes);
        }

        public static BinaryStream GetStream()
        {
            return new BinaryStream();
        }

        public static void Unpack(string inFile, string outFile, int format, bool changeimage = false, bool delete = false)
        {
            switch (format)
            {
                case 0: Package.Dz.Dz.Unpack(inFile, outFile, changeimage, delete); return;
                case 1: Package.Rsb.Rsb.Unpack(inFile, outFile, changeimage, delete); return;
                case 2: Package.Pak.Pak.Unpack(inFile, outFile, changeimage, delete); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void Pack(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: Package.Dz.Dz.Pack(inFile, outFile); return;
                case 1: Package.Rsb.Rsb.Pack(inFile, outFile); return;
                case 2: Package.Pak.Pak.Pack(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void Decompress(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: PopStudio.Compress.Zlib.Decompress(inFile, outFile); return;
                case 1: PopStudio.Compress.Gzip.Decompress(inFile, outFile); return;
                case 2: PopStudio.Compress.Deflate.Decompress(inFile, outFile); return;
                case 3: PopStudio.Compress.Brotli.Decompress(inFile, outFile); return;
                case 4: PopStudio.Compress.Lzma.Decompress(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void Compress(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: PopStudio.Compress.Zlib.Compress(inFile, outFile); return;
                case 1: PopStudio.Compress.Gzip.Compress(inFile, outFile); return;
                case 2: PopStudio.Compress.Deflate.Compress(inFile, outFile); return;
                case 3: PopStudio.Compress.Brotli.Compress(inFile, outFile); return;
                case 4: PopStudio.Compress.Lzma.Compress(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }
    }
}
