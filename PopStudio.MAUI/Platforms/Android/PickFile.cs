namespace PopStudio.MAUI
{
    internal static partial class PickFile
    {
		public static partial async Task<string> ChooseOpenFile(this ContentPage page)
		{
			string file = "/sdcard";
			string createnew = "新建文件夹\0";
			string back = "↩️返回上级目录\0";
			string ok = "确定\0";
			string cancel = "取消\0";
			while (true)
			{
				try
				{
					string[] rawary = Directory.GetDirectories(file);
					string[] rawary2 = Directory.GetFiles(file);
					Array.Sort(rawary);
					Array.Sort(rawary2);
					string[] showary;
					int ary1 = rawary.Length;
					int ary2 = rawary2.Length;
					if (file.Length <= 7)
					{
						showary = new string[ary1 + ary2];
						for (int i = 0; i < ary1; i++)
						{
							showary[i] = "📁" + Path.GetFileName(rawary[i]);
						}
						for (int i = 0; i < ary2; i++)
						{
							showary[i + ary1] = "📄" + Path.GetFileName(rawary2[i]);
						}
					}
					else
					{
						showary = new string[ary1 + rawary2.Length + 1];
						showary[0] = back;
						for (int i = 0; i < ary1; i++)
						{
							showary[i + 1] = "📁" + Path.GetFileName(rawary[i]);
						}
						for (int i = 0; i < ary2; i++)
						{
							showary[i + ary1 + 1] = "📄" + Path.GetFileName(rawary2[i]);
						}
					}
					string ans = await page.DisplayActionSheet(file + Const.PATHSEPARATOR, cancel, createnew, showary);
					if (string.IsNullOrEmpty(ans) || ans == cancel)
					{
						return null;
					}
					else if (ans == createnew)
					{
						string newname = await page.DisplayPromptAsync("新建文件夹", "请输入文件夹名", ok, cancel, initialValue: "新建文件夹");
						if (!string.IsNullOrEmpty(newname))
						{
							try
							{
								Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
								file += Const.PATHSEPARATOR + newname;
							}
							catch (Exception)
							{
								await page.DisplayAlert("创建错误", "新建文件夹失败", ok, cancel);
							}
						}
					}
					else if (ans == back)
					{
						if (file.Length > 7) file = Path.GetDirectoryName(file);
					}
					else
					{
						file += Const.PATHSEPARATOR + ans[2..]; //The length of 📁 or 📄 is 2
						if (File.Exists(file))
						{
							return file;
						}
					}
				}
				catch (UnauthorizedAccessException)
				{
					await page.DisplayAlert("无权限", "进入文件夹失败，程序无访问权限！", ok, cancel);
					return null;
				}
			}
		}

		public static partial async Task<string> ChooseSaveFile(this ContentPage page)
		{
			string file = "/sdcard";
			string createnew = "新建文件夹\0";
			string back = "↩️返回上级目录\0";
			string ok = "确定\0";
			string choosenow = "保存到此目录\0";
			string cancel = "取消\0";
			while (true)
			{
				try
				{
					string[] rawary = Directory.GetDirectories(file);
					string[] rawary2 = Directory.GetFiles(file);
					Array.Sort(rawary);
					Array.Sort(rawary2);
					string[] showary;
					int ary1 = rawary.Length;
					int ary2 = rawary2.Length;
					if (file.Length <= 7)
					{
						showary = new string[ary1 + ary2];
						for (int i = 0; i < ary1; i++)
						{
							showary[i] = "📁" + Path.GetFileName(rawary[i]);
						}
						for (int i = 0; i < ary2; i++)
						{
							showary[i + ary1] = "📄" + Path.GetFileName(rawary2[i]);
						}
					}
					else
					{
						showary = new string[ary1 + rawary2.Length + 1];
						showary[0] = back;
						for (int i = 0; i < ary1; i++)
						{
							showary[i + 1] = "📁" + Path.GetFileName(rawary[i]);
						}
						for (int i = 0; i < ary2; i++)
						{
							showary[i + ary1 + 1] = "📄" + Path.GetFileName(rawary2[i]);
						}
					}
					string ans = await page.DisplayActionSheet(file + Const.PATHSEPARATOR, choosenow, createnew, showary);
					if (string.IsNullOrEmpty(ans))
					{
						return null;
					}
					else if (ans == choosenow)
					{
						string val = await page.DisplayPromptAsync("保存文件", "请输入文件名", ok, cancel);
						if (string.IsNullOrEmpty(val)) return null;
						return file + Const.PATHSEPARATOR + val;
					}
					else if (ans == createnew)
					{
						string newname = await page.DisplayPromptAsync("新建文件夹", "请输入文件夹名", ok, cancel, initialValue: "新建文件夹");
						if (!string.IsNullOrEmpty(newname))
						{
							try
							{
								Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
								file += Const.PATHSEPARATOR + newname;
							}
							catch (Exception)
							{
								await page.DisplayAlert("创建错误", "新建文件夹失败", ok, cancel);
							}
						}
					}
					else if (ans == back)
					{
						if (file.Length > 7) file = Path.GetDirectoryName(file);
					}
					else
					{
						file += Const.PATHSEPARATOR + ans[2..];
						if (File.Exists(file))
						{
							return file;
						}
					}
				}
				catch (UnauthorizedAccessException)
				{
					await page.DisplayAlert("无权限", "进入文件夹失败，程序无访问权限！", ok, cancel);
					return null;
				}
			}
		}

		public static partial async Task<string> ChooseFolder(this ContentPage page)
		{
			string file = "/sdcard";
			string createnew = "新建文件夹\0";
			string back = "↩️返回上级目录\0";
			string ok = "确定\0";
			string choosenow = "选择当前文件夹\0";
			string cancel = "取消\0";
			while (true)
			{
				try
				{
					string[] rawary = Directory.GetDirectories(file);
					Array.Sort(rawary);
					string[] showary;
					if (file.Length <= 7)
					{
						showary = new string[rawary.Length];
						for (int i = 0; i < rawary.Length; i++)
						{
							showary[i] = "📁" + Path.GetFileName(rawary[i]);
						}
					}
					else
					{
						showary = new string[rawary.Length + 1];
						showary[0] = back;
						for (int i = 0; i < rawary.Length; i++)
						{
							showary[i + 1] = "📁" + Path.GetFileName(rawary[i]);
						}
					}
					string ans = await page.DisplayActionSheet(file + Const.PATHSEPARATOR, choosenow, createnew, showary);
					if (string.IsNullOrEmpty(ans))
					{
						return null;
					}
					else if (ans == choosenow)
					{
						return file;
					}
					else if (ans == createnew)
					{
						string newname = await page.DisplayPromptAsync("新建文件夹", "请输入文件夹名", ok, cancel, initialValue: "新建文件夹");
						if (!string.IsNullOrEmpty(newname))
						{
							try
							{
								Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
								file += Const.PATHSEPARATOR + newname;
							}
							catch (Exception)
							{
								await page.DisplayAlert("创建错误", "新建文件夹失败", ok, cancel);
							}
						}
					}
					else if (ans == back)
					{
						if (file.Length > 7) file = Path.GetDirectoryName(file);
					}
					else
					{
						file += Const.PATHSEPARATOR + ans[2..]; //The length of 📁 is 2
					}
				}
				catch (UnauthorizedAccessException)
				{
					await page.DisplayAlert("无权限", "进入文件夹失败，程序无访问权限！", ok, cancel);
					return null;
				}
			}
		}
	}
}
