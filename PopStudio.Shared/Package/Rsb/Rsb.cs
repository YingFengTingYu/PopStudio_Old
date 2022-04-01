using System.Xml;

namespace PopStudio.Package.Rsb
{
    internal static class Rsb
    {
        static string autopoolname = "_AutoPool";

        public static void Pack(string inFolder, string outFile)
        {
            Dir.FormatAndDeleteEndPathSeparator(ref inFolder);
            Dir.FormatAndDeleteEndPathSeparator(ref outFile);
            if (!Directory.Exists(inFolder))
            {
                throw new Exception(string.Format(Str.Obj.FolderNotFound, inFolder));
            }
            Dir.NewDir(outFile, false);
            inFolder += Const.PATHSEPARATOR;
            string studioPath = inFolder + "POPSTUDIOINFO" + Const.PATHSEPARATOR;
            //get the method to pack
            RsbInfo rsb = new RsbInfo();
            rsb.head = new RsbHeadInfo();
            int version = 3;
            Endian endian = Endian.Small;
            bool compressPart0 = false;
            bool compressPart1 = true;
            int ptxEachLength = 0x10;
            bool smf = false;
            CompressionLevel level = CompressionLevel.Optimal;
            string xmldata;
            XmlDocument xml;
            XmlNode root;
            XmlNodeList childlist;
            if (File.Exists(studioPath + "PACKINFO.XML"))
            {
                xmldata = File.ReadAllText(studioPath + "PACKINFO.XML");
                xml = new XmlDocument();
                xml.LoadXml(xmldata);
                root = xml.SelectSingleNode("/PackInfo");
                if (root == null) return;
                childlist = root.ChildNodes;
                foreach (XmlNode child in childlist)
                {
                    if (child.Name == "PackageVersion")
                    {
                        version = Convert.ToInt32(child.InnerText);
                    }
                    else if (child.Name == "UseBigEndian")
                    {
                        endian = Convert.ToBoolean(child.InnerText) ? Endian.Big : Endian.Small;
                    }
                    else if (child.Name == "CompressMethod")
                    {
                        int flags = Convert.ToInt32(child.InnerText);
                        compressPart0 = (flags & 0b10) != 0;
                        compressPart1 = (flags & 0b1) != 0;
                    }
                    else if (child.Name == "PtxInfoLength")
                    {
                        ptxEachLength = Convert.ToInt32(child.InnerText);
                        rsb.head.ptxInfo_EachLength = ptxEachLength;
                    }
                    else if (child.Name == "ZlibAll")
                    {
                        smf = Convert.ToBoolean(child.InnerText);
                    }
                    else if (child.Name == "CompressLevel")
                    {
                        level = child.InnerText switch
                        {
                            "Fastest" => CompressionLevel.Fastest,
                            "Smallest" => CompressionLevel.SmallestSize,
                            _ => CompressionLevel.Optimal
                        };
                    }
                }
            }
            byte emptyfilecompresshead = level switch
            {
                CompressionLevel.Fastest => 0x01,
                CompressionLevel.SmallestSize => 0xDA,
                _ => 0x9C
            };
            using TempFilePool tempFilePool = new TempFilePool(); //I just want to delete temp files without using try catch
            using BinaryStream bs_rsgpfile = new BinaryStream(tempFilePool.Add(), FileMode.Create);
            bs_rsgpfile.Endian = endian;
            rsb.head.version = version;
            xmldata = File.ReadAllText(studioPath + "RESOURCESGROUP.XML");
            xml = new XmlDocument();
            xml.LoadXml(xmldata);
            root = xml.SelectSingleNode("/ResourcesGroupInfo");
            childlist = root.ChildNodes;
            if (childlist == null) return;
            rsb.head.rsgp_Number = childlist.Count;
            rsb.rsgpInfo = new RsbRsgpInfo[childlist.Count];
            rsb.rsgp = new RsgpInfo[childlist.Count];
            rsb.head.autopool_Number = childlist.Count;
            rsb.autopoolInfo = new RsbAutoPoolInfo[childlist.Count];
            int ptxNumber = 0;
            List<RsbPtxInfo> ptxList = new List<RsbPtxInfo>();
            int f = 0;
            if (compressPart0)
            {
                f |= 0b10;
            }
            if (compressPart1)
            {
                f |= 0b1;
            }
            for (int i = 0; i < childlist.Count; i++)
            {
                long off = bs_rsgpfile.Position;
                int thisPtxNumber = 0;
                var rsgpinfo = rsb.rsgpInfo[i] = new RsbRsgpInfo();
                rsgpinfo.ID = childlist[i].Attributes["id"].Value;
                rsgpinfo.index = i;
                rsgpinfo.flags = f;
                rsgpinfo.offset = (int)off;
                var autopool = rsb.autopoolInfo[i] = new RsbAutoPoolInfo();
                autopool.ID = rsgpinfo.ID + autopoolname;
                var rsgp = rsb.rsgp[i] = new RsgpInfo();
                rsgp.head = new RsgpHeadInfo();
                rsgp.head.flags = f;
                rsgp.head.version = version;
                rsb.rsgpInfo[i].ptx_BeforeNumber = ptxNumber;
                rsb.rsgpList.Add(new CompressString(rsgpinfo.ID.ToUpper(), new RsbExtraInfo(i)));
                using (BinaryStream bsP0over = new BinaryStream())
                {
                    using (BinaryStream bsP1over = new BinaryStream())
                    {
                        using (BinaryStream bsP0 = new BinaryStream())
                        {
                            using (BinaryStream bsP1 = new BinaryStream())
                            {
                                var childchildlist = childlist[i].ChildNodes;
                                for (int j = 0; j < childchildlist.Count; j++)
                                {
                                    string fileid = childchildlist[j].Attributes["id"].Value;
                                    rsb.fileList.Add(new CompressString(fileid, new RsbExtraInfo(i)));
                                    if (childchildlist[j].Name == "Img")
                                    {
                                        //p1
                                        var ptx = new RsbPtxInfo(ptxEachLength);
                                        string name = Dir.FormatPath(inFolder + fileid);
                                        bool delete = false;
                                        if (!File.Exists(name))
                                        {
                                            string name2 = Path.ChangeExtension(name, ".PNG");
                                            if (File.Exists(name2))
                                            {
                                                Image.Ptx.PtxFormat format = (Image.Ptx.PtxFormat)Convert.ToInt32(childchildlist[j].Attributes["defaultformat"].Value);
                                                Image.Ptx.Ptx.Encode(name2, name, format, bs_rsgpfile.Endian, ptxEachLength != 0x10);
                                                delete = true;
                                            }
                                        }
                                        using (BinaryStream bsindexunknow = BinaryStream.Open(name))
                                        {
                                            bsindexunknow.Endian = endian;
                                            bsindexunknow.IdInt32(1886681137);
                                            bsindexunknow.IdInt32(1);
                                            ptx.width = bsindexunknow.ReadInt32();
                                            ptx.height = bsindexunknow.ReadInt32();
                                            ptx.check = bsindexunknow.ReadInt32();
                                            ptx.format = bsindexunknow.ReadInt32();
                                            ptx.alphaSize = bsindexunknow.ReadInt32();
                                            ptx.alphaFormat = bsindexunknow.ReadInt32();
                                            rsgp.fileList.Add(new CompressString(fileid, new RsgpPart1ExtraInfo((int)bsP1.Position, (int)(bsindexunknow.Length - 0x20), thisPtxNumber++, ptx.width, ptx.height)));
                                            bsindexunknow.CopyTo(bsP1);
                                            bsP1.Length = FourK(bsP1.Position);
                                            bsP1.Position = bsP1.Length;
                                        }
                                        ptxList.Add(ptx);
                                        if (delete) File.Delete(name);
                                    }
                                    else
                                    {
                                        //p0
                                        using (BinaryStream bsindexunknow = BinaryStream.Open(Dir.FormatPath(inFolder + childchildlist[j].Attributes["id"].Value)))
                                        {
                                            rsgp.fileList.Add(new CompressString(fileid, new RsgpPart0ExtraInfo((int)bsP0.Position, (int)bsindexunknow.Length)));
                                            bsindexunknow.CopyTo(bsP0);
                                            bsP0.Length = FourK(bsP0.Position);
                                            bsP0.Position = bsP0.Length;
                                        }
                                    }
                                }
                                rsgpinfo.part0_Size = rsgpinfo.part0_Size2 = rsgp.head.part0_Size = (int)bsP0.Length;
                                rsgpinfo.part1_Size = autopool.part1_Size = rsgp.head.part1_Size = (int)bsP1.Length;
                                bsP0.Position = 0;
                                bsP1.Position = 0;
                                if (compressPart0)
                                {
                                    using (ZLibStream zLibStream = new ZLibStream(bsP0over, level, true))
                                    {
                                        bsP0.CopyTo(zLibStream);
                                    }
                                    if (bsP0over.Length == 0)
                                    {
                                        bsP0over.WriteUInt8(0x78);
                                        bsP0over.WriteUInt8(emptyfilecompresshead);
                                        bsP0over.WriteUInt8(0x03);
                                        bsP0over.WriteUInt8(0x00);
                                        bsP0over.WriteUInt8(0x00);
                                        bsP0over.WriteUInt8(0x00);
                                        bsP0over.WriteUInt8(0x00);
                                        bsP0over.WriteUInt8(0x01);
                                    }
                                }
                                else
                                {
                                    bsP0.CopyTo(bsP0over);
                                }
                                if (compressPart1)
                                {
                                    using (ZLibStream zLibStream = new ZLibStream(bsP1over, level, true))
                                    {
                                        bsP1.CopyTo(zLibStream);
                                    }
                                }
                                else
                                {
                                    bsP1.CopyTo(bsP1over);
                                }
                                bsP0over.Length = FourK(bsP0over.Position);
                                bsP1over.Length = FourK(bsP1over.Position);
                                bsP0over.Position = 0;
                                bsP1over.Position = 0;
                            }
                        }
                        rsgp.head.part0_ZSize = rsgpinfo.part0_ZSize = (int)bsP0over.Length;
                        rsgp.head.part1_ZSize = rsgpinfo.part1_ZSize = (int)bsP1over.Length;
                        bs_rsgpfile.Position = off + rsgp.head.fileList_BeginOffset;
                        byte[] list = rsgp.fileList.Write();
                        rsgp.head.fileList_Length = list.Length;
                        using (BinaryStream bs_list = new BinaryStream(list))
                        {
                            int times = list.Length >> 2;
                            for (int i_xj = 0; i_xj < times; i_xj++)
                            {
                                bs_rsgpfile.WriteInt32(bs_list.ReadInt32());
                            }
                        }
                        bs_rsgpfile.Length = FourK(bs_rsgpfile.Position);
                        bs_rsgpfile.Position = bs_rsgpfile.Length;
                        rsgp.head.fileOffset = rsgp.head.part0_Offset = rsgpinfo.fileOffset = rsgpinfo.part0_Offset = (int)(bs_rsgpfile.Position - off);
                        bsP0over.CopyTo(bs_rsgpfile);
                        rsgp.head.part1_Offset = rsgpinfo.part1_Offset = (int)(bs_rsgpfile.Position - off);
                        autopool.part1_Offset_InDecompress = rsgp.head.part0_Offset + rsgp.head.part0_Size; //Not zsize
                        bsP1over.CopyTo(bs_rsgpfile);
                    }
                }
                //write head
                bs_rsgpfile.Position = off;
                rsgp.head.Write(bs_rsgpfile);
                rsgpinfo.ptx_Number = thisPtxNumber;
                ptxNumber += thisPtxNumber;
                bs_rsgpfile.Position = bs_rsgpfile.Length;
                rsb.rsgpInfo[i].size = (int)(bs_rsgpfile.Length - off);
                GC.Collect();
            }
            rsb.head.ptx_Number = ptxNumber;
            string mfile = outFile;
            if (smf) mfile = tempFilePool.Add();
            using (BinaryStream bs = new BinaryStream(mfile, FileMode.Create))
            {
                bs.Endian = endian;
                rsb.head.Write(bs);
                byte[] fileList = rsb.fileList.Write();
                rsb.head.fileList_Length = fileList.Length;
                rsb.head.fileList_BeginOffset = (int)bs.Position;
                using (BinaryStream bs_list = new BinaryStream(fileList))
                {
                    int times = fileList.Length >> 2;
                    for (int i_xj = 0; i_xj < times; i_xj++)
                    {
                        bs.WriteInt32(bs_list.ReadInt32());
                    }
                }
                fileList = null;
                byte[] rsgpList = rsb.rsgpList.Write();
                rsb.head.rsgpList_Length = rsgpList.Length;
                rsb.head.rsgpList_BeginOffset = (int)bs.Position;
                using (BinaryStream bs_list = new BinaryStream(rsgpList))
                {
                    int times = rsgpList.Length >> 2;
                    for (int i_xj = 0; i_xj < times; i_xj++)
                    {
                        bs.WriteInt32(bs_list.ReadInt32());
                    }
                }
                rsgpList = null;
                rsb.head.compositeInfo_BeginOffset = (int)bs.Position;
                xmldata = File.ReadAllText(studioPath + "COMPOSITERESOURCES.XML");
                xml = new XmlDocument();
                xml.LoadXml(xmldata);
                root = xml.SelectSingleNode("/CompositeResourcesInfo");
                childlist = root.ChildNodes;
                if (childlist == null) return;
                rsb.compositeInfo = new RsbCompositeInfo[childlist.Count];
                rsb.head.composite_Number = childlist.Count;
                for (int i = 0; i < childlist.Count; i++)
                {
                    rsb.compositeInfo[i] = new RsbCompositeInfo();
                    rsb.compositeInfo[i].ID = childlist[i].Attributes["id"].Value;
                    var childchildlist = childlist[i].ChildNodes;
                    int num = childchildlist.Count;
                    rsb.compositeInfo[i].child_Number = num;
                    rsb.compositeList.Add(new CompressString(rsb.compositeInfo[i].ID.ToUpper(), new RsbExtraInfo(i)));
                    for (int j = 0; j < num; j++)
                    {
                        rsb.compositeInfo[i].child_Info[j].index = Convert.ToInt32(childchildlist[j].Attributes["index"].Value);
                        rsb.compositeInfo[i].child_Info[j].ratio = Convert.ToInt32(childchildlist[j].Attributes["res"].Value);
                        rsb.compositeInfo[i].child_Info[j].language = childchildlist[j].Attributes["loc"].Value;
                    }
                    rsb.compositeInfo[i].Write(bs);
                }
                byte[] compositeList = rsb.compositeList.Write();
                rsb.head.compositeList_Length = compositeList.Length;
                rsb.head.compositeList_BeginOffset = (int)bs.Position;
                using (BinaryStream bs_list = new BinaryStream(compositeList))
                {
                    int times = compositeList.Length >> 2;
                    for (int i_xj = 0; i_xj < times; i_xj++)
                    {
                        bs.WriteInt32(bs_list.ReadInt32());
                    }
                }
                compositeList = null;
                rsb.head.rsgpInfo_BeginOffset = (int)bs.Position;
                bs.Length = bs.Position + RsbHeadInfo.rsgpInfo_EachLength * rsb.head.rsgp_Number;
                bs.Position = bs.Length;
                rsb.head.autopoolInfo_BeginOffset = (int)bs.Position;
                for (int i = 0; i < rsb.head.autopool_Number; i++)
                {
                    rsb.autopoolInfo[i].Write(bs);
                }
                rsb.head.ptxInfo_BeginOffset = (int)bs.Position;
                for (int i = 0; i < rsb.head.ptx_Number; i++)
                {
                    ptxList[i].Write(bs);
                }
                string xmlpath = Dir.FormatPath(studioPath + "RESOURCES.XML");
                if (File.Exists(xmlpath))
                {
                    if (endian == Endian.Big)
                    {
                        bs.Length = FourK(bs.Position);
                        bs.Position = bs.Length;
                    }
                    using (BinaryStream bs_xml = new BinaryStream())
                    {
                        bs_xml.Endian = endian;
                        XmlConvert.XmlToDat(xmlpath, bs_xml);
                        bs_xml.Position = 0;
                        bs_xml.IdInt32(1919251249);
                        bs_xml.IdInt32(1);
                        rsb.head.xmlPart1_BeginOffset = (int)bs.Position;
                        int k = bs_xml.ReadInt32();
                        rsb.head.xmlPart2_BeginOffset = rsb.head.xmlPart1_BeginOffset + bs_xml.ReadInt32() - k;
                        rsb.head.xmlPart3_BeginOffset = rsb.head.xmlPart1_BeginOffset + bs_xml.ReadInt32() - k;
                        bs_xml.CopyTo(bs);
                    }
                }
                bs.Length = FourK(bs.Position);
                bs.Position = bs.Length;
                rsb.head.headLength = (int)bs.Position;
                for (int i = 0; i < rsb.head.rsgp_Number; i++)
                {
                    rsb.rsgpInfo[i].offset += rsb.head.headLength;
                }
                bs_rsgpfile.Position = 0;
                bs_rsgpfile.CopyTo(bs);
                bs.Position = rsb.head.rsgpInfo_BeginOffset;
                for (int i = 0; i < rsb.head.rsgp_Number; i++)
                {
                    rsb.rsgpInfo[i].Write(bs);
                }
                bs.Position = 0;
                rsb.head.Write(bs);
                if (smf)
                {
                    using (BinaryStream bs_e = new BinaryStream(outFile, FileMode.Create))
                    {
                        bs.Position = 0;
                        bs_e.WriteInt32(-559022380);
                        bs_e.WriteInt32((int)bs.Length);
                        using (ZLibStream zLibStream = new ZLibStream(bs_e, CompressionLevel.SmallestSize))
                        {
                            bs.CopyTo(zLibStream);
                        }
                    }
                }
            }
        }

        public static long FourK(long off)
        {
            return off % 0x1000 == 0 ? off : (off + 0x1000 - (off % 0x1000));
        }

        public static void Unpack(string inFile, string outFolder, bool changeimage = false, bool delete = false)
        {
            Dir.FormatAndDeleteEndPathSeparator(ref inFile);
            Dir.FormatAndDeleteEndPathSeparator(ref outFolder);
            if (!File.Exists(inFile))
            {
                throw new Exception(string.Format(Str.Obj.FileNotFound, inFile));
            }
            Dir.NewDir(outFolder);
            outFolder += Const.PATHSEPARATOR;
            using (BinaryStream bs_memory = new BinaryStream())
            {
                using (BinaryStream bs_origin = BinaryStream.Open(inFile))
                {
                    bool usesmf = false;
                    BinaryStream bs = bs_origin;
                    int temphead = bs.PeekInt32();
                    if (temphead == -559022380) //D4 FE AD DE
                    {
                        bs.Position += 8;
                        using (ZLibStream zLibStream = new ZLibStream(bs, CompressionMode.Decompress))
                        {
                            zLibStream.CopyTo(bs_memory);
                        }
                        bs = bs_memory;
                        bs.Position = 0;
                        temphead = bs.PeekInt32();
                        usesmf = true;
                    }
                    if (temphead == 828535666) //rsb1
                    {
                        bs.Endian = bs.Endian == Endian.Small ? Endian.Big : Endian.Small;
                    }
                    else if (temphead != 1920164401) //not 1bsr => wrong file magic
                    {
                        throw new Exception(Str.Obj.DataMisMatch);
                    }
                    RsbInfo rsb = new RsbInfo().Read(bs);
                    string lst = outFolder + "POPSTUDIOINFO";
                    Dir.NewDir(lst);
                    lst += Const.PATHSEPARATOR;
                    int compresslevel = 0;
                    bool getlevel = true;
                    if (rsb.head.xmlPart1_BeginOffset != 0)
                    {
                        using (BinaryStream bs2 = new BinaryStream())
                        {
                            bs2.Endian = bs.Endian;
                            bs2.WriteInt32(1919251249);
                            bs2.WriteInt32(1);
                            bs2.WriteInt32(0x14);
                            bs2.WriteInt32(rsb.head.xmlPart2_BeginOffset - rsb.head.xmlPart1_BeginOffset + 0x14);
                            bs2.WriteInt32(rsb.head.xmlPart3_BeginOffset - rsb.head.xmlPart1_BeginOffset + 0x14);
                            bs.Position = rsb.head.xmlPart1_BeginOffset;
                            bs2.WriteBytes(bs.ReadBytes(rsb.head.headLength - rsb.head.xmlPart1_BeginOffset));
                            bs2.Position = 0;
                            XmlConvert.DatToXml(bs2, lst + "RESOURCES.XML");
                        }
                    }
                    using (StreamWriter sw = new StreamWriter(lst + "COMPOSITERESOURCES.XML", false))
                    {
                        sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        sw.WriteLine("<!-- DO NOT EDIT THIS FILE. This file is generated by PopStudio. -->");
                        sw.WriteLine("<CompositeResourcesInfo>");
                        for (int i = 0; i < rsb.compositeInfo.Length; i++)
                        {
                            sw.WriteLine("  <CompositeResources id=\"" + rsb.compositeInfo[i].ID + "\">");
                            for (int j = 0; j < rsb.compositeInfo[i].child_Number; j++)
                            {
                                sw.WriteLine("    <Group index=\"" + rsb.compositeInfo[i].child_Info[j].index + "\" res=\"" + rsb.compositeInfo[i].child_Info[j].ratio + "\" loc=\"" + rsb.compositeInfo[i].child_Info[j].language + "\" />");
                            }
                            sw.WriteLine("  </CompositeResources>");
                        }
                        sw.WriteLine("</CompositeResourcesInfo>");
                    }
                    using (StreamWriter sw = new StreamWriter(lst + "RESOURCESGROUP.XML", false))
                    {
                        sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        sw.WriteLine("<!-- DO NOT EDIT THIS FILE. This file is generated by PopStudio. (unless you want to add or delete some resources, or want to change default format of ptx which was transformed into png) -->");
                        sw.WriteLine("<ResourcesGroupInfo>");
                        for (int i = 0; i < rsb.head.rsgp_Number; i++)
                        {
                            sw.WriteLine("\n  <Group id=\"" + rsb.rsgpInfo[i].ID + "\">");
                            int rback = rsb.rsgpInfo[i].offset;
                            using (BinaryStream bs_p0 = new BinaryStream())
                            {
                                using (BinaryStream bs_p1 = new BinaryStream())
                                {
                                    bs.Position = rback + rsb.rsgp[i].head.part0_Offset;
                                    using (BinaryStream bs_temp = new BinaryStream())
                                    {
                                        bs_temp.WriteBytes(bs.ReadBytes(rsb.rsgp[i].head.part0_ZSize));
                                        bs_temp.Position = 0;
                                        try
                                        {
                                            if (getlevel)
                                            {
                                                compresslevel = bs.PeekUInt16(Endian.Small) >> 14;
                                            }
                                            using (ZLibStream zLibStream = new ZLibStream(bs_temp, CompressionMode.Decompress, true))
                                            {
                                                zLibStream.CopyTo(bs_p0);
                                            }
                                            getlevel = false;
                                        }
                                        catch (Exception) //Someone (such as SmallPea) use a wrong compression flags on purpose. Maybe it isn't zlib compress file.
                                        {
                                            bs_temp.Position = 0;
                                            bs_p0.Position = 0;
                                            bs_temp.CopyTo(bs_p0);
                                        }
                                    }
                                    bs.Position = rback + rsb.rsgp[i].head.part1_Offset;
                                    using (BinaryStream bs_temp = new BinaryStream())
                                    {
                                        bs_temp.WriteBytes(bs.ReadBytes(rsb.rsgp[i].head.part1_ZSize));
                                        bs_temp.Position = 0;
                                        try
                                        {
                                            if (getlevel)
                                            {
                                                compresslevel = bs.PeekUInt16(Endian.Small) >> 14;
                                            }
                                            using (ZLibStream zLibStream = new ZLibStream(bs_temp, CompressionMode.Decompress, true))
                                            {
                                                zLibStream.CopyTo(bs_p1);
                                            }
                                            getlevel = false;
                                        }
                                        catch (Exception) //Someone (such as SmallPea) use a wrong compression flags on purpose. Maybe it isn't zlib compress file.
                                        {
                                            bs_temp.Position = 0;
                                            bs_p1.Position = 0;
                                            bs_temp.CopyTo(bs_p1);
                                        }
                                    }
                                    bs_p0.Position = 0;
                                    bs_p1.Position = 0;
                                    for (int xhi = 0; xhi < rsb.rsgp[i].fileList.Length; xhi++)
                                    {
                                        CompressString str = rsb.rsgp[i].fileList[xhi];
                                        string nname = Dir.FormatPath(outFolder + str.name);
                                        Dir.NewDir(nname, false);
                                        if (str.type == 1)
                                        {
                                            var p0ex = (RsgpPart0ExtraInfo)str.extraInfo;
                                            if (p0ex == null) throw new Exception();
                                            bs_p0.Position = p0ex.offset;
                                            using (BinaryStream bs4 = BinaryStream.Create(nname))
                                            {
                                                bs4.WriteBytes(bs_p0.ReadBytes(p0ex.size));
                                            }
                                            sw.WriteLine("    <Res id=\"" + str.name.Replace("&", "&amp;") + "\" />");
                                        }
                                        else if (str.type == 2)
                                        {
                                            var p1ex = (RsgpPart1ExtraInfo)str.extraInfo;
                                            if (p1ex == null) throw new Exception();
                                            var ptx = rsb.ptxInfo[rsb.rsgpInfo[i].ptx_BeforeNumber + p1ex.index];
                                            if (ptx == null) throw new Exception();
                                            bs_p1.Position = p1ex.offset;
                                            using (BinaryStream bs4 = BinaryStream.Create(nname))
                                            {
                                                bs4.Endian = bs.Endian;
                                                bs4.WriteInt32(1886681137);
                                                bs4.WriteInt32(1);
                                                bs4.WriteInt32(ptx.width);
                                                bs4.WriteInt32(ptx.height);
                                                bs4.WriteInt32(ptx.check);
                                                bs4.WriteInt32(ptx.format);
                                                bs4.WriteInt32(ptx.alphaSize);
                                                bs4.WriteInt32(ptx.alphaFormat);
                                                bs4.WriteBytes(bs_p1.ReadBytes(p1ex.size));
                                            }
                                            if (changeimage)
                                            {
                                                Image.Ptx.Ptx.Decode(nname, Path.ChangeExtension(nname, ".PNG"), true);
                                                if (delete) File.Delete(nname);
                                            }
                                            sw.WriteLine("    <Img id=\"" + str.name.Replace("&", "&amp;") + "\" defaultformat=\"" + ptx.format + "\" />");
                                        }
                                        else
                                        {
                                            throw new Exception();
                                        }
                                    }
                                }
                            }
                            sw.WriteLine("  </Group>");
                            GC.Collect();
                        }
                        sw.WriteLine("</ResourcesGroupInfo>");
                    }
                    string compresslevelinfo = getlevel ? "Optimal" : compresslevel switch
                    {
                        0 => "Fastest",
                        3 => "Smallest",
                        _ => "Optimal"
                    };
                    using (StreamWriter sw = new StreamWriter(lst + "PACKINFO.XML", false))
                    {
                        sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        sw.WriteLine("<!-- DO NOT EDIT THIS FILE. This file is generated by PopStudio. (unless you want to change the way for pack) -->");
                        sw.WriteLine("<PackInfo version=\"1\">");
                        sw.WriteLine("    <PackageVersion>" + rsb.head.version + "</PackageVersion>");
                        sw.WriteLine("    <UseBigEndian>" + (bs.Endian == Endian.Big) + "</UseBigEndian>");
                        sw.WriteLine("    <CompressMethod>" + (rsb.rsgp.Length > 0 ? (rsb.rsgp[0].head.flags & 0b11) : 1) + "</CompressMethod>");
                        sw.WriteLine("    <CompressLevel>" + compresslevelinfo + "</CompressLevel>");
                        sw.WriteLine("    <PtxInfoLength>" + rsb.head.ptxInfo_EachLength + "</PtxInfoLength>");
                        sw.WriteLine("    <ZlibAll>" + usesmf + "</ZlibAll>");
                        sw.WriteLine("</PackInfo>");
                    }
                }
            }
        }
    }
}