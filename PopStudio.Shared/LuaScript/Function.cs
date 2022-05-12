namespace PopStudio.LuaScript
{
    internal class Function
    {
        public static void CloseException(params object[] args)
        {
            try
            {
                API.CloseException();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void OpenException(params object[] args)
        {
            try
            {
                API.OpenException();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void ThrowException(params object[] args)
        {
            try
            {
                API.ThrowException(args.Length > 0 ? args[0]?.ToString() : null);
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void Print(params object[] args)
        {
            try
            {
                API.Print(args);
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static bool? Alert(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                return API.Alert(n[0]?.ToString() ?? "", n[1]?.ToString() ?? "PopStudio", (n[2] != null) && (n[2] is not bool boolean || (boolean == true)));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string Prompt(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                return API.Prompt(n[0]?.ToString() ?? "", n[1]?.ToString() ?? "PopStudio", n[2]?.ToString() ?? "");
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string Sheet(params object[] args)
        {
            try
            {
                if (args.Length <= 1)
                {
                    string title = (args.Length == 0) ? "PopStudio" : args[0]?.ToString() ?? "PopStudio";
                    return API.Sheet(title);
                }
                else
                {
                    string[] n = new string[args.Length - 1];
                    int min = n.Length;
                    for (int i = 0; i < min; i++)
                    {
                        n[i] = args[i + 1]?.ToString() ?? "";
                    }
                    return API.Sheet(args[0]?.ToString() ?? "PopStudio", n);
                }
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static BinaryStream GetFileStream(params object[] args)
        {
            try
            {
                object[] n = new object[2];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                return API.GetFileStream(n[0]?.ToString(), Convert.ToInt32(n[1] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static BinaryStream GetMemoryStream(params object[] args)
        {
            try
            {
                byte[] arg = (args.Length >= 1 && args[0] is byte[] b) ? b : null;
                return API.GetMemoryStream(arg);
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static BinaryStream GetHttpStream(params object[] args)
        {
            try
            {
                return API.GetHttpStream(args.Length >= 1 ? args[0].ToString() : null);
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static TempFilePool GetTempFilePool(params object[] args)
        {
            try
            {
                return API.GetTempFilePool();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static void Unpack(params object[] args)
        {
            try
            {
                object[] n = new object[5];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.Unpack(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"), (n[3] != null) && (n[3] is not bool boolean || (boolean == true)), (n[4] != null) && (n[4] is not bool boolean2 || (boolean2 == true)));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void Pack(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.Pack(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void CutImage(params object[] args)
        {
            try
            {
                object[] n = new object[5];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.CutImage(n[0]?.ToString(), n[1]?.ToString(), n[2]?.ToString(), n[3]?.ToString(), Convert.ToInt32(n[4] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void SpliceImage(params object[] args)
        {
            try
            {
                object[] n = new object[7];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.SpliceImage(n[0]?.ToString(), n[1]?.ToString(), n[2]?.ToString(), n[3]?.ToString(), Convert.ToInt32(n[4] ?? "-1"), Convert.ToInt32(n[5] ?? "2048"), Convert.ToInt32(n[6] ?? "2048"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void DecodePam(params object[] args)
        {
            try
            {
                object[] n = new object[2];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.DecodePam(n[0]?.ToString(), n[1]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void EncodePam(params object[] args)
        {
            try
            {
                object[] n = new object[2];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.EncodePam(n[0]?.ToString(), n[1]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void DecodeImage(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.DecodeImage(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void EncodeImage(params object[] args)
        {
            try
            {
                object[] n = new object[4];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.EncodeImage(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"), Convert.ToInt32(n[3] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void ParseReanim(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.ParseReanim(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void ParseTrail(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.ParseTrail(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void ParseParticles(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.ParseParticles(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void Reanim(params object[] args)
        {
            try
            {
                object[] n = new object[4];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.Reanim(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"), Convert.ToInt32(n[3] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void Particles(params object[] args)
        {
            try
            {
                object[] n = new object[4];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.Particles(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"), Convert.ToInt32(n[3] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void Trail(params object[] args)
        {
            try
            {
                object[] n = new object[4];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.Trail(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"), Convert.ToInt32(n[3] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void DecodeRTON(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.DecodeRTON(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void EncodeRTON(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.EncodeRTON(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void Decompress(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.Decompress(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void Compress(params object[] args)
        {
            try
            {
                object[] n = new object[3];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.Compress(n[0]?.ToString(), n[1]?.ToString(), Convert.ToInt32(n[2] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static void NewDir(params object[] args)
        {
            try
            {
                object[] n = new object[2];
                int min = args.Length > n.Length ? n.Length : args.Length;
                for (int i = 0; i < min; i++)
                {
                    n[i] = args[i];
                }
                API.NewDir(n[0]?.ToString(), (n[2] == null) || n[2] is not bool boolean || (boolean == true));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static string[] GetFiles(params object[] args)
        {
            try
            {
                return API.GetFiles(args.Length == 0 ? null : args[0]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string GetFileExtension(params object[] args)
        {
            try
            {
                return API.GetFileExtension(args.Length == 0 ? null : args[0]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string GetFileName(params object[] args)
        {
            try
            {
                return API.GetFileName(args.Length == 0 ? null : args[0]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string GetFilePath(params object[] args)
        {
            try
            {
                return API.GetFilePath(args.Length == 0 ? null : args[0]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string GetFileNameWithoutExtension(params object[] args)
        {
            try
            {
                return API.GetFileNameWithoutExtension(args.Length == 0 ? null : args[0]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static int GetVersion(params object[] args)
        {
            try
            {
                return API.GetVersion();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return -1;
            }
        }

        public static int GetSystem(params object[] args)
        {
            try
            {
                return API.GetSystem();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return -1;
            }
        }

        public static int GetLanguage(params object[] args)
        {
            try
            {
                return API.GetLanguage();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return -1;
            }
        }

        public static string FormatPath(params object[] args)
        {
            try
            {
                return API.FormatPath(args.Length == 0 ? null : args[0]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static void DoFile(params object[] args)
        {
            try
            {
                if (args.Length <= 1)
                {
                    API.DoFile((args.Length == 0) ? null : args[0]?.ToString());
                }
                else
                {
                    object[] n = new object[args.Length - 1];
                    Array.Copy(args, 1, n, 0, n.Length);
                    API.DoFile(args[0]?.ToString(), n);
                }
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static object[] CreateArray(params object[] args)
        {
            try
            {
                return API.CreateArray(args.Length == 0 ? -1 : Convert.ToInt32(args[0] ?? "-1"));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string ChooseOpenFile(params object[] args)
        {
            try
            {
                return API.ChooseOpenFile();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string ChooseSaveFile(params object[] args)
        {
            try
            {
                return API.ChooseSaveFile();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static string ChooseFolder(params object[] args)
        {
            try
            {
                return API.ChooseFolder();
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static bool? DeleteFile(params object[] args)
        {
            try
            {
                return API.DeleteFile(args.Length == 0 ? null : args[0]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static void OpenUrl(params object[] args)
        {
            try
            {
                API.OpenUrl(args.Length > 0 ? (args[0]?.ToString()) : null);
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static int? FileExists(params object[] args)
        {
            try
            {
                return API.FileExists(args.Length == 0 ? null : args[0]?.ToString());
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }

        public static void Sleep(params object[] args)
        {
            try
            {
                API.Sleep(args.Length == 0 ? 0 : Convert.ToInt32(args[0]));
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
            }
        }

        public static string HttpGet(params object[] args)
        {
            try
            {
                return API.HttpGet(args.Length > 0 ? (args[0]?.ToString()) : null);
            }
            catch (Exception ex)
            {
                if (Script.luavm.ErrorThrow) throw;
                else Print(ex.Message);
                return null;
            }
        }
    }
}
