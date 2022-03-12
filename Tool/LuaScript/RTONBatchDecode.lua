--Check the rainy version
if (rainy.getversion() < 10) then error("The rainy version is too low! ") end
--Set language string
local loc = {
	[0] = { --ZHCN
		Name = "RTONBatchDecode.lua 作者：萌新迎风听雨",
		Description = "说明：一个将指定文件夹内的所有RTON后缀格式文件转为json文件的批处理脚本",
		Error = "请输入文件夹位置",
		OutFormat = "请选择RTON格式",
        DeleteTitle = "删除文件",
        Delete = "是否删除被转换的文件",
		Finish = "执行完成！"
	},
	[1] = { --ENUS
		Name = "RTONBatchDecode.lua Author: YingFengTingYu",
		Description = "Description: A batch script that converts all RTON files in the specified folder into json files",
		Error = "Please enter the file path",
		OutFormat = "Choose RTON format",
        DeleteTitle = "Delete Files",
        Delete = "Do you want to delete the converted file",
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
    local m = rainy.sheet(loc.OutFormat,"Simple","Encrypted");
    local tab = {Simple = 0, Encrypted = 1};
    mode = tab[m];
end
--Ask if you want to delete the old file
local delete = args[3];
if delete == nil then
    delete = rainy.alert(loc.Delete, loc.DeleteTitle, true);
end
--Compute and create the generated folder
local files = rainy.getfiles(filepath);
for i=1,#files do
    local a = string.lower(rainy.getfileextension(files[i]));
    local b = rainy.getfilenamewithoutextension(files[i]);
    if a == ".rton" then
        rainy.decoderton(files[i], rainy.formatpath(rainy.getfilepath(files[i]) .. "\\" .. b .. ".json"), mode);
        if delete then
            rainy.deletefile(files[i]);
        end
    end
end
--Finish
rainy.alert(loc.Finish);