print("纹理图像批量解码.lua 作者：萌新迎风听雨");
print("脚本说明：一个将指定文件夹内的所有纹理图像转为png图像的批处理脚本");
if (rainy.getversion() < 10) then error("rainy类库版本号最低为V10") end
local filepath = args[1] or rainy.choosefolder() or error("请输入文件夹位置");
local mode = args[2];
local ex;
if mode == nil then
    local m = rainy.sheet("请选择被转换图像格式","PTX(rsb)","cdat(Android,iOS)","tex(iOS)","txz(Android,iOS)","tex(TV)","ptx(xbox360)","ptx(ps3)","ptx(psv)","xnb(WindowsPhone)");
    local tab = {["PTX(rsb)"] = 0, ["cdat(Android,iOS)"] = 1,["tex(iOS)"] = 2,["txz(Android,iOS)"] = 3,["tex(TV)"] = 4,["ptx(xbox360)"] = 5,["ptx(ps3)"] = 6,["ptx(psv)"] = 7,["xnb(WindowsPhone)"] = 8};
    mode = tab[m];
    local tab2 = {[0] = ".ptx",[1] = ".cdat",[2] = ".tex",[3] = ".txz",[4] = ".tex",[5] = ".ptx",[6] = ".ptx",[7] = ".ptx",[8] = ".xnb"};
    ex = tab2[mode];
end
local files = rainy.getfiles(filepath);
for i=1,#files do
    local a = string.lower(rainy.getfileextension(files[i]));
    local b = rainy.getfilenamewithoutextension(files[i]);
    if (a == ex) then
        rainy.decodeimage(files[i],rainy.formatpath(rainy.getfilepath(files[i]) .. "\\" .. b .. ".png"),mode);
    end
end