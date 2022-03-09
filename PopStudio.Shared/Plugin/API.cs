namespace PopStudio.Plugin
{
    internal static partial class API
    {
        static List<IDisposable> Disposable = new List<IDisposable>();

        public static void DoScript(string script)
        {
            Script.Do(script);
        }

        public static void DisposeAll()
        {
            int count = Disposable.Count;
            for (int i = 0; i < count; i++)
            {
                Disposable[i].Dispose();
            }
            Disposable.Clear();
        }

        public static partial void Print(params object[] os);

        public static partial bool? Alert(string text, string title, bool ask = false);

        public static partial string Prompt(string text, string title, string defaulttext = "");

        public static partial string Sheet(string title, params string[] items);

        public static partial string ChooseFolder();

        public static partial string ChooseOpenFile();

        public static partial string ChooseSaveFile();

        //Write for lua(coming soon) and MAUI
        public static BinaryStream GetFileStream(string path, int mode)
        {
            BinaryStream ans = mode switch
            {
                0 => new BinaryStream(path, FileMode.Open),
                1 => new BinaryStream(path, FileMode.Create),
                2 => new BinaryStream(path, FileMode.OpenOrCreate),
                _ => throw new Exception(Str.Obj.TypeMisMatch)
            };
            Disposable.Add(ans);
            return ans;
        }

        public static BinaryStream GetMemoryStream(byte[] bytes = null)
        {
            BinaryStream ans;
            if (bytes == null)
            {
                ans = new BinaryStream();
            }
            else
            {
                ans = new BinaryStream(bytes);
            }
            Disposable.Add(ans);
            return ans;
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

        public static void Trail(string inFile, string outFile, int informat, int outformat)
        {
            Trail.TrailFormat inFormat = (Trail.TrailFormat)informat;
            Trail.TrailFormat outFormat = (Trail.TrailFormat)outformat;
            Trail.Trail trail = inFormat switch
            {
                PopStudio.Trail.TrailFormat.PCCompiled => PopStudio.Trail.PC.Decode(inFile),
                PopStudio.Trail.TrailFormat.Phone32Compiled => PopStudio.Trail.Phone32.Decode(inFile),
                PopStudio.Trail.TrailFormat.Phone64Compiled => PopStudio.Trail.Phone64.Decode(inFile),
                PopStudio.Trail.TrailFormat.WPXnb => PopStudio.Trail.WP.Decode(inFile),
                PopStudio.Trail.TrailFormat.GameConsoleCompiled => PopStudio.Trail.GameConsole.Decode(inFile),
                PopStudio.Trail.TrailFormat.TVCompiled => PopStudio.Trail.TV.Decode(inFile),
                PopStudio.Trail.TrailFormat.Json => PopStudio.Trail.TrailJson.Decode(inFile),
                _ => throw new NotImplementedException()
            };
            switch (outFormat)
            {
                case PopStudio.Trail.TrailFormat.PCCompiled: PopStudio.Trail.PC.Encode(trail, outFile); break;
                case PopStudio.Trail.TrailFormat.Phone32Compiled: PopStudio.Trail.Phone32.Encode(trail, outFile); break;
                case PopStudio.Trail.TrailFormat.Phone64Compiled: PopStudio.Trail.Phone64.Encode(trail, outFile); break;
                case PopStudio.Trail.TrailFormat.WPXnb: PopStudio.Trail.WP.Encode(trail, outFile); break;
                case PopStudio.Trail.TrailFormat.GameConsoleCompiled: PopStudio.Trail.GameConsole.Encode(trail, outFile); break;
                case PopStudio.Trail.TrailFormat.TVCompiled: PopStudio.Trail.TV.Encode(trail, outFile); break;
                case PopStudio.Trail.TrailFormat.Json: PopStudio.Trail.TrailJson.Encode(trail, outFile); break;
                default: throw new NotImplementedException();
            }
        }

        public static void Particles(string inFile, string outFile, int informat, int outformat)
        {
            Particles.ParticlesFormat inFormat = (Particles.ParticlesFormat)informat;
            Particles.ParticlesFormat outFormat = (Particles.ParticlesFormat)outformat;
            Particles.Particles particles = inFormat switch
            {
                PopStudio.Particles.ParticlesFormat.PCCompiled => PopStudio.Particles.PC.Decode(inFile),
                PopStudio.Particles.ParticlesFormat.Phone32Compiled => PopStudio.Particles.Phone32.Decode(inFile),
                PopStudio.Particles.ParticlesFormat.Phone64Compiled => PopStudio.Particles.Phone64.Decode(inFile),
                PopStudio.Particles.ParticlesFormat.WPXnb => PopStudio.Particles.WP.Decode(inFile),
                PopStudio.Particles.ParticlesFormat.GameConsoleCompiled => PopStudio.Particles.GameConsole.Decode(inFile),
                PopStudio.Particles.ParticlesFormat.TVCompiled => PopStudio.Particles.TV.Decode(inFile),
                PopStudio.Particles.ParticlesFormat.Json => PopStudio.Particles.ParticlesJson.Decode(inFile),
                _ => throw new NotImplementedException()
            };
            switch (outFormat)
            {
                case PopStudio.Particles.ParticlesFormat.PCCompiled: PopStudio.Particles.PC.Encode(particles, outFile); break;
                case PopStudio.Particles.ParticlesFormat.Phone32Compiled: PopStudio.Particles.Phone32.Encode(particles, outFile); break;
                case PopStudio.Particles.ParticlesFormat.Phone64Compiled: PopStudio.Particles.Phone64.Encode(particles, outFile); break;
                case PopStudio.Particles.ParticlesFormat.WPXnb: PopStudio.Particles.WP.Encode(particles, outFile); break;
                case PopStudio.Particles.ParticlesFormat.GameConsoleCompiled: PopStudio.Particles.GameConsole.Encode(particles, outFile); break;
                case PopStudio.Particles.ParticlesFormat.TVCompiled: PopStudio.Particles.TV.Encode(particles, outFile); break;
                case PopStudio.Particles.ParticlesFormat.Json: PopStudio.Particles.ParticlesJson.Encode(particles, outFile); break;
                default: throw new NotImplementedException();
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

        public static void NewDir(string filePath, bool toEnd = true)
        {
            Dir.NewDir(filePath, toEnd);
        }

        public static string[] GetFiles(string filePath)
        {
            return Dir.GetFiles(filePath);
        }

        public static string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public static string GetFileName(string fileName)
        {
            return Path.GetFileName(fileName);
        }

        public static string GetFilePath(string fileName)
        {
            return Path.GetDirectoryName(fileName);
        }

        public static string GetFileNameWithoutExtension(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        public static int GetVersion()
        {
            return Const.RAINYVERSION;
        }

        public static int GetSystem()
        {
            return Const.SYSTEM;
        }

        public static string FormatPath(string path)
        {
            return Dir.FormatPath(path);
        }

        public static void DoFile(string filepath, params object[] args)
        {
            string s;
            (Script.luavm["rainy"] as NLua.LuaTable)["cache"] = args;
            using (StreamReader sr = new StreamReader(filepath.ToString()))
            {
                s = "local args = rainy.array2table(rainy.cache); rainy.cache = nil; " + sr.ReadToEnd();
            }
            Script.luavm.DoString(s);
        }

        public static object[] CreateArray(int length)
        {
            return new object[length];
        }
    }
}