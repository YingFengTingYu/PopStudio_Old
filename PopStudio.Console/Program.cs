// See https://aka.ms/new-console-template for more information
//If you want to use English mode, please change this setting. (ZHCN to ENUS)
//如果你想转为英文模式，请改变这个设置。（ZHCN改为ENUS）
Str.Obj = new ZHCN();
while (true)
{
    Console.WriteLine("{0} {1}", Str.Obj.AppName, Str.Obj.AppVersion);
    Console.WriteLine("Author:{0}", Str.Obj.AppAuthor);
    Console.WriteLine("Hello! This is the program to convert files used in PopCap Games. Now it can unpack and pack dz, rsb (including small version in Android and iOS, big endian version in ps3 and xbox360 and Chinese version) and pak (including Windows, MacOS, PS3 and Xbox360 version).");
    Console.WriteLine("enter 1 to run unpack mode");
    Console.WriteLine("enter 2 to run pack mode");
    Console.WriteLine("enter 3 to run image decode mode");
    Console.WriteLine("enter 4 to run image encode mode");
    Console.WriteLine("你好！这是一个用于转换宝开游戏中使用的格式的项目。现在他可以解包打包dz、rsb（包括小端序，大端序和中国版本）和pak（包括Windows，MacOS，PS3和Xbox360版本）。");
    Console.WriteLine("输入1以运行解包模式");
    Console.WriteLine("输入2以运行打包模式");
    Console.WriteLine("输入3以运行图像解码模式");
    Console.WriteLine("输入4以运行图像编码模式");
    int mode = Convert.ToInt32(Console.ReadLine());
    if (mode == 1)
    {
        Console.WriteLine("If you want to unpack \".dz\", please enter 1.");
        Console.WriteLine("If you want to unpack \".rsb\", please enter 2.");
        Console.WriteLine("If you want to unpack \".pak\", please enter 3.");
        Console.WriteLine("如果你想解包.dz，请输入1.");
        Console.WriteLine("如果你想解包.rsb，请输入2.");
        Console.WriteLine("如果你想解包.pak，请输入3.");
        mode = Convert.ToInt32(Console.ReadLine());
        if (mode == 1)
        {
            //dz unpack
            //dz解包
            Console.WriteLine("Please choose dz file");
            Console.WriteLine("请选择dz文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose dz file!!!");
                Console.WriteLine("请选择dz文件！！！");
                continue;
            }
            PopStudio.Package.Dz.Dz.Unpack(filepath, filepath + ".out");
            continue;
        }
        else if (mode == 2)
        {
            //rsb unpack
            //rsb解包
            Console.WriteLine("Please choose rsb file");
            Console.WriteLine("请选择rsb文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose rsb file!!!");
                Console.WriteLine("请选择rsb文件！！！");
                continue;
            }
            PopStudio.Package.Rsb.Rsb.Unpack(filepath, filepath + ".out");
            continue;
        }
        else if (mode == 3)
        {
            //pak unpack
            //pak解包
            Console.WriteLine("Please choose pak file");
            Console.WriteLine("请选择pak文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose pak file!!!");
                Console.WriteLine("请选择pak文件！！！");
                continue;
            }
            PopStudio.Package.Pak.Pak.Unpack(filepath, filepath + ".out");
            continue;
        }
    }
    else if (mode == 2)
    {
        Console.WriteLine("If you want to pack \".dz\", please enter 1.");
        Console.WriteLine("If you want to pack \".rsb\", please enter 2.");
        Console.WriteLine("If you want to pack \".pak\", please enter 3.");
        Console.WriteLine("如果你想打包.dz，请输入1.");
        Console.WriteLine("如果你想打包.rsb，请输入2.");
        Console.WriteLine("如果你想打包.pak，请输入3.");
        mode = Convert.ToInt32(Console.ReadLine());
        if (mode == 1)
        {
            //dz pack
            //dz打包
            Console.WriteLine("Please choose dz source folder");
            Console.WriteLine("请选择dz源文件夹");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose dz source folder!!!");
                Console.WriteLine("请选择dz源文件夹！！！");
                continue;
            }
            PopStudio.Package.Dz.Dz.Pack(filepath, filepath + ".out");
            continue;
        }
        else if (mode == 2)
        {
            //rsb pack
            //rsb打包
            Console.WriteLine("Please choose rsb source folder");
            Console.WriteLine("请选择rsb源文件夹");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose rsb source folder!!!");
                Console.WriteLine("请选择rsb源文件夹！！！");
                continue;
            }
            PopStudio.Package.Rsb.Rsb.Pack(filepath, filepath + ".out");
            continue;
        }
        else if (mode == 3)
        {
            //pak pack
            //pak打包
            Console.WriteLine("Please choose pak source folder");
            Console.WriteLine("请选择pak源文件夹");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose pak source folder!!!");
                Console.WriteLine("请选择pak源文件夹！！！");
                continue;
            }
            PopStudio.Package.Pak.Pak.Pack(filepath, filepath + ".out");
            continue;
        }
    }
    else if (mode == 3)
    {
        Console.WriteLine("If you want to decode \".ptx\"(in rsb), please enter 1.");
        Console.WriteLine("If you want to decode \".cdat\", please enter 2.");
        Console.WriteLine("If you want to decode \".tex\"(iOS version), please enter 3.");
        Console.WriteLine("If you want to decode \".txz\", please enter 4.");
        Console.WriteLine("If you want to decode \".tex\"(Android TV version), please enter 5.");
        Console.WriteLine("If you want to decode \".ptx\"(in PVZ Xbox360), please enter 6.");
        Console.WriteLine("如果你想解码.ptx（rsb中的），请输入1.");
        Console.WriteLine("如果你想解码.cdat，请输入2.");
        Console.WriteLine("如果你想解码.tex（iOS版），请输入3.");
        Console.WriteLine("如果你想解码.txz，请输入4.");
        Console.WriteLine("如果你想解码.tex（安卓TV版），请输入5.");
        Console.WriteLine("如果你想解码.ptx（植物大战僵尸Xbox360中的），请输入6.");
        mode = Convert.ToInt32(Console.ReadLine());
        if (mode == 1)
        {
            Console.WriteLine("Please choose ptx file");
            Console.WriteLine("请选择ptx文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose ptx file!!!");
                Console.WriteLine("请选择ptx文件！！！");
                continue;
            }
            PopStudio.Image.Ptx.Ptx.Decode(filepath, filepath + ".png");
            continue;
        }
        else if (mode == 2)
        {
            Console.WriteLine("Please choose cdat file");
            Console.WriteLine("请选择cdat文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose cdat file!!!");
                Console.WriteLine("请选择cdat文件！！！");
                continue;
            }
            PopStudio.Image.Cdat.Cdat.Decode(filepath, filepath + ".png");
            continue;
        }
        else if (mode == 3)
        {
            Console.WriteLine("Please choose tex file");
            Console.WriteLine("请选择tex文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose tex file!!!");
                Console.WriteLine("请选择tex文件！！！");
                continue;
            }
            PopStudio.Image.Tex.Tex.Decode(filepath, filepath + ".png");
            continue;
        }
        else if (mode == 4)
        {
            Console.WriteLine("Please choose txz file");
            Console.WriteLine("请选择txz文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose txz file!!!");
                Console.WriteLine("请选择txz文件！！！");
                continue;
            }
            PopStudio.Image.Txz.Txz.Decode(filepath, filepath + ".png");
            continue;
        }
        else if (mode == 5)
        {
            Console.WriteLine("Please choose tex file");
            Console.WriteLine("请选择tex文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose tex file!!!");
                Console.WriteLine("请选择tex文件！！！");
                continue;
            }
            PopStudio.Image.TexTV.Tex.Decode(filepath, filepath + ".png");
            continue;
        }
        else if (mode == 6)
        {
            Console.WriteLine("Please choose ptx file");
            Console.WriteLine("请选择ptx文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose ptx file!!!");
                Console.WriteLine("请选择ptx文件！！！");
                continue;
            }
            PopStudio.Image.PtxXbox360.Ptx.Decode(filepath, filepath + ".png");
            continue;
        }
    }
    else if (mode == 4)
    {
        Console.WriteLine("If you want to encode \".ptx\"(in rsb), please enter 1.");
        Console.WriteLine("If you want to encode \".cdat\", please enter 2.");
        Console.WriteLine("If you want to encode \".tex\"(iOS version), please enter 3.");
        Console.WriteLine("If you want to encode \".txz\", please enter 4.");
        Console.WriteLine("If you want to encode \".tex\"(Android TV version), please enter 5.");
        Console.WriteLine("If you want to encode \".ptx\"(in PVZ Xbox360), please enter 6.");
        Console.WriteLine("如果你想编码.ptx（rsb中的），请输入1.");
        Console.WriteLine("如果你想编码.cdat，请输入2.");
        Console.WriteLine("如果你想编码.tex（iOS版），请输入3.");
        Console.WriteLine("如果你想编码.txz，请输入4.");
        Console.WriteLine("如果你想编码.tex（安卓TV版），请输入5.");
        Console.WriteLine("如果你想编码.ptx（植物大战僵尸Xbox360中的），请输入6.");
        mode = Convert.ToInt32(Console.ReadLine());
        if (mode == 1)
        {
            Console.WriteLine("Please choose png file");
            Console.WriteLine("请选择png文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose png file!!!");
                Console.WriteLine("请选择png文件！！！");
                continue;
            }
            Console.WriteLine("Please select the mode");
            Console.WriteLine("enter 1 to use ARGB8888 texture(format 0)");
            Console.WriteLine("enter 2 to use ABGR8888 texture(format 0)");
            Console.WriteLine("enter 3 to use RGBA4444 texture(format 1)");
            Console.WriteLine("enter 4 to use RGB565 texture(format 2)");
            Console.WriteLine("enter 5 to use RGBA5551 texture(format 3)");
            Console.WriteLine("enter 6 to use RGBA4444_Block texture(format 21)");
            Console.WriteLine("enter 7 to use RGB565_Block texture(format 22)");
            Console.WriteLine("enter 8 to use RGBA5551_Block texture(format 23)");
            Console.WriteLine("enter 9 to use XRGB8888_A8 texture(format 149)");
            Console.WriteLine("enter 10 to use ARGB8888 big endian texture(format 0)");
            Console.WriteLine("enter 11 to use ARGB8888 big endian and padding texture(format 0)");
            Console.WriteLine("enter 12 to use DXT1_RGB texture(format 35)");
            Console.WriteLine("enter 13 to use DXT3_RGBA texture(format 36)");
            Console.WriteLine("enter 14 to use DXT5_RGBA texture(format 37)");
            Console.WriteLine("enter 15 to use DXT5 texture(format 5)");
            Console.WriteLine("enter 16 to use DXT5 big endian texture(format 5)");
            Console.WriteLine("enter 17 to use ETC1_RGB texture(format 32)");
            Console.WriteLine("enter 18 to use ETC1_RGB_A8 texture(format 147)");
            Console.WriteLine("enter 19 to use ETC1_RGB_A_Compress texture(format 147)");
            Console.WriteLine("enter 20 to use ETC1_RGB_A_Compress texture(format 150)");
            Console.WriteLine("请选择模式");
            Console.WriteLine("输入1使用ARGB8888纹理（格式0）");
            Console.WriteLine("输入2使用ABGR8888纹理（格式0）");
            Console.WriteLine("输入3使用RGBA4444纹理（格式1）");
            Console.WriteLine("输入4使用RGB565纹理（格式2）");
            Console.WriteLine("输入5使用RGBA5551纹理（格式3）");
            Console.WriteLine("输入6使用RGBA4444_Block纹理（格式21）");
            Console.WriteLine("输入7使用RGB565_Block纹理（格式22）");
            Console.WriteLine("输入8使用RGBA5551_Block纹理（格式23）");
            Console.WriteLine("输入9使用XRGB8888_A8纹理（格式149）");
            Console.WriteLine("输入10使用ARGB8888大端序纹理（格式0）");
            Console.WriteLine("输入11使用ARGB8888大端序补码纹理（格式0）");
            Console.WriteLine("输入12使用DXT1_RGB纹理（格式35）");
            Console.WriteLine("输入13使用DXT3_RGBA纹理（格式36）");
            Console.WriteLine("输入14使用DXT5_RGBA纹理（格式37）");
            Console.WriteLine("输入15使用DXT5纹理（格式5）");
            Console.WriteLine("输入16使用DXT5大端序纹理（格式5）");
            Console.WriteLine("输入17使用ETC1_RGB纹理（格式32）");
            Console.WriteLine("输入18使用ETC1_RGB_A8纹理（格式147）");
            Console.WriteLine("输入19使用ETC1_RGB_A_Compress纹理（格式147）");
            Console.WriteLine("输入20使用ETC1_RGB_A_Compress纹理（格式150）");
            PopStudio.Image.Ptx.Ptx.Encode(filepath, filepath + ".PTX", Convert.ToInt32(Console.ReadLine()));
            continue;
        }
        else if (mode == 2)
        {
            Console.WriteLine("Please choose png file");
            Console.WriteLine("请选择png文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose png file!!!");
                Console.WriteLine("请选择png文件！！！");
                continue;
            }
            PopStudio.Image.Cdat.Cdat.Encode(filepath, filepath + ".cdat");
            continue;
        }
        else if (mode == 3)
        {
            Console.WriteLine("Please choose png file");
            Console.WriteLine("请选择png文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose png file!!!");
                Console.WriteLine("请选择png文件！！！");
                continue;
            }
            Console.WriteLine("Please select the mode");
            Console.WriteLine("enter 1 to use ABGR8888 texture");
            Console.WriteLine("enter 2 to use RGBA4444 texture");
            Console.WriteLine("enter 3 to use RGBA5551 texture");
            Console.WriteLine("enter 4 to use RGB565 texture");
            Console.WriteLine("请选择模式");
            Console.WriteLine("输入1使用ABGR8888纹理");
            Console.WriteLine("输入2使用RGBA4444纹理");
            Console.WriteLine("输入3使用RGBA5551纹理");
            Console.WriteLine("输入4使用RGB565纹理");
            PopStudio.Image.Tex.Tex.Encode(filepath, filepath + ".tex", Convert.ToInt32(Console.ReadLine()));
            continue;
        }
        else if (mode == 4)
        {
            Console.WriteLine("Please choose png file");
            Console.WriteLine("请选择png文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose png file!!!");
                Console.WriteLine("请选择png文件！！！");
                continue;
            }
            Console.WriteLine("Please select the mode");
            Console.WriteLine("enter 1 to use ABGR8888 texture");
            Console.WriteLine("enter 2 to use RGBA4444 texture");
            Console.WriteLine("enter 3 to use RGBA5551 texture");
            Console.WriteLine("enter 4 to use RGB565 texture");
            Console.WriteLine("请选择模式");
            Console.WriteLine("输入1使用ABGR8888纹理");
            Console.WriteLine("输入2使用RGBA4444纹理");
            Console.WriteLine("输入3使用RGBA5551纹理");
            Console.WriteLine("输入4使用RGB565纹理");
            PopStudio.Image.Txz.Txz.Encode(filepath, filepath + ".txz", Convert.ToInt32(Console.ReadLine()));
            continue;
        }
        else if (mode == 5)
        {
            Console.WriteLine("Please choose png file");
            Console.WriteLine("请选择png文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose png file!!!");
                Console.WriteLine("请选择png文件！！！");
                continue;
            }
            Console.WriteLine("Please select the mode");
            Console.WriteLine("enter 1 to use ARGB8888 texture");
            Console.WriteLine("enter 2 to use ARGB4444 texture");
            Console.WriteLine("enter 3 to use ARGB1555 texture");
            Console.WriteLine("enter 4 to use RGB565 texture");
            Console.WriteLine("enter 5 to use ABGR8888 texture");
            Console.WriteLine("enter 6 to use RGBA4444 texture");
            Console.WriteLine("enter 7 to use RGBA5551 texture");
            Console.WriteLine("enter 8 to use XRGB8888 texture");
            Console.WriteLine("enter 9 to use LA88 texture");
            Console.WriteLine("请选择模式");
            Console.WriteLine("输入1使用ARGB8888纹理");
            Console.WriteLine("输入2使用ARGB4444纹理");
            Console.WriteLine("输入3使用ARGB1555纹理");
            Console.WriteLine("输入4使用RGB565纹理");
            Console.WriteLine("输入5使用ABGR8888纹理");
            Console.WriteLine("输入6使用RGBA4444纹理");
            Console.WriteLine("输入7使用RGBA5551纹理");
            Console.WriteLine("输入8使用XRGB8888纹理");
            Console.WriteLine("输入9使用LA88纹理");
            PopStudio.Image.TexTV.Tex.Encode(filepath, filepath + ".tex", Convert.ToInt32(Console.ReadLine()));
            continue;
        }
        else if (mode == 6)
        {
            Console.WriteLine("Please choose png file");
            Console.WriteLine("请选择png文件");
            string filepath = Console.ReadLine();
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please choose png file!!!");
                Console.WriteLine("请选择png文件！！！");
                continue;
            }
            PopStudio.Image.PtxXbox360.Ptx.Encode(filepath, filepath + ".ptx");
            continue;
        }
    }
    else
    {
        Console.WriteLine("Invalid mode!");
        Console.WriteLine("无效模式！");
        continue;
    }
}