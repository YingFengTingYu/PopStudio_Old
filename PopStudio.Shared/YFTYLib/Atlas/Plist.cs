using System.Xml;

namespace PopStudio.Atlas
{
    internal class Plist
    {
        public static bool Splice(string inFolder, string outFile, string infoPath, string _, int width, int height)
        {
            Dictionary<string, SubImageInfo> cutinfo = CutImage.Splice(inFolder, outFile, width, height);
            string xmldata;
            using (StreamReader sr = new StreamReader(infoPath))
            {
                xmldata = sr.ReadToEnd();
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmldata);
            XmlNode root = xml.SelectSingleNode("/plist");
            XmlNodeList ansNodes = root.ChildNodes[0].ChildNodes[1].ChildNodes;
            if (ansNodes.Count == 0) return false;
            for (int i = 0; i < ansNodes.Count; i += 2)
            {
                string ID = Path.GetFileNameWithoutExtension(Dir.FormatPath(ansNodes[i].InnerText)).ToLower();
                SubImageInfo temp = cutinfo[ID];
                var tnode = ansNodes[i + 1].ChildNodes;
                for (int j = 0; j < tnode.Count; j += 2)
                {
                    switch (tnode[j].InnerText)
                    {
                        case "spriteOffset":
                            tnode[j + 1].InnerText = "{0,0}";
                            break;
                        case "spriteSize":
                        case "spriteSourceSize":
                            tnode[j + 1].InnerText = $"{{{temp.Width},{temp.Height}}}";
                            break;
                        case "textureRect":
                            tnode[j + 1].InnerText = $"{{{{{temp.X},{temp.Y}}},{{{temp.Width},{temp.Height}}}}}";
                            break;
                        case "textureRotated":
                            ansNodes[i + 1].ReplaceChild(xml.CreateElement("false"), tnode[j + 1]);
                            break;
                    }
                }
            }
            string t;
            using (Stream s = new MemoryStream())
            {
                xml.Save(s);
                s.Position = 0;
                using (StreamReader sr = new StreamReader(s))
                {
                    t = sr.ReadToEnd().Replace(@"<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd""[]>", @"<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">");
                }
            }
            using (StreamWriter sw = new StreamWriter(infoPath, false))
            {
                sw.Write(t);
            }
            return true;
        }

        public static bool Cut(string inFile, string outFolder, string infoPath, string _)
        {
            string xmldata;
            using (StreamReader sr = new StreamReader(infoPath))
            {
                xmldata = sr.ReadToEnd();
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmldata);
            XmlNode root = xml.SelectSingleNode("/plist");
            XmlNodeList ansNodes = root.ChildNodes[0].ChildNodes[1].ChildNodes;
            List<SubImageInfo> cutinfo = new List<SubImageInfo>();
            SubImageInfo temp = new SubImageInfo();
            for (int i = 0; i < ansNodes.Count; i += 2)
            {
                temp.X = 0;
                temp.Y = 0;
                temp.Width = 0;
                temp.Height = 0;
                temp.rotate270 = false;
                temp.ID = Path.GetFileNameWithoutExtension(Dir.FormatPath(ansNodes[i].InnerText));
                var tnode = ansNodes[i + 1].ChildNodes;
                for (int j = 0; j < tnode.Count; j += 2)
                {
                    if (tnode[j].InnerText == "textureRect")
                    {
                        var strall = tnode[j + 1].InnerText.Replace("},{", "雨").Split('雨');
                        var str1 = strall[0].Replace("{", "").Split(',');
                        temp.X = Convert.ToInt32(str1[0]);
                        temp.Y = Convert.ToInt32(str1[1]);
                        var str2 = strall[1].Replace("}", "").Split(',');
                        temp.Width = Convert.ToInt32(str2[0]);
                        temp.Height = Convert.ToInt32(str2[1]);
                    }
                    else if (tnode[j].InnerText == "textureRotated")
                    {
                        if (tnode[j + 1].Name == "true")
                        {
                            temp.rotate270 = true;
                        }
                    }
                }
                cutinfo.Add(temp);
            }
            if (cutinfo.Count == 0) return false;
            CutImage.Cut(inFile, outFolder, cutinfo);
            return true;
        }
    }
}
