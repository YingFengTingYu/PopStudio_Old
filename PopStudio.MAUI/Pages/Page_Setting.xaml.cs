using PopStudio.Language.Languages;
using PopStudio.Platform;

namespace PopStudio.MAUI
{
	public partial class Page_Setting : ContentPage
	{
        void LoadFont()
        {
            Title = MAUIStr.Obj.Setting_Title;
            label_introduction.Text = MAUIStr.Obj.Setting_Introduction;
            label_itemlanguage.Text = string.Format(MAUIStr.Obj.Setting_ItemLanguage, Setting.LanguageName[Setting.AppLanguage]);
            button_chooselanguage.Text = MAUIStr.Obj.Setting_SetLanguage;
            label_itemdz.Text = MAUIStr.Obj.Setting_ItemDz;
            label_introdz.Text = MAUIStr.Obj.Setting_IntroDz;
            button_dz_1.Text = MAUIStr.Obj.Setting_Add;
            button_dz_2.Text = MAUIStr.Obj.Setting_Clear;
            label_itempak.Text = MAUIStr.Obj.Setting_ItemPak;
            label_intropak.Text = MAUIStr.Obj.Setting_IntroPak;
            button_pak_1.Text = MAUIStr.Obj.Setting_Add;
            button_pak_2.Text = MAUIStr.Obj.Setting_Clear;
            label_itemrsb.Text = MAUIStr.Obj.Setting_ItemRsb;
            label_introrsb.Text = MAUIStr.Obj.Setting_IntroRsb;
            label_itemptx.Text = MAUIStr.Obj.Setting_ItemPtx;
            label_introptx.Text = MAUIStr.Obj.Setting_IntroPtx;
            label_itemcdat.Text = MAUIStr.Obj.Setting_ItemCdat;
            label_introcdat.Text = MAUIStr.Obj.Setting_IntroCdat;
            label_itemrton.Text = MAUIStr.Obj.Setting_ItemRTON;
            label_introrton.Text = MAUIStr.Obj.Setting_IntroRTON;
            label_itemcompiled.Text = MAUIStr.Obj.Setting_ItemCompiled;
            label_introcompiled.Text = MAUIStr.Obj.Setting_IntroCompiled;
            label_itemxfl.Text = MAUIStr.Obj.Setting_ItemXfl;
            label_introxfl.Text = MAUIStr.Obj.Setting_IntroXfl;
            label_xflwidth.Text = MAUIStr.Obj.Setting_XflWidth;
            label_xflheight.Text = MAUIStr.Obj.Setting_XflHeight;
            label_xfllabelname.Text = MAUIStr.Obj.Setting_XflLabelName;
            label_xflscalex.Text = MAUIStr.Obj.Setting_XflScaleX;
            label_xflscaley.Text = MAUIStr.Obj.Setting_XflScaleY;
            button_compiled_1.Text = MAUIStr.Obj.Setting_Load;
            button_compiled_2.Text = MAUIStr.Obj.Setting_Unload;
            button_recover.Text = MAUIStr.Obj.Setting_Recover;
            button_cdat.Text = MAUIStr.Obj.Setting_Submit;
            button_rton.Text = MAUIStr.Obj.Setting_Submit;
            button_xflwidth.Text = MAUIStr.Obj.Setting_Submit;
            button_xflheight.Text = MAUIStr.Obj.Setting_Submit;
            button_xflscalex.Text = MAUIStr.Obj.Setting_Submit;
            button_xflscaley.Text = MAUIStr.Obj.Setting_Submit;
        }

        void LoadSetting()
        {
            Setting.CanSave = false;
            InitDzPackSetting();
            InitPakPS3PackSetting();
            InitRsbPackSetting();
            InitPTXDecodeSetting();
            InitCdatKeySetting();
            InitRTONKeySetting();
            InitImageStringSetting();
            InitXflSetting();
            Setting.CanSave = true;
        }


        public Page_Setting()
		{
			InitializeComponent();
			LoadFont();
            MAUIStr.OnLanguageChanged += LoadFont;
            LoadSetting();
            xfllabelname.SelectedIndexChanged += xfllabel_TextChanged;
        }

        ~Page_Setting()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

		private void Button_Dz_1_Clicked(object sender, EventArgs e)
		{
			AddDzPackSetting();
		}

		private void Button_Dz_2_Clicked(object sender, EventArgs e)
		{
			ClearDzPackSetting();
		}

		void InitDzPackSetting()
		{
            Dictionary<string, Package.Dz.CompressFlags> dzpacksetting = Setting.DzCompressDictionary;
            Dictionary<Package.Dz.CompressFlags, string> dzcompressname = Setting.DzCompressMethodName;
            Setting_Dz.Children.Clear();
            AddListButton(Setting_Dz, "default", dzcompressname[Setting.DzDefaultCompressMethod], (s, e) => ChangeDzPackSetting("default"));
            foreach (KeyValuePair<string, Package.Dz.CompressFlags> keyValuePair in dzpacksetting)
            {
                AddListButton(Setting_Dz, keyValuePair.Key, dzcompressname[keyValuePair.Value], (s, e) => ChangeDzPackSetting(keyValuePair.Key));
            }
        }

		async void AddDzPackSetting()
		{
			try
			{
				string cancel = MAUIStr.Obj.Setting_Cancel;
				string ok = MAUIStr.Obj.Setting_OK;
				string ans = await DisplayPromptAsync(MAUIStr.Obj.Setting_EnterExtension, MAUIStr.Obj.Setting_EnterExtensionText, ok, cancel);
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
					ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2, item3, item4);
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
		}

		void ClearDzPackSetting()
		{
			Setting.DzCompressDictionary.Clear();
			InitDzPackSetting();
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		async void ChangeDzPackSetting(string itemname)
		{
			try
			{
				string cancel = MAUIStr.Obj.Setting_Cancel;
				string ok = MAUIStr.Obj.Setting_OK;
				string choose1 = MAUIStr.Obj.Setting_CompressItem1;
				string choose2 = MAUIStr.Obj.Setting_CompressItem2;
				string ans;
				if (itemname == "default")
				{
					ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1);
				}
				else
				{
					ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1, choose2);
				}
				if (ans == choose1)
				{
					string item1 = "Store";
					string item2 = "Lzma";
					string item3 = "Gzip";
					string item4 = "Bzip2";
					ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2, item3, item4);
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
		}

		private void Button_PakPS3_1_Clicked(object sender, EventArgs e)
		{
			AddPakPS3PackSetting();
		}

		private void Button_PakPS3_2_Clicked(object sender, EventArgs e)
		{
			ClearPakPS3PackSetting();
		}

		void InitPakPS3PackSetting()
		{
			Dictionary<string, Package.Pak.CompressFlags> pakps3packsetting = Setting.PakPS3CompressDictionary;
			Dictionary<Package.Pak.CompressFlags, string> pakps3compressname = Setting.PakPS3CompressMethodName;
			Setting_PakPS3.Children.Clear();
            AddListButton(Setting_PakPS3, "default", pakps3compressname[Setting.PakPS3DefaultCompressMethod], (s, e) => ChangePakPS3PackSetting("default"));
            foreach (KeyValuePair<string, Package.Pak.CompressFlags> keyValuePair in pakps3packsetting)
            {
                AddListButton(Setting_PakPS3, keyValuePair.Key, pakps3compressname[keyValuePair.Value], (s, e) => ChangePakPS3PackSetting(keyValuePair.Key));
            }
        }

        void AddListButton(StackLayout stack, string itemName, string itemValue, EventHandler OnClicked) => AddListButton(stack, $"{itemName}------{itemValue}", OnClicked);

        void AddListButton(StackLayout stack, string text, EventHandler OnClicked)
        {
            Button b = new Button
            {
                Text = text,
                FontSize = 18,
                BorderWidth = 1,
                Padding = new Thickness(0, 5),
                CornerRadius = 0,
                BackgroundColor = Colors.Transparent
            };
            b.SetAppThemeColor(Button.TextColorProperty, Colors.Black, Colors.White);
            b.SetAppThemeColor(Button.BorderColorProperty, Colors.Black, Colors.White);
            b.Clicked += OnClicked;
            stack.Children.Add(b);
        }

        async void AddPakPS3PackSetting()
		{
			try
			{
				string cancel = MAUIStr.Obj.Setting_Cancel;
				string ok = MAUIStr.Obj.Setting_OK;
				string ans = await DisplayPromptAsync(MAUIStr.Obj.Setting_EnterExtension, MAUIStr.Obj.Setting_EnterExtensionText, ok, cancel);
				if (!string.IsNullOrEmpty(ans))
				{
					string itemname = ans;
					if (!itemname.StartsWith('.'))
					{
						itemname = '.' + itemname;
					}
					string item1 = "Store";
					string item2 = "Zlib";
					ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2);
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
		}

		void ClearPakPS3PackSetting()
		{
			Setting.PakPS3CompressDictionary.Clear();
			InitPakPS3PackSetting();
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		async void ChangePakPS3PackSetting(string itemname)
		{
			try
			{
				string cancel = MAUIStr.Obj.Setting_Cancel;
				string ok = MAUIStr.Obj.Setting_OK;
				string choose1 = MAUIStr.Obj.Setting_CompressItem1;
				string choose2 = MAUIStr.Obj.Setting_CompressItem2;
				string ans;
				if (itemname == "default")
				{
					ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1);
				}
				else
				{
					ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1, choose2);
				}
				if (ans == choose1)
				{
					string item1 = "Store";
					string item2 = "Zlib";
					ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2);
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
		}

		/// <summary>
		/// RsbPTX Setting Begin
		/// </summary>
		void InitRsbPackSetting()
		{
			rsbptx1.IsToggled = Setting.RsbPtxABGR8888Mode;
			rsbptx2.IsToggled = Setting.RsbPtxARGB8888PaddingMode;
		}

		private void Switch_RsbPtx_1_Toggled(object sender, ToggledEventArgs e)
		{
			Setting.RsbPtxABGR8888Mode = e.Value;
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		private void Switch_RsbPtx_2_Toggled(object sender, ToggledEventArgs e)
		{
			Setting.RsbPtxARGB8888PaddingMode = e.Value;
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		/// <summary>
		/// PTX Setting Begin
		/// </summary>
		void InitPTXDecodeSetting()
		{
			ptx.IsToggled = Setting.PtxABGR8888Mode;
			ptx2.IsToggled = Setting.PtxARGB8888PaddingMode;
		}

		private void Switch_Ptx_1_Toggled(object sender, ToggledEventArgs e)
		{
			Setting.PtxABGR8888Mode = e.Value;
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		private void Switch_Ptx_2_Toggled(object sender, ToggledEventArgs e)
		{
			Setting.PtxARGB8888PaddingMode = e.Value;
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		/// <summary>
		/// Cdat Setting Begin
		/// </summary>
		void InitCdatKeySetting()
		{
			cdat.Text = Setting.CdatKey;
		}

		private void Button_Cdat_Click(object sender, EventArgs e)
		{
			Setting.CdatKey = cdat.Text;
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		/// <summary>
		/// RTON Setting Begin
		/// </summary>
		void InitRTONKeySetting()
		{
			rton.Text = Setting.RTONKey;
		}

		private void Button_RTON_Click(object sender, EventArgs e)
		{
			Setting.RTONKey = rton.Text;
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		/// <summary>
		/// ImageString Setting Begin
		/// </summary>
		void InitImageStringSetting()
		{
			imagestring.Text = Setting.ImageConvertName;
		}

		private async void Button_ImageString_1_Clicked(object sender, EventArgs e)
		{
            try
            {
                string result = await DisplayActionSheet(MAUIStr.Obj.Setting_CompiledLoadMethod, MAUIStr.Obj.Setting_Cancel, MAUIStr.Obj.Setting_OK, MAUIStr.Obj.Setting_CompiledLoadFromProgram, MAUIStr.Obj.Setting_CompiledLoadFromFile);
                if (result == MAUIStr.Obj.Setting_CompiledLoadFromProgram)
                {
                    string[] l = YFFileListStream.GetStringList();
                    if (l == null)
                    {
                        await DisplayAlert(MAUIStr.Obj.Setting_CompiledLoadError_Title, MAUIStr.Obj.Setting_CompiledLoadError_Text_NoFile, MAUIStr.Obj.Setting_OK);
                    }
                    else
                    {
                        string cancel = MAUIStr.Obj.Setting_Cancel;
                        string ok = MAUIStr.Obj.Setting_OK;
                        result = await DisplayActionSheet(MAUIStr.Obj.Setting_CompiledLoadFileName, cancel, ok, l);
                        if (result != null && result != cancel && result != ok)
                        {
                            Setting.ClearImageConvertXml();
                            Setting.LoadImageConvertXml(YFFileListStream.GetFile(result));
                            InitImageStringSetting();
                            Setting.SaveAsXml(Permission.GetSettingPath());
                        }
                    }
                }
                else if (result == MAUIStr.Obj.Setting_CompiledLoadFromFile)
                {
                    string path = await this.ChooseOpenFile();
                    if (string.IsNullOrEmpty(path)) return;
                    Setting.ClearImageConvertXml();
                    Setting.LoadImageConvertXml(path);
                    InitImageStringSetting();
                    Setting.SaveAsXml(Permission.GetSettingPath());
                }
            }
            catch (Exception)
            {
                await DisplayAlert(MAUIStr.Obj.Setting_CompiledLoadError_Title, MAUIStr.Obj.Setting_CompiledLoadError_Text, MAUIStr.Obj.Setting_OK);
                Setting.ClearImageConvertXml();
                InitImageStringSetting();
                Setting.SaveAsXml(Permission.GetSettingPath());
            }
        }

		private void Button_ImageString_2_Clicked(object sender, EventArgs e)
		{
			Setting.ClearImageConvertXml();
			InitImageStringSetting();
			Setting.SaveAsXml(Permission.GetSettingPath());
		}

		/// <summary>
		/// Xfl Setting Begin
		/// </summary>
		void InitXflSetting()
		{
			xflwidth.Text = Setting.ReanimXflWidth.ToString();
			xflheight.Text = Setting.ReanimXflHeight.ToString();
            xfllabelname.ItemsSource = new List<string>
            {
                "Image Name",
                "Short Name",
                "Label Name"
            };
            xfllabelname.SelectedIndex = Setting.ReanimXflLabelName + 1;
            xflscalex.Text = Setting.ReanimXflScaleX.ToString();
			xflscaley.Text = Setting.ReanimXflScaleY.ToString();
		}

		private async void Entry_XflSize_Width_TextChanged(object sender, EventArgs e)
		{
            if (!Setting.CanSave) return;
			try
			{
				Setting.ReanimXflWidth = Convert.ToSingle(xflwidth.Text);
				Setting.SaveAsXml(Permission.GetSettingPath());
			}
			catch (Exception)
			{
                await DisplayAlert(MAUIStr.Obj.Setting_InvalidData_Title, MAUIStr.Obj.Setting_InvalidData_Text, MAUIStr.Obj.Setting_OK);
            }
		}

		private async void Entry_XflSize_Height_TextChanged(object sender, EventArgs e)
		{
            if (!Setting.CanSave) return;
			try
			{
				Setting.ReanimXflHeight = Convert.ToSingle(xflheight.Text);
				Setting.SaveAsXml(Permission.GetSettingPath());
			}
			catch (Exception)
			{
                await DisplayAlert(MAUIStr.Obj.Setting_InvalidData_Title, MAUIStr.Obj.Setting_InvalidData_Text, MAUIStr.Obj.Setting_OK);
            }
		}

		private void xfllabel_TextChanged(object sender, EventArgs e)
		{
            if (!Setting.CanSave) return;
            int n = xfllabelname.SelectedIndex - 1;
            try
			{
				Setting.ReanimXflLabelName = n;
				Setting.SaveAsXml(Permission.GetSettingPath());
			}
			catch (Exception)
			{
			}
		}

		private async void xflscalex_TextChanged(object sender, EventArgs e)
		{
            if (!Setting.CanSave) return;
            try
			{
				Setting.ReanimXflScaleX = Convert.ToDouble(xflscalex.Text);
				Setting.SaveAsXml(Permission.GetSettingPath());
			}
			catch (Exception)
			{
                await DisplayAlert(MAUIStr.Obj.Setting_InvalidData_Title, MAUIStr.Obj.Setting_InvalidData_Text, MAUIStr.Obj.Setting_OK);
            }
		}

		private async void xflscaley_TextChanged(object sender, EventArgs e)
		{
            if (!Setting.CanSave) return;
			try
			{
				Setting.ReanimXflScaleY = Convert.ToDouble(xflscaley.Text);
				Setting.SaveAsXml(Permission.GetSettingPath());
			}
			catch (Exception)
			{
                await DisplayAlert(MAUIStr.Obj.Setting_InvalidData_Title, MAUIStr.Obj.Setting_InvalidData_Text, MAUIStr.Obj.Setting_OK);
            }
		}

		/// <summary>
		/// Reset All(Without language)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Button_ResetSetting_Clicked(object sender, EventArgs e)
		{
			string cancel = MAUIStr.Obj.Setting_Cancel;
			string ok = MAUIStr.Obj.Setting_OK;
			bool sure = await DisplayAlert(MAUIStr.Obj.Setting_SureRecover, MAUIStr.Obj.Setting_SureRecoverText, ok, cancel);
			if (sure)
			{
				Setting.ResetXml(Permission.GetSettingPath());
                Setting.LoadFromXml(Permission.GetSettingPath());
                LoadSetting();
                await DisplayAlert(MAUIStr.Obj.Setting_FinishRecover, MAUIStr.Obj.Setting_FinishRecoverText, MAUIStr.Obj.Setting_OK);
			}
		}

		async void ChangeLanguageSetting()
		{
			try
			{
				string cancel = MAUIStr.Obj.Setting_Cancel;
				string ok = MAUIStr.Obj.Setting_OK;
				string item1 = "\u7B80\u4F53\u4E2D\u6587";
				string item2 = "English";
				string ans = await DisplayActionSheet(MAUIStr.Obj.Setting_ChooseLanguage, cancel, ok, item1, item2);
				if (ans != cancel && ans != ok && Setting.LanguageEnum[ans] != Setting.AppLanguage)
				{
					Setting.AppLanguage = Setting.LanguageEnum[ans];
					Setting.SaveAsXml(Permission.GetSettingPath());
                    MAUIStr.LoadLanguage();
                    await DisplayAlert(MAUIStr.Obj.Setting_FinishChooseLanguage, MAUIStr.Obj.Setting_FinishChooseLanguageText, MAUIStr.Obj.Setting_OK);
				}
			}
			catch (Exception)
			{

			}
		}

		private void button_chooselanguage_Clicked(object sender, EventArgs e)
		{
			ChangeLanguageSetting();
		}
	}

	public class ListInfo
	{
		public string ItemName { get; set; }
		public string ItemValue { get; set; }

		public ListInfo()
		{

		}

		public ListInfo(string itemName, string itemValue)
		{
			ItemName = itemName;
			ItemValue = itemValue;
		}
	}
}