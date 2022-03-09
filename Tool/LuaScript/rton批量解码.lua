print("rton批量解码.lua 作者：萌新迎风听雨");
print("脚本说明：一个将指定文件夹内的所有RTON后缀格式文件转为json文件的批处理脚本");
if (rainy.getversion() < 10) then error("rainy类库版本号最低为V10") end
local filepath = args[1] or rainy.choosefolder() or error("请输入文件夹位置");
local mode = args[2];
if mode == nil then
    local m = rainy.sheet("请选择被转换RTON格式","Simple","Encrypted");
    local tab = {Simple = 0, Encrypted = 1};
    mode = tab[m];
end
local files = rainy.getfiles(filepath);
for i=1,#files do
    local a = string.lower(rainy.getfileextension(files[i]));
    local b = rainy.getfilenamewithoutextension(files[i]);
    if (a == ".rton") then
        rainy.decoderton(files[i],rainy.formatpath(rainy.getfilepath(files[i]) .. "\\" .. b .. ".json"),mode);
    end
end