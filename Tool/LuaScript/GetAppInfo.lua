local version = rainy.getversion();
local language = 0;
if version >= 10 then
	language = rainy.getlanguage();
end
--Set language string
local loc = {
	[0] = { --ZHCN
		Name = "GetAppInfo.lua 作者：萌新迎风听雨",
		Description = "说明：一个用于获取当前程序信息的娱乐脚本",
		Finish = "执行完成！",
		AppVersion = "Rainy类库版本：",
		AppType = "程序类型：",
		AppLanguage = "程序语言：简体中文"
	},
	[1] = { --ENUS
		Name = "GetAppInfo.lua Author: YingFengTingYu",
		Description = "Description: An entertainment script to obtain the current program information",
		Finish = "Execution complete! ",
		AppVersion = "Rainy Library Version: ",
		AppType = "Program Type: ",
		AppLanguage = "Program Language: English"
	}
};
loc = loc[language];
--Show info
print(loc.Name);
print(loc.Description);
local flags = rainy.getsystem();
local console = "GUI";
if (flags & 0x80) ~= 0 then
	console = "Console";
end
local system = "Unknown";
if (flags & 0x1) ~= 0 then
	system = "Windows";
elseif (flags & 0x2) ~= 0 then
	system = "Android";
elseif (flags & 0x4) ~= 0 then
	system = "Linux";
elseif (flags & 0x8) ~= 0 then
	system = "MacOS";
end
if (version >= 10) then
	local alertstring = loc.AppVersion .. version .. "\n" .. loc.AppType .. system .. " " .. console .. "\n" .. loc.AppLanguage;
	rainy.alert(alertstring);
else
	print(loc.AppVersion .. version);
	print(loc.AppType .. system .. " " .. console);
	print(loc.AppLanguage);
end