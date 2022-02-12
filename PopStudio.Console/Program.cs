// See https://aka.ms/new-console-template for more information
//If you want to use English mode, please change this setting. (ZHCN to ENUS)
//如果你想转为英文模式，请改变这个设置。（ZHCN改为ENUS）
Str.Obj = new ZHCN();
while (true)
{
    Console.WriteLine("{0} {1}", Str.Obj.AppName, Str.Obj.AppVersion);
    Console.WriteLine("Author:{0}", Str.Obj.AppAuthor);
    Console.WriteLine("(English) Hello! This is the program to convert files used in PopCap Games. Now it can unpack and pack dz, rsb (including small version in Android and iOS, big endian version in ps3 and xbox360 and Chinese version) and pak (including Windows, MacOS, PS3 and Xbox360 version).");
    Console.WriteLine("If you want to unpack \".dz\", please enter 1.");
    Console.WriteLine("If you want to unpack \".rsb\", please enter 2.");
    Console.WriteLine("If you want to unpack \".pak\", please enter 3.");
    Console.WriteLine("If you want to pack \".dz\", please enter 11.");
    Console.WriteLine("If you want to pack \".rsb\", please enter 12.");
    Console.WriteLine("If you want to pack \".pak\", please enter 13.");
    Console.WriteLine("(Chinese) 你好！这是一个用于转换宝开游戏中使用的格式的项目。现在他可以解包打包dz、rsb（包括小端序，大端序和中国版本）和pak（包括Windows，MacOS，PS3和Xbox360版本）。");
    Console.WriteLine("如果你想解包.dz，请输入1.");
    Console.WriteLine("如果你想解包.rsb，请输入2.");
    Console.WriteLine("如果你想解包.pak，请输入3.");
    Console.WriteLine("如果你想打包.dz，请输入11.");
    Console.WriteLine("如果你想打包.rsb，请输入12.");
    Console.WriteLine("如果你想打包.pak，请输入13.");
    int mode = Convert.ToInt32(Console.ReadLine());
    if (mode == 1)
    {
        //dz unpack
        //dz解包
        Console.WriteLine("(English) Please choose dz file");
        Console.WriteLine("(Chinese) 请选择dz文件");
        string? filepath = Console.ReadLine();
        if (string.IsNullOrEmpty(filepath))
        {
            Console.WriteLine("(English) Please choose dz file!!!");
            Console.WriteLine("(Chinese) 请选择dz文件！！！");
            continue;
        }
        PopStudio.Package.Dz.Dz.Unpack(filepath, filepath + ".out");
        continue;
    }
    else if (mode == 2)
    {
        //rsb unpack
        //rsb解包
        Console.WriteLine("(English) Please choose rsb file");
        Console.WriteLine("(Chinese) 请选择rsb文件");
        string? filepath = Console.ReadLine();
        if (string.IsNullOrEmpty(filepath))
        {
            Console.WriteLine("(English) Please choose rsb file!!!");
            Console.WriteLine("(Chinese) 请选择rsb文件！！！");
            continue;
        }
        PopStudio.Package.Rsb.Rsb.Unpack(filepath, filepath + ".out");
        continue;
    }
    else if (mode == 3)
    {
        //pak unpack
        //pak解包
        Console.WriteLine("(English) Please choose pak file");
        Console.WriteLine("(Chinese) 请选择pak文件");
        string? filepath = Console.ReadLine();
        if (string.IsNullOrEmpty(filepath))
        {
            Console.WriteLine("(English) Please choose pak file!!!");
            Console.WriteLine("(Chinese) 请选择pak文件！！！");
            continue;
        }
        PopStudio.Package.Pak.Pak.Unpack(filepath, filepath + ".out");
        continue;
    }
    else if (mode == 11)
    {
        //dz pack
        //dz打包
        Console.WriteLine("(English) Please choose dz source folder");
        Console.WriteLine("(Chinese) 请选择dz源文件夹");
        string? filepath = Console.ReadLine();
        if (string.IsNullOrEmpty(filepath))
        {
            Console.WriteLine("(English) Please choose dz source folder!!!");
            Console.WriteLine("(Chinese) 请选择dz源文件夹！！！");
            continue;
        }
        PopStudio.Package.Dz.Dz.Pack(filepath, filepath + ".out");
        continue;
    }
    else if (mode == 12)
    {
        //rsb pack
        //rsb打包
        Console.WriteLine("(English) Please choose rsb source folder");
        Console.WriteLine("(Chinese) 请选择rsb源文件夹");
        string? filepath = Console.ReadLine();
        if (string.IsNullOrEmpty(filepath))
        {
            Console.WriteLine("(English) Please choose rsb source folder!!!");
            Console.WriteLine("(Chinese) 请选择rsb源文件夹！！！");
            continue;
        }
        PopStudio.Package.Rsb.Rsb.Pack(filepath, filepath + ".out");
        continue;
    }
    else if (mode == 13)
    {
        //pak pack
        //pak打包
        Console.WriteLine("(English) Please choose pak source folder");
        Console.WriteLine("(Chinese) 请选择pak源文件夹");
        string? filepath = Console.ReadLine();
        if (string.IsNullOrEmpty(filepath))
        {
            Console.WriteLine("(English) Please choose pak source folder!!!");
            Console.WriteLine("(Chinese) 请选择pak源文件夹！！！");
            continue;
        }
        PopStudio.Package.Pak.Pak.Pack(filepath, filepath + ".out");
        continue;
    }
    else
    {
        Console.WriteLine("(English) Invalid mode!");
        Console.WriteLine("(Chinese) 无效模式！");
        continue;
    }
}