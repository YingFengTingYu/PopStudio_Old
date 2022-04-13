local closure = function (args)
    local UserDataItem = {
        value = nil,
        BaseStream = nil
    };

    function UserDataItem:new(v,stream)
        local o = {};
        setmetatable(o, self);
        self.__index = self;
        if (v < 0) then
            v = v + stream.Length;
        end
        o.value = v;
        o.BaseStream = stream;
        return o;
    end

    function UserDataItem:get(length,delta)
        self.BaseStream.Position = self.value + (delta or 0);
        if (length == "byte") then
            return self.BaseStream:ReadByte();
        elseif (length == "bool") then
            return self.BaseStream:ReadBoolean();
        elseif (length == "i16") then
            return self.BaseStream:ReadInt16();
        elseif (length == "i32") then
            return self.BaseStream:ReadInt32();
        elseif (length == "i64") then
            return self.BaseStream:ReadInt64();
        elseif (length == "u16") then
            return self.BaseStream:ReadUInt16();
        elseif (length == "u32") then
            return self.BaseStream:ReadUInt32();
        elseif (length == "u64") then
            return self.BaseStream:ReadUInt64();
        elseif (length == "f32") then
            return self.BaseStream:ReadFloat32();
        elseif (length == "f64") then
            return self.BaseStream:ReadFloat64();
        end
    end

    function UserDataItem:set(v,length,delta)
        self.BaseStream.Position = self.value + (delta or 0);
        if (length == "byte") then
            self.BaseStream:WriteByte(v);
        elseif (length == "bool") then
            self.BaseStream:WriteBoolean(not (not v));
        elseif (length == "i16") then
            self.BaseStream:WriteInt16(v);
        elseif (length == "i32") then
            self.BaseStream:WriteInt32(v);
        elseif (length == "i64") then
            self.BaseStream:WriteInt64(v);
        elseif (length == "u16") then
            self.BaseStream:WriteUInt16(v);
        elseif (length == "u32") then
            self.BaseStream:WriteUInt32(v);
        elseif (length == "u64") then
            self.BaseStream:WriteUInt64(v);
        elseif (length == "f32") then
            self.BaseStream:WriteFloat32(v);
        elseif (length == "f64") then
            self.BaseStream:WriteFloat64(v);
        end
    end

    local UserData = {
        BaseStream = nil,
        AdventureLevel = nil,
        Coins = nil,
        AdventureTimes = nil,
        SurvivalFinishTimes = nil,
        MiniGamesFinishTimes = nil,
        VasesFinishTimes = nil,
        IZombieFinishTimes = nil,
        LastStandFinishTimes = nil,
        ShopPlants = nil,
        ShopGamePack = nil,
        Challenge = nil
    };

    function UserData:new(file,mode)
        local o = {};
        setmetatable(o, self);
        self.__index = self;
        if (mode == 1) then
            o:InitFromStream(file);
        else
            o:InitFromFile(file);
        end
        o:LoadInfo();
        return o;
    end

    function UserData:InitFromStream(stream)
        self.BaseStream = stream;
    end

    function UserData:InitFromFile(path)
        self.BaseStream = rainy.getfilestream(path,0);
    end

    function UserData:LoadInfo()
        self.BaseStream.Position = 0;
        self.BaseStream:IdInt32(0x22);
        self.AdventureLevel = UserDataItem:new(0x4,self.BaseStream);
        self.Coins = UserDataItem:new(0x8,self.BaseStream);
        self.AdventureTimes = UserDataItem:new(0xC,self.BaseStream);
        self.SurvivalFinishTimes = UserDataItem:new(0x10,self.BaseStream);
        self.MiniGamesFinishTimes = UserDataItem:new(0x4C,self.BaseStream);
        self.VasesFinishTimes = UserDataItem:new(0xD4,self.BaseStream);
        self.IZombieFinishTimes = UserDataItem:new(0xFC,self.BaseStream);
        self.LastStandFinishTimes = UserDataItem:new(0x1F4,self.BaseStream);
        self.ShopPlants = UserDataItem:new(0x330,self.BaseStream);
        self.ShopGamePack = UserDataItem:new(0x3A4,self.BaseStream);
        self.Challenge = UserDataItem:new(-0x1AC,self.BaseStream);
    end

    local App = {
        console = nil,
        system = nil,
        version = nil,
        language = nil,
        loc = nil,
        data = nil,
        isrun = nil
    };

    function App:new()
        local o = {};
        setmetatable(o, self);
        self.__index = self;
        local systeminfo = rainy.getsystem();
        o.console = ((systeminfo & 0x80) ~= 0);
        o.system = (systeminfo & 0x7F);
        o.version = rainy.getversion();
        o.language = rainy.getlanguage();
        o:CheckVersion();
        o:InitLoc();
        o:CheckPassword();
        o.isrun = true;
        return o;
    end

    function App:CheckVersion()
        if (self.version < 12) then
            error("The rainy version is too low! ")
        end
    end

    function App:InitLoc()
        local loc = {
            [0] = { --ZHCN
                Name = "PVZGoogle6.1.11DataModifier.lua 作者：萌新迎风听雨",
                Description = "说明：一个修改植物大战僵尸谷歌版6.1.11存档文件的脚本（只支持userX.dat）",
                Title = "存档修改器",
                Error = "请输入文件位置",
                Password = "请输入密码",
                BaseValue = "基础数值更改",
                FinishLevel = "一键完成关卡成就",
                Shop = "商店一键购买",
                Exit = "退出脚本",
                Back = "返回上一级",
                BaseValue_AdventrueLevel = "冒险模式关卡",
                BaseValue_Coins = "金币数量",
                BaseValue_AdventrueTimes = "冒险通关次数",
                FinishLevel_Vases = "通关花瓶终结者",
                FinishLevel_IZombie = "通关僵尸公敌",
                FinishLevel_LastStand = "通关坚不可摧",
                FinishLevel_Survival = "通关生存模式",
                FinishLevel_Challenge = "完成所有成就",
                Shop_Plants = "购买所有植物",
                Shop_GamePack = "购买所有游戏",
                NewValue = "请输入要修改的值",
                ModifyFinish = "修改完成",
                End = "脚本运行结束，感谢您的使用！"
            },
            [1] = { --ENUS
                Name = "PVZGoogle6.1.11DataModifier.lua Author: YingFengTingYu",
                Description = "Description: A script that modify data of PVZ Google 6.1.11(just support userX.dat)",
                Title = "Data Modifier",
                Error = "Please enter the file path",
                Password = "Please enter the password",
                BaseValue = "Change Base Value",
                FinishLevel = "Finish Level And Challenge",
                Shop = "Shop",
                Exit = "Exit Script",
                Back = "Back",
                BaseValue_AdventrueLevel = "Adventrue Level",
                BaseValue_Coins = "Coins Number",
                BaseValue_AdventrueTimes = "Adventrue Times",
                FinishLevel_Vases = "Finish Vases",
                FinishLevel_IZombie = "Finish IZombie",
                FinishLevel_LastStand = "Finish LastStand",
                FinishLevel_Survival = "Finish Survival",
                FinishLevel_Challenge = "Finsih Challenge",
                Shop_Plants = "Buy All Plants",
                Shop_GamePack = "Buy All Games",
                NewValue = "Please enter the new value",
                ModifyFinish = "Finish Modifying",
                End = "The script is end. Thanks for using!"
            }
        };
        self.loc = loc[rainy.getlanguage()];
    end

    function App:PrintInfo()
        print(self.loc.Name);
        print(self.loc.Description);
    end

    function App:AlertInfo()
        rainy.alert(self.loc.Name .. "\n" .. self.loc.Description,self.loc.Title);
    end

    function App:AlertEnd()
        rainy.alert(self.loc.End,self.loc.Title);
    end

    function App:CheckPassword(word)
        word = word or args[1] or rainy.prompt(self.loc.Password,self.loc.Title,"PASSWORD");
        if (word ~= "PASSWORD") then
            error(self.loc.Password);
        end
    end

    function App:LoadDataFile(path)
        path = path or args[2] or rainy.chooseopenfile();
        self.data = UserData:new(path);
    end

    function App:ShowUI()
        if (not self.isrun) then
            return;
        end
        local ans;
        while (true) do
            ans = rainy.sheet(self.loc.Title,self.loc.BaseValue,self.loc.FinishLevel,self.loc.Shop,self.loc.Exit);
            if (ans == self.loc.Exit) then
                self.isrun = false;
                break;
            elseif (ans == self.loc.BaseValue) then
                self:ShowUI_BaseValue();
            elseif (ans == self.loc.FinishLevel) then
                self:ShowUI_FinishLevel();
            elseif (ans == self.loc.Shop) then
                self:ShowUI_Shop();
            end
        end
    end

    function App:ShowUI_BaseValue()
        if (not self.isrun) then
            return;
        end
        local ans;
        while (true) do
            local nowAdventrueLevel = self.data.AdventureLevel:get("i32");
            local strAdventrueLevel = self.loc.BaseValue_AdventrueLevel .. nowAdventrueLevel;
            local nowCoins = self.data.Coins:get("i32") * 10;
            local strCoins = self.loc.BaseValue_Coins .. nowCoins;
            local nowAdventrueTimes = self.data.AdventureTimes:get("i32");
            local strAdventrueTimes = self.loc.BaseValue_AdventrueTimes .. nowAdventrueTimes;
            ans = rainy.sheet(self.loc.Title,strAdventrueLevel,strCoins,strAdventrueTimes,self.loc.Back,self.loc.Exit);
            if (ans == self.loc.Exit) then
                self.isrun = false;
                break;
            elseif (ans == self.loc.Back) then
                break;
            elseif (ans == strAdventrueLevel) then
                local innertext = rainy.prompt(self.loc.NewValue,self.loc.Title,nowAdventrueLevel);
                if (innertext ~= nil and innertext ~= "") then
                    self.data.AdventureLevel:set(tonumber(innertext),"i32");
                end
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            elseif (ans == strCoins) then
                local innertext = rainy.prompt(self.loc.NewValue,self.loc.Title,nowCoins);
                if (innertext ~= nil and innertext ~= "") then
                    self.data.Coins:set(tonumber(innertext) / 10,"i32");
                end
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            elseif (ans == strAdventrueTimes) then
                local innertext = rainy.prompt(self.loc.NewValue,self.loc.Title,nowAdventrueTimes);
                if (innertext ~= nil and innertext ~= "") then
                    self.data.AdventureTimes:set(tonumber(innertext),"i32");
                end
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            end
        end
    end

    function App:ShowUI_FinishLevel()
        if (not self.isrun) then
            return;
        end
        local ans;
        while (true) do
            ans = rainy.sheet(self.loc.Title,self.loc.FinishLevel_Vases,self.loc.FinishLevel_IZombie,self.loc.FinishLevel_LastStand,self.loc.FinishLevel_Survival,self.loc.FinishLevel_Challenge,self.loc.Back,self.loc.Exit);
            if (ans == self.loc.Exit) then
                self.isrun = false;
                break;
            elseif (ans == self.loc.Back) then
                break;
            elseif (ans == self.loc.FinishLevel_Vases) then
                for i=0,8 do
                    self.data.VasesFinishTimes:set(1,"i32",i << 2);
                end
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            elseif (ans == self.loc.FinishLevel_IZombie) then
                for i=0,8 do
                    self.data.IZombieFinishTimes:set(1,"i32",i << 2);
                end
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            elseif (ans == self.loc.FinishLevel_LastStand) then
                for i=0,4 do
                    self.data.LastStandFinishTimes:set(5,"i32",i << 2);
                end
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            elseif (ans == self.loc.FinishLevel_Survival) then
                for i=0,4 do
                    self.data.SurvivalFinishTimes:set(5,"i32",i << 2);
                end
                for i=5,9 do
                    self.data.SurvivalFinishTimes:set(10,"i32",i << 2);
                end
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            elseif (ans == self.loc.FinishLevel_Challenge) then
                for i=0,91 do
                    self.data.Challenge:set(true,"bool",i);
                end
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            end
        end
    end

    function App:ShowUI_Shop()
        if (not self.isrun) then
            return;
        end
        local ans;
        while (true) do
            ans = rainy.sheet(self.loc.Title,self.loc.Shop_Plants,self.loc.Shop_GamePack,self.loc.Back,self.loc.Exit);
            if (ans == self.loc.Exit) then
                self.isrun = false;
                break;
            elseif (ans == self.loc.Back) then
                break;
            elseif (ans == self.loc.Shop_Plants) then
                for i=0,8 do
                    self.data.ShopPlants:set(1,"i32",i << 2);
                end
                self.data.ShopPlants:set(1,"i32",0x44);
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            elseif (ans == self.loc.Shop_GamePack) then
                for i=0,7 do
                    self.data.ShopGamePack:set(1,"i32",i << 2);
                end
                self.data.ShopGamePack:set(1,"i32",0x34);
                rainy.alert(self.loc.ModifyFinish,self.loc.Title);
            end
        end
    end

    function App:Exit()
        error("Finish");
    end
    
    local myApp = App:new();
    myApp:AlertInfo();
    myApp:LoadDataFile();
    myApp:ShowUI();
    myApp:AlertEnd();
end
closure(args or {});