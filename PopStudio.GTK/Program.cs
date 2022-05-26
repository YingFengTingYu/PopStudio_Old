using PopStudio.Platform;

namespace PopStudio.GTK
{
    class Program
    {
        static bool JumpToScript = false;
        static string ScriptFileName = null;

        static bool init = false;
        static string buffer;

        static void Init()
        {
            if (!init)
            {
                init = true;
                Gtk.Application.Init();
            }
        }

        public static bool ShowScript(out string script)
        {
            script = ScriptFileName;
            return JumpToScript;
        }

        static void JumpToScriptAndRun(string fileName)
        {
            JumpToScript = true;
            ScriptFileName = fileName;
            RunAppGUI();
        }

        static void ShowMessage(string message)
        {
            Init();
            Plugin.Dialogs.DisplayAlert("PopStudio", message, "Cancel");
        }

        static int GetInteger(string description = null)
        {
            Init();
            description ??= Language.Languages.MAUIStr.Obj.Command_EnterInteger;
            string str = buffer ?? Plugin.Dialogs.DisplayPromptAsync("PopStudio", description);
            int num;
            try
            {
                num = int.Parse(str);
            }
            catch (Exception)
            {
                num = -1;
            }
            return num;
        }


        static void Main(params string[] args)
        {
            YFBitmap.RegistPlatform<Plugin.SkiaBitmap>();
            try
            {
                string settingxml = "setting.xml";
                if (File.Exists(settingxml))
                {
                    Setting.LoadFromXml(settingxml);
                }
            }
            catch (Exception)
            {

            }
            if (args?.Length > 0)
            {
                try
                {
                    //command mode
                    //Code
                    string fileName = args[0];
                    if (fileName.StartsWith('"')) fileName = fileName[1..];
                    if (fileName.EndsWith('"')) fileName = fileName[..^1];
                    Dir.FormatAndDeleteEndPathSeparator(ref fileName);
                    CommandCode code;
                    if (args.Length == 1)
                    {
                        code = CommandCode.RunScript;
                    }
                    else
                    {
                        code = (CommandCode)int.Parse(args[1]);
                    }
                    if (args.Length > 2) buffer = args[2];
                    switch (code)
                    {
                        case CommandCode.Unpack_Dz:
                            API.Unpack(fileName, $"{fileName}_unpack", 0, true);
                            break;
                        case CommandCode.Unpack_Rsb:
                            API.Unpack(fileName, $"{fileName}_unpack", 1, true);
                            break;
                        case CommandCode.Unpack_Pak:
                            API.Unpack(fileName, $"{fileName}_unpack", 2, true);
                            break;
                        case CommandCode.Unpack_Arcv:
                            API.Unpack(fileName, $"{fileName}_unpack", 3, true);
                            break;
                        case CommandCode.Pack_Dz:
                            API.Pack(fileName, $"{fileName}_pack.dz", 0);
                            break;
                        case CommandCode.Pack_Rsb:
                            API.Pack(fileName, $"{fileName}_pack.rsb", 1);
                            break;
                        case CommandCode.Pack_Pak:
                            API.Pack(fileName, $"{fileName}_pack.pak", 2);
                            break;
                        case CommandCode.Pack_Arcv:
                            API.Pack(fileName, $"{fileName}_pack.bin", 3);
                            break;
                        case CommandCode.DecodeImage_PtxRsb:
                            API.DecodeImage(fileName, $"{fileName}.PNG", 0);
                            break;
                        case CommandCode.DecodeImage_Cdat:
                            API.DecodeImage(fileName, $"{fileName}.png", 1);
                            break;
                        case CommandCode.DecodeImage_TexiOS:
                            API.DecodeImage(fileName, $"{fileName}.png", 2);
                            break;
                        case CommandCode.DecodeImage_Txz:
                            API.DecodeImage(fileName, $"{fileName}.png", 3);
                            break;
                        case CommandCode.DecodeImage_TexTV:
                            API.DecodeImage(fileName, $"{fileName}.png", 4);
                            break;
                        case CommandCode.DecodeImage_PtxXbox360:
                            API.DecodeImage(fileName, $"{fileName}.png", 5);
                            break;
                        case CommandCode.DecodeImage_PtxPS3:
                            API.DecodeImage(fileName, $"{fileName}.png", 6);
                            break;
                        case CommandCode.DecodeImage_PtxPSV:
                            API.DecodeImage(fileName, $"{fileName}.png", 7);
                            break;
                        case CommandCode.DecodeImage_Xnb:
                            API.DecodeImage(fileName, $"{fileName}.png", 8);
                            break;
                        case CommandCode.EncodeImage_PtxRsb:
                            API.EncodeImage(fileName, $"{fileName}.PTX", 0, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_Cdat:
                            API.EncodeImage(fileName, $"{fileName}.cdat", 1, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_TexiOS:
                            API.EncodeImage(fileName, $"{fileName}.tex", 2, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_Txz:
                            API.EncodeImage(fileName, $"{fileName}.txz", 3, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_TexTV:
                            API.EncodeImage(fileName, $"{fileName}.tex", 4, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_PtxXbox360:
                            API.EncodeImage(fileName, $"{fileName}.ptx", 5, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_PtxPS3:
                            API.EncodeImage(fileName, $"{fileName}.ptx", 6, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_PtxPSV:
                            API.EncodeImage(fileName, $"{fileName}.ptx", 7, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_Xnb:
                            API.EncodeImage(fileName, $"{fileName}.xnb", 8, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.Reanim_ToPCCompiled:
                            API.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 0);
                            break;
                        case CommandCode.Reanim_ToPhone32Compiled:
                            API.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 1);
                            break;
                        case CommandCode.Reanim_ToPhone64Compiled:
                            API.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 2);
                            break;
                        case CommandCode.Reanim_ToWPXnb:
                            API.ParseReanim(fileName, $"{fileName}.xnb", 3);
                            break;
                        case CommandCode.Reanim_ToGameConsoleCompiled:
                            API.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 4);
                            break;
                        case CommandCode.Reanim_ToTVCompiled:
                            API.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 5);
                            break;
                        case CommandCode.Reanim_ToStudioJson:
                            API.ParseReanim(fileName, $"{fileName}.json", 6);
                            break;
                        case CommandCode.Reanim_ToRawXml:
                            API.ParseReanim(fileName, $"{fileName}.reanim", 7);
                            break;
                        case CommandCode.Reanim_ToFlashXfl:
                            API.ParseReanim(fileName, $"{fileName}_animate", 8);
                            break;
                        case CommandCode.Particles_ToPCCompiled:
                            API.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 0);
                            break;
                        case CommandCode.Particles_ToPhone32Compiled:
                            API.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 1);
                            break;
                        case CommandCode.Particles_ToPhone64Compiled:
                            API.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 2);
                            break;
                        case CommandCode.Particles_ToWPXnb:
                            API.ParseParticles(fileName, $"{fileName}.xnb", 3);
                            break;
                        case CommandCode.Particles_ToGameConsoleCompiled:
                            API.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 4);
                            break;
                        case CommandCode.Particles_ToTVCompiled:
                            API.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 5);
                            break;
                        case CommandCode.Particles_ToStudioJson:
                            API.ParseParticles(fileName, $"{fileName}.json", 6);
                            break;
                        case CommandCode.Particles_ToRawXml:
                            API.ParseParticles(fileName, $"{fileName}.xml", 7);
                            break;
                        case CommandCode.Trail_ToPCCompiled:
                            API.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 0);
                            break;
                        case CommandCode.Trail_ToPhone32Compiled:
                            API.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 1);
                            break;
                        case CommandCode.Trail_ToPhone64Compiled:
                            API.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 2);
                            break;
                        case CommandCode.Trail_ToWPXnb:
                            API.ParseTrail(fileName, $"{fileName}.xnb", 3);
                            break;
                        case CommandCode.Trail_ToGameConsoleCompiled:
                            API.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 4);
                            break;
                        case CommandCode.Trail_ToTVCompiled:
                            API.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 5);
                            break;
                        case CommandCode.Trail_ToStudioJson:
                            API.ParseTrail(fileName, $"{fileName}.json", 6);
                            break;
                        case CommandCode.Trail_ToRawXml:
                            API.ParseTrail(fileName, $"{fileName}.trail", 7);
                            break;
                        case CommandCode.DecodePam:
                            API.DecodePam(fileName, (fileName.EndsWith(".PAM") || fileName.EndsWith(".pam")) ? $"{fileName}.json" : $"{fileName}.pam.json");
                            break;
                        case CommandCode.EncodePam:
                            API.EncodePam(fileName, $"{fileName}.PAM");
                            break;
                        case CommandCode.DecodeRTON_Simple:
                            API.DecodeRTON(fileName, $"{fileName}.json", 0);
                            break;
                        case CommandCode.DecodeRTON_Encrypted:
                            API.DecodeRTON(fileName, $"{fileName}.json", 1);
                            break;
                        case CommandCode.EncodeRTON_Simple:
                            API.EncodeRTON(fileName, $"{fileName}.RTON", 0);
                            break;
                        case CommandCode.EncodeRTON_Encrypted:
                            API.EncodeRTON(fileName, $"{fileName}.RTON", 1);
                            break;
                        case CommandCode.Decompress_Zlib:
                            API.Decompress(fileName, $"{fileName}_decompress", 0);
                            break;
                        case CommandCode.Decompress_Gzip:
                            API.Decompress(fileName, $"{fileName}_decompress", 1);
                            break;
                        case CommandCode.Decompress_Deflate:
                            API.Decompress(fileName, $"{fileName}_decompress", 2);
                            break;
                        case CommandCode.Decompress_Brotli:
                            API.Decompress(fileName, $"{fileName}_decompress", 3);
                            break;
                        case CommandCode.Decompress_Lzma:
                            API.Decompress(fileName, $"{fileName}_decompress", 4);
                            break;
                        case CommandCode.Decompress_Lz4:
                            API.Decompress(fileName, $"{fileName}_decompress", 5);
                            break;
                        case CommandCode.Decompress_Bzip2:
                            API.Decompress(fileName, $"{fileName}_decompress", 6);
                            break;
                        case CommandCode.Compress_Zlib:
                            API.Compress(fileName, $"{fileName}.zlib", 0);
                            break;
                        case CommandCode.Compress_Gzip:
                            API.Compress(fileName, $"{fileName}.gz", 1);
                            break;
                        case CommandCode.Compress_Deflate:
                            API.Compress(fileName, $"{fileName}.deflate", 2);
                            break;
                        case CommandCode.Compress_Brotli:
                            API.Compress(fileName, $"{fileName}.brotli", 3);
                            break;
                        case CommandCode.Compress_Lzma:
                            API.Compress(fileName, $"{fileName}.lzma", 4);
                            break;
                        case CommandCode.Compress_Lz4:
                            API.Compress(fileName, $"{fileName}.lz4", 5);
                            break;
                        case CommandCode.Compress_Bzip2:
                            API.Compress(fileName, $"{fileName}.bz", 6);
                            break;
                        case CommandCode.RunScript:
                            JumpToScriptAndRun(fileName);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                }
            }
            else
            {
                RunAppGUI();
            }
        }

        static void RunAppGUI()
        {
            Init();
            using (MainPage win = new MainPage())
            {
                win.DeleteEvent += (s, e) =>
                {
                    Gtk.Application.Quit();
                };
                win.ShowAll();
                Gtk.Application.Run();
            }
        }

        enum CommandCode
        {
            Unpack_Dz = 0,
            Unpack_Rsb,
            Unpack_Pak,
            Unpack_Arcv,
            Pack_Dz = 20,
            Pack_Rsb,
            Pack_Pak,
            Pack_Arcv,
            DecodeImage_PtxRsb = 40,
            DecodeImage_Cdat,
            DecodeImage_TexiOS,
            DecodeImage_Txz,
            DecodeImage_TexTV,
            DecodeImage_PtxXbox360,
            DecodeImage_PtxPS3,
            DecodeImage_PtxPSV,
            DecodeImage_Xnb,
            EncodeImage_PtxRsb = 60,
            EncodeImage_Cdat,
            EncodeImage_TexiOS,
            EncodeImage_Txz,
            EncodeImage_TexTV,
            EncodeImage_PtxXbox360,
            EncodeImage_PtxPS3,
            EncodeImage_PtxPSV,
            EncodeImage_Xnb,
            Reanim_ToPCCompiled = 80,
            Reanim_ToPhone32Compiled,
            Reanim_ToPhone64Compiled,
            Reanim_ToWPXnb,
            Reanim_ToGameConsoleCompiled,
            Reanim_ToTVCompiled,
            Reanim_ToStudioJson,
            Reanim_ToRawXml,
            Reanim_ToFlashXfl,
            Particles_ToPCCompiled = 100,
            Particles_ToPhone32Compiled,
            Particles_ToPhone64Compiled,
            Particles_ToWPXnb,
            Particles_ToGameConsoleCompiled,
            Particles_ToTVCompiled,
            Particles_ToStudioJson,
            Particles_ToRawXml,
            Trail_ToPCCompiled = 120,
            Trail_ToPhone32Compiled,
            Trail_ToPhone64Compiled,
            Trail_ToWPXnb,
            Trail_ToGameConsoleCompiled,
            Trail_ToTVCompiled,
            Trail_ToStudioJson,
            Trail_ToRawXml,
            DecodePam = 140,
            EncodePam = 160,
            DecodeRTON_Simple = 180,
            DecodeRTON_Encrypted,
            EncodeRTON_Simple = 200,
            EncodeRTON_Encrypted,
            Decompress_Zlib = 220,
            Decompress_Gzip,
            Decompress_Deflate,
            Decompress_Brotli,
            Decompress_Lzma,
            Decompress_Lz4,
            Decompress_Bzip2,
            Compress_Zlib = 240,
            Compress_Gzip,
            Compress_Deflate,
            Compress_Brotli,
            Compress_Lzma,
            Compress_Lz4,
            Compress_Bzip2,
            RunScript = 260
        }
    }
}