using System.Xml;

namespace PopStudio.Atlas
{
    internal class AncientXml
    {
        public static bool Splice(string inFolder, string outFile, string infoPath, string itemName, int width, int height)
        {
            if (string.IsNullOrEmpty(itemName) && File.Exists(inFolder + Const.PATHSEPARATOR + "AtlasID.txt"))
            {
                itemName = File.ReadAllText(inFolder + Const.PATHSEPARATOR + "AtlasID.txt").Replace("\r", "").Replace("\n", "");
            }
            string same1 = Path.GetFileNameWithoutExtension(outFile).ToLower();
            Func<XmlNode, string, bool> IsSame = string.IsNullOrEmpty(itemName) ? (XmlNode x, string _) => Path.GetFileNameWithoutExtension(x.Attributes["path"].Value).ToLower() == same1 : (XmlNode x, string d) => (d + x.Attributes["id"].Value) == itemName;
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
            XmlNode ansNode = null;
            foreach (XmlNode child in childlist)
            {
                string defaultstring = string.Empty;
                childchildlist = child.ChildNodes;
                foreach (XmlNode cd in childchildlist)
                {
                    if (cd.Name == "SetDefaults")
                    {
                        defaultstring = cd.Attributes["idprefix"].Value;
                    }
                    else if (cd.Name == "Atlas" && IsSame(cd, defaultstring))
                    {
                        ansNode = cd;
                        break;
                    }
                }
                if (ansNode != null)
                {
                    break;
                }
            }
            if (ansNode == null)
            {
                return false;
            }
            childlist = ansNode.ChildNodes;
            string defaultstring2 = string.Empty;
            foreach (XmlElement cd in childlist)
            {
                if (cd.Name == "SetDefaults")
                {
                    defaultstring2 = cd.GetAttribute("idprefix");
                }
                else
                {
                    SubImageInfo temp = cutinfo[cd.GetAttribute("id").ToLower()];
                    cd.SetAttribute("x", temp.X.ToString());
                    cd.SetAttribute("y", temp.Y.ToString());
                    cd.SetAttribute("width", temp.Width.ToString());
                    cd.SetAttribute("height", temp.Height.ToString());
                }
            }
            File.Delete(infoPath);
            xml.Save(infoPath);
            return true;
        }

        public static bool Cut(string inFile, string outFolder, string infoPath, string itemName)
        {
            string same1 = Path.GetFileNameWithoutExtension(inFile).ToLower();
            Func<XmlNode, string, bool> IsSame = string.IsNullOrEmpty(itemName) ? (XmlNode x, string _) => Path.GetFileNameWithoutExtension(x.Attributes["path"].Value).ToLower() == same1 : (XmlNode x, string d) => (d + x.Attributes["id"].Value) == itemName;
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
            XmlNode ansNode = null;
            foreach (XmlNode child in childlist)
            {
                string defaultstring = string.Empty;
                childchildlist = child.ChildNodes;
                foreach (XmlNode cd in childchildlist)
                {
                    if (cd.Name == "SetDefaults")
                    {
                        defaultstring = cd.Attributes["idprefix"].Value;
                    }
                    else if (cd.Name == "Atlas" && IsSame(cd, defaultstring))
                    {
                        ansNode = cd;
                        break;
                    }
                }
                if (ansNode != null)
                {
                    break;
                }
            }
            if (ansNode == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(itemName))
            {
                itemName = ansNode.Attributes["id"].Value;
            }
            List<SubImageInfo> cutinfo = new List<SubImageInfo>();
            string defaultstring2 = string.Empty;
            foreach (XmlNode cd in ansNode)
            {
                if (cd.Name == "SetDefaults")
                {
                    defaultstring2 = cd.Attributes["idprefix"].Value;
                }
                else
                {
                    cutinfo.Add(new SubImageInfo(cd.Attributes["x"].Value, cd.Attributes["y"].Value, cd.Attributes["width"].Value, cd.Attributes["height"].Value, defaultstring2 + cd.Attributes["id"].Value));
                }
            }
            if (cutinfo.Count == 0) return false;
            CutImage.Cut(inFile, outFolder, cutinfo);
            File.WriteAllText(outFolder + Const.PATHSEPARATOR + "AtlasID.txt", itemName);
            return true;
        }
    }
}
