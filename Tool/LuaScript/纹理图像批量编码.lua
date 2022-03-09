print("纹理图像批量编码.lua 作者：萌新迎风听雨");
print("脚本说明：一个将指定文件夹内的所有png图像转为纹理图像的批处理脚本");
if (rainy.getversion() < 10) then error("rainy类库版本号最低为V10") end
local filepath = args[1] or rainy.choosefolder() or error("请输入文件夹位置");
local mode = args[2];
local ex;
local format = 0;
if mode == nil then
    local m = rainy.sheet("请选择生成图像格式","PTX(rsb)","cdat(Android,iOS)","tex(iOS)","txz(Android,iOS)","tex(TV)","ptx(xbox360)","ptx(ps3)","ptx(psv)");
    local tab = {["PTX(rsb)"] = 0, ["cdat(Android,iOS)"] = 1,["tex(iOS)"] = 2,["txz(Android,iOS)"] = 3,["tex(TV)"] = 4,["ptx(xbox360)"] = 5,["ptx(ps3)"] = 6,["ptx(psv)"] = 7};
    mode = tab[m];
    local tab2 = {[0] = ".PTX",[1] = ".cdat",[2] = ".tex",[3] = ".txz",[4] = ".tex",[5] = ".ptx",[6] = ".ptx",[7] = ".ptx"};
    ex = tab2[mode];
    if (mode == 0) then
        local cho = rainy.sheet("请选择生成图像格式","ARGB8888(0)", "ABGR8888(0)", "RGBA4444(1)", "RGB565(2)", "RGBA5551(3)", "RGBA4444_Block(21)", "RGB565_Block(22)", "RGBA5551_Block(23)", "XRGB8888_A8(149)", "ARGB8888(BE)(0)", "ARGB8888_Padding(BE)(0)", "DXT1_RGB(35)", "DXT3_RGBA(36)", "DXT5_RGBA(37)", "DXT5(5)", "DXT5(BE)(5)", "ETC1_RGB(32)", "ETC1_RGB_A8(147)", "ETC1_RGB_A_Palette(147)", "ETC1_RGB_A_Palette(150)", "PVRTC_4BPP_RGBA(30)", "PVRTC_4BPP_RGB_A8(148)");
        local tab3 = {["ARGB8888(0)"] = 0,["ABGR8888(0)"] = 1,["RGBA4444(1)"] = 2,["RGB565(2)"] = 3,["RGBA5551(3)"] = 4,["RGBA4444_Block(21)"] = 5,["RGB565_Block(22)"] = 6,["RGBA5551_Block(23)"] = 7,["XRGB8888_A8(149)"] = 8,["ARGB8888(BE)(0)"] = 9,["ARGB8888_Padding(BE)(0)"] = 10,["DXT1_RGB(35)"] = 11,["DXT3_RGBA(36)"] = 12,["DXT5_RGBA(37)"] = 13,["DXT5(5)"] = 14,["DXT5(BE)(5)"] = 15,["ETC1_RGB(32)"] = 16,["ETC1_RGB_A8(147)"] = 17,["ETC1_RGB_A_Palette(147)"] = 18,["ETC1_RGB_A_Palette(150)"] = 19,["PVRTC_4BPP_RGBA(30)"] = 20,["PVRTC_4BPP_RGB_A8(148)"] = 21};
        format = tab3[cho];
    end
    if (mode == 2 or mode == 3) then
        local cho = rainy.sheet("请选择生成图像格式","ABGR8888(1)", "RGBA4444(2)", "RGBA5551(3)", "RGB565(4)");
        local tab3 = {["ABGR8888(1)"] = 0,["RGBA4444(2)"] = 1,["RGBA5551(3)"] = 2,["RGB565(4)"] = 3};
        format = tab3[cho];
    end
    if (mode == 4) then
        local cho = rainy.sheet("请选择生成图像格式","LUT8(1)(Invalid)", "ARGB8888(2)", "ARGB4444(3)", "ARGB1555(4)", "RGB565(5)", "ABGR8888(6)", "RGBA4444(7)", "RGBA5551(8)", "XRGB8888(9)", "LA88(10)");
        local tab3 = {["LUT8(1)(Invalid)"] = 0,["ARGB8888(2)"] = 1,["ARGB4444(3)"] = 2,["ARGB1555(4)"] = 3,["RGB565(5)"] = 4,["ABGR8888(6)"] = 5,["RGBA4444(7)"] = 6,["RGBA5551(8)"] = 7,["XRGB8888(9)"] = 8,["LA88(10)"] = 9};
        format = tab3[cho];
    end
end
local files = rainy.getfiles(filepath);
for i=1,#files do
    local a = string.lower(rainy.getfileextension(files[i]));
    local b = rainy.getfilenamewithoutextension(files[i]);
    if (a == ".png") then
        rainy.encodeimage(files[i],rainy.formatpath(rainy.getfilepath(files[i]) .. "\\" .. b .. ex),mode,format);
    end
end