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
            YFAPI.RegistPlatform<Platform.GTKAPI>();
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
                            YFAPI.Unpack(fileName, $"{fileName}_unpack", 0, true);
                            break;
                        case CommandCode.Unpack_Rsb:
                            YFAPI.Unpack(fileName, $"{fileName}_unpack", 1, true);
                            break;
                        case CommandCode.Unpack_Pak:
                            YFAPI.Unpack(fileName, $"{fileName}_unpack", 2, true);
                            break;
                        case CommandCode.Unpack_Arcv:
                            YFAPI.Unpack(fileName, $"{fileName}_unpack", 3, true);
                            break;
                        case CommandCode.Pack_Dz:
                            YFAPI.Pack(fileName, $"{fileName}_pack.dz", 0);
                            break;
                        case CommandCode.Pack_Rsb:
                            YFAPI.Pack(fileName, $"{fileName}_pack.rsb", 1);
                            break;
                        case CommandCode.Pack_Pak:
                            YFAPI.Pack(fileName, $"{fileName}_pack.pak", 2);
                            break;
                        case CommandCode.Pack_Arcv:
                            YFAPI.Pack(fileName, $"{fileName}_pack.bin", 3);
                            break;
                        case CommandCode.DecodeImage_PtxRsb:
                            YFAPI.DecodeImage(fileName, $"{fileName}.PNG", 0);
                            break;
                        case CommandCode.DecodeImage_Cdat:
                            YFAPI.DecodeImage(fileName, $"{fileName}.png", 1);
                            break;
                        case CommandCode.DecodeImage_TexiOS:
                            YFAPI.DecodeImage(fileName, $"{fileName}.png", 2);
                            break;
                        case CommandCode.DecodeImage_Txz:
                            YFAPI.DecodeImage(fileName, $"{fileName}.png", 3);
                            break;
                        case CommandCode.DecodeImage_TexTV:
                            YFAPI.DecodeImage(fileName, $"{fileName}.png", 4);
                            break;
                        case CommandCode.DecodeImage_PtxXbox360:
                            YFAPI.DecodeImage(fileName, $"{fileName}.png", 5);
                            break;
                        case CommandCode.DecodeImage_PtxPS3:
                            YFAPI.DecodeImage(fileName, $"{fileName}.png", 6);
                            break;
                        case CommandCode.DecodeImage_PtxPSV:
                            YFAPI.DecodeImage(fileName, $"{fileName}.png", 7);
                            break;
                        case CommandCode.DecodeImage_Xnb:
                            YFAPI.DecodeImage(fileName, $"{fileName}.png", 8);
                            break;
                        case CommandCode.EncodeImage_PtxRsb:
                            YFAPI.EncodeImage(fileName, $"{fileName}.PTX", 0, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_Cdat:
                            YFAPI.EncodeImage(fileName, $"{fileName}.cdat", 1, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_TexiOS:
                            YFAPI.EncodeImage(fileName, $"{fileName}.tex", 2, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_Txz:
                            YFAPI.EncodeImage(fileName, $"{fileName}.txz", 3, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_TexTV:
                            YFAPI.EncodeImage(fileName, $"{fileName}.tex", 4, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_PtxXbox360:
                            YFAPI.EncodeImage(fileName, $"{fileName}.ptx", 5, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_PtxPS3:
                            YFAPI.EncodeImage(fileName, $"{fileName}.ptx", 6, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_PtxPSV:
                            YFAPI.EncodeImage(fileName, $"{fileName}.ptx", 7, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.EncodeImage_Xnb:
                            YFAPI.EncodeImage(fileName, $"{fileName}.xnb", 8, GetInteger(Language.Languages.MAUIStr.Obj.Command_EnterFormat));
                            break;
                        case CommandCode.Reanim_ToPCCompiled:
                            YFAPI.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 0);
                            break;
                        case CommandCode.Reanim_ToPhone32Compiled:
                            YFAPI.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 1);
                            break;
                        case CommandCode.Reanim_ToPhone64Compiled:
                            YFAPI.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 2);
                            break;
                        case CommandCode.Reanim_ToWPXnb:
                            YFAPI.ParseReanim(fileName, $"{fileName}.xnb", 3);
                            break;
                        case CommandCode.Reanim_ToGameConsoleCompiled:
                            YFAPI.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 4);
                            break;
                        case CommandCode.Reanim_ToTVCompiled:
                            YFAPI.ParseReanim(fileName, fileName.EndsWith(".reanim") ? $"{fileName}.compiled" : $"{fileName}.reanim.compiled", 5);
                            break;
                        case CommandCode.Reanim_ToStudioJson:
                            YFAPI.ParseReanim(fileName, $"{fileName}.json", 6);
                            break;
                        case CommandCode.Reanim_ToRawXml:
                            YFAPI.ParseReanim(fileName, $"{fileName}.reanim", 7);
                            break;
                        case CommandCode.Reanim_ToFlashXfl:
                            YFAPI.ParseReanim(fileName, $"{fileName}_animate", 8);
                            break;
                        case CommandCode.Particles_ToPCCompiled:
                            YFAPI.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 0);
                            break;
                        case CommandCode.Particles_ToPhone32Compiled:
                            YFAPI.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 1);
                            break;
                        case CommandCode.Particles_ToPhone64Compiled:
                            YFAPI.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 2);
                            break;
                        case CommandCode.Particles_ToWPXnb:
                            YFAPI.ParseParticles(fileName, $"{fileName}.xnb", 3);
                            break;
                        case CommandCode.Particles_ToGameConsoleCompiled:
                            YFAPI.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 4);
                            break;
                        case CommandCode.Particles_ToTVCompiled:
                            YFAPI.ParseParticles(fileName, fileName.EndsWith(".xml") ? $"{fileName}.compiled" : $"{fileName}.xml.compiled", 5);
                            break;
                        case CommandCode.Particles_ToStudioJson:
                            YFAPI.ParseParticles(fileName, $"{fileName}.json", 6);
                            break;
                        case CommandCode.Particles_ToRawXml:
                            YFAPI.ParseParticles(fileName, $"{fileName}.xml", 7);
                            break;
                        case CommandCode.Trail_ToPCCompiled:
                            YFAPI.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 0);
                            break;
                        case CommandCode.Trail_ToPhone32Compiled:
                            YFAPI.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 1);
                            break;
                        case CommandCode.Trail_ToPhone64Compiled:
                            YFAPI.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 2);
                            break;
                        case CommandCode.Trail_ToWPXnb:
                            YFAPI.ParseTrail(fileName, $"{fileName}.xnb", 3);
                            break;
                        case CommandCode.Trail_ToGameConsoleCompiled:
                            YFAPI.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 4);
                            break;
                        case CommandCode.Trail_ToTVCompiled:
                            YFAPI.ParseTrail(fileName, fileName.EndsWith(".trail") ? $"{fileName}.compiled" : $"{fileName}.trail.compiled", 5);
                            break;
                        case CommandCode.Trail_ToStudioJson:
                            YFAPI.ParseTrail(fileName, $"{fileName}.json", 6);
                            break;
                        case CommandCode.Trail_ToRawXml:
                            YFAPI.ParseTrail(fileName, $"{fileName}.trail", 7);
                            break;
                        case CommandCode.DecodePam:
                            YFAPI.DecodePam(fileName, (fileName.EndsWith(".PAM") || fileName.EndsWith(".pam")) ? $"{fileName}.json" : $"{fileName}.pam.json");
                            break;
                        case CommandCode.EncodePam:
                            YFAPI.EncodePam(fileName, $"{fileName}.PAM");
                            break;
                        case CommandCode.DecodeRTON_Simple:
                            YFAPI.DecodeRTON(fileName, $"{fileName}.json", 0);
                            break;
                        case CommandCode.DecodeRTON_Encrypted:
                            YFAPI.DecodeRTON(fileName, $"{fileName}.json", 1);
                            break;
                        case CommandCode.EncodeRTON_Simple:
                            YFAPI.EncodeRTON(fileName, $"{fileName}.RTON", 0);
                            break;
                        case CommandCode.EncodeRTON_Encrypted:
                            YFAPI.EncodeRTON(fileName, $"{fileName}.RTON", 1);
                            break;
                        case CommandCode.Decompress_Zlib:
                            YFAPI.Decompress(fileName, $"{fileName}_decompress", 0);
                            break;
                        case CommandCode.Decompress_Gzip:
                            YFAPI.Decompress(fileName, $"{fileName}_decompress", 1);
                            break;
                        case CommandCode.Decompress_Deflate:
                            YFAPI.Decompress(fileName, $"{fileName}_decompress", 2);
                            break;
                        case CommandCode.Decompress_Brotli:
                            YFAPI.Decompress(fileName, $"{fileName}_decompress", 3);
                            break;
                        case CommandCode.Decompress_Lzma:
                            YFAPI.Decompress(fileName, $"{fileName}_decompress", 4);
                            break;
                        case CommandCode.Decompress_Lz4:
                            YFAPI.Decompress(fileName, $"{fileName}_decompress", 5);
                            break;
                        case CommandCode.Decompress_Bzip2:
                            YFAPI.Decompress(fileName, $"{fileName}_decompress", 6);
                            break;
                        case CommandCode.Compress_Zlib:
                            YFAPI.Compress(fileName, $"{fileName}.zlib", 0);
                            break;
                        case CommandCode.Compress_Gzip:
                            YFAPI.Compress(fileName, $"{fileName}.gz", 1);
                            break;
                        case CommandCode.Compress_Deflate:
                            YFAPI.Compress(fileName, $"{fileName}.deflate", 2);
                            break;
                        case CommandCode.Compress_Brotli:
                            YFAPI.Compress(fileName, $"{fileName}.brotli", 3);
                            break;
                        case CommandCode.Compress_Lzma:
                            YFAPI.Compress(fileName, $"{fileName}.lzma", 4);
                            break;
                        case CommandCode.Compress_Lz4:
                            YFAPI.Compress(fileName, $"{fileName}.lz4", 5);
                            break;
                        case CommandCode.Compress_Bzip2:
                            YFAPI.Compress(fileName, $"{fileName}.bz", 6);
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