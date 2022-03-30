using System.Xml;

namespace PopStudio.Atlas
{
    internal class NewXml
    {
        public static bool Splice(string inFolder, string outFile, string infoPath, string itemName, int width, int height)
        {
            Dictionary<string, SubImageInfo> cutinfo = CutImage.Splice(inFolder, outFile, width, height);
            string xmldata;
            using (StreamReader sr = new StreamReader(infoPath))
            {
                xmldata = sr.ReadToEnd();
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmldata);
            XmlNode root = xml.SelectSingleNode("/ResourceManifest");
            XmlNodeList childlist = root.ChildNodes;
            XmlNodeList childchildlist;
            XmlNodeList childchildchildlist;
            XmlNode ansNode = null;
            XmlNode ansNode2 = null;
            //Get the itemName
            if (string.IsNullOrEmpty(itemName) && !File.Exists(inFolder + Const.PATHSEPARATOR + "AtlasID.txt"))
            {
                string PP = Path.GetFileNameWithoutExtension(outFile).ToLower(); //Delete the extension name
                foreach (XmlNode child in childlist)
                {
                    childchildlist = child.ChildNodes;
                    foreach (XmlNode cdccc in childchildlist)
                    {
                        childchildchildlist = cdccc.ChildNodes;
                        foreach (XmlNode cd in childchildchildlist)
                        {
                            if (cd.Attributes["type"].Value == "0" && cd.Attributes["imagetype"].Value == "2" && PP == Path.GetFileNameWithoutExtension(Dir.FormatPath(cd.Attributes["path"].Value)).ToLower())
                            {
                                itemName = cd.Attributes["id"].Value;
                                ansNode = cdccc;
                                ansNode2 = cd;
                                break;
                            }
                        }
                        if (ansNode != null) break;
                    }
                    if (ansNode != null) break;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(itemName))
                {
                    itemName = File.ReadAllText(inFolder + Const.PATHSEPARATOR + "AtlasID.txt").Replace("\r", "").Replace("\n", "");
                }
                foreach (XmlNode child in childlist)
                {
                    childchildlist = child.ChildNodes;
                    foreach (XmlNode cdccc in childchildlist)
                    {
                        childchildchildlist = cdccc.ChildNodes;
                        foreach (XmlNode cd in childchildchildlist)
                        {
                            if (cd.Attributes["type"].Value == "0" && cd.Attributes["imagetype"].Value == "2" && cd.Attributes["id"].Value == itemName)
                            {
                                ansNode = cdccc;
                                ansNode2 = cd;
                                break;
                            }
                        }
                        if (ansNode != null) break;
                    }
                    if (ansNode != null) break;
                }
            }
            if (ansNode == null)
            {
                return false;
            }
            ansNode2.Attributes["aw"].Value = width.ToString();
            ansNode2.Attributes["ah"].Value = height.ToString();
            itemName = itemName.Split('|')[0];
            childlist = ansNode.ChildNodes;
            foreach (XmlElement cd in childlist)
            {
                if (cd.GetAttribute("type") == "0" && cd.GetAttribute("imagetype") == "4" && cd.GetAttribute("parent") == itemName)
                {
                    SubImageInfo temp = cutinfo[cd.GetAttribute("id").ToLower().Split('|')[0]];
                    if (temp.X == 0 && temp.Y == 0)
                    {
                        if (cd.HasAttribute("ax"))
                        {
                            cd.RemoveAttribute("ax");
                        }
                        if (cd.HasAttribute("ay"))
                        {
                            cd.RemoveAttribute("ay");
                        }
                    }
                    else if (temp.X == 0)
                    {
                        if (cd.HasAttribute("ax"))
                        {
                            cd.RemoveAttribute("ax");
                        }
                        cd.SetAttribute("ay", temp.Y.ToString());
                    }
                    else if (temp.Y == 0)
                    {
                        cd.SetAttribute("ax", temp.X.ToString());
                        if (cd.HasAttribute("ay"))
                        {
                            cd.RemoveAttribute("ay");
                        }
                    }
                    else
                    {
                        cd.SetAttribute("ax", temp.X.ToString());
                        cd.SetAttribute("ay", temp.Y.ToString());
                    }
                    cd.SetAttribute("aw", temp.Width.ToString());
                    cd.SetAttribute("ah", temp.Height.ToString());
                }
            }
            File.Delete(infoPath);
            xml.Save(infoPath);
            return true;
        }

        public static bool Cut(string inFile, string outFolder, string infoPath, string itemName)
        {
            string xmldata;
            using (StreamReader sr = new StreamReader(infoPath))
            {
                xmldata = sr.ReadToEnd();
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmldata);
            XmlNode root = xml.SelectSingleNode("/ResourceManifest");
            XmlNodeList childlist = root.ChildNodes;
            XmlNodeList childchildlist;
            XmlNodeList childchildchildlist;
            XmlNode ansNode = null;
            //Get the itemName
            if (string.IsNullOrEmpty(itemName))
            {
                string PP = Path.GetFileNameWithoutExtension(inFile).ToLower(); //Delete the extension name
                foreach (XmlNode child in childlist)
                {
                    childchildlist = child.ChildNodes;
                    foreach (XmlNode cdccc in childchildlist)
                    {
                        childchildchildlist = cdccc.ChildNodes;
                        foreach (XmlNode cd in childchildchildlist)
                        {
                            if (cd.Attributes["type"].Value == "0" && cd.Attributes["imagetype"].Value == "2" && PP == Path.GetFileNameWithoutExtension(Dir.FormatPath(cd.Attributes["path"].Value)).ToLower())
                            {
                                itemName = cd.Attributes["id"].Value;
                                ansNode = cdccc;
                                break;
                            }
                        }
                        if (ansNode != null) break;
                    }
                    if (ansNode != null) break;
                }
            }
            else
            {
                foreach (XmlNode child in childlist)
                {
                    childchildlist = child.ChildNodes;
                    foreach (XmlNode cdccc in childchildlist)
                    {
                        childchildchildlist = cdccc.ChildNodes;
                        foreach (XmlNode cd in childchildchildlist)
                        {
                            if (cd.Attributes["type"].Value == "0" && cd.Attributes["imagetype"].Value == "2" && cd.Attributes["id"].Value == itemName)
                            {
                                ansNode = cdccc;
                                break;
                            }
                        }
                        if (ansNode != null) break;
                    }
                    if (ansNode != null) break;
                }
            }
            if (ansNode == null)
            {
                return false;
            }
            string ori = itemName;
            List<SubImageInfo> cutinfo = new List<SubImageInfo>();
            itemName = itemName.Split('|')[0];
            foreach (XmlNode cd in ansNode)
            {
                if (cd.Attributes["type"].Value == "0" && cd.Attributes["imagetype"].Value == "4" && cd.Attributes["parent"].Value == itemName)
                {
                    cutinfo.Add(new SubImageInfo(cd.Attributes["ax"]?.Value ?? "0", cd.Attributes["ay"]?.Value ?? "0", cd.Attributes["aw"]?.Value ?? "0", cd.Attributes["ah"]?.Value ?? "0", cd.Attributes["id"].Value.Split('|')[0]));
                }
            }
            if (cutinfo.Count == 0) return false;
            CutImage.Cut(inFile, outFolder, cutinfo);
            File.WriteAllText(outFolder + Const.PATHSEPARATOR + "AtlasID.txt", ori);
            return true;
        }
    }
}
