using System.Xml;

namespace PopStudio.Atlas
{
    internal class OldXml
    {
        public static bool Splice(string inFolder, string outFile, string infoPath, string itemName, int width, int height)
        {
            Dictionary<string, SubImageInfo> cutinfo = CutImage.Splice(inFolder, outFile, width, height);
            if (string.IsNullOrEmpty(itemName) && File.Exists(inFolder + Const.PATHSEPARATOR + "AtlasID.txt"))
            {
                itemName = File.ReadAllText(inFolder + Const.PATHSEPARATOR + "AtlasID.txt").Replace("\r", "").Replace("\n", "");
            }
            string same1 = Path.GetFileNameWithoutExtension(outFile).ToLower();
            Func<XmlNode, bool> IsSame = string.IsNullOrEmpty(itemName) ? (XmlNode x) => Path.GetFileNameWithoutExtension(Dir.FormatPath(x.Attributes["path"].Value)).ToLower() == same1 : (XmlNode x) => x.Attributes["id"].Value == itemName;
            string xmldata;
            using (StreamReader sr = new StreamReader(infoPath))
            {
                xmldata = sr.ReadToEnd();
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmldata);
            //Get the itemName
            XmlNode root = xml.SelectSingleNode("/resources-manifest");
            XmlNodeList childlist = root.ChildNodes;
            XmlNodeList childchildlist;
            XmlNode ansNode = null;
            foreach (XmlNode child in childlist)
            {
                childchildlist = child.ChildNodes;
                foreach (XmlNode cd in childchildlist)
                {
                    if (cd.Name == "atlas" && IsSame(cd))
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
            foreach (XmlElement cd in childlist)
            {
                SubImageInfo temp = cutinfo[cd.GetAttribute("id").ToLower()];
                cd.SetAttribute("x", temp.X.ToString());
                cd.SetAttribute("y", temp.Y.ToString());
                cd.SetAttribute("width", temp.Width.ToString());
                cd.SetAttribute("height", temp.Height.ToString());
            }
            File.Delete(infoPath);
            xml.Save(infoPath);
            return true;
        }

        public static bool Cut(string inFile, string outFolder, string infoPath, string itemName)
        {
            string same1 = Path.GetFileNameWithoutExtension(inFile).ToLower();
            Func<XmlNode, bool> IsSame = string.IsNullOrEmpty(itemName) ? (XmlNode x) => Path.GetFileNameWithoutExtension(Dir.FormatPath(x.Attributes["path"].Value)).ToLower() == same1 : (XmlNode x) => x.Attributes["id"].Value == itemName;
            string xmldata;
            using (StreamReader sr = new StreamReader(infoPath))
            {
                xmldata = sr.ReadToEnd();
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmldata);
            XmlNode root = xml.SelectSingleNode("/resources-manifest");
            XmlNodeList childlist = root.ChildNodes;
            XmlNodeList childchildlist;
            XmlNode ansNode = null;
            foreach (XmlNode child in childlist)
            {
                childchildlist = child.ChildNodes;
                foreach (XmlNode cd in childchildlist)
                {
                    if (cd.Name == "atlas" && IsSame(cd))
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
            foreach (XmlNode cd in ansNode)
            {
                cutinfo.Add(new SubImageInfo(cd.Attributes["x"].Value, cd.Attributes["y"].Value, cd.Attributes["width"].Value, cd.Attributes["height"].Value, cd.Attributes["id"].Value));
            }
            if (cutinfo.Count == 0) return false;
            CutImage.Cut(inFile, outFolder, cutinfo);
            File.WriteAllText(outFolder + Const.PATHSEPARATOR + "AtlasID.txt", itemName);
            return true;
        }
    }
}
