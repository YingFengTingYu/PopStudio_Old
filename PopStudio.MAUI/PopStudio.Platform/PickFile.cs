using PopStudio.Language.Languages;

namespace PopStudio.Platform
{
    internal static class PickFile
    {
		public static async Task<string> ChooseOpenFile(this ContentPage page)
		{
            string file = "/sdcard";
            string createnew = MAUIStr.Obj.PickFile_NewFolder;
            string back = MAUIStr.Obj.PickFile_Back;
            string ok = MAUIStr.Obj.PickFile_OK;
            string cancel = MAUIStr.Obj.PickFile_Cancel;
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
                        string newname = await page.DisplayPromptAsync(createnew, MAUIStr.Obj.PickFile_EnterFolderName, ok, cancel, initialValue: createnew[..5]);
                        if (!string.IsNullOrEmpty(newname))
                        {
                            try
                            {
                                Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
                                file += Const.PATHSEPARATOR + newname;
                            }
                            catch (Exception)
                            {
                                await page.DisplayAlert(MAUIStr.Obj.PickFile_CreateWrong, MAUIStr.Obj.PickFile_CreateWrong, ok, cancel);
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
                    await page.DisplayAlert(MAUIStr.Obj.PickFile_NoPermission, MAUIStr.Obj.PickFile_NoPermissionToEnter, ok, cancel);
                    return null;
                }
            }
        }

		public static async Task<string> ChooseSaveFile(this ContentPage page)
		{
            string file = "/sdcard";
            string createnew = MAUIStr.Obj.PickFile_NewFolder;
            string back = MAUIStr.Obj.PickFile_Back;
            string ok = MAUIStr.Obj.PickFile_OK;
            string choosenow = MAUIStr.Obj.PickFlie_SaveThere;
            string cancel = MAUIStr.Obj.PickFile_Cancel;
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
                        string val = await page.DisplayPromptAsync(MAUIStr.Obj.PickFlie_SaveFile, MAUIStr.Obj.PickFile_EnterFileName, ok, cancel);
                        if (string.IsNullOrEmpty(val)) return null;
                        return file + Const.PATHSEPARATOR + val;
                    }
                    else if (ans == createnew)
                    {
                        string newname = await page.DisplayPromptAsync(createnew, MAUIStr.Obj.PickFile_EnterFolderName, ok, cancel, initialValue: createnew[..5]);
                        if (!string.IsNullOrEmpty(newname))
                        {
                            try
                            {
                                Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
                                file += Const.PATHSEPARATOR + newname;
                            }
                            catch (Exception)
                            {
                                await page.DisplayAlert(MAUIStr.Obj.PickFile_CreateWrong, MAUIStr.Obj.PickFile_CreateWrong, ok, cancel);
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
                    await page.DisplayAlert(MAUIStr.Obj.PickFile_NoPermission, MAUIStr.Obj.PickFile_NoPermissionToEnter, ok, cancel);
                    return null;
                }
            }
        }

		public static async Task<string> ChooseFolder(this ContentPage page)
		{
            string file = "/sdcard";
            string createnew = MAUIStr.Obj.PickFile_NewFolder;
            string back = MAUIStr.Obj.PickFile_Back;
            string ok = MAUIStr.Obj.PickFile_OK;
            string choosenow = MAUIStr.Obj.PickFile_ChooseThisFolder;
            string cancel = MAUIStr.Obj.PickFile_Cancel;
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
                        string newname = await page.DisplayPromptAsync(createnew, MAUIStr.Obj.PickFile_EnterFolderName, ok, cancel, initialValue: createnew[..5]);
                        if (!string.IsNullOrEmpty(newname))
                        {
                            try
                            {
                                Directory.CreateDirectory(file + Const.PATHSEPARATOR + newname);
                                file += Const.PATHSEPARATOR + newname;
                            }
                            catch (Exception)
                            {
                                await page.DisplayAlert(MAUIStr.Obj.PickFile_CreateWrong, MAUIStr.Obj.PickFile_CreateWrong, ok, cancel);
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
                    await page.DisplayAlert(MAUIStr.Obj.PickFile_NoPermission, MAUIStr.Obj.PickFile_NoPermissionToEnter, ok, cancel);
                    return null;
                }
            }
        }
	}
}
