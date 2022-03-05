namespace PopStudio.Plugin
{
    internal static class API
    {
        //Write for lua(coming soon) and MAUI
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

        public static void DecodeImage(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: Image.Ptx.Ptx.Decode(inFile, outFile); return;
                case 1: Image.Cdat.Cdat.Decode(inFile, outFile); return;
                case 2: Image.Tex.Tex.Decode(inFile, outFile); return;
                case 3: Image.Txz.Txz.Decode(inFile, outFile); return;
                case 4: Image.TexTV.Tex.Decode(inFile, outFile); return;
                case 5: Image.PtxXbox360.Ptx.Decode(inFile, outFile); return;
                case 6: Image.PtxPS3.Ptx.Decode(inFile, outFile); return;
                case 7: Image.PtxPSV.Ptx.Decode(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void EncodeImage(string inFile, string outFile, int format, int format2)
        {
            switch (format)
            {
                case 0: Image.Ptx.Ptx.Encode(inFile, outFile, format2); return;
                case 1: Image.Cdat.Cdat.Encode(inFile, outFile); return;
                case 2: Image.Tex.Tex.Encode(inFile, outFile, format2); return;
                case 3: Image.Txz.Txz.Encode(inFile, outFile, format2); return;
                case 4: Image.TexTV.Tex.Encode(inFile, outFile, format2); return;
                case 5: Image.PtxXbox360.Ptx.Encode(inFile, outFile); return;
                case 6: Image.PtxPS3.Ptx.Encode(inFile, outFile); return;
                case 7: Image.PtxPSV.Ptx.Encode(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void Reanim(string inFile, string outFile, int informat, int outformat)
        {
            Reanim.ReanimFormat inFormat = (Reanim.ReanimFormat)informat;
            Reanim.ReanimFormat outFormat = (Reanim.ReanimFormat)outformat;
            Reanim.Reanim reanim = inFormat switch
            {
                PopStudio.Reanim.ReanimFormat.PCCompiled => PopStudio.Reanim.PC.Decode(inFile),
                PopStudio.Reanim.ReanimFormat.Phone32Compiled => PopStudio.Reanim.Phone32.Decode(inFile),
                PopStudio.Reanim.ReanimFormat.Phone64Compiled => PopStudio.Reanim.Phone64.Decode(inFile),
                PopStudio.Reanim.ReanimFormat.WPXnb => PopStudio.Reanim.WP.Decode(inFile),
                PopStudio.Reanim.ReanimFormat.GameConsoleCompiled => PopStudio.Reanim.GameConsole.Decode(inFile),
                PopStudio.Reanim.ReanimFormat.TVCompiled => PopStudio.Reanim.TV.Decode(inFile),
                PopStudio.Reanim.ReanimFormat.Json => PopStudio.Reanim.ReanimJson.Decode(inFile),
                PopStudio.Reanim.ReanimFormat.RawXml => PopStudio.Reanim.RawXml.Decode(inFile),
                _ => throw new NotImplementedException()
            };
            switch (outFormat)
            {
                case PopStudio.Reanim.ReanimFormat.PCCompiled: PopStudio.Reanim.PC.Encode(reanim, outFile); break;
                case PopStudio.Reanim.ReanimFormat.Phone32Compiled: PopStudio.Reanim.Phone32.Encode(reanim, outFile); break;
                case PopStudio.Reanim.ReanimFormat.Phone64Compiled: PopStudio.Reanim.Phone64.Encode(reanim, outFile); break;
                case PopStudio.Reanim.ReanimFormat.WPXnb: PopStudio.Reanim.WP.Encode(reanim, outFile); break;
                case PopStudio.Reanim.ReanimFormat.GameConsoleCompiled: PopStudio.Reanim.GameConsole.Encode(reanim, outFile); break;
                case PopStudio.Reanim.ReanimFormat.TVCompiled: PopStudio.Reanim.TV.Encode(reanim, outFile); break;
                case PopStudio.Reanim.ReanimFormat.Json: PopStudio.Reanim.ReanimJson.Encode(reanim, outFile); break;
                case PopStudio.Reanim.ReanimFormat.RawXml: PopStudio.Reanim.RawXml.Encode(reanim, outFile); break;
                case PopStudio.Reanim.ReanimFormat.FlashXfl: PopStudio.Reanim.FlashXfl.Encode(reanim, outFile); break;
                default: throw new NotImplementedException();
            }
        }

        public static void DecodeTrail(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: Trail.PC.Decode(inFile, outFile); return;
                case 1: Trail.Phone32.Decode(inFile, outFile); return;
                case 2: Trail.Phone64.Decode(inFile, outFile); return;
                case 3: Trail.WP.Decode(inFile, outFile); return;
                case 4: Trail.GameConsole.Decode(inFile, outFile); return;
                case 5: Trail.TV.Decode(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void EncodeTrail(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: Trail.PC.Encode(inFile, outFile); return;
                case 1: Trail.Phone32.Encode(inFile, outFile); return;
                case 2: Trail.Phone64.Encode(inFile, outFile); return;
                case 3: Trail.WP.Encode(inFile, outFile); return;
                case 4: Trail.GameConsole.Encode(inFile, outFile); return;
                case 5: Trail.TV.Encode(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void DecodeParticles(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: Particles.PC.Decode(inFile, outFile); return;
                case 1: Particles.Phone32.Decode(inFile, outFile); return;
                case 2: Particles.Phone64.Decode(inFile, outFile); return;
                case 3: Particles.WP.Decode(inFile, outFile); return;
                case 4: Particles.GameConsole.Decode(inFile, outFile); return;
                case 5: Particles.TV.Decode(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void EncodeParticles(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: Particles.PC.Encode(inFile, outFile); return;
                case 1: Particles.Phone32.Encode(inFile, outFile); return;
                case 2: Particles.Phone64.Encode(inFile, outFile); return;
                case 3: Particles.WP.Encode(inFile, outFile); return;
                case 4: Particles.GameConsole.Encode(inFile, outFile); return;
                case 5: Particles.TV.Encode(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void DecodeRTON(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: RTON.RTON.Decode(inFile, outFile); return;
                case 1: RTON.RTON.DecodeAndDecrypt(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void EncodeRTON(string inFile, string outFile, int format)
        {
            switch (format)
            {
                case 0: RTON.RTON.Encode(inFile, outFile); return;
                case 1: RTON.RTON.EncodeAndEncrypt(inFile, outFile); return;
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
                case 5: PopStudio.Compress.Lz4.Decompress(inFile, outFile); return;
                case 6: PopStudio.Compress.Bzip2.Decompress(inFile, outFile); return;
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
                case 5: PopStudio.Compress.Lz4.Compress(inFile, outFile); return;
                case 6: PopStudio.Compress.Bzip2.Compress(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }
    }
}