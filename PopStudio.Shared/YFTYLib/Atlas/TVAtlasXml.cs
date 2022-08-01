using System.Xml;

namespace PopStudio.Atlas
{
    internal class TVAtlasXml
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
            //Get the itemName
            XmlNode root = xml.SelectSingleNode("/atlases");
            XmlNode ansNode = root.ChildNodes[0];
            if (ansNode == null) return false;
            foreach (XmlElement cd in ansNode)
            {
                SubImageInfo temp = cutinfo[Path.GetFileNameWithoutExtension(Dir.FormatPath(cd.GetAttribute("name"))).ToLower()];
                cd.SetAttribute("x", temp.X.ToString());
                cd.SetAttribute("y", temp.Y.ToString());
                cd.SetAttribute("w", temp.Width.ToString());
                cd.SetAttribute("h", temp.Height.ToString());
            }
            File.Delete(infoPath);
            xml.Save(infoPath);
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
            XmlNode root = xml.SelectSingleNode("/atlases");
            XmlNode ansNode = root.ChildNodes[0];
            if (ansNode == null) return false;
            List<SubImageInfo> cutinfo = new List<SubImageInfo>();
            foreach (XmlNode cd in ansNode)
            {
                cutinfo.Add(new SubImageInfo(cd.Attributes["x"].Value, cd.Attributes["y"].Value, cd.Attributes["w"].Value, cd.Attributes["h"].Value, Path.GetFileNameWithoutExtension(Dir.FormatPath(cd.Attributes["name"].Value))));
            }
            if (cutinfo.Count == 0) return false;
            CutImage.Cut(inFile, outFolder, cutinfo);
            return true;
        }
    }
}
