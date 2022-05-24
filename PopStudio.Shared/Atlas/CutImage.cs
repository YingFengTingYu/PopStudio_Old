using PopStudio.Platform;

namespace PopStudio.Atlas
{
    internal class CutImage
    {
        public static Dictionary<string, SubImageInfo> Splice(string inFolder, string outFile, int width, int height)
        {
            using (IDisposablePool pool = new IDisposablePool())
            {
                //Load All Images
                string[] all = Directory.GetFiles(inFolder);
                List<SubImageInfo> lst = new List<SubImageInfo>();
                foreach (string s in all)
                {
                    if (Path.GetExtension(s).ToLower() == ".png")
                    {
                        Bitmap img = pool.Add(Bitmap.Create(s));
                        if (img != null) lst.Add(new SubImageInfo(img, Path.GetFileNameWithoutExtension(s).ToLower()));
                    }
                }
                lst.Sort((SubImageInfo b, SubImageInfo a) => a.Width * a.Height - b.Width * b.Height);
                MaxRectsBinPack maxRectsBinPack = new MaxRectsBinPack(width, height, false);
                int c = lst.Count;
                Dictionary<string, SubImageInfo> ans = new Dictionary<string, SubImageInfo>();
                using (Bitmap bitmap = Bitmap.Create(width, height))
                {
                    for (int i = 0; i < c; i++)
                    {
                        SubImageInfo temp = lst[i];
                        temp.ResetCoordinate(maxRectsBinPack.Insert(temp, MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestAreaFit));
                        temp.Image.MoveTo(bitmap, temp.X, temp.Y);
                        temp.Image = null; //Will auto-dispose soon by IDisposablePool
                        ans.Add(temp.ID, temp);
                    }
                    bitmap.Save(outFile);
                }
                return ans;
            }
        }

        public static void Cut(string inFile, string outFolder, List<SubImageInfo> cutinfo)
        {
            Dir.NewDir(outFolder);
            outFolder = outFolder + Const.PATHSEPARATOR;
            using (Bitmap bitmap = Bitmap.Create(inFile))
            {
                foreach (SubImageInfo info in cutinfo)
                {

                    string p = outFolder + info.ID.ToLower() + ".png";
                    if (info.rotate270)
                    {
                        using (Bitmap map = bitmap.Cut(info.X, info.Y, info.Height, info.Width))
                        {
                            using (Bitmap mp2 = map.Rotate270())
                            {
                                mp2.Save(p);
                            }
                        }
                    }
                    else
                    {
                        using (Bitmap map = bitmap.Cut(info.X, info.Y, info.Width, info.Height))
                        {
                            map.Save(p);
                        }
                    }
                }
            }
        }
    }
}
