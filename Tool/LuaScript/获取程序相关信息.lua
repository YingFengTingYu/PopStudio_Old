print("获取程序相关信息.lua 作者：萌新迎风听雨");
print("脚本说明：一个用于获取当前是否为控制台程序、系统类型、rainy类库版本号的娱乐脚本");
local flags = rainy.getsystem();
local console = "GUI";
if ((flags & 0x80) ~= 0) then
    console = "Console";
end
local system = "Unknown";
if ((flags & 0x1) ~= 0) then
	system = "Windows";
elseif ((flags & 0x2) ~= 0) then
	system = "Android";
elseif ((flags & 0x4) ~= 0) then
	system = "Linux";
elseif ((flags & 0x8) ~= 0) then
	system = "MacOS";
end
local version = rainy.getversion();
print("您正在使用" .. system .. " " .. console .. "程序，rainy类库版本号为" .. version);