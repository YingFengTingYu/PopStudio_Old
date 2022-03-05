using System.Xml;

namespace PopStudio.Package.Rsb
{
    internal class XmlConvert
    {
        public static void XmlToDat(string inFile, BinaryStream bs)
        {
            string xmldata;
            using (StreamReader sr = new StreamReader(inFile))
            {
                xmldata = sr.ReadToEnd();
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmldata);
            XmlNode root = xml.SelectSingleNode("/ResourceManifest");
            XmlNodeList childlist = root.ChildNodes;
            XmlNodeList childchildlist;
            XmlNodeList childchildchildlist;
            List<XmlCompositeResourcesInfo> xmlInfo = new List<XmlCompositeResourcesInfo>();
            //边读xml边写序列化xml
            using (BinaryStream bsxmlpart1 = new BinaryStream())
            {
                using (BinaryStream bsxmlpart2 = new BinaryStream())
                {
                    using (BinaryStream bsxmlpart3 = new BinaryStream())
                    {
                        bsxmlpart1.Endian = bs.Endian;
                        bsxmlpart2.Endian = bs.Endian;
                        bsxmlpart3.Endian = bs.Endian;
                        Dictionary<string, int> stringPool = new Dictionary<string, int>();
                        int ThrowInPool(string poolKey)
                        {
                            if (!stringPool.ContainsKey(poolKey))
                            {
                                stringPool.Add(poolKey, (int)bsxmlpart3.Position);
                                bsxmlpart3.WriteStringByEmpty(poolKey);
                            }
                            return stringPool[poolKey];
                        }
                        bsxmlpart3.WriteByte(0x0);
                        stringPool.Add("", 0);
                        for (int i = 0; i < childlist.Count; i++)
                        {
                            XmlNode child = childlist[i];//child是CompositeResources
                            childchildlist = child.ChildNodes;
                            xmlInfo.Add(new XmlCompositeResourcesInfo());
                            xmlInfo[i].idOffsetInPart3 = ThrowInPool(child.Attributes["id"].Value);
                            xmlInfo[i].rsgpNumber = childchildlist.Count;
                            xmlInfo[i].rsgpInfoLibrary = new XmlRsgpInfo[childchildlist.Count];
                            for (int j = 0; j < childchildlist.Count; j++)
                            {
                                XmlNode childchild = childchildlist[j];//childchild是Group
                                childchildchildlist = childchild.ChildNodes;
                                XmlRsgpInfo xmlRsgpInfo = new XmlRsgpInfo();
                                xmlInfo[i].rsgpInfoLibrary[j] = xmlRsgpInfo;
                                xmlRsgpInfo.resolutionRatio = Convert.ToInt32(childchild.Attributes["res"].Value);
                                xmlRsgpInfo.language = childchild.Attributes["loc"]?.Value ?? string.Empty;
                                xmlRsgpInfo.idOffsetInPart3 = ThrowInPool(childchild.Attributes["id"]?.Value);
                                xmlRsgpInfo.resourcesNumber = childchildchildlist.Count;
                                xmlRsgpInfo.resourcesInfoLibrary = new XmlResourcesInfo[childchildchildlist.Count];
                                for (int k = 0; k < childchildchildlist.Count; k++)
                                {
                                    XmlNode childchildchild = childchildchildlist[k];//childchildchild是Res
                                    XmlResourcesInfo xmlResourcesInfo = xmlInfo[i].rsgpInfoLibrary[j].resourcesInfoLibrary[k] = new XmlResourcesInfo();
                                    xmlResourcesInfo.type = Convert.ToUInt16(childchildchild.Attributes["type"]?.Value);
                                    xmlResourcesInfo.idOffsetInPart3 = ThrowInPool(childchildchild.Attributes["id"]?.Value);
                                    xmlResourcesInfo.pathOffsetInPart3 = ThrowInPool(childchildchild.Attributes["path"]?.Value ?? string.Empty);
                                    childchildchild.Attributes.RemoveNamedItem("type");
                                    childchildchild.Attributes.RemoveNamedItem("id");
                                    childchildchild.Attributes.RemoveNamedItem("path");
                                    //如果type是0那就是图片
                                    if (xmlResourcesInfo.type == 0)
                                    {
                                        xmlResourcesInfo.ptxInfo = new XmlPtxInfo();
                                        if (childchildchild.Attributes["imagetype"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.type = Convert.ToUInt16(childchildchild.Attributes["imagetype"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("imagetype");
                                        }
                                        if (childchildchild.Attributes["aflags"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.aflags = Convert.ToUInt16(childchildchild.Attributes["aflags"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("aflags");
                                        }
                                        if (childchildchild.Attributes["x"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.x = Convert.ToUInt16(childchildchild.Attributes["x"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("x");
                                        }
                                        if (childchildchild.Attributes["y"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.y = Convert.ToUInt16(childchildchild.Attributes["y"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("y");
                                        }
                                        if (childchildchild.Attributes["ax"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.ax = Convert.ToUInt16(childchildchild.Attributes["ax"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("ax");
                                        }
                                        if (childchildchild.Attributes["ay"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.ay = Convert.ToUInt16(childchildchild.Attributes["ay"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("ay");
                                        }
                                        if (childchildchild.Attributes["aw"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.aw = Convert.ToUInt16(childchildchild.Attributes["aw"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("aw");
                                        }
                                        if (childchildchild.Attributes["ah"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.ah = Convert.ToUInt16(childchildchild.Attributes["ah"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("ah");
                                        }
                                        if (childchildchild.Attributes["rows"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.rows = Convert.ToUInt16(childchildchild.Attributes["rows"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("rows");
                                        }
                                        if (childchildchild.Attributes["cols"] != null)
                                        {
                                            xmlResourcesInfo.ptxInfo.cols = Convert.ToUInt16(childchildchild.Attributes["cols"].Value);
                                            childchildchild.Attributes.RemoveNamedItem("cols");
                                        }
                                        xmlResourcesInfo.ptxInfo.parentOffsetInPart3 = ThrowInPool(childchildchild.Attributes["parent"]?.Value ?? string.Empty);
                                        childchildchild.Attributes.RemoveNamedItem("parent");
                                    }
                                    xmlResourcesInfo.propertiesNumber = childchildchild.Attributes.Count;
                                    xmlResourcesInfo.propertiesInfoLibrary = new XmlPropertiesInfo[xmlResourcesInfo.propertiesNumber];
                                    for (int l = 0; l < xmlResourcesInfo.propertiesNumber; l++)
                                    {
                                        xmlResourcesInfo.propertiesInfoLibrary[l] = new XmlPropertiesInfo();
                                        xmlResourcesInfo.propertiesInfoLibrary[l].keyOffsetInPart3 = ThrowInPool(childchildchild.Attributes[l]?.Name ?? string.Empty);
                                        xmlResourcesInfo.propertiesInfoLibrary[l].valueOffsetInPart3 = ThrowInPool(childchildchild.Attributes[l]?.Value ?? string.Empty);
                                    }
                                    xmlResourcesInfo.infoOffsetInPart2 = (int)bsxmlpart2.Position;
                                    xmlResourcesInfo.WritePart2(bsxmlpart2);
                                }
                            }
                            xmlInfo[i].Write(bsxmlpart1);
                        }
                        bsxmlpart1.Position = 0;
                        bsxmlpart2.Position = 0;
                        bsxmlpart3.Position = 0;
                        bs.WriteInt32(1919251249);
                        bs.WriteInt32(1);
                        bs.WriteInt32(0x14);
                        bs.WriteInt32((int)(0x14 + bsxmlpart1.Length));
                        bs.WriteInt32((int)(0x14 + bsxmlpart1.Length + bsxmlpart2.Length));
                        bsxmlpart1.CopyTo(bs);
                        bsxmlpart2.CopyTo(bs);
                        bsxmlpart3.CopyTo(bs);
                    }
                }
            }
        }

        public static void DatToXml(BinaryStream bs, string outFile)
        {
            Dir.NewDir(outFile, false);
            XmlPack xmlpack;
            xmlpack = new XmlPack().Read(bs);
            using (StreamWriter sw = new StreamWriter(outFile))
            {
                sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
                sw.Write("<!-- DO NOT EDIT THIS FILE. This file is generated by PopStudio. (unless you want to change the cutting method of some pictures) -->\n");
                sw.Write("<ResourceManifest version=\"3\">\n");
                for (int i = 0; i < xmlpack.xmlInfo_Library.Length; i++)
                {
                    sw.Write("\n<CompositeResources id=\"" + xmlpack.xmlInfo_Library[i].id + "\">\n");
                    for (int j = 0; j < xmlpack.xmlInfo_Library[i].rsgpInfoLibrary.Length; j++)
                    {
                        sw.Write("  <Group id=\"" + xmlpack.xmlInfo_Library[i].rsgpInfoLibrary[j].id + "\" res=\"" + xmlpack.xmlInfo_Library[i].rsgpInfoLibrary[j].resolutionRatio + "\" loc=\"" + xmlpack.xmlInfo_Library[i].rsgpInfoLibrary[j].language + "\">\n");
                        for (int k = 0; k < xmlpack.xmlInfo_Library[i].rsgpInfoLibrary[j].resourcesInfoLibrary.Length; k++)
                        {
                            var k_xml = xmlpack.xmlInfo_Library[i].rsgpInfoLibrary[j].resourcesInfoLibrary[k];
                            if (k_xml == null) throw new Exception();
                            sw.Write("    <Res type=\"" + k_xml.type + "\" id=\"" + k_xml.id + "\" path=\"" + k_xml.path + "\" ");
                            if (k_xml.ptxInfoBeginOffsetInPart2 * k_xml.ptxInfoEndOffsetInPart2 != 0)
                            {
                                var k_xml_ptx = k_xml.ptxInfo;
                                if (k_xml_ptx == null) throw new Exception();
                                sw.Write("imagetype=\"" + k_xml_ptx.type + "\" ");
                                sw.Write("aflags=\"" + k_xml_ptx.aflags + "\" ");
                                if (k_xml_ptx.x != 0) sw.Write("x=\"" + k_xml_ptx.x + "\" ");
                                if (k_xml_ptx.y != 0) sw.Write("y=\"" + k_xml_ptx.y + "\" ");
                                if (k_xml_ptx.ax != 0) sw.Write("ax=\"" + k_xml_ptx.ax + "\" ");
                                if (k_xml_ptx.ay != 0) sw.Write("ay=\"" + k_xml_ptx.ay + "\" ");
                                if (k_xml_ptx.aw != 0) sw.Write("aw=\"" + k_xml_ptx.aw + "\" ");
                                if (k_xml_ptx.ah != 0) sw.Write("ah=\"" + k_xml_ptx.ah + "\" ");
                                if (k_xml_ptx.rows != 1) sw.Write("rows=\"" + k_xml_ptx.rows + "\" ");
                                if (k_xml_ptx.cols != 1) sw.Write("cols=\"" + k_xml_ptx.cols + "\" ");
                                sw.Write("parent=\"" + k_xml_ptx.parent + "\" ");
                            }
                            for (int l = 0; l < k_xml.propertiesInfoLibrary.Length; l++)
                            {
                                sw.Write(k_xml.propertiesInfoLibrary[l].key + "=\"" + k_xml.propertiesInfoLibrary[l].value + "\" ");
                            }
                            sw.Write("/>\n");
                        }
                        sw.Write("  </Group>\n");
                    }
                    sw.Write("</CompositeResources>\n");
                }
                sw.Write("\n</ResourceManifest>");
            }
        }
    }

    internal class XmlPack
    {
        public int magic = 1919251249;
        public int version = 1;
        public int xmlPart1_BeginOffset;
        public int xmlPart2_BeginOffset;
        public int xmlPart3_BeginOffset;
        public XmlCompositeResourcesInfo[] xmlInfo_Library;

        public XmlPack Read(BinaryStream bs)
        {
            bs.IdInt32(magic);
            bs.IdInt32(version);
            xmlPart1_BeginOffset = bs.ReadInt32();
            xmlPart2_BeginOffset = bs.ReadInt32();
            xmlPart3_BeginOffset = bs.ReadInt32();
            List<XmlCompositeResourcesInfo> xmlInfo_Library = new List<XmlCompositeResourcesInfo>();
            bs.Position = (xmlPart1_BeginOffset);
            for (int i = 0; bs.Position < xmlPart2_BeginOffset; i++)
            {
                xmlInfo_Library.Add(new XmlCompositeResourcesInfo().Read(bs, xmlPart3_BeginOffset));
                long tempOffset = bs.Position;
                for (int j = 0; j < xmlInfo_Library[i].rsgpNumber; j++)
                {
                    for (int k = 0; k < xmlInfo_Library[i].rsgpInfoLibrary[j].resourcesNumber; k++)
                    {
                        bs.Position = xmlPart2_BeginOffset + xmlInfo_Library[i].rsgpInfoLibrary[j].resourcesInfoLibrary[k].infoOffsetInPart2;
                        xmlInfo_Library[i].rsgpInfoLibrary[j].resourcesInfoLibrary[k].ReadPart2(bs, xmlPart3_BeginOffset);
                    }
                }
                bs.Position = (tempOffset);
            }
            this.xmlInfo_Library = xmlInfo_Library.ToArray();
            return this;
        }
    }

    //必要的类还是要有的

    internal class XmlCompositeResourcesInfo
    {
        public int idOffsetInPart3 = 0x0;
        public int rsgpNumber = 0x0;
        public int rsgpInfoLength = 0x10;
        public XmlRsgpInfo[] rsgpInfoLibrary;

        public string id = string.Empty;

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(idOffsetInPart3);
            bs.WriteInt32(rsgpNumber);
            bs.WriteInt32(rsgpInfoLength);
            for (int i = 0; i < rsgpNumber; i++)
            {
                rsgpInfoLibrary[i].Write(bs);
            }
        }

        public XmlCompositeResourcesInfo Read(BinaryStream bs, int xmlPart3Offset)
        {
            idOffsetInPart3 = bs.ReadInt32();
            id = bs.GetStringByEmpty(xmlPart3Offset + idOffsetInPart3);
            rsgpNumber = bs.ReadInt32();
            if (bs.ReadInt32() != rsgpInfoLength)
            {
                throw new Exception(Str.Obj.DataMisMatch);
            }
            rsgpInfoLibrary = new XmlRsgpInfo[rsgpNumber];
            for (int i = 0; i < rsgpNumber; i++)
            {
                rsgpInfoLibrary[i] = new XmlRsgpInfo().Read(bs, xmlPart3Offset);
            }
            return this;
        }
    }

    internal class XmlRsgpInfo
    {
        public int resolutionRatio = 0x0;
        public string language = string.Empty;
        public int idOffsetInPart3 = 0x0;
        public int resourcesNumber = 0x0;
        public XmlResourcesInfo[] resourcesInfoLibrary;

        public string id = string.Empty;

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(resolutionRatio);
            if (language == string.Empty)
            {
                bs.WriteInt32(0x0);
            }
            else
            {
                bs.WriteString((language + "    ")[..4], bs.Endian);
            }
            bs.WriteInt32(idOffsetInPart3);
            bs.WriteInt32(resourcesNumber);
            for (int i = 0; i < resourcesNumber; i++)
            {
                resourcesInfoLibrary[i].WritePart1(bs);
            }
        }

        public XmlRsgpInfo Read(BinaryStream bs, int xmlPart3Offset)
        {
            resolutionRatio = bs.ReadInt32();
            language = bs.ReadString(0x4, bs.Endian).Replace("\0", "");
            idOffsetInPart3 = bs.ReadInt32();
            resourcesNumber = bs.ReadInt32();
            resourcesInfoLibrary = new XmlResourcesInfo[resourcesNumber];
            for (int i = 0; i < resourcesNumber; i++)
            {
                resourcesInfoLibrary[i] = new XmlResourcesInfo().ReadPart1(bs);
            }
            id = bs.GetStringByEmpty(xmlPart3Offset + idOffsetInPart3);
            return this;
        }
    }

    internal class XmlResourcesInfo
    {
        //第一段
        public int infoOffsetInPart2 = 0x0;

        public void WritePart1(BinaryStream bs)
        {
            bs.WriteInt32(infoOffsetInPart2);
        }

        public XmlResourcesInfo ReadPart1(BinaryStream bs)
        {
            infoOffsetInPart2 = bs.ReadInt32();
            return this;
        }

        //第二段
        public int empty = 0x0;
        public ushort type = 0x0;
        public ushort headLength = 0x1C;
        public int ptxInfoEndOffsetInPart2 = 0x0;
        public int ptxInfoBeginOffsetInPart2 = 0x0;
        public int idOffsetInPart3 = 0x0;
        public int pathOffsetInPart3 = 0x0;
        public int propertiesNumber = 0x0;
        public XmlPtxInfo ptxInfo;
        public XmlPropertiesInfo[] propertiesInfoLibrary;

        public string id;
        public string path;

        public void WritePart2(BinaryStream bs)
        {
            bs.WriteInt32(empty);
            bs.WriteUInt16(type);
            bs.WriteUInt16(headLength);
            long offsetBak1 = bs.Position;
            bs.Position += (0x8);
            bs.WriteInt32(idOffsetInPart3);
            bs.WriteInt32(pathOffsetInPart3);
            bs.WriteInt32(propertiesNumber);
            if (type == 0)
            {
                ptxInfoBeginOffsetInPart2 = (int)bs.Position;
                ptxInfo.Write(bs);
                ptxInfoEndOffsetInPart2 = (int)bs.Position;
                bs.Position = (offsetBak1);
                bs.WriteInt32(ptxInfoEndOffsetInPart2);
                bs.WriteInt32(ptxInfoBeginOffsetInPart2);
                bs.Position = (ptxInfoEndOffsetInPart2);
            }
            for (int i = 0; i < propertiesNumber; i++)
            {
                propertiesInfoLibrary[i].Wtire(bs);
            }
        }

        public XmlResourcesInfo ReadPart2(BinaryStream bs, int xmlPart3Offset)
        {
            if (bs.ReadInt32() != empty)
            {
                throw new Exception(Str.Obj.DataMisMatch);
            }
            type = bs.ReadUInt16();
            if (bs.ReadUInt16() != headLength)
            {
                throw new Exception(Str.Obj.DataMisMatch);
            }
            ptxInfoEndOffsetInPart2 = bs.ReadInt32();
            ptxInfoBeginOffsetInPart2 = bs.ReadInt32();
            idOffsetInPart3 = bs.ReadInt32();
            pathOffsetInPart3 = bs.ReadInt32();
            id = bs.GetStringByEmpty(xmlPart3Offset + idOffsetInPart3);
            path = bs.GetStringByEmpty(xmlPart3Offset + pathOffsetInPart3);
            propertiesNumber = bs.ReadInt32();
            if (ptxInfoEndOffsetInPart2 * ptxInfoBeginOffsetInPart2 != 0)
            {
                ptxInfo = new XmlPtxInfo().Read(bs, xmlPart3Offset);
            }
            propertiesInfoLibrary = new XmlPropertiesInfo[propertiesNumber];
            for (int i = 0; i < propertiesNumber; i++)
            {
                propertiesInfoLibrary[i] = new XmlPropertiesInfo().Read(bs, xmlPart3Offset);
            }
            return this;
        }
    }

    internal class XmlPtxInfo
    {
        public ushort type = 0x0;
        public ushort aflags = 0x0;
        public ushort x = 0x0;
        public ushort y = 0x0;
        public ushort ax = 0x0;
        public ushort ay = 0x0;
        public ushort aw = 0x0;
        public ushort ah = 0x0;
        public ushort rows = 0x1;
        public ushort cols = 0x1;
        public int parentOffsetInPart3 = 0x0;

        public string parent;

        public void Write(BinaryStream bs)
        {
            bs.WriteUInt16(type);
            bs.WriteUInt16(aflags);
            bs.WriteUInt16(x);
            bs.WriteUInt16(y);
            bs.WriteUInt16(ax);
            bs.WriteUInt16(ay);
            bs.WriteUInt16(aw);
            bs.WriteUInt16(ah);
            bs.WriteUInt16(rows);
            bs.WriteUInt16(cols);
            bs.WriteInt32(parentOffsetInPart3);
        }

        public XmlPtxInfo Read(BinaryStream bs, int xmlPart3Offset)
        {
            type = bs.ReadUInt16();
            aflags = bs.ReadUInt16();
            x = bs.ReadUInt16();
            y = bs.ReadUInt16();
            ax = bs.ReadUInt16();
            ay = bs.ReadUInt16();
            aw = bs.ReadUInt16();
            ah = bs.ReadUInt16();
            rows = bs.ReadUInt16();
            cols = bs.ReadUInt16();
            parentOffsetInPart3 = bs.ReadInt32();
            parent = bs.GetStringByEmpty(xmlPart3Offset + parentOffsetInPart3);
            return this;
        }
    }

    internal class XmlPropertiesInfo
    {
        public int keyOffsetInPart3 = 0x0;
        public int empty = 0x0;
        public int valueOffsetInPart3 = 0x0;

        public string key;
        public string value;

        public void Wtire(BinaryStream bs)
        {
            bs.WriteInt32(keyOffsetInPart3);
            bs.WriteInt32(empty);
            bs.WriteInt32(valueOffsetInPart3);
        }

        public XmlPropertiesInfo Read(BinaryStream bs, int xmlPart3Offset)
        {
            keyOffsetInPart3 = bs.ReadInt32();
            if (bs.ReadInt32() != empty)
            {
                throw new Exception(Str.Obj.DataMisMatch);
            }
            valueOffsetInPart3 = bs.ReadInt32();
            key = bs.GetStringByEmpty(xmlPart3Offset + keyOffsetInPart3);
            value = bs.GetStringByEmpty(xmlPart3Offset + valueOffsetInPart3);
            return this;
        }
    }
}