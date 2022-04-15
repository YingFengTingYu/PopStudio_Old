namespace PopStudio.Plugin
{
    internal static partial class API
    {
        static IDisposablePool Disposable = new IDisposablePool();

        public static void CloseException() => LuaScript.Script.luavm.ErrorThrow = false;

        public static void OpenException() => LuaScript.Script.luavm.ErrorThrow = true;

        public static void ThrowException(string msg) => throw new Exception(msg);

        public static void DoScript(string script) => LuaScript.Script.Do(script ?? string.Empty);

        public static void DisposeAll() => Disposable.Dispose();

        public static partial void Print(params object[] os);

        public static partial bool? Alert(string text = "", string title = "PopStudio", bool ask = false);

        public static partial string Prompt(string text = "", string title = "PopStudio", string defaulttext = "");

        public static partial string Sheet(string title = "PopStudio", params string[] items);

        public static partial string ChooseFolder();

        public static partial string ChooseOpenFile();

        public static partial string ChooseSaveFile();

        public static partial void OpenUrl(string url = "");

        public static BinaryStream GetFileStream(string path, int mode) => Disposable.Add(mode switch
        {
            0 => new BinaryStream(path, FileMode.Open),
            1 => new BinaryStream(path, FileMode.Create),
            2 => new BinaryStream(path, FileMode.OpenOrCreate),
            _ => throw new Exception(Str.Obj.TypeMisMatch)
        });

        public static BinaryStream GetMemoryStream(byte[] bytes = null) => Disposable.Add(bytes == null ? new BinaryStream() : new BinaryStream(bytes));

        public static TempFilePool GetTempFilePool() => Disposable.Add(new TempFilePool());

        public static void Unpack(string inFile, string outFile, int format, bool changeimage = false, bool delete = false)
        {
            switch (format)
            {
                case 0: Package.Dz.Dz.Unpack(inFile, outFile, changeimage, delete); return;
                case 1: Package.Rsb.Rsb.Unpack(inFile, outFile, changeimage, delete); return;
                case 2: Package.Pak.Pak.Unpack(inFile, outFile, changeimage, delete); return;
                case 3: Package.Arcv.Arcv.Unpack(inFile, outFile); return;
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
                case 3: Package.Arcv.Arcv.Pack(inFile, outFile); return;
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
                case 8: Image.Xnb.Xnb.Decode(inFile, outFile); return;
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
                case 8: Image.Xnb.Xnb.Encode(inFile, outFile); return;
                default: throw new Exception(Str.Obj.TypeMisMatch);
            }
        }

        public static void Reanim(string inFile, string outFile, int informat, int outformat)
        {
            Reanim.Reanim reanim = informat switch
            {
                0 => PopStudio.Reanim.PC.Decode(inFile),
                1 => PopStudio.Reanim.Phone32.Decode(inFile),
                2 => PopStudio.Reanim.Phone64.Decode(inFile),
                3 => PopStudio.Reanim.WP.Decode(inFile),
                4 => PopStudio.Reanim.GameConsole.Decode(inFile),
                5 => PopStudio.Reanim.TV.Decode(inFile),
                6 => PopStudio.Reanim.ReanimJson.Decode(inFile),
                7 => PopStudio.Reanim.RawXml.Decode(inFile),
                _ => throw new NotImplementedException()
            };
            switch (outformat)
            {
                case 0: PopStudio.Reanim.PC.Encode(reanim, outFile); break;
                case 1: PopStudio.Reanim.Phone32.Encode(reanim, outFile); break;
                case 2: PopStudio.Reanim.Phone64.Encode(reanim, outFile); break;
                case 3: PopStudio.Reanim.WP.Encode(reanim, outFile); break;
                case 4: PopStudio.Reanim.GameConsole.Encode(reanim, outFile); break;
                case 5: PopStudio.Reanim.TV.Encode(reanim, outFile); break;
                case 6: PopStudio.Reanim.ReanimJson.Encode(reanim, outFile); break;
                case 7: PopStudio.Reanim.RawXml.Encode(reanim, outFile); break;
                case 8: PopStudio.Reanim.FlashXfl.Encode(reanim, outFile); break;
                default: throw new NotImplementedException();
            }
        }

        public static void Trail(string inFile, string outFile, int informat, int outformat)
        {
            Trail.Trail trail = informat switch
            {
                0 => PopStudio.Trail.PC.Decode(inFile),
                1 => PopStudio.Trail.Phone32.Decode(inFile),
                2 => PopStudio.Trail.Phone64.Decode(inFile),
                3 => PopStudio.Trail.WP.Decode(inFile),
                4 => PopStudio.Trail.GameConsole.Decode(inFile),
                5 => PopStudio.Trail.TV.Decode(inFile),
                6 => PopStudio.Trail.TrailJson.Decode(inFile),
                7 => PopStudio.Trail.RawXml.Decode(inFile),
                _ => throw new NotImplementedException()
            };
            switch (outformat)
            {
                case 0: PopStudio.Trail.PC.Encode(trail, outFile); break;
                case 1: PopStudio.Trail.Phone32.Encode(trail, outFile); break;
                case 2: PopStudio.Trail.Phone64.Encode(trail, outFile); break;
                case 3: PopStudio.Trail.WP.Encode(trail, outFile); break;
                case 4: PopStudio.Trail.GameConsole.Encode(trail, outFile); break;
                case 5: PopStudio.Trail.TV.Encode(trail, outFile); break;
                case 6: PopStudio.Trail.TrailJson.Encode(trail, outFile); break;
                case 7: PopStudio.Trail.RawXml.Encode(trail, outFile); break;
                default: throw new NotImplementedException();
            }
        }

        public static void Particles(string inFile, string outFile, int informat, int outformat)
        {
            Particles.Particles particles = informat switch
            {
                0 => PopStudio.Particles.PC.Decode(inFile),
                1 => PopStudio.Particles.Phone32.Decode(inFile),
                2 => PopStudio.Particles.Phone64.Decode(inFile),
                3 => PopStudio.Particles.WP.Decode(inFile),
                4 => PopStudio.Particles.GameConsole.Decode(inFile),
                5 => PopStudio.Particles.TV.Decode(inFile),
                6 => PopStudio.Particles.ParticlesJson.Decode(inFile),
                7 => PopStudio.Particles.RawXml.Decode(inFile),
                _ => throw new NotImplementedException()
            };
            switch (outformat)
            {
                case 0: PopStudio.Particles.PC.Encode(particles, outFile); break;
                case 1: PopStudio.Particles.Phone32.Encode(particles, outFile); break;
                case 2: PopStudio.Particles.Phone64.Encode(particles, outFile); break;
                case 3: PopStudio.Particles.WP.Encode(particles, outFile); break;
                case 4: PopStudio.Particles.GameConsole.Encode(particles, outFile); break;
                case 5: PopStudio.Particles.TV.Encode(particles, outFile); break;
                case 6: PopStudio.Particles.ParticlesJson.Encode(particles, outFile); break;
                case 7: PopStudio.Particles.RawXml.Encode(particles, outFile); break;
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

        public static bool CutImage(string inFile, string outFolder, string infoFile, string itemName, int format) => format switch
        {
            0 => Atlas.NewXml.Cut(inFile, outFolder, infoFile, itemName),
            1 => Atlas.OldXml.Cut(inFile, outFolder, infoFile, itemName),
            2 => Atlas.AncientXml.Cut(inFile, outFolder, infoFile, itemName),
            3 => Atlas.Plist.Cut(inFile, outFolder, infoFile, itemName),
            4 => Atlas.AtlasImageDat.Cut(inFile, outFolder, infoFile, itemName),
            5 => Atlas.TVAtlasXml.Cut(inFile, outFolder, infoFile, itemName),
            6 => Atlas.ResRTON.Cut(inFile, outFolder, infoFile, itemName),
            _ => throw new Exception(Str.Obj.TypeMisMatch)
        };

        public static bool SpliceImage(string inFile, string outFolder, string infoFile, string itemName, int format, int maxWidth = 2048, int maxHeight = 2048) => format switch
        {
            0 => Atlas.NewXml.Splice(inFile, outFolder, infoFile, itemName, maxWidth, maxHeight),
            1 => Atlas.OldXml.Splice(inFile, outFolder, infoFile, itemName, maxWidth, maxHeight),
            2 => Atlas.AncientXml.Splice(inFile, outFolder, infoFile, itemName, maxWidth, maxHeight),
            3 => Atlas.Plist.Splice(inFile, outFolder, infoFile, itemName, maxWidth, maxHeight),
            4 => Atlas.AtlasImageDat.Splice(inFile, outFolder, infoFile, itemName, maxWidth, maxHeight),
            5 => Atlas.TVAtlasXml.Splice(inFile, outFolder, infoFile, itemName, maxWidth, maxHeight),
            6 => Atlas.ResRTON.Splice(inFile, outFolder, infoFile, itemName, maxWidth, maxHeight),
            _ => throw new Exception(Str.Obj.TypeMisMatch)
        };

        public static void NewDir(string filePath, bool toEnd = true) => Dir.NewDir(filePath, toEnd);

        public static string[] GetFiles(string filePath) => Dir.GetFiles(Dir.FormatPath(filePath));

        public static string GetFileExtension(string fileName) => Path.GetExtension(fileName);

        public static string GetFileName(string fileName) => Path.GetFileName(Dir.FormatPath(fileName));

        public static string GetFilePath(string fileName) => Path.GetDirectoryName(Dir.FormatPath(fileName));

        public static string GetFileNameWithoutExtension(string fileName) => Path.GetFileNameWithoutExtension(Dir.FormatPath(fileName));

        public static int GetVersion() => Const.RAINYVERSION;

        public static int GetSystem() => Const.SYSTEM;

        public static int GetLanguage() => (int)Setting.AppLanguage;

        public static string FormatPath(string path) => Dir.FormatPath(path);

        public static void DoFile(string filepath, params object[] args)
        {
            string s;
            (LuaScript.Script.luavm["rainy"] as NLua.LuaTable)["cache"] = args;
            using (StreamReader sr = new StreamReader(filepath.ToString()))
            {
                s = "local args = rainy.array2table(rainy.cache); rainy.cache = nil; " + sr.ReadToEnd();
            }
            LuaScript.Script.luavm.DoString(s);
        }

        public static object[] CreateArray(int length) => new object[length];

        public static bool DeleteFile(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
                return true;
            }
            else if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        public static int FileExists(string filePath) => (File.Exists(filePath) ? 0b1 : 0b0) | (Directory.Exists(filePath) ? 0b10 : 0b00);

        public static void Sleep(int ms) => Thread.Sleep(ms);

        public static string HttpGet(string url)
        {
            try
            {
                using (var a = new HttpClient()) return a.GetStringAsync(url).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}