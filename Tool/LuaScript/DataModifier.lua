local closure = function (args)
    local App = {};

    local StreamPointer = {
        offset = nil,
        type = nil,
        length = nil,
        BaseStream = nil,
        __add = function (a,b)
            if (type(a) == "number") then
                local temp = a;
                a = b;
                b = temp;
            end
            return StreamPointer:new(a.BaseStream,a.offset + b * (a.type & 0xF) * (a.length or 1),a.type,a.length);
        end,
        typeenum = {
            byte = 0x11,
            u8 = 0x11,
            ushort = 0x12,
            u16 = 0x12,
            uint = 0x14,
            u32 = 0x14,
            ulong = 0x18,
            u64 = 0x18,
            sbyte = 0x21,
            i8 = 0x21,
            short = 0x22,
            i16 = 0x22,
            int = 0x24,
            i32 = 0x24,
            long = 0x28,
            i64 = 0x28,
            float = 0x44,
            f32 = 0x44,
            double = 0x48,
            f64 = 0x48,
            bool = 0x81,
            b = 0x81
        }
    };

    function StreamPointer:new(stream,offset,mtype,length)
        local o = {};
        setmetatable(o, self);
        self.__index = self;
        o.length = length;
        if (type(mtype) == "number") then
            o.type = mtype;
        else
            o.type = self.typeenum[mtype];
        end
        if (offset < 0) then
            offset = offset + stream.Length;
        end
        o.offset = offset;
        o.BaseStream = stream;
        return o;
    end

    function StreamPointer:get()
        self.BaseStream.Position = self.offset;
        local typekind = (self.type & 0xF0) >> 4;
        local typedepth = (self.type & 0xF);
        if (self.length) then
            local anstable = {};
            if ((typekind & 0x1) ~= 0) then
                if (typedepth == 0x1) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadByte();
                    end
                elseif (typedepth == 0x2) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadUInt16();
                    end
                elseif (typedepth == 0x4) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadUInt32();
                    end
                elseif (typedepth == 0x8) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadUInt64();
                    end
                end
            elseif ((typekind & 0x2) ~= 0) then
                if (typedepth == 0x1) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadSbyte();
                    end
                elseif (typedepth == 0x2) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadInt16();
                    end
                elseif (typedepth == 0x4) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadInt32();
                    end
                elseif (typedepth == 0x8) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadInt64();
                    end
                end
            elseif ((typekind & 0x4) ~= 0) then
                if (typedepth == 0x4) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadFloat32();
                    end
                elseif (typedepth == 0x8) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadFloat64();
                    end
                end
            elseif ((typekind & 0x8) ~= 0) then
                if (typedepth == 0x1) then
                    for i=1,self.Length do
                        anstable[i] = self.BaseStream:ReadBoolean();
                    end
                end
            end
            return anstable;
        else
            if ((typekind & 0x1) ~= 0) then
                if (typedepth == 0x1) then
                    return self.BaseStream:ReadByte();
                elseif (typedepth == 0x2) then
                    return self.BaseStream:ReadUInt16();
                elseif (typedepth == 0x4) then
                    return self.BaseStream:ReadUInt32();
                elseif (typedepth == 0x8) then
                    return self.BaseStream:ReadUInt64();
                end
            elseif ((typekind & 0x2) ~= 0) then
                if (typedepth == 0x1) then
                    return self.BaseStream:ReadSbyte();
                elseif (typedepth == 0x2) then
                    return self.BaseStream:ReadInt16();
                elseif (typedepth == 0x4) then
                    return self.BaseStream:ReadInt32();
                elseif (typedepth == 0x8) then
                    return self.BaseStream:ReadInt64();
                end
            elseif ((typekind & 0x4) ~= 0) then
                if (typedepth == 0x4) then
                    return self.BaseStream:ReadFloat32();
                elseif (typedepth == 0x8) then
                    return self.BaseStream:ReadFloat64();
                end
            elseif ((typekind & 0x8) ~= 0) then
                if (typedepth == 0x1) then
                    return self.BaseStream:ReadBoolean();
                end
            end
        end
    end

    function StreamPointer:set(value)
        self.BaseStream.Position = self.offset;
        local typekind = (self.type & 0xF0) >> 4;
        local typedepth = (self.type & 0xF);
        if (self.length) then
            if ((typekind & 0x1) ~= 0) then
                if (typedepth == 0x1) then
                    for i=1,self.Length do
                        self.BaseStream:WriteByte(value[i] or 0);
                    end
                elseif (typedepth == 0x2) then
                    for i=1,self.Length do
                        self.BaseStream:WriteUInt16(value[i] or 0);
                    end
                elseif (typedepth == 0x4) then
                    for i=1,self.Length do
                        self.BaseStream:WriteUInt32(value[i] or 0);
                    end
                elseif (typedepth == 0x8) then
                    for i=1,self.Length do
                        self.BaseStream:WriteUInt64(value[i] or 0);
                    end
                end
            elseif ((typekind & 0x2) ~= 0) then
                if (typedepth == 0x1) then
                    for i=1,self.Length do
                        self.BaseStream:WriteSbyte(value[i] or 0);
                    end
                elseif (typedepth == 0x2) then
                    for i=1,self.Length do
                        self.BaseStream:WriteInt16(value[i] or 0);
                    end
                elseif (typedepth == 0x4) then
                    for i=1,self.Length do
                        self.BaseStream:WriteInt32(value[i] or 0);
                    end
                elseif (typedepth == 0x8) then
                    for i=1,self.Length do
                        self.BaseStream:WriteInt64(value[i] or 0);
                    end
                end
            elseif ((typekind & 0x4) ~= 0) then
                if (typedepth == 0x4) then
                    for i=1,self.Length do
                        self.BaseStream:WriteFloat32(value[i] or 0);
                    end
                elseif (typedepth == 0x8) then
                    for i=1,self.Length do
                        self.BaseStream:WriteFloat64(value[i] or 0);
                    end
                end
            elseif ((typekind & 0x8) ~= 0) then
                if (typedepth == 0x1) then
                    for i=1,self.Length do
                        self.BaseStream:WriteBoolean(not (not value[i]));
                    end
                end
            end
        else
            if ((typekind & 0x1) ~= 0) then
                if (typedepth == 0x1) then
                    self.BaseStream:WriteByte(value);
                elseif (typedepth == 0x2) then
                    self.BaseStream:WriteUInt16(value);
                elseif (typedepth == 0x4) then
                    self.BaseStream:WriteUInt32(value);
                elseif (typedepth == 0x8) then
                    self.BaseStream:WriteUInt64(value);
                end
            elseif ((typekind & 0x2) ~= 0) then
                if (typedepth == 0x1) then
                    self.BaseStream:WriteSbyte(value);
                elseif (typedepth == 0x2) then
                    self.BaseStream:WriteInt16(value);
                elseif (typedepth == 0x4) then
                    self.BaseStream:WriteInt32(value);
                elseif (typedepth == 0x8) then
                    self.BaseStream:WriteInt64(value);
                end
            elseif ((typekind & 0x4) ~= 0) then
                if (typedepth == 0x4) then
                    self.BaseStream:WriteFloat32(value);
                elseif (typedepth == 0x8) then
                    self.BaseStream:WriteFloat64(value);
                end
            elseif ((typekind & 0x8) ~= 0) then
                if (typedepth == 0x1) then
                    self.BaseStream:WriteBoolean(value);
                end
            end
        end
    end

    local UIItem = {
        Pointer = nil,
        type = nil,
        title = nil,
        Parent = nil,
        value = nil,
        typeenum = {
            value = 0x1,
            bool = 0x2,
            default = 0x4,
            back = 0x8,
            exit = 0x10,
        }
    };

    function UIItem:new(title,itemtype,stream,offset,mtype,length,value)
        local o = {};
        setmetatable(o, self);
        self.__index = self;
        o.title = title;
        if (type(itemtype) == "number") then
            o.type = itemtype;
        else
            o.type = self.typeenum[itemtype];
        end
        if (o.type > 0x4) then
            return o;
        end
        o.value = value;
        o.Pointer = StreamPointer:new(stream,offset,mtype,length);
        return o;
    end

    function UIItem:Content()
        if ((self.type & 0x10) ~= 0) then
            return "退出脚本";
        elseif ((self.type & 0x8) ~= 0) then
                return "返回上级";
        elseif ((self.type & 0x4) ~= 0) then
            return self.title;
        else
            return string.format(self.title,self.Pointer:get());
        end
    end

    function UIItem:Touch(v)
        if ((self.type & 0x1) ~= 0) then
            local s = rainy.prompt(nil,"请选择新增项",self.Pointer:get());
            local num = tonumber(s);
            if (self.Pointer.length) then
                local numtable = {};
                for i=1,self.Pointer.length do
                    numtable[i] = num;
                end
                self.Pointer:set(numtable);
            else
                self.Pointer:set(num);
            end
        elseif ((self.type & 0x2) ~= 0) then
            local s = rainy.sheet(nil,"请选择新增项","True","False");
            local num = (s == "True");
            if (self.Pointer.length) then
                local numtable = {};
                for i=1,self.Pointer.length do
                    numtable[i] = num;
                end
                self.Pointer:set(numtable);
            else
                self.Pointer:set(num);
            end
        elseif ((self.type & 0x4) ~= 0) then
            self.Pointer:set(v or self.value);
        elseif ((self.type & 0x8) ~= 0) then
            if (Father) then
                Father:Back();
            end
        elseif ((self.type & 0x10) ~= 0) then
            App:Exit();
        end
    end

    local UserData = {
        BaseStream = nil
    };

    function UserData:new(v,mode)
        local o = {};
        setmetatable(o, self);
        self.__index = self;
        if (mode == 1) then
            self.BaseStream = v;
        else
            self.BaseStream = rainy.getfilestream(v,0);
        end
        return o;
    end

    local UIPage = {
        Items = nil,
        Parent = nil,
        title = nil,
        hasload = nil,
        hasdestroy = nil
    };

    function UIPage:new(title,stream,items)
        local o = {};
        setmetatable(o, self);
        self.__index = self;
        o.title = title;
        o.Items = {};
        for i=1,items.length do
            UIPage:Add(UIItem:new(items[i][1],items[i][2],stream,items[i][3],items[i][4],items[i][5],items[i][6]));
        end
        o.hasload = false;
        return o;
    end

    function UIPage:Add(item)
        table.insert(self.Items,item);
        item.Parent = self.Items;
    end

    function UIPage:Show()
        self.hasdestroy = false;
        if (not self.hasload) then
            self.hasload = true;
            if (self.Parent) then
                UIPage:Add(UIItem:new(nil,0x8,nil,nil,nil,nil,nil));
            end
            UIPage:Add(UIItem:new(nil,0x10,nil,nil,nil,nil,nil));
        end
        while (not self.hasdestroy) do
            local t = {};
            for i=1,self.Items.length do
                table.insert(t,self.Items[i]:Content());
            end
            local ans = rainy.sheet(self.title,table.unpack(t));
            local index = 0;
            for i=1,self.Items.length do
                if (ans == t[i]) then
                    index = i;
                    break;
                end
            end
            if (index ~= 0) then
                self.Items[index]:Touch();
            end
        end
    end

    function UIPage:Back()
        self.hasdestroy = true;
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