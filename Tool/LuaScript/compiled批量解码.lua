print("compiled批量解码.lua 作者：萌新迎风听雨");
print("脚本说明：一个将指定文件夹内所有compiled后缀格式文件转为json文件的批处理脚本");
if (rainy.getversion() < 10) then error("rainy类库版本号最低为V10") end
local filepath = args[1] or rainy.choosefolder() or error("请输入文件夹位置");
local mode = args[2];
if mode == nil then
    local m = rainy.sheet("请选择被转换compiled格式","PC","Phone32","Phone64","Android TV","GameConsole");
    local tab = {PC = 0, Phone32 = 1, Phone64 = 2, ["Android TV"] = 5, GameConsole = 4};
    mode = tab[m];
end
local files = rainy.getfiles(filepath);
local newpath = rainy.formatpath(filepath .. "\\finished\\");
rainy.newdir(newpath);
for i=1,#files do
    local a = string.lower(rainy.getfileextension(files[i]));
    local b = rainy.getfilenamewithoutextension(files[i]);
    local c = string.lower(rainy.getfileextension(b));
    if (a == ".compiled") then
        if (c == ".xml") then
			rainy.particles(files[i],newpath .. b,mode,6);
		elseif (c == ".reanim") then
			rainy.reanim(files[i],newpath .. b,mode,6);
		elseif (c == ".trail") then
			rainy.trail(files[i],newpath .. b,mode,6);
		end
    end
end