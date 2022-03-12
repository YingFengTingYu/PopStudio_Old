--Check the rainy version
if (rainy.getversion() < 10) then error("The rainy version is too low! ") end
--Set language string
local loc = {
	[0] = { --ZHCN
		Name = "CompiledBatchEncode.lua 作者：萌新迎风听雨",
		Description = "说明：一个将指定文件夹内的json文件转为compiled后缀格式文件的批处理脚本",
		Error = "请输入文件夹位置",
		OutFormat = "请选择compiled格式",
		Finish = "执行完成！"
	},
	[1] = { --ENUS
		Name = "CompiledBatchEncode.lua Author: YingFengTingYu",
		Description = "Description: A batch script that converts json files in the specified folder into compiled files",
		Error = "Please enter the file path",
		OutFormat = "Choose compiled format",
		Finish = "Execution complete! "
	}
};
loc = loc[rainy.getlanguage()];
--Show info
print(loc.Name);
print(loc.Description);
--Ask the file path and the out format
local filepath = args[1] or rainy.choosefolder() or error(loc.Error);
local mode = args[2];
if mode == nil then
    local m = rainy.sheet(loc.OutFormat,"PC","Phone32","Phone64","Android TV","GameConsole");
    local tab = {PC = 0, Phone32 = 1, Phone64 = 2, ["Android TV"] = 5, GameConsole = 4};
    mode = tab[m];
end
--Compute and create the generated folder
local newpathr = rainy.formatpath(filepath .. "\\finished\\reanim\\");
local newpathp = rainy.formatpath(filepath .. "\\finished\\particles\\");
rainy.newdir(newpathp);
rainy.newdir(newpathr);
--Convert the file
local files = rainy.getfiles(filepath);
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
--Finish
rainy.alert(loc.Finish);