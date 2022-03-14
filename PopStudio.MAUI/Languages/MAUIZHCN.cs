namespace PopStudio.MAUI.Languages
{
    internal class MAUIZHCN : ILocalization
    {
        //Unity
        public string Unity_Title => "Unity资源提取";
        public string Unity_Introduction => "通过Unity资源提取功能，获取PVZ3等使用Unity引擎制作的游戏的资源！";
        public string Unity_Choose1 => "请填写被提取的文件路径";
        public string Unity_Choose2 => "请填写提取生成文件存放路径";
        public string Unity_Format => "请选择被提取的文件格式";

        //LuaScript
        public string LuaScript_Title => "Lua脚本";
        public string LuaScript_Introduction => "通过Lua脚本功能，调用程序内置函数，实现批处理等功能！";
        public string LuaScript_TracePrint => "输出";

        //Shell
        public string Shell_OK => "确定\0";
        public string Shell_Cancel => "取消\0";
        public string Shell_ExitTitle => "确定要退出吗";
        public string Shell_ExitText => "确定要退出吗？";

        //Setting
        public string Setting_Title => "设置";
        public string Setting_Introduction => "通过设置功能，改变工具的默认行为！";
        public string Setting_ItemLanguage => "语言：{0}";
        public string Setting_SetLanguage => "更改语言";
        public string Setting_ItemDz => "Dz打包文件压缩方式";
        public string Setting_IntroDz => "Dz数据包打包时对每种格式的文件可以选择不同的压缩方式，目前支持Store，Lzma，Gzip和Bzip2压缩。若未指定某格式的压缩方式则采取默认压缩方式。";
        public string Setting_Add => "添加新项";
        public string Setting_Clear => "清空所有";
        public string Setting_ItemPak => "PakPS3打包文件压缩方式";
        public string Setting_IntroPak => "Pak数据包对于PS3平台打包时对每种格式的文件可以选择是否进行压缩，目前支持Store和Zlib压缩。若未指定某格式的压缩方式则采取默认压缩方式。";
        public string Setting_ItemRsb => "Rsb解包打包PTX0格式";
        public string Setting_IntroRsb => "Rsb解包打包时，若需同时进行解码编码PTX，需规定对PTX0解码编码的方式，两者都不勾选即为ARGB8888方式。";
        public string Setting_ItemPtx => "PTX0图像解码格式";
        public string Setting_IntroPtx => "PTX0解码时，需规定对PTX0解码的方式，不勾选即为ARGB8888(Padding)方式。";
        public string Setting_ItemCdat => "Cdat加密解密密钥";
        public string Setting_IntroCdat => "Cdat是PVZ Free的加密png格式，程序默认提供密钥，你也可以自行更改。";
        public string Setting_ItemRTON => "RTON加密解密密钥";
        public string Setting_IntroRTON => "部分RTON为加密格式，程序不提供密钥，需要你自行提供密钥来解密加密。";
        public string Setting_ItemCompiled => "compiled自动转换数字i标签";
        public string Setting_IntroCompiled => "PVZ在部分平台compiled文件中Image标签由标准字符串形式变为数字形式，你可以自行在so中找到对应关系并提供给程序，来将数字变为字符串。";
        public string Setting_ItemXfl => "Xfl生成画布大小";
        public string Setting_IntroXfl => "当使用reanim转码生成Flash_Xfl文档以供使用Adobe Animate查看时，可指定画布大小、生成图片名是使用i标签名还是name标签名、横纵坐标缩放倍数。";
        public string Setting_XflWidth => "画布宽度";
        public string Setting_XflHeight => "画布高度";
        public string Setting_XflLabelName => "使用标签名";
        public string Setting_XflScaleX => "横坐标缩放";
        public string Setting_XflScaleY => "纵坐标缩放";
        public string Setting_AD => "程序启动时加载广告";
        public string Setting_Load => "加载文件";
        public string Setting_Unload => "清空项目";
        public string Setting_Recover => "恢复默认设置";
        public string Setting_OK => "确定\0";
        public string Setting_Cancel => "取消\0";
        public string Setting_SureRecover => "你确定要恢复吗";
        public string Setting_SureRecoverText => "你确定要恢复默认设置吗？该操作将不可逆！";
        public string Setting_FinishRecover => "恢复完成";
        public string Setting_FinishRecoverText => "恢复默认设置完成，需要立即重启程序";
        public string Setting_ChooseItem => "请选择执行项";
        public string Setting_ChooseCompressMode => "请选择压缩方式";
        public string Setting_EnterExtension => "请填写文件后缀名";
        public string Setting_EnterExtensionText => "请填写文件后缀名，相同后缀文件采用相同压缩方式";
        public string Setting_CompressItem1 => "修改压缩方式";
        public string Setting_CompressItem2 => "删除此项目";
        public string Setting_ChooseLanguage => "请选择语言";
        public string Setting_FinishChooseLanguage => "语言设置完成";
        public string Setting_FinishChooseLanguageText => "语言设置完成，需要立即重启程序";

        //Texture
        public string Texture_Title => "图像转码";
        public string Texture_Introduction => "通过图像转码功能，获取和修改游戏特殊图像内容！";
        public string Texture_Mode1 => "解码模式";
        public string Texture_Mode2 => "编码模式";
        public string Texture_Choose1 => "请填写被解码的特殊图像路径";
        public string Texture_Choose2 => "请填写解码生成png图像存放路径";
        public string Texture_Choose3 => "请选择解码模式";
        public string Texture_Choose4 => "请填写被编码的png图像路径";
        public string Texture_Choose5 => "请填写编码生成特殊图像存放路径";
        public string Texture_Choose6 => "请选择编码模式";
        public string Texture_Choose7 => "请选择编码格式";

        //RTON
        public string RTON_Title => "RTON转码";
        public string RTON_Introduction => "通过RTON转码功能，实现RTON文件和json文件的互相转换操作，来修改游戏内容！如果你选择加密RTON模式，你需要在设置中自行填写密钥。";
        public string RTON_Mode1 => "解码模式";
        public string RTON_Mode2 => "编码模式";
        public string RTON_Choose1 => "请填写被解码的文件路径";
        public string RTON_Choose2 => "请填写解码生成文件存放路径";
        public string RTON_Choose3 => "请选择解码模式";
        public string RTON_Choose4 => "请填写被编码的文件路径";
        public string RTON_Choose5 => "请填写编码生成文件存放路径";
        public string RTON_Choose6 => "请选择编码模式";

        //Trail
        public string Trail_Title => "Trail转码";
        public string Trail_Introduction => "通过Trail转码功能获取和修改游戏拖尾特效内容！";
        public string Trail_Choose1 => "请填写被转换的文件路径";
        public string Trail_Choose2 => "请填写转换生成文件存放路径";
        public string Trail_InFormat => "请选择被转换的文件格式";
        public string Trail_OutFormat => "请选择转换生成文件格式";

        //Reanim
        public string Reanim_Title => "Reanim转码";
        public string Reanim_Introduction => "通过Reanim转码功能获取和修改游戏动作内容！";
        public string Reanim_Choose1 => "请填写被转换的文件路径";
        public string Reanim_Choose2 => "请填写转换生成文件存放路径";
        public string Reanim_InFormat => "请选择被转换的文件格式";
        public string Reanim_OutFormat => "请选择转换生成文件格式";

        //Particles
        public string Particles_Title => "Particles转码";
        public string Particles_Introduction => "通过Particles转码功能获取和修改游戏粒子特效内容！";
        public string Particles_Choose1 => "请填写被转换的文件路径";
        public string Particles_Choose2 => "请填写转换生成文件存放路径";
        public string Particles_InFormat => "请选择被转换的文件格式";
        public string Particles_OutFormat => "请选择转换生成文件格式";

        //Compress
        public string Compress_Title => "压缩解压";
        public string Compress_Introduction => "通过压缩解压功能，实现单个文件的压缩解压操作！";
        public string Compress_Mode1 => "解压模式";
        public string Compress_Mode2 => "压缩模式";
        public string Compress_Choose1 => "请填写被解压的文件路径";
        public string Compress_Choose2 => "请填写解压生成文件存放路径";
        public string Compress_Choose3 => "请选择解压模式";
        public string Compress_Choose4 => "请填写被压缩的文件路径";
        public string Compress_Choose5 => "请填写压缩生成文件存放路径";
        public string Compress_Choose6 => "请选择压缩模式";

        //Package
        public string Package_Title => "解包打包";
        public string Package_Introduction => "通过解包打包功能获取游戏素材、修改游戏图像等内容！";
        public string Package_Mode1 => "解包模式";
        public string Package_Mode2 => "打包模式";
        public string Package_Choose1 => "请填写被解包的文件路径";
        public string Package_Choose2 => "请填写解包生成文件夹路径";
        public string Package_Choose3 => "请选择解包模式";
        public string Package_Choose4 => "请填写被打包的文件夹路径";
        public string Package_Choose5 => "请填写打包生成文件路径";
        public string Package_Choose6 => "请选择打包模式";
        public string Package_ChangeImage => "将所有纹理图像转为png图像";
        public string Package_DeleteImage => "纹理转为png后删除原文件";

        //HomePage
        public string HomePage_Title => "主页";
        public string HomePage_Begin => "开始你的创造之旅吧！";
        public string HomePage_Function => "通过PopStudio修改宝开游戏的数据包、图像、动作、粒子特效等各种内容！";
        public string HomePage_Agreement => "若您使用本工具制作作品，请务必在醒目位置注明使用本工具，商用本工具必须得到作者许可，否则视为侵权！";
        public string HomePage_Version => "PopStudio 版本号{0}";
        public string HomePage_Author_String => "作者：";
        public string HomePage_Author => "萌新迎风听雨";
        public string HomePage_Thanks_String => "特别感谢：";
        public string HomePage_Thanks => "2508 和风唐舞 补补23456 63enjoy AS魇梦蚀 伊特 某个萌新 天天 Indestructible_Ch 僵学者 An-Haze";
        public string HomePage_QQGroup_String => "交流QQ群：";
        public string HomePage_QQGroup => "1017246977";
        public string HomePage_Course_String => "教学视频：";
        public string HomePage_Course => "https://space.bilibili.com/411256864";
        public string HomePage_AppNewNotice_String => "更新公告：";
        public string HomePage_AppNewNotice => "1.支持particles和trail的Raw_Xml格式；\n2.修复动作转xfl的kx和ky出错问题。";
        //Share
        public string Share_FileNotFound => "文件{0}不存在！";
        public string Share_FolderNotFound => "文件夹{0}不存在！";
        public string Share_Finish => "执行完成";
        public string Share_Wrong => "执行异常：{0}";
        public string Share_ChooseMode => "请选择运行模式";
        public string Share_Choose => "选择";
        public string Share_Run => "运行";
        public string Share_RunStatue => "执行状态：";
        public string Share_Waiting => "等待中";
        public string Share_Running => "执行中......";

        //Permission
        public string Permission_Title => "权限申请";
        public string Permission_Request1 => "在Android6及以上系统版本中，请授予程序存储权限，否则程序将无权读写文件！";
        public string Permission_Request2 => "在Android11及以上系统版本中，请授予程序所有文件访问权限，否则程序将只能读写程序内部文件夹中的文件！";
        public string Permission_GoTo => "前往授权";
        public string Permission_Cancel => "取消";

        //PickFile
        public string PickFile_AllFiles => "所有文件";
        public string PickFile_NewFolder => "新建文件夹\0";
        public string PickFile_Back => "↩️返回上级目录\0";
        public string PickFile_OK => "确定\0";
        public string PickFile_Cancel => "取消\0";
        public string PickFile_EnterFolderName => "请输入文件夹名";
        public string PickFile_CreateWrong => "创建失败";
        public string PickFile_NoPermission => "无权限";
        public string PickFile_NoPermissionToEnter => "进入文件夹失败，程序无访问权限！";
        public string PickFlie_SaveThere => "保存到此目录\0";
        public string PickFlie_SaveFile => "保存文件";
        public string PickFile_EnterFileName => "请输入文件名";
        public string PickFile_ChooseThisFolder => "选择当前文件夹\0";

        //AD
        public string AD_Cancel => "关闭";
        public string AD_Title => "广告";
    }
}
