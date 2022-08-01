using System.Xml;

namespace PopStudio.Reanim
{
    internal static class RawXml
    {
        public static void Encode(Reanim reanim, string outFile)
        {
            using (StreamWriter sw = new StreamWriter(outFile, false))
            {
                if (reanim.doScale != null)
                {
                    sw.WriteLine("<doScale>" + reanim.doScale + "</doScale>");
                }
                sw.WriteLine("<fps>" + reanim.fps + "</fps>");
                int trackNumber = reanim.tracks.Length;
                for (int i = 0; i < trackNumber; i++)
                {
                    ReanimTrack temptrack = reanim.tracks[i];
                    sw.WriteLine("<track>");
                    if (temptrack.name != null)
                    {
                        sw.WriteLine("<name>" + temptrack.name + "</name>");
                    }
                    int transformsNumber = temptrack.transforms.Length;
                    for (int j = 0; j < transformsNumber; j++)
                    {
                        sw.Write("<t>");
                        var k = temptrack.transforms[j];
                        if (k.x != null)
                        {
                            sw.Write("<x>");
                            sw.Write(k.x);
                            sw.Write("</x>");
                        }
                        if (k.y != null)
                        {
                            sw.Write("<y>");
                            sw.Write(k.y);
                            sw.Write("</y>");
                        }
                        if (k.kx != null)
                        {
                            sw.Write("<kx>");
                            sw.Write(k.kx);
                            sw.Write("</kx>");
                        }
                        if (k.ky != null)
                        {
                            sw.Write("<ky>");
                            sw.Write(k.ky);
                            sw.Write("</ky>");
                        }
                        if (k.sx != null)
                        {
                            sw.Write("<sx>");
                            sw.Write(k.sx);
                            sw.Write("</sx>");
                        }
                        if (k.sy != null)
                        {
                            sw.Write("<sy>");
                            sw.Write(k.sy);
                            sw.Write("</sy>");
                        }
                        if (k.f != null)
                        {
                            sw.Write("<f>");
                            sw.Write(k.f);
                            sw.Write("</f>");
                        }
                        if (k.a != null)
                        {
                            sw.Write("<a>");
                            sw.Write(k.a);
                            sw.Write("</a>");
                        }
                        if (k.i != null)
                        {
                            sw.Write("<i>");
                            sw.Write(k.i);
                            sw.Write("</i>");
                        }
                        if (k.iPath != null)
                        {
                            sw.Write("<resource>");
                            sw.Write(k.iPath);
                            sw.Write("</resource>");
                        }
                        if (k.i2 != null)
                        {
                            sw.Write("<i2>");
                            sw.Write(k.i2);
                            sw.Write("</i2>");
                        }
                        if (k.i2Path != null)
                        {
                            sw.Write("<resource2>");
                            sw.Write(k.i2Path);
                            sw.Write("</resource2>");
                        }
                        if (k.font != null)
                        {
                            sw.Write("<font>");
                            sw.Write(k.font);
                            sw.Write("</font>");
                        }
                        if (k.text != null)
                        {
                            sw.Write("<text>");
                            sw.Write(k.text);
                            sw.Write("</text>");
                        }
                        sw.WriteLine("</t>");
                    }
                    sw.WriteLine("</track>");
                }
            }
        }

        public static Reanim Decode(string inFile)
        {
            Reanim reanimInfo = new Reanim();
            string xmldata;
            using (StreamReader sr = new StreamReader(inFile))
            {
                xmldata = ("<?xml version=\"1.0\" encoding=\"utf-8\"?><root>" + sr.ReadToEnd().Replace("&", "&amp;") + "</root>");
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmldata);
            XmlNode root = xml.SelectSingleNode("/root");
            XmlNodeList childlist = root.ChildNodes;
            if (childlist[0].Name == "doScale")
            {
                reanimInfo.doScale = Convert.ToSByte(childlist[0].InnerText);
                root.RemoveChild(childlist[0]);
            }
            if (childlist[0].Name == "fps")
            {
                reanimInfo.fps = Convert.ToSingle(childlist[0].InnerText);
                root.RemoveChild(childlist[0]);
            }
            int trackNumber = childlist.Count;
            reanimInfo.tracks = new ReanimTrack[trackNumber];
            for (int i = 0; i < trackNumber; i++)
            {
                if (childlist[i].Name != "track")
                {
                    throw new Exception(Str.Obj.DataMisMatch);
                }
                XmlNodeList childchildlist = childlist[i].ChildNodes;
                ReanimTrack temptrack = new ReanimTrack();
                if (childchildlist[0].Name == "name")
                {
                    temptrack.name = childchildlist[0].InnerText;
                    childlist[i].RemoveChild(childchildlist[0]);
                }
                int tNumber = childchildlist.Count;
                temptrack.transforms = new ReanimTransform[tNumber];
                for (int j = 0; j < tNumber; j++)
                {
                    ReanimTransform k = new ReanimTransform();
                    foreach (XmlNode node in childchildlist[j].ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "x":
                                k.x = Convert.ToSingle(node.InnerText);
                                break;
                            case "y":
                                k.y = Convert.ToSingle(node.InnerText);
                                break;
                            case "kx":
                                k.kx = Convert.ToSingle(node.InnerText);
                                break;
                            case "ky":
                                k.ky = Convert.ToSingle(node.InnerText);
                                break;
                            case "sx":
                                k.sx = Convert.ToSingle(node.InnerText);
                                break;
                            case "sy":
                                k.sy = Convert.ToSingle(node.InnerText);
                                break;
                            case "f":
                                k.f = Convert.ToSingle(node.InnerText);
                                break;
                            case "a":
                                k.a = Convert.ToSingle(node.InnerText);
                                break;
                            case "i":
                                k.i = node.InnerText;
                                break;
                            case "resource":
                                k.iPath = node.InnerText;
                                break;
                            case "i2":
                                k.i2 = node.InnerText;
                                break;
                            case "resource2":
                                k.i2Path = node.InnerText;
                                break;
                            case "font":
                                k.font = node.InnerText;
                                break;
                            case "text":
                                k.text = node.InnerText;
                                break;
                        }
                    }
                    temptrack.transforms[j] = k;
                }
                reanimInfo.tracks[i] = temptrack;
            }
            return reanimInfo;
        }
    }
}