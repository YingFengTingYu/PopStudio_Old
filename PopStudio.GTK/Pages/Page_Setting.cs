using Gtk;
using PopStudio.Language.Languages;
using PopStudio.GTK.Plugin;

#pragma warning disable 0612, 0618

namespace PopStudio.GTK.Pages
{
    internal class Page_Setting : Page
    {
        public ListStore dzpackinfo = new ListStore(typeof(string), typeof(string));
        public ListStore pakps3packinfo = new ListStore(typeof(string), typeof(string));

        public Label label_introduction;
        public Label label_itemlanguage;
        public Button button_chooselanguage;
        public Label label_itemdz;
        public Label label_introdz;
        public TreeView Setting_Dz;
        public HBox buttonpairs_dz;
        public Button button_dz_1;
        public Button button_dz_2;
        public Label label_itempak;
        public Label label_intropak;
        public TreeView Setting_PakPS3;
        public HBox buttonpairs_pak;
        public Button button_pak_1;
        public Button button_pak_2;
        public Label label_itemrsb;
        public Label label_introrsb;
        public HBox switchpairs_rsbptx1;
        public Label label_rsbptx1;
        public Switch switch_rsbptx1;
        public HBox switchpairs_rsbptx2;
        public Label label_rsbptx2;
        public Switch switch_rsbptx2;
        public Label label_itemptx;
        public Label label_introptx;
        public HBox switchpairs_ptx;
        public Label label_ptx;
        public Switch switch_ptx;
        public HBox switchpairs_ptx2;
        public Label label_ptx2;
        public Switch switch_ptx2;
        public Label label_itemcdat;
        public Label label_introcdat;
        public Entry entry_cdat;
        public Label label_itemrton;
        public Label label_introrton;
        public Entry entry_rton;
        public Label label_itemcompiled;
        public Label label_introcompiled;
        public Label label_imagestring;
        public HBox buttonpairs_compiled;
        public Button button_compiled_1;
        public Button button_compiled_2;
        public Label label_itemxfl;
        public Label label_introxfl;
        public HBox entrypairs_xflwidth;
        public Label label_xflwidth;
        public Entry entry_xflwidth;
        public HBox entrypairs_xflheight;
        public Label label_xflheight;
        public Entry entry_xflheight;
        public HBox entrypairs_xfllabelname;
        public Label label_xfllabelname;
        public Entry entry_xfllabelname;
        public HBox entrypairs_xflscalex;
        public Label label_xflscalex;
        public Entry entry_xflscalex;
        public HBox entrypairs_xflscaley;
        public Label label_xflscaley;
        public Entry entry_xflscaley;
        public HBox switchpairs_ad;
        public Label label_ad;
        public Switch switch_ad;
        public Button button_recover;

        public Page_Setting()
        {
            #region Init
            label_introduction = CreateText(MAUIStr.Obj.Setting_Introduction);
            label_itemlanguage = CreateTitle(string.Format(MAUIStr.Obj.Setting_ItemLanguage, Setting.LanguageName[Setting.AppLanguage]));
            button_chooselanguage = CreateButton(MAUIStr.Obj.Setting_SetLanguage);
            label_itemdz = CreateTitle(MAUIStr.Obj.Setting_ItemDz);
            label_introdz = CreateText(MAUIStr.Obj.Setting_IntroDz);
            Setting_Dz = CreateListView(1);
            buttonpairs_dz = CreateButtonPairs(out button_dz_1, out button_dz_2, MAUIStr.Obj.Setting_Add, MAUIStr.Obj.Setting_Clear);
            label_itempak = CreateTitle(MAUIStr.Obj.Setting_ItemPak);
            label_intropak = CreateText(MAUIStr.Obj.Setting_IntroPak);
            Setting_PakPS3 = CreateListView(2);
            buttonpairs_pak = CreateButtonPairs(out button_pak_1, out button_pak_2, MAUIStr.Obj.Setting_Add, MAUIStr.Obj.Setting_Clear);
            label_itemrsb = CreateTitle(MAUIStr.Obj.Setting_ItemRsb);
            label_introrsb = CreateText(MAUIStr.Obj.Setting_IntroRsb);
            switchpairs_rsbptx1 = CreateSwitchPairs(out label_rsbptx1, out switch_rsbptx1, "ABGR8888");
            switchpairs_rsbptx2 = CreateSwitchPairs(out label_rsbptx2, out switch_rsbptx2, "ARGB8888_Padding");
            label_itemptx = CreateTitle(MAUIStr.Obj.Setting_ItemPtx);
            label_introptx = CreateText(MAUIStr.Obj.Setting_IntroPtx);
            switchpairs_ptx = CreateSwitchPairs(out label_ptx, out switch_ptx, "ABGR8888");
            switchpairs_ptx2 = CreateSwitchPairs(out label_ptx2, out switch_ptx2, "ARGB8888_Padding");
            label_itemcdat = CreateTitle(MAUIStr.Obj.Setting_ItemCdat);
            label_introcdat = CreateText(MAUIStr.Obj.Setting_IntroCdat);
            entry_cdat = CreateEntry();
            label_itemrton = CreateTitle(MAUIStr.Obj.Setting_ItemRTON);
            label_introrton = CreateText(MAUIStr.Obj.Setting_IntroRTON);
            entry_rton = CreateEntry();
            label_itemcompiled = CreateTitle(MAUIStr.Obj.Setting_ItemCompiled);
            label_introcompiled = CreateText(MAUIStr.Obj.Setting_IntroCompiled);
            label_imagestring = CreateTitle(Setting.ImageConvertName);
            buttonpairs_compiled = CreateButtonPairs(out button_compiled_1, out button_compiled_2, MAUIStr.Obj.Setting_Load, MAUIStr.Obj.Setting_Unload);
            label_itemxfl = CreateTitle(MAUIStr.Obj.Setting_ItemXfl);
            label_introxfl = CreateText(MAUIStr.Obj.Setting_IntroXfl);
            entrypairs_xflwidth = CreateEntryPairs(out label_xflwidth, out entry_xflwidth, MAUIStr.Obj.Setting_XflWidth);
            entrypairs_xflheight = CreateEntryPairs(out label_xflheight, out entry_xflheight, MAUIStr.Obj.Setting_XflHeight);
            entrypairs_xfllabelname = CreateEntryPairs(out label_xfllabelname, out entry_xfllabelname, MAUIStr.Obj.Setting_XflLabelName);
            entrypairs_xflscalex = CreateEntryPairs(out label_xflscalex, out entry_xflscalex, MAUIStr.Obj.Setting_XflScaleX);
            entrypairs_xflscaley = CreateEntryPairs(out label_xflscaley, out entry_xflscaley, MAUIStr.Obj.Setting_XflScaleY);
            switchpairs_ad = CreateSwitchPairs(out label_ad, out switch_ad, MAUIStr.Obj.Setting_AD);
            button_recover = CreateButton(MAUIStr.Obj.Setting_Recover);
            #endregion
            #region Regist
            PackStart(label_introduction, false, false, 5);
            PackStart(label_itemlanguage, false, false, 5);
            PackStart(button_chooselanguage, false, false, 5);
            PackStart(label_itemdz, false, false, 5);
            PackStart(label_introdz, false, false, 5);
            PackStart(Setting_Dz, false, false, 5);
            PackStart(buttonpairs_dz, false, false, 5);
            PackStart(label_itempak, false, false, 5);
            PackStart(label_intropak, false, false, 5);
            PackStart(Setting_PakPS3, false, false, 5);
            PackStart(buttonpairs_pak, false, false, 5);
            PackStart(label_itemrsb, false, false, 5);
            PackStart(label_introrsb, false, false, 5);
            PackStart(switchpairs_rsbptx1, false, false, 5);
            PackStart(switchpairs_rsbptx2, false, false, 5);
            PackStart(label_itemptx, false, false, 5);
            PackStart(label_introptx, false, false, 5);
            PackStart(switchpairs_ptx, false, false, 5);
            PackStart(switchpairs_ptx2, false, false, 5);
            PackStart(label_itemcdat, false, false, 5);
            PackStart(label_introcdat, false, false, 5);
            PackStart(entry_cdat, false, false, 5);
            PackStart(label_itemrton, false, false, 5);
            PackStart(label_introrton, false, false, 5);
            PackStart(entry_rton, false, false, 5);
            PackStart(label_itemcompiled, false, false, 5);
            PackStart(label_introcompiled, false, false, 5);
            PackStart(label_imagestring, false, false, 5);
            PackStart(buttonpairs_compiled, false, false, 5);
            PackStart(label_itemxfl, false, false, 5);
            PackStart(label_introxfl, false, false, 5);
            PackStart(entrypairs_xflwidth, false, false, 5);
            PackStart(entrypairs_xflheight, false, false, 5);
            PackStart(entrypairs_xfllabelname, false, false, 5);
            PackStart(entrypairs_xflscalex, false, false, 5);
            PackStart(entrypairs_xflscaley, false, false, 5);
            PackStart(switchpairs_ad, false, false, 5);
            PackStart(button_recover, false, false, 5);
            switch_rsbptx1.Active = Setting.RsbPtxABGR8888Mode;
            switch_rsbptx2.Active = Setting.RsbPtxARGB8888PaddingMode;
            switch_ptx.Active = Setting.PtxABGR8888Mode;
            switch_ptx2.Active = Setting.PtxARGB8888PaddingMode;
            entry_cdat.Text = Setting.CdatKey;
            entry_rton.Text = Setting.RTONKey;
            entry_xflwidth.Text = Setting.ReanimXflWidth.ToString();
            entry_xflheight.Text = Setting.ReanimXflHeight.ToString();
            entry_xfllabelname.Text = Setting.ReanimXflLabelName.ToString();
            entry_xflscalex.Text = Setting.ReanimXflScaleX.ToString();
            entry_xflscaley.Text = Setting.ReanimXflScaleY.ToString();
            switch_ad.Active = Setting.OpenProgramAD;
            #endregion
            #region Event
            button_chooselanguage.Clicked += (s, e) =>
            {
                try
                {
                    string cancel = MAUIStr.Obj.Setting_Cancel;
                    string ok = MAUIStr.Obj.Setting_OK;
                    string item1 = "简体中文";
                    string item2 = "English";
                    string ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseLanguage, cancel, ok, item1, item2);
                    if (ans != cancel && ans != ok && Setting.LanguageEnum[ans] != Setting.AppLanguage)
                    {
                        Setting.AppLanguage = Setting.LanguageEnum[ans];
                        Setting.SaveAsXml(Permission.GetSettingPath());
                        Dialogs.DisplayAlert(MAUIStr.Obj.Setting_FinishChooseLanguage, MAUIStr.Obj.Setting_FinishChooseLanguageText, ok);
                        Permission.Restart();
                    }
                }
                catch (Exception)
                {

                }
            };
            button_dz_1.Clicked += (s, e) =>
            {
                try
                {
                    string cancel = MAUIStr.Obj.Setting_Cancel;
                    string ok = MAUIStr.Obj.Setting_OK;
                    string ans = Dialogs.DisplayPromptAsync(MAUIStr.Obj.Setting_EnterExtension, MAUIStr.Obj.Setting_EnterExtensionText, ok, cancel);
                    if (!string.IsNullOrEmpty(ans))
                    {
                        string itemname = ans;
                        if (!itemname.StartsWith('.'))
                        {
                            itemname = '.' + itemname;
                        }
                        string item1 = "Store";
                        string item2 = "Lzma";
                        string item3 = "Gzip";
                        string item4 = "Bzip2";
                        ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2, item3, item4);
                        if (ans != cancel && ans != ok)
                        {
                            Dictionary<string, Package.Dz.CompressFlags> tempdic = Setting.DzCompressDictionary;
                            if (tempdic.ContainsKey(itemname))
                            {
                                tempdic[itemname] = Setting.DzCompressMethodEnum[ans];
                            }
                            else
                            {
                                tempdic.Add(itemname, Setting.DzCompressMethodEnum[ans]);
                            }
                            InitDzPackSetting();
                            Setting.SaveAsXml(Permission.GetSettingPath());
                        }
                    }
                }
                catch (Exception)
                {

                }
            };
            button_dz_2.Clicked += (s, e) =>
            {
                Setting.DzCompressDictionary.Clear();
                InitDzPackSetting();
                Setting.SaveAsXml(Permission.GetSettingPath());
            };
            Setting_Dz.ButtonReleaseEvent += (s, e) => //Can't use CursorChanged
            {
                try
                {
                    TreeSelection t = ((TreeView)s).Selection;
                    t.GetSelected(out ITreeModel model, out TreeIter iter);
                    string itemname = (string)model.GetValue(iter, 0);
                    string cancel = MAUIStr.Obj.Setting_Cancel;
                    string ok = MAUIStr.Obj.Setting_OK;
                    string choose1 = MAUIStr.Obj.Setting_CompressItem1;
                    string choose2 = MAUIStr.Obj.Setting_CompressItem2;
                    string ans;
                    if (itemname == "default")
                    {
                        ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1);
                    }
                    else
                    {
                        ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1, choose2);
                    }
                    if (ans == choose1)
                    {
                        string item1 = "Store";
                        string item2 = "Lzma";
                        string item3 = "Gzip";
                        string item4 = "Bzip2";
                        ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2, item3, item4);
                        if (ans != cancel && ans != ok)
                        {
                            if (itemname == "default")
                            {
                                Setting.DzDefaultCompressMethod = Setting.DzCompressMethodEnum[ans];
                            }
                            else
                            {
                                Setting.DzCompressDictionary[itemname] = Setting.DzCompressMethodEnum[ans];
                            }
                            InitDzPackSetting();
                            Setting.SaveAsXml(Permission.GetSettingPath());
                        }
                    }
                    else if (ans == choose2)
                    {
                        if (Setting.DzCompressDictionary.ContainsKey(itemname))
                        {
                            Setting.DzCompressDictionary.Remove(itemname);
                            InitDzPackSetting();
                            Setting.SaveAsXml(Permission.GetSettingPath());
                        }
                    }
                }
                catch (Exception)
                {

                }
            };
            button_pak_1.Clicked += (s, e) =>
            {
                try
                {
                    string cancel = MAUIStr.Obj.Setting_Cancel;
                    string ok = MAUIStr.Obj.Setting_OK;
                    string ans = Dialogs.DisplayPromptAsync(MAUIStr.Obj.Setting_EnterExtension, MAUIStr.Obj.Setting_EnterExtensionText, ok, cancel);
                    if (!string.IsNullOrEmpty(ans))
                    {
                        string itemname = ans;
                        if (!itemname.StartsWith('.'))
                        {
                            itemname = '.' + itemname;
                        }
                        string item1 = "Store";
                        string item2 = "Zlib";
                        ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2);
                        if (ans != cancel && ans != ok)
                        {
                            Dictionary<string, Package.Pak.CompressFlags> tempdic = Setting.PakPS3CompressDictionary;
                            if (tempdic.ContainsKey(itemname))
                            {
                                tempdic[itemname] = Setting.PakPS3CompressMethodEnum[ans];
                            }
                            else
                            {
                                tempdic.Add(itemname, Setting.PakPS3CompressMethodEnum[ans]);
                            }
                            InitPakPS3PackSetting();
                            Setting.SaveAsXml(Permission.GetSettingPath());
                        }
                    }
                }
                catch (Exception)
                {

                }
            };
            button_pak_2.Clicked += (s, e) =>
            {
                Setting.PakPS3CompressDictionary.Clear();
                InitPakPS3PackSetting();
                Setting.SaveAsXml(Permission.GetSettingPath());
            };
            Setting_PakPS3.ButtonReleaseEvent += (s, e) => //Can't use CursorChanged
            {
                try
                {
                    TreeSelection t = ((TreeView)s).Selection;
                    t.GetSelected(out ITreeModel model, out TreeIter iter);
                    string itemname = (string)model.GetValue(iter, 0);
                    string cancel = MAUIStr.Obj.Setting_Cancel;
                    string ok = MAUIStr.Obj.Setting_OK;
                    string choose1 = MAUIStr.Obj.Setting_CompressItem1;
                    string choose2 = MAUIStr.Obj.Setting_CompressItem2;
                    string ans;
                    if (itemname == "default")
                    {
                        ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1);
                    }
                    else
                    {
                        ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1, choose2);
                    }
                    if (ans == choose1)
                    {
                        string item1 = "Store";
                        string item2 = "Zlib";
                        ans = Dialogs.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2);
                        if (ans != cancel && ans != ok)
                        {
                            if (itemname == "default")
                            {
                                Setting.PakPS3DefaultCompressMethod = Setting.PakPS3CompressMethodEnum[ans];
                            }
                            else
                            {
                                Setting.PakPS3CompressDictionary[itemname] = Setting.PakPS3CompressMethodEnum[ans];
                            }
                            InitPakPS3PackSetting();
                            Setting.SaveAsXml(Permission.GetSettingPath());
                        }
                    }
                    else if (ans == choose2)
                    {
                        if (Setting.PakPS3CompressDictionary.ContainsKey(itemname))
                        {
                            Setting.PakPS3CompressDictionary.Remove(itemname);
                            InitPakPS3PackSetting();
                            Setting.SaveAsXml(Permission.GetSettingPath());
                        }
                    }
                }
                catch (Exception)
                {

                }
            };
            switch_rsbptx1.Activate += Switch_RsbPtx_1_Toggled;
            switch_rsbptx1.ButtonReleaseEvent += Switch_RsbPtx_1_Toggled;
            switch_rsbptx2.Activate += Switch_RsbPtx_2_Toggled;
            switch_rsbptx2.ButtonReleaseEvent += Switch_RsbPtx_2_Toggled;
            switch_ptx.Activate += Switch_Ptx_1_Toggled;
            switch_ptx.ButtonReleaseEvent += Switch_Ptx_1_Toggled;
            switch_ptx2.Activate += Switch_Ptx_2_Toggled;
            switch_ptx2.ButtonReleaseEvent += Switch_Ptx_2_Toggled;
            entry_cdat.Changed += (s, e) =>
            {
                Setting.CdatKey = entry_cdat.Text;
                Setting.SaveAsXml(Permission.GetSettingPath());
            };
            entry_rton.Changed += (s, e) =>
            {
                Setting.RTONKey = entry_rton.Text;
                Setting.SaveAsXml(Permission.GetSettingPath());
            };
            button_compiled_1.Clicked += (s, e) =>
            {
                try
                {
                    string path = PickFile.ChooseOpenFile();
                    if (string.IsNullOrEmpty(path)) return;
                    Setting.LoadImageConvertXml(path);
                    label_imagestring.Text = Setting.ImageConvertName;
                    Setting.SaveAsXml(Permission.GetSettingPath());
                }
                catch (Exception)
                {

                }
            };
            button_compiled_2.Clicked += (s, e) =>
            {
                Setting.ClearImageConvertXml();
                label_imagestring.Text = Setting.ImageConvertName;
                Setting.SaveAsXml(Permission.GetSettingPath());
            };
            entry_xflwidth.Changed += (s, e) =>
            {
                try
                {
                    float w = Convert.ToSingle(entry_xflwidth.Text);
                    Setting.ReanimXflWidth = w;
                    Setting.SaveAsXml(Permission.GetSettingPath());
                }
                catch (Exception)
                {

                }
            };
            entry_xflheight.Changed += (s, e) =>
            {
                try
                {
                    float w = Convert.ToSingle(entry_xflheight.Text);
                    Setting.ReanimXflHeight = w;
                    Setting.SaveAsXml(Permission.GetSettingPath());
                }
                catch (Exception)
                {

                }
            };
            entry_xfllabelname.Changed += (s, e) =>
            {
                try
                {
                    int w = Convert.ToInt32(entry_xfllabelname.Text);
                    Setting.ReanimXflLabelName = w;
                    Setting.SaveAsXml(Permission.GetSettingPath());
                }
                catch (Exception)
                {

                }
            };
            entry_xflscalex.Changed += (s, e) =>
            {
                try
                {
                    float w = Convert.ToSingle(entry_xflscalex.Text);
                    Setting.ReanimXflScaleX = w;
                    Setting.SaveAsXml(Permission.GetSettingPath());
                }
                catch (Exception)
                {

                }
            };
            entry_xflscaley.Changed += (s, e) =>
            {
                try
                {
                    float w = Convert.ToSingle(entry_xflscaley.Text);
                    Setting.ReanimXflScaleY = w;
                    Setting.SaveAsXml(Permission.GetSettingPath());
                }
                catch (Exception)
                {

                }
            };
            switch_ad.Activate += Switch_AD_1_Toggled;
            switch_ad.ButtonReleaseEvent += Switch_AD_1_Toggled;
            button_recover.Clicked += (s, e) =>
            {
                string cancel = MAUIStr.Obj.Setting_Cancel;
                string ok = MAUIStr.Obj.Setting_OK;
                bool sure = Dialogs.DisplayAlert(MAUIStr.Obj.Setting_SureRecover, MAUIStr.Obj.Setting_SureRecoverText, ok, cancel);
                if (sure)
                {
                    Setting.ResetXml(Permission.GetSettingPath());
                    Dialogs.DisplayAlert(MAUIStr.Obj.Setting_FinishRecover, MAUIStr.Obj.Setting_FinishRecoverText, ok);
                    Permission.Restart();
                }
            };
            #endregion
        }

        private void Switch_AD_1_Toggled(object sender, EventArgs e)
        {
            Setting.OpenProgramAD = !((Switch)sender).Active;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        private void Switch_RsbPtx_1_Toggled(object sender, EventArgs e)
        {
            Setting.RsbPtxABGR8888Mode = !((Switch)sender).Active;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        private void Switch_RsbPtx_2_Toggled(object sender, EventArgs e)
        {
            Setting.RsbPtxARGB8888PaddingMode = !((Switch)sender).Active;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        private void Switch_Ptx_1_Toggled(object sender, EventArgs e)
        {
            Setting.PtxABGR8888Mode = !((Switch)sender).Active;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        private void Switch_Ptx_2_Toggled(object sender, EventArgs e)
        {
            Setting.PtxARGB8888PaddingMode = !((Switch)sender).Active;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        TreeView CreateListView(int type)
        {
            switch (type)
            {
                case 1:
                    {
                        InitDzPackSetting();
                        TreeView tree = new TreeView(dzpackinfo);
                        TreeViewColumn v1 = new TreeViewColumn { Title = MAUIStr.Obj.Setting_Extension };
                        CellRendererText v1cell = new CellRendererText();
                        v1cell.FontDesc = GenFont(11);
                        v1.PackStart(v1cell, true);
                        v1.AddAttribute(v1cell, "text", 0);
                        TreeViewColumn v2 = new TreeViewColumn { Title = MAUIStr.Obj.Setting_CompressionMethod };
                        CellRendererText v2cell = new CellRendererText();
                        v2cell.FontDesc = GenFont(11);
                        v2.PackStart(v2cell, true);
                        v2.AddAttribute(v2cell, "text", 1);
                        tree.AppendColumn(v1);
                        tree.AppendColumn(v2);
                        return tree;
                    }
                case 2:
                    {
                        InitPakPS3PackSetting();
                        TreeView tree = new TreeView(pakps3packinfo);
                        TreeViewColumn v1 = new TreeViewColumn { Title = MAUIStr.Obj.Setting_Extension };
                        CellRendererText v1cell = new CellRendererText();
                        v1cell.FontDesc = GenFont(11);
                        v1.PackStart(v1cell, true);
                        v1.AddAttribute(v1cell, "text", 0);
                        TreeViewColumn v2 = new TreeViewColumn { Title = MAUIStr.Obj.Setting_CompressionMethod };
                        CellRendererText v2cell = new CellRendererText();
                        v2cell.FontDesc = GenFont(11);
                        v2.PackEnd(v2cell, true);
                        v2.AddAttribute(v2cell, "text", 1);
                        tree.AppendColumn(v1);
                        tree.AppendColumn(v2);
                        return tree;
                    }
                default:
                    return new TreeView();
            }
        }

        void InitDzPackSetting()
        {
            dzpackinfo.Clear();
            Dictionary<string, Package.Dz.CompressFlags> dzpacksetting = Setting.DzCompressDictionary;
            Dictionary<Package.Dz.CompressFlags, string> dzcompressname = Setting.DzCompressMethodName;
            dzpackinfo.AppendValues("default", dzcompressname[Setting.DzDefaultCompressMethod]);
            foreach (KeyValuePair<string, Package.Dz.CompressFlags> keyValuePair in dzpacksetting)
            {
                dzpackinfo.AppendValues(keyValuePair.Key, dzcompressname[keyValuePair.Value]);
            }
        }

        void InitPakPS3PackSetting()
        {
            pakps3packinfo.Clear();
            Dictionary<string, Package.Pak.CompressFlags> pakps3packsetting = Setting.PakPS3CompressDictionary;
            Dictionary<Package.Pak.CompressFlags, string> pakps3compressname = Setting.PakPS3CompressMethodName;
            pakps3packinfo.AppendValues("default", pakps3compressname[Setting.PakPS3DefaultCompressMethod]);
            foreach (KeyValuePair<string, Package.Pak.CompressFlags> keyValuePair in pakps3packsetting)
            {
                pakps3packinfo.AppendValues(keyValuePair.Key, pakps3compressname[keyValuePair.Value]);
            }
        }

        static HBox CreateButtonPairs(out Button b1, out Button b2, string s1, string s2)
        {
            b1 = CreateButton(s1);
            b1.MarginRight = 5;
            b2 = CreateButton(s2);
            b2.MarginLeft = 5;
            HBox ans = new HBox();
            ans.PackStart(b1, true, true, 0);
            ans.PackEnd(b2, true, true, 0);
            return ans;
        }

        static HBox CreateEntryPairs(out Label l, out Entry s, string t)
        {
            l = CreateTitle(t);
            s = CreateEntry();
            HBox ans = new HBox();
            ans.PackStart(l, false, false, 0);
            ans.PackEnd(s, false, false, 5);
            return ans;
        }

        static HBox CreateSwitchPairs(out Label l, out Switch s, string t)
        {
            l = CreateTitle(t);
            s = CreateSwitch();
            HBox ans = new HBox();
            ans.PackStart(l, false, false, 0);
            ans.PackEnd(s, false, false, 5);
            return ans;
        }

        static Switch CreateSwitch() => new Switch();

        static Label CreateTitle(string subtitle)
        {
            Label l = new Label(subtitle);
            l.Wrap = true;
            l.LineWrapMode = Pango.WrapMode.Char;
            l.ModifyFont(GenFont(14));
            l.Xalign = 0;
            return l;
        }

        static Label CreateText(string text)
        {
            Label l = new Label(text);
            l.LineWrap = true;
            l.LineWrapMode = Pango.WrapMode.Char;
            l.ModifyFont(GenFont(11));
            l.Xalign = 0;
            return l;
        }

        static Button CreateButton(string t)
        {
            Button ans = new Button(t);
            ans.ModifyFont(GenFont(11));
            return ans;
        }

        static Entry CreateEntry()
        {
            Entry ans = new Entry();
            ans.ModifyFont(GenFont(11));
            return ans;
        }
    }
}
