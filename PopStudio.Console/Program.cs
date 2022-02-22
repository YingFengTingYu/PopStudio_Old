//If you want to use English mode, please change this setting. (ZHCN to ENUS)
//如果你想转为英文模式，请改变这个设置。（ZHCN改为ENUS）
Str.Obj = new ZHCN();
PopStudio.ConsoleProject.Constant.Str.Obj = new PopStudio.ConsoleProject.Constant.ZHCN();
string[] Lng = PopStudio.ConsoleProject.Constant.Str.Obj.All;
Console.WriteLine(Lng[0], Str.Obj.AppName, Str.Obj.AppVersion);
Console.WriteLine(Lng[1], Str.Obj.AppAuthor);
Console.WriteLine(Lng[2]);
while (true)
{
    try
    {
        Console.WriteLine(Lng[3]);
        Console.WriteLine(Lng[4]);
        Console.WriteLine(Lng[5]);
        Console.WriteLine(Lng[6]);
        Console.WriteLine(Lng[77]);
        Console.WriteLine(Lng[78]);
        Console.WriteLine(Lng[86]);
        Console.WriteLine(Lng[87]);
        Console.WriteLine(Lng[103]);
        Console.WriteLine(Lng[104]);
        int mode = Convert.ToInt32(Console.ReadLine());
        if (mode == 1)
        {
            Console.WriteLine(Lng[7]);
            Console.WriteLine(Lng[8]);
            Console.WriteLine(Lng[9]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                //dz unpack
                //dz解包
                Console.WriteLine(Lng[10]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[10]);
                    continue;
                }
                PopStudio.Package.Dz.Dz.Unpack(filepath, filepath + ".out");
                continue;
            }
            else if (mode == 2)
            {
                //rsb unpack
                //rsb解包
                Console.WriteLine(Lng[11]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[11]);
                    continue;
                }
                PopStudio.Package.Rsb.Rsb.Unpack(filepath, filepath + ".out");
                continue;
            }
            else if (mode == 3)
            {
                //pak unpack
                //pak解包
                Console.WriteLine(Lng[12]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[12]);
                    continue;
                }
                PopStudio.Package.Pak.Pak.Unpack(filepath, filepath + ".out");
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 2)
        {
            Console.WriteLine(Lng[13]);
            Console.WriteLine(Lng[14]);
            Console.WriteLine(Lng[15]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                //dz pack
                //dz打包
                Console.WriteLine(Lng[16]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[16]);
                    continue;
                }
                PopStudio.Package.Dz.Dz.Pack(filepath, filepath + ".out");
                continue;
            }
            else if (mode == 2)
            {
                //rsb pack
                //rsb打包
                Console.WriteLine(Lng[17]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[17]);
                    continue;
                }
                PopStudio.Package.Rsb.Rsb.Pack(filepath, filepath + ".out");
                continue;
            }
            else if (mode == 3)
            {
                //pak pack
                //pak打包
                Console.WriteLine(Lng[18]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[18]);
                    continue;
                }
                PopStudio.Package.Pak.Pak.Pack(filepath, filepath + ".out");
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 3)
        {
            Console.WriteLine(Lng[19]);
            Console.WriteLine(Lng[20]);
            Console.WriteLine(Lng[21]);
            Console.WriteLine(Lng[22]);
            Console.WriteLine(Lng[23]);
            Console.WriteLine(Lng[24]);
            Console.WriteLine(Lng[25]);
            Console.WriteLine(Lng[108]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                Console.WriteLine(Lng[26]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[26]);
                    continue;
                }
                PopStudio.Image.Ptx.Ptx.Decode(filepath, filepath + ".png");
                continue;
            }
            else if (mode == 2)
            {
                Console.WriteLine(Lng[27]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[27]);
                    continue;
                }
                PopStudio.Image.Cdat.Cdat.Decode(filepath, filepath + ".png");
                continue;
            }
            else if (mode == 3)
            {
                Console.WriteLine(Lng[28]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[28]);
                    continue;
                }
                PopStudio.Image.Tex.Tex.Decode(filepath, filepath + ".png");
                continue;
            }
            else if (mode == 4)
            {
                Console.WriteLine(Lng[29]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[29]);
                    continue;
                }
                PopStudio.Image.Txz.Txz.Decode(filepath, filepath + ".png");
                continue;
            }
            else if (mode == 5)
            {
                Console.WriteLine(Lng[28]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[28]);
                    continue;
                }
                PopStudio.Image.TexTV.Tex.Decode(filepath, filepath + ".png");
                continue;
            }
            else if (mode == 6)
            {
                Console.WriteLine(Lng[26]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[26]);
                    continue;
                }
                PopStudio.Image.PtxXbox360.Ptx.Decode(filepath, filepath + ".png");
                continue;
            }
            else if (mode == 7)
            {
                Console.WriteLine(Lng[26]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[26]);
                    continue;
                }
                PopStudio.Image.PtxPS3.Ptx.Decode(filepath, filepath + ".png");
                continue;
            }
            else if (mode == 8)
            {
                Console.WriteLine(Lng[26]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[26]);
                    continue;
                }
                PopStudio.Image.PtxPSV.Ptx.Decode(filepath, filepath + ".png");
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 4)
        {
            //30
            Console.WriteLine(Lng[30]);
            Console.WriteLine(Lng[31]);
            Console.WriteLine(Lng[32]);
            Console.WriteLine(Lng[33]);
            Console.WriteLine(Lng[34]);
            Console.WriteLine(Lng[35]);
            Console.WriteLine(Lng[36]);
            Console.WriteLine(Lng[109]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                Console.WriteLine(Lng[37]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[37]);
                    continue;
                }
                Console.WriteLine(Lng[38]);
                Console.WriteLine(Lng[39]);
                Console.WriteLine(Lng[40]);
                Console.WriteLine(Lng[41]);
                Console.WriteLine(Lng[42]);
                Console.WriteLine(Lng[43]);
                Console.WriteLine(Lng[44]);
                Console.WriteLine(Lng[45]);
                Console.WriteLine(Lng[46]);
                Console.WriteLine(Lng[47]);
                Console.WriteLine(Lng[48]);
                Console.WriteLine(Lng[49]);
                Console.WriteLine(Lng[50]);
                Console.WriteLine(Lng[51]);
                Console.WriteLine(Lng[52]);
                Console.WriteLine(Lng[53]);
                Console.WriteLine(Lng[54]);
                Console.WriteLine(Lng[55]);
                Console.WriteLine(Lng[56]);
                Console.WriteLine(Lng[57]);
                Console.WriteLine(Lng[58]);
                Console.WriteLine(Lng[59]);
                Console.WriteLine(Lng[60]);
                PopStudio.Image.Ptx.Ptx.Encode(filepath, filepath + ".PTX", Convert.ToInt32(Console.ReadLine()));
                continue;
            }
            else if (mode == 2)
            {
                Console.WriteLine(Lng[37]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[37]);
                    continue;
                }
                PopStudio.Image.Cdat.Cdat.Encode(filepath, filepath + ".cdat");
                continue;
            }
            else if (mode == 3)
            {
                Console.WriteLine(Lng[37]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[37]);
                    continue;
                }
                Console.WriteLine(Lng[38]);
                Console.WriteLine(Lng[62]);
                Console.WriteLine(Lng[63]);
                Console.WriteLine(Lng[64]);
                Console.WriteLine(Lng[65]);
                PopStudio.Image.Tex.Tex.Encode(filepath, filepath + ".tex", Convert.ToInt32(Console.ReadLine()));
                continue;
            }
            else if (mode == 4)
            {
                Console.WriteLine(Lng[37]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[37]);
                    continue;
                }
                Console.WriteLine(Lng[38]);
                Console.WriteLine(Lng[62]);
                Console.WriteLine(Lng[63]);
                Console.WriteLine(Lng[64]);
                Console.WriteLine(Lng[65]);
                PopStudio.Image.Txz.Txz.Encode(filepath, filepath + ".txz", Convert.ToInt32(Console.ReadLine()));
                continue;
            }
            else if (mode == 5)
            {
                Console.WriteLine(Lng[37]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[37]);
                    continue;
                }
                Console.WriteLine(Lng[38]);
                Console.WriteLine(Lng[66]);
                Console.WriteLine(Lng[67]);
                Console.WriteLine(Lng[68]);
                Console.WriteLine(Lng[69]);
                Console.WriteLine(Lng[70]);
                Console.WriteLine(Lng[71]);
                Console.WriteLine(Lng[72]);
                Console.WriteLine(Lng[73]);
                Console.WriteLine(Lng[74]);
                Console.WriteLine(Lng[75]);
                PopStudio.Image.TexTV.Tex.Encode(filepath, filepath + ".tex", Convert.ToInt32(Console.ReadLine()));
                continue;
            }
            else if (mode == 6)
            {
                Console.WriteLine(Lng[37]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[37]);
                    continue;
                }
                PopStudio.Image.PtxXbox360.Ptx.Encode(filepath, filepath + ".ptx");
                continue;
            }
            else if (mode == 7)
            {
                Console.WriteLine(Lng[37]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[37]);
                    continue;
                }
                PopStudio.Image.PtxPS3.Ptx.Encode(filepath, filepath + ".ptx");
                continue;
            }
            else if (mode == 8)
            {
                Console.WriteLine(Lng[37]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[37]);
                    continue;
                }
                PopStudio.Image.PtxPSV.Ptx.Encode(filepath, filepath + ".ptx");
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 5)
        {
            Console.WriteLine(Lng[79]);
            Console.WriteLine(Lng[80]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                Console.WriteLine(Lng[83]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[83]);
                    continue;
                }
                PopStudio.RTON.RTON.Decode(filepath, filepath + ".json");
                continue;
            }
            else if (mode == 2)
            {
                Console.WriteLine(Lng[83]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[83]);
                    continue;
                }
                string key = PopStudio.RTON.RTON.KEY;
                if (key == null)
                {
                    Console.WriteLine(Lng[85]);
                    key = Console.ReadLine();
                }
                PopStudio.RTON.RTON.DecodeAndDecrypt(filepath, filepath + ".json", key);
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 6)
        {
            Console.WriteLine(Lng[81]);
            Console.WriteLine(Lng[82]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                Console.WriteLine(Lng[84]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[84]);
                    continue;
                }
                PopStudio.RTON.RTON.Encode(filepath, filepath + ".RTON");
                continue;
            }
            else if (mode == 2)
            {
                Console.WriteLine(Lng[84]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[84]);
                    continue;
                }
                string key = PopStudio.RTON.RTON.KEY;
                if (key == null)
                {
                    Console.WriteLine(Lng[85]);
                    key = Console.ReadLine();
                }
                PopStudio.RTON.RTON.EncodeAndEncrypt(filepath, filepath + ".RTON", key);
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 7)
        {
            Console.WriteLine(Lng[88]);
            Console.WriteLine(Lng[89]);
            Console.WriteLine(Lng[90]);
            Console.WriteLine(Lng[91]);
            Console.WriteLine(Lng[92]);
            Console.WriteLine(Lng[93]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                Console.WriteLine(Lng[100]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[100]);
                    continue;
                }
                PopStudio.Reanim.PC.Decode(filepath, filepath + ".reanim");
                continue;
            }
            else if (mode == 2)
            {
                Console.WriteLine(Lng[100]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[100]);
                    continue;
                }
                PopStudio.Reanim.Phone32.Decode(filepath, filepath + ".reanim");
                continue;
            }
            else if (mode == 3)
            {
                Console.WriteLine(Lng[100]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[100]);
                    continue;
                }
                PopStudio.Reanim.Phone64.Decode(filepath, filepath + ".reanim");
                continue;
            }
            else if (mode == 4)
            {
                Console.WriteLine(Lng[101]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[101]);
                    continue;
                }
                PopStudio.Reanim.WP.Decode(filepath, filepath + ".reanim");
                continue;
            }
            else if (mode == 5)
            {
                Console.WriteLine(Lng[100]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[100]);
                    continue;
                }
                PopStudio.Reanim.GameConsole.Decode(filepath, filepath + ".reanim");
                continue;
            }
            else if (mode == 6)
            {
                Console.WriteLine(Lng[100]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[100]);
                    continue;
                }
                PopStudio.Reanim.TV.Decode(filepath, filepath + ".reanim");
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 8)
        {
            Console.WriteLine(Lng[94]);
            Console.WriteLine(Lng[95]);
            Console.WriteLine(Lng[96]);
            Console.WriteLine(Lng[97]);
            Console.WriteLine(Lng[98]);
            Console.WriteLine(Lng[99]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                Console.WriteLine(Lng[102]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[102]);
                    continue;
                }
                PopStudio.Reanim.PC.Encode(filepath, filepath + ".reanim.compiled");
                continue;
            }
            else if (mode == 2)
            {
                Console.WriteLine(Lng[102]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[102]);
                    continue;
                }
                PopStudio.Reanim.Phone32.Encode(filepath, filepath + ".reanim.compiled");
                continue;
            }
            else if (mode == 3)
            {
                Console.WriteLine(Lng[102]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[102]);
                    continue;
                }
                PopStudio.Reanim.Phone64.Encode(filepath, filepath + ".reanim.compiled");
                continue;
            }
            else if (mode == 4)
            {
                Console.WriteLine(Lng[102]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[102]);
                    continue;
                }
                PopStudio.Reanim.WP.Encode(filepath, filepath + ".xnb");
                continue;
            }
            else if (mode == 5)
            {
                Console.WriteLine(Lng[102]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[102]);
                    continue;
                }
                PopStudio.Reanim.GameConsole.Encode(filepath, filepath + ".reanim.compiled");
                continue;
            }
            else if (mode == 6)
            {
                Console.WriteLine(Lng[102]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[102]);
                    continue;
                }
                PopStudio.Reanim.TV.Encode(filepath, filepath + ".reanim.compiled");
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 9)
        {
            Console.WriteLine(Lng[88]);
            Console.WriteLine(Lng[89]);
            Console.WriteLine(Lng[90]);
            Console.WriteLine(Lng[91]);
            Console.WriteLine(Lng[92]);
            Console.WriteLine(Lng[93]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                Console.WriteLine(Lng[105]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[105]);
                    continue;
                }
                PopStudio.Trail.PC.Decode(filepath, filepath + ".trail");
                continue;
            }
            else if (mode == 2)
            {
                Console.WriteLine(Lng[105]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[105]);
                    continue;
                }
                PopStudio.Trail.Phone32.Decode(filepath, filepath + ".trail");
                continue;
            }
            else if (mode == 3)
            {
                Console.WriteLine(Lng[105]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[105]);
                    continue;
                }
                PopStudio.Trail.Phone64.Decode(filepath, filepath + ".trail");
                continue;
            }
            else if (mode == 4)
            {
                Console.WriteLine(Lng[106]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[106]);
                    continue;
                }
                PopStudio.Trail.WP.Decode(filepath, filepath + ".trail");
                continue;
            }
            else if (mode == 5)
            {
                Console.WriteLine(Lng[105]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[105]);
                    continue;
                }
                PopStudio.Trail.GameConsole.Decode(filepath, filepath + ".trail");
                continue;
            }
            else if (mode == 6)
            {
                Console.WriteLine(Lng[105]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[105]);
                    continue;
                }
                PopStudio.Trail.TV.Decode(filepath, filepath + ".trail");
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else if (mode == 10)
        {
            Console.WriteLine(Lng[94]);
            Console.WriteLine(Lng[95]);
            Console.WriteLine(Lng[96]);
            Console.WriteLine(Lng[97]);
            Console.WriteLine(Lng[98]);
            Console.WriteLine(Lng[99]);
            mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1)
            {
                Console.WriteLine(Lng[107]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[107]);
                    continue;
                }
                PopStudio.Trail.PC.Encode(filepath, filepath + ".trail.compiled");
                continue;
            }
            else if (mode == 2)
            {
                Console.WriteLine(Lng[107]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[107]);
                    continue;
                }
                PopStudio.Trail.Phone32.Encode(filepath, filepath + ".trail.compiled");
                continue;
            }
            else if (mode == 3)
            {
                Console.WriteLine(Lng[107]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[107]);
                    continue;
                }
                PopStudio.Trail.Phone64.Encode(filepath, filepath + ".trail.compiled");
                continue;
            }
            else if (mode == 4)
            {
                Console.WriteLine(Lng[107]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[107]);
                    continue;
                }
                PopStudio.Trail.WP.Encode(filepath, filepath + ".xnb");
                continue;
            }
            else if (mode == 5)
            {
                Console.WriteLine(Lng[107]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[107]);
                    continue;
                }
                PopStudio.Trail.GameConsole.Encode(filepath, filepath + ".trail.compiled");
                continue;
            }
            else if (mode == 6)
            {
                Console.WriteLine(Lng[107]);
                string filepath = Console.ReadLine();
                if (string.IsNullOrEmpty(filepath))
                {
                    Console.WriteLine(Lng[107]);
                    continue;
                }
                PopStudio.Trail.TV.Encode(filepath, filepath + ".trail.compiled");
                continue;
            }
            else
            {
                Console.WriteLine(Lng[76]);
                continue;
            }
        }
        else
        {
            Console.WriteLine(Lng[76]);
            continue;
        }
    }
#if !DEBUG
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }
#endif
    finally
    {

    }
}