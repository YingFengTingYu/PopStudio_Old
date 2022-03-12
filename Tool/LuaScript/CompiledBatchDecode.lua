--Check the rainy version
if (rainy.getversion() < 10) then error("The rainy version is too low! ") end
--Set language string
local loc = {
	[0] = { --ZHCN
		Name = "CompiledBatchDecode.lua 作者：萌新迎风听雨",
		Description = "说明：一个将指定文件夹内所有compiled后缀格式文件转为json文件的批处理脚本",
		Error = "请输入文件夹位置",
		OutFormat = "请选择compiled格式",
		Finish = "执行完成！"
	},
	[1] = { --ENUS
		Name = "CompiledBatchDecode.lua Author: YingFengTingYu",
		Description = "Description: A batch script that converts all compiled files in the specified folder into json files",
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
--Finish
rainy.alert(loc.Finish);