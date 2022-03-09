print("compiled批量编码.lua 作者：萌新迎风听雨");
print("脚本说明：一个将指定文件夹内的json文件转为compiled后缀格式文件的批处理脚本");
if (rainy.getversion() < 10) then error("rainy类库版本号最低为V10") end
local filepath = args[1] or rainy.choosefolder() or error("请输入文件夹位置");
local mode = args[2];
if mode == nil then
    local m = rainy.sheet("请选择生成compiled格式","PC","Phone32","Phone64","Android TV","GameConsole");
    local tab = {PC = 0, Phone32 = 1, Phone64 = 2, ["Android TV"] = 5, GameConsole = 4};
    mode = tab[m];
end
local files = rainy.getfiles(filepath);
local newpathr = rainy.formatpath(filepath .. "\\finished\\reanim\\");
local newpathp = rainy.formatpath(filepath .. "\\finished\\particles\\");
rainy.newdir(newpathp);
rainy.newdir(newpathr);
for i=1,#files do
    local b = rainy.getfilename(files[i]) .. ".compiled";
	local c = string.lower(rainy.getfileextension(files[i]));
	if (c == ".xml") then
		rainy.particles(files[i],newpathp .. b,6,mode);
	elseif (c == ".reanim") then
		rainy.reanim(files[i],newpathr .. b,6,mode);
	elseif (c == ".trail") then
		rainy.trail(files[i],newpathp .. b,6,mode);
	end
end