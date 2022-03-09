using System.Text;
using NLua;

namespace PopStudio.Plugin
{
    internal class Script : Lua
    {
        public Script()
        {
            State.Encoding = Encoding.UTF8; //指定lua状态机字符串编码格式为utf8，以支持中文
            DoString("rainy = {} rainy.array2table = function (ary) local ans,l = {},ary.Length - 1; for i=0,l do ans[i + 1] = ary[i]; end return ans; end rainy.table2array = function(a) local l = #a; local ans = rainy.createarray(l); for i = 1,l do ans[i - 1] = a[i]; end return ans; end"); //rainy扩展
            RegisterFunction("print", null, typeof(API).GetMethod("Print"));
            RegisterFunction("rainy.alert", null, typeof(API).GetMethod("Alert"));
            RegisterFunction("rainy.prompt", null, typeof(API).GetMethod("Prompt"));
            RegisterFunction("rainy.sheet", null, typeof(API).GetMethod("Sheet"));
            RegisterFunction("rainy.getfilestream", null, typeof(API).GetMethod("GetFileStream"));
            RegisterFunction("rainy.getmemorystream", null, typeof(API).GetMethod("GetMemoryStream"));
            RegisterFunction("rainy.unpack", null, typeof(API).GetMethod("Unpack"));
            RegisterFunction("rainy.pack", null, typeof(API).GetMethod("Pack"));
            RegisterFunction("rainy.decodeimage", null, typeof(API).GetMethod("DecodeImage"));
            RegisterFunction("rainy.encodeimage", null, typeof(API).GetMethod("EncodeImage"));
            RegisterFunction("rainy.reanim", null, typeof(API).GetMethod("Reanim"));
            RegisterFunction("rainy.trail", null, typeof(API).GetMethod("Trail"));
            RegisterFunction("rainy.particles", null, typeof(API).GetMethod("Particles"));
            RegisterFunction("rainy.decoderton", null, typeof(API).GetMethod("DecodeRTON"));
            RegisterFunction("rainy.encoderton", null, typeof(API).GetMethod("EncodeRTON"));
            RegisterFunction("rainy.decompress", null, typeof(API).GetMethod("Decompress"));
            RegisterFunction("rainy.compress", null, typeof(API).GetMethod("Compress"));
            RegisterFunction("rainy.newdir", null, typeof(API).GetMethod("NewDir"));
            RegisterFunction("rainy.getfiles", null, typeof(API).GetMethod("GetFiles"));
            RegisterFunction("rainy.getfileextension", null, typeof(API).GetMethod("GetFileExtension"));
            RegisterFunction("rainy.getfilename", null, typeof(API).GetMethod("GetFileName"));
            RegisterFunction("rainy.getfilepath", null, typeof(API).GetMethod("GetFilePath"));
            RegisterFunction("rainy.getfilenamewithoutextension", null, typeof(API).GetMethod("GetFileNameWithoutExtension"));
            RegisterFunction("rainy.getversion", null, typeof(API).GetMethod("GetVersion"));
            RegisterFunction("rainy.getsystem", null, typeof(API).GetMethod("GetSystem"));
            RegisterFunction("rainy.formatpath", null, typeof(API).GetMethod("FormatPath"));
            RegisterFunction("rainy.dofile", null, typeof(API).GetMethod("DoFile"));
            RegisterFunction("rainy.createarray", null, typeof(API).GetMethod("CreateArray"));
            RegisterFunction("rainy.choosefolder", null, typeof(API).GetMethod("ChooseFolder"));
            RegisterFunction("rainy.chooseopenfile", null, typeof(API).GetMethod("ChooseOpenFile"));
            RegisterFunction("rainy.choosesavefile", null, typeof(API).GetMethod("ChooseSaveFile"));
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