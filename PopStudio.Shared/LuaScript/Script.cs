using NLua;

namespace PopStudio.LuaScript
{
    internal class Script : Lua
    {
        public bool ErrorThrow = true;

        public Script()
        {
            State.Encoding = System.Text.Encoding.UTF8;
            DoString("rainy = {} rainy.array2table = function (ary) local ans,l = {},ary.Length - 1; for i=0,l do ans[i + 1] = ary[i]; end return ans; end rainy.table2array = function(a) local l = #a; local ans = rainy.createarray(l); for i = 1,l do ans[i - 1] = a[i]; end return ans; end"); //rainy扩展
            RegisterFunction("print", null, typeof(Function).GetMethod("Print"));
            RegisterFunction("rainy.closeexception", null, typeof(Function).GetMethod("CloseException"));
            RegisterFunction("rainy.openexception", null, typeof(Function).GetMethod("OpenException"));
            RegisterFunction("rainy.alert", null, typeof(Function).GetMethod("Alert"));
            RegisterFunction("rainy.prompt", null, typeof(Function).GetMethod("Prompt"));
            RegisterFunction("rainy.sheet", null, typeof(Function).GetMethod("Sheet"));
            RegisterFunction("rainy.getfilestream", null, typeof(Function).GetMethod("GetFileStream"));
            RegisterFunction("rainy.getmemorystream", null, typeof(Function).GetMethod("GetMemoryStream"));
            RegisterFunction("rainy.gettempfilepool", null, typeof(Function).GetMethod("GetTempFilePool"));
            RegisterFunction("rainy.unpack", null, typeof(Function).GetMethod("Unpack"));
            RegisterFunction("rainy.pack", null, typeof(Function).GetMethod("Pack"));
            RegisterFunction("rainy.cutimage", null, typeof(Function).GetMethod("CutImage"));
            RegisterFunction("rainy.spliceimage", null, typeof(Function).GetMethod("SpliceImage"));
            RegisterFunction("rainy.decodeimage", null, typeof(Function).GetMethod("DecodeImage"));
            RegisterFunction("rainy.encodeimage", null, typeof(Function).GetMethod("EncodeImage"));
            RegisterFunction("rainy.reanim", null, typeof(Function).GetMethod("Reanim"));
            RegisterFunction("rainy.trail", null, typeof(Function).GetMethod("Trail"));
            RegisterFunction("rainy.particles", null, typeof(Function).GetMethod("Particles"));
            RegisterFunction("rainy.decoderton", null, typeof(Function).GetMethod("DecodeRTON"));
            RegisterFunction("rainy.encoderton", null, typeof(Function).GetMethod("EncodeRTON"));
            RegisterFunction("rainy.decompress", null, typeof(Function).GetMethod("Decompress"));
            RegisterFunction("rainy.compress", null, typeof(Function).GetMethod("Compress"));
            RegisterFunction("rainy.newdir", null, typeof(Function).GetMethod("NewDir"));
            RegisterFunction("rainy.getfiles", null, typeof(Function).GetMethod("GetFiles"));
            RegisterFunction("rainy.getfileextension", null, typeof(Function).GetMethod("GetFileExtension"));
            RegisterFunction("rainy.getfilename", null, typeof(Function).GetMethod("GetFileName"));
            RegisterFunction("rainy.getfilepath", null, typeof(Function).GetMethod("GetFilePath"));
            RegisterFunction("rainy.getfilenamewithoutextension", null, typeof(Function).GetMethod("GetFileNameWithoutExtension"));
            RegisterFunction("rainy.getversion", null, typeof(Function).GetMethod("GetVersion"));
            RegisterFunction("rainy.getsystem", null, typeof(Function).GetMethod("GetSystem"));
            RegisterFunction("rainy.getlanguage", null, typeof(Function).GetMethod("GetLanguage"));
            RegisterFunction("rainy.formatpath", null, typeof(Function).GetMethod("FormatPath"));
            RegisterFunction("rainy.dofile", null, typeof(Function).GetMethod("DoFile"));
            RegisterFunction("rainy.createarray", null, typeof(Function).GetMethod("CreateArray"));
            RegisterFunction("rainy.choosefolder", null, typeof(Function).GetMethod("ChooseFolder"));
            RegisterFunction("rainy.chooseopenfile", null, typeof(Function).GetMethod("ChooseOpenFile"));
            RegisterFunction("rainy.choosesavefile", null, typeof(Function).GetMethod("ChooseSaveFile"));
            RegisterFunction("rainy.deletefile", null, typeof(Function).GetMethod("DeleteFile"));
            RegisterFunction("rainy.fileexists", null, typeof(Function).GetMethod("FileExists"));
            DoString("local temp = rainy.getfiles rainy.getfiles = function (a) return rainy.array2table(temp(a)) end");
        }

        public override void Dispose()
        {
            base.Dispose();
            API.DisposeAll();
        }

        //静态部分

        public static Script luavm;
        
        public static void Load()
        {
            if (luavm != null)
            {
                luavm.Dispose();
            }
            luavm = new Script();
        }

        public static void Unload()
        {
            if (luavm != null)
            {
                luavm.Dispose();
                luavm = null;
            }
        }
        public static void Do(string str)
        {
            Load();
            try
            {
                luavm?.DoString(str);
            }
            finally
            {
                Unload();
            }
        }
    }
}