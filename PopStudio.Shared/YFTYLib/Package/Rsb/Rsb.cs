using System.Xml;

namespace PopStudio.Package.Rsb
{
    internal static class Rsb
    {
        static string autopoolname = "_AutoPool";

        public static void Pack(string inFolder, string outFile)
        {
            byte[] RsbPackBuffer = new byte[81920];
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
            uint ptxEachLength = 0x10;
            bool smf = false;
            bool usePoolXml = false;
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
                        uint flags = Convert.ToUInt32(child.InnerText);
                        compressPart0 = (flags & 0b10) != 0;
                        compressPart1 = (flags & 0b1) != 0;
                    }
                    else if (child.Name == "PtxInfoLength")
                    {
                        ptxEachLength = Convert.ToUInt32(child.InnerText);
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
                    else if (child.Name == "SpecialPool")
                    {
                        usePoolXml = Convert.ToBoolean(child.InnerText);
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
            uint ptxNumber = 0;
            List<RsbPtxInfo> ptxList = new List<RsbPtxInfo>();
            if (usePoolXml)
            {
                #region Use Special Pool
                xmldata = File.ReadAllText(studioPath + "POOL.XML");
                xml = new XmlDocument();
                xml.LoadXml(xmldata);
                root = xml.SelectSingleNode("/PoolInfo");
                childlist = root.ChildNodes;
                if (childlist == null) return;
                rsb.autopoolInfo = new RsbAutoPoolInfo[childlist.Count];
                rsb.head.autopool_Number = (uint)childlist.Count;
                for (int i = 0; i < childlist.Count; i++)
                {
                    var pool = new RsbAutoPoolInfo();
                    pool.ID = childlist[i].Attributes["id"]?.Value;
                    pool.type = Convert.ToInt32(childlist[i].Attributes["type"]?.Value);
                    pool.part1_MaxOffset_InDecompress = 0;
                    pool.part1_MaxSize = 0;
                    rsb.autopoolInfo[i] = pool;
                }
                xmldata = File.ReadAllText(studioPath + "RESOURCESGROUP.XML");
                xml = new XmlDocument();
                xml.LoadXml(xmldata);
                root = xml.SelectSingleNode("/ResourcesGroupInfo");
                childlist = root.ChildNodes;
                if (childlist == null) return;
                rsb.head.rsgp_Number = (uint)childlist.Count;
                rsb.rsgpInfo = new RsbRsgpInfo[childlist.Count];
                rsb.rsgp = new RsgpInfo[childlist.Count];
                uint f_global = 0;
                if (compressPart0)
                {
                    f_global |= 0b10;
                }
                if (compressPart1)
                {
                    f_global |= 0b1;
                }
                for (int i = 0; i < childlist.Count; i++)
                {
                    long off = bs_rsgpfile.Position;
                    uint thisPtxNumber = 0;
                    var rsgpinfo = rsb.rsgpInfo[i] = new RsbRsgpInfo();
                    uint f;
                    string tpstring = childlist[i].Attributes["compressmethod"]?.Value;
                    if (tpstring == null)
                    {
                        f = f_global;
                    }
                    else
                    {
                        f = Convert.ToUInt32(tpstring);
                    }
                    compressPart0 = (f & 0b10) != 0;
                    compressPart1 = (f & 0b01) != 0;
                    rsgpinfo.ID = childlist[i].Attributes["id"].Value;
                    rsgpinfo.pool_Index = Convert.ToUInt32(childlist[i].Attributes["poolindex"]?.Value ?? i.ToString());
                    rsgpinfo.flags = f;
                    rsgpinfo.offset = (uint)off;
                    var autopool = rsb.autopoolInfo[rsgpinfo.pool_Index];
                    var rsgp = rsb.rsgp[i] = new RsgpInfo();
                    rsgp.head = new RsgpHeadInfo();
                    rsgp.head.flags = f;
                    rsgp.head.version = version;
                    rsb.rsgpInfo[i].ptx_BeforeNumber = ptxNumber;
                    rsb.rsgpList.Add(new CompressString(rsgpinfo.ID.ToUpper(), new RsbExtraInfo((uint)i)));
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
                                        rsb.fileList.Add(new CompressString(fileid, new RsbExtraInfo((uint)i)));
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
                                                ptx.width = bsindexunknow.ReadUInt32();
                                                ptx.height = bsindexunknow.ReadUInt32();
                                                ptx.check = bsindexunknow.ReadUInt32();
                                                ptx.format = bsindexunknow.ReadUInt32();
                                                ptx.alphaSize = bsindexunknow.ReadUInt32();
                                                ptx.alphaFormat = bsindexunknow.ReadUInt32();
                                                rsgp.fileList.Add(new CompressString(fileid, new RsgpPart1ExtraInfo((uint)bsP1.Position, (uint)(bsindexunknow.Length - 0x20), thisPtxNumber++, ptx.width, ptx.height)));
                                                bsindexunknow.CopyTo(bsP1, RsbPackBuffer);
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
                                                rsgp.fileList.Add(new CompressString(fileid, new RsgpPart0ExtraInfo((uint)bsP0.Position, (uint)bsindexunknow.Length)));
                                                bsindexunknow.CopyTo(bsP0, RsbPackBuffer);
                                                bsP0.Length = FourK(bsP0.Position);
                                                bsP0.Position = bsP0.Length;
                                            }
                                        }
                                    }
                                    rsgpinfo.part0_Size = rsgpinfo.part0_Size2 = rsgp.head.part0_Size = (uint)bsP0.Length;
                                    rsgpinfo.part1_Size = rsgp.head.part1_Size = (uint)bsP1.Length;
                                    if (rsgp.head.part1_Size > autopool.part1_MaxSize)
                                    {
                                        autopool.part1_MaxSize = rsgp.head.part1_Size;
                                    }
                                    bsP0.Position = 0;
                                    bsP1.Position = 0;
                                    if (compressPart0)
                                    {
                                        using (ZLibStream zLibStream = new ZLibStream(bsP0over, level, true))
                                        {
                                            bsP0.CopyTo(zLibStream, RsbPackBuffer);
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
                                        bsP0.CopyTo(bsP0over, RsbPackBuffer);
                                    }
                                    if (compressPart1)
                                    {
                                        using (ZLibStream zLibStream = new ZLibStream(bsP1over, level, true))
                                        {
                                            bsP1.CopyTo(zLibStream, RsbPackBuffer);
                                        }
                                    }
                                    else
                                    {
                                        bsP1.CopyTo(bsP1over, RsbPackBuffer);
                                    }
                                    bsP0over.Length = FourK(bsP0over.Position);
                                    bsP1over.Length = FourK(bsP1over.Position);
                                    bsP0over.Position = 0;
                                    bsP1over.Position = 0;
                                }
                            }
                            rsgp.head.part0_ZSize = rsgpinfo.part0_ZSize = (uint)bsP0over.Length;
                            rsgp.head.part1_ZSize = rsgpinfo.part1_ZSize = (uint)bsP1over.Length;
                            bs_rsgpfile.Position = off + rsgp.head.fileList_BeginOffset;
                            byte[] list = rsgp.fileList.Write();
                            rsgp.head.fileList_Length = (uint)list.Length;
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
                            rsgp.head.fileOffset = rsgp.head.part0_Offset = rsgpinfo.fileOffset = rsgpinfo.part0_Offset = (uint)(bs_rsgpfile.Position - off);
                            bsP0over.CopyTo(bs_rsgpfile, RsbPackBuffer);
                            rsgp.head.part1_Offset = rsgpinfo.part1_Offset = (uint)(bs_rsgpfile.Position - off);
                            uint poolp1size = rsgp.head.part0_Offset + rsgp.head.part0_Size;
                            if (poolp1size > autopool.part1_MaxOffset_InDecompress)
                            {
                                autopool.part1_MaxOffset_InDecompress = poolp1size;
                            }
                            bsP1over.CopyTo(bs_rsgpfile, RsbPackBuffer);
                        }
                    }
                    //write head
                    bs_rsgpfile.Position = off;
                    rsgp.head.Write(bs_rsgpfile);
                    rsgpinfo.ptx_Number = thisPtxNumber;
                    ptxNumber += thisPtxNumber;
                    bs_rsgpfile.Position = bs_rsgpfile.Length;
                    rsb.rsgpInfo[i].size = (uint)(bs_rsgpfile.Length - off);
                    GC.Collect();
                }
                #endregion
            }
            else
            {
                #region Simple AutoPool
                xmldata = File.ReadAllText(studioPath + "RESOURCESGROUP.XML");
                xml = new XmlDocument();
                xml.LoadXml(xmldata);
                root = xml.SelectSingleNode("/ResourcesGroupInfo");
                childlist = root.ChildNodes;
                if (childlist == null) return;
                rsb.head.rsgp_Number = (uint)childlist.Count;
                rsb.rsgpInfo = new RsbRsgpInfo[childlist.Count];
                rsb.rsgp = new RsgpInfo[childlist.Count];
                rsb.head.autopool_Number = (uint)childlist.Count;
                rsb.autopoolInfo = new RsbAutoPoolInfo[childlist.Count];
                uint f_global = 0;
                if (compressPart0)
                {
                    f_global |= 0b10;
                }
                if (compressPart1)
                {
                    f_global |= 0b1;
                }
                for (int i = 0; i < childlist.Count; i++)
                {
                    long off = bs_rsgpfile.Position;
                    uint thisPtxNumber = 0;
                    var rsgpinfo = rsb.rsgpInfo[i] = new RsbRsgpInfo();
                    uint f;
                    string tpstring = childlist[i].Attributes["compressmethod"]?.Value;
                    if (tpstring == null)
                    {
                        f = f_global;
                    }
                    else
                    {
                        f = Convert.ToUInt32(tpstring);
                    }
                    compressPart0 = (f & 0b10) != 0;
                    compressPart1 = (f & 0b01) != 0;
                    rsgpinfo.ID = childlist[i].Attributes["id"].Value;
                    rsgpinfo.pool_Index = (uint)i;
                    rsgpinfo.flags = f;
                    rsgpinfo.offset = (uint)off;
                    var autopool = rsb.autopoolInfo[i] = new RsbAutoPoolInfo();
                    autopool.ID = rsgpinfo.ID + autopoolname;
                    var rsgp = rsb.rsgp[i] = new RsgpInfo();
                    rsgp.head = new RsgpHeadInfo();
                    rsgp.head.flags = f;
                    rsgp.head.version = version;
                    rsb.rsgpInfo[i].ptx_BeforeNumber = ptxNumber;
                    rsb.rsgpList.Add(new CompressString(rsgpinfo.ID.ToUpper(), new RsbExtraInfo((uint)i)));
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
                                        rsb.fileList.Add(new CompressString(fileid, new RsbExtraInfo((uint)i)));
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
                                                ptx.width = bsindexunknow.ReadUInt32();
                                                ptx.height = bsindexunknow.ReadUInt32();
                                                ptx.check = bsindexunknow.ReadUInt32();
                                                ptx.format = bsindexunknow.ReadUInt32();
                                                ptx.alphaSize = bsindexunknow.ReadUInt32();
                                                ptx.alphaFormat = bsindexunknow.ReadUInt32();
                                                rsgp.fileList.Add(new CompressString(fileid, new RsgpPart1ExtraInfo((uint)bsP1.Position, (uint)(bsindexunknow.Length - 0x20), thisPtxNumber++, ptx.width, ptx.height)));
                                                bsindexunknow.CopyTo(bsP1, RsbPackBuffer);
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
                                                rsgp.fileList.Add(new CompressString(fileid, new RsgpPart0ExtraInfo((uint)bsP0.Position, (uint)bsindexunknow.Length)));
                                                bsindexunknow.CopyTo(bsP0, RsbPackBuffer);
                                                bsP0.Length = FourK(bsP0.Position);
                                                bsP0.Position = bsP0.Length;
                                            }
                                        }
                                    }
                                    rsgpinfo.part0_Size = rsgpinfo.part0_Size2 = rsgp.head.part0_Size = (uint)bsP0.Length;
                                    rsgpinfo.part1_Size = autopool.part1_MaxSize = rsgp.head.part1_Size = (uint)bsP1.Length;
                                    bsP0.Position = 0;
                                    bsP1.Position = 0;
                                    if (compressPart0)
                                    {
                                        using (ZLibStream zLibStream = new ZLibStream(bsP0over, level, true))
                                        {
                                            bsP0.CopyTo(zLibStream, RsbPackBuffer);
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
                                        bsP0.CopyTo(bsP0over, RsbPackBuffer);
                                    }
                                    if (compressPart1)
                                    {
                                        using (ZLibStream zLibStream = new ZLibStream(bsP1over, level, true))
                                        {
                                            bsP1.CopyTo(zLibStream, RsbPackBuffer);
                                        }
                                    }
                                    else
                                    {
                                        bsP1.CopyTo(bsP1over, RsbPackBuffer);
                                    }
                                    bsP0over.Length = FourK(bsP0over.Position);
                                    bsP1over.Length = FourK(bsP1over.Position);
                                    bsP0over.Position = 0;
                                    bsP1over.Position = 0;
                                }
                            }
                            rsgp.head.part0_ZSize = rsgpinfo.part0_ZSize = (uint)bsP0over.Length;
                            rsgp.head.part1_ZSize = rsgpinfo.part1_ZSize = (uint)bsP1over.Length;
                            bs_rsgpfile.Position = off + rsgp.head.fileList_BeginOffset;
                            byte[] list = rsgp.fileList.Write();
                            rsgp.head.fileList_Length = (uint)list.Length;
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
                            rsgp.head.fileOffset = rsgp.head.part0_Offset = rsgpinfo.fileOffset = rsgpinfo.part0_Offset = (uint)(bs_rsgpfile.Position - off);
                            bsP0over.CopyTo(bs_rsgpfile, RsbPackBuffer);
                            rsgp.head.part1_Offset = rsgpinfo.part1_Offset = (uint)(bs_rsgpfile.Position - off);
                            autopool.part1_MaxOffset_InDecompress = rsgp.head.part0_Offset + rsgp.head.part0_Size; //Not zsize
                            bsP1over.CopyTo(bs_rsgpfile, RsbPackBuffer);
                        }
                    }
                    //write head
                    bs_rsgpfile.Position = off;
                    rsgp.head.Write(bs_rsgpfile);
                    rsgpinfo.ptx_Number = thisPtxNumber;
                    ptxNumber += thisPtxNumber;
                    bs_rsgpfile.Position = bs_rsgpfile.Length;
                    rsb.rsgpInfo[i].size = (uint)(bs_rsgpfile.Length - off);
                    GC.Collect();
                }
                #endregion
            }
            rsb.head.ptx_Number = ptxNumber;
            string mfile = outFile;
            if (smf) mfile = tempFilePool.Add();
            using (BinaryStream bs = new BinaryStream(mfile, FileMode.Create))
            {
                bs.Endian = endian;
                rsb.head.Write(bs);
                byte[] fileList = rsb.fileList.Write();
                rsb.head.fileList_Length = (uint)fileList.Length;
                rsb.head.fileList_BeginOffset = (uint)bs.Position;
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
                rsb.head.rsgpList_Length = (uint)rsgpList.Length;
                rsb.head.rsgpList_BeginOffset = (uint)bs.Position;
                using (BinaryStream bs_list = new BinaryStream(rsgpList))
                {
                    int times = rsgpList.Length >> 2;
                    for (int i_xj = 0; i_xj < times; i_xj++)
                    {
                        bs.WriteInt32(bs_list.ReadInt32());
                    }
                }
                rsgpList = null;
                rsb.head.compositeInfo_BeginOffset = (uint)bs.Position;
                xmldata = File.ReadAllText(studioPath + "COMPOSITERESOURCES.XML");
                xml = new XmlDocument();
                xml.LoadXml(xmldata);
                root = xml.SelectSingleNode("/CompositeResourcesInfo");
                childlist = root.ChildNodes;
                if (childlist == null) return;
                rsb.compositeInfo = new RsbCompositeInfo[childlist.Count];
                rsb.head.composite_Number = (uint)childlist.Count;
                for (int i = 0; i < childlist.Count; i++)
                {
                    rsb.compositeInfo[i] = new RsbCompositeInfo();
                    rsb.compositeInfo[i].ID = childlist[i].Attributes["id"].Value;
                    var childchildlist = childlist[i].ChildNodes;
                    int num = childchildlist.Count;
                    rsb.compositeInfo[i].child_Number = (uint)num;
                    rsb.compositeList.Add(new CompressString(rsb.compositeInfo[i].ID.ToUpper(), new RsbExtraInfo((uint)i)));
                    for (int j = 0; j < num; j++)
                    {
                        rsb.compositeInfo[i].child_Info[j].index = Convert.ToUInt32(childchildlist[j].Attributes["index"].Value);
                        rsb.compositeInfo[i].child_Info[j].ratio = Convert.ToUInt32(childchildlist[j].Attributes["res"].Value);
                        rsb.compositeInfo[i].child_Info[j].language = childchildlist[j].Attributes["loc"].Value;
                    }
                    rsb.compositeInfo[i].Write(bs);
                }
                byte[] compositeList = rsb.compositeList.Write();
                rsb.head.compositeList_Length = (uint)compositeList.Length;
                rsb.head.compositeList_BeginOffset = (uint)bs.Position;
                using (BinaryStream bs_list = new BinaryStream(compositeList))
                {
                    int times = compositeList.Length >> 2;
                    for (int i_xj = 0; i_xj < times; i_xj++)
                    {
                        bs.WriteInt32(bs_list.ReadInt32());
                    }
                }
                compositeList = null;
                rsb.head.rsgpInfo_BeginOffset = (uint)bs.Position;
                bs.Length = bs.Position + RsbHeadInfo.rsgpInfo_EachLength * rsb.head.rsgp_Number;
                bs.Position = bs.Length;
                rsb.head.autopoolInfo_BeginOffset = (uint)bs.Position;
                for (int i = 0; i < rsb.head.autopool_Number; i++)
                {
                    rsb.autopoolInfo[i].Write(bs);
                }
                rsb.head.ptxInfo_BeginOffset = (uint)bs.Position;
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
                        rsb.head.xmlPart1_BeginOffset = (uint)bs.Position;
                        int k = bs_xml.ReadInt32();
                        rsb.head.xmlPart2_BeginOffset = (uint)(rsb.head.xmlPart1_BeginOffset + bs_xml.ReadInt32() - k);
                        rsb.head.xmlPart3_BeginOffset = (uint)(rsb.head.xmlPart1_BeginOffset + bs_xml.ReadInt32() - k);
                        bs_xml.CopyTo(bs, RsbPackBuffer);
                    }
                }
                bs.Length = FourK(bs.Position);
                bs.Position = bs.Length;
                rsb.head.headLength = (uint)bs.Position;
                for (int i = 0; i < rsb.head.rsgp_Number; i++)
                {
                    rsb.rsgpInfo[i].offset += rsb.head.headLength;
                }
                bs_rsgpfile.Position = 0;
                bs_rsgpfile.CopyTo(bs, RsbPackBuffer);
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
                            bs.CopyTo(zLibStream, RsbPackBuffer);
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
            string inFile_Folder = Path.GetDirectoryName(inFile) + Const.PATHSEPARATOR;
            if (!File.Exists(inFile))
            {
                throw new Exception(string.Format(Str.Obj.FileNotFound, inFile));
            }
            Dir.NewDir(outFolder);
            outFolder += Const.PATHSEPARATOR;
            uint? CompressMethod = null;
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
                    //ReadSubInfo
                    //rsb.rsgp = new RsgpInfo[rsb.head.rsgp_Number];
                    //for (int i = 0; i < rsb.rsgpInfo.Length; i++)
                    //{
                    //    bs.Position = rsb.rsgpInfo[i].offset;
                    //    rsb.rsgp[i] = new RsgpInfo().Read(bs);
                    //}
                    //We need to test if it's necessary to use Pool.xml
                    bool usePoolXml = false;
                    if (rsb.rsgpInfo.Length != rsb.autopoolInfo.Length)
                    {
                        usePoolXml = true;
                    }
                    else
                    {
                        for (int rsgp_info_index = 0; rsgp_info_index < rsb.rsgpInfo.Length; rsgp_info_index++)
                        {
                            if (rsb.rsgpInfo[rsgp_info_index].pool_Index != rsgp_info_index || rsb.autopoolInfo[rsgp_info_index].type != 1)
                            {
                                usePoolXml = true;
                                break;
                            }
                        }
                    }
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
                            bs2.WriteInt32((int)(rsb.head.xmlPart2_BeginOffset - rsb.head.xmlPart1_BeginOffset + 0x14));
                            bs2.WriteInt32((int)(rsb.head.xmlPart3_BeginOffset - rsb.head.xmlPart1_BeginOffset + 0x14));
                            bs.Position = rsb.head.xmlPart1_BeginOffset;
                            bs2.WriteBytes(bs.ReadBytes((int)(rsb.head.headLength - rsb.head.xmlPart1_BeginOffset)));
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
                            sw.Write($"\n  <Group id=\"{rsb.rsgpInfo[i].ID}\" compressmethod=\"{rsb.rsgpInfo[i].flags}\"");
                            if (usePoolXml)
                            {
                                sw.Write($" poolindex=\"{rsb.rsgpInfo[i].pool_Index}\"");
                            }
                            sw.WriteLine(">");
                            RsgpInfo rsgp = null;
                            uint rback = rsb.rsgpInfo[i].offset;
                            BinaryStream innerRsgpFile = null;
                            try
                            {
                                try
                                {
                                    bs.Position = rback;
                                    rsgp = new RsgpInfo().Read(bs);
                                }
                                catch (Exception)
                                {
                                    // rsgp is out of rsb
                                    string guess_rsg_name = inFile_Folder + rsb.rsgpInfo[i].ID;
                                    if (!File.Exists(guess_rsg_name))
                                    {
                                        guess_rsg_name += ".rsg";
                                        if (!File.Exists(guess_rsg_name))
                                        {
                                            string guess_rsg_name_2 = guess_rsg_name + "p";
                                            if (!File.Exists(guess_rsg_name_2))
                                            {
                                                guess_rsg_name += ".smf";
                                                if (!File.Exists(guess_rsg_name))
                                                {
                                                    guess_rsg_name = guess_rsg_name_2 + ".smf";
                                                    if (!File.Exists(guess_rsg_name))
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                guess_rsg_name = guess_rsg_name_2;
                                            }
                                        }
                                    }
                                    rback = 0;
                                    innerRsgpFile = new BinaryStream(guess_rsg_name, FileMode.Open);
                                    innerRsgpFile.Position = rback;
                                    int headmagic = innerRsgpFile.PeekInt32();
                                    if (headmagic == 1885827954)
                                    {
                                        innerRsgpFile.Endian = Endian.Big;
                                    }
                                    else if (headmagic == 1920165744)
                                    {
                                        innerRsgpFile.Endian = Endian.Small;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    rsgp = new RsgpInfo().Read(innerRsgpFile);
                                }
                                BinaryStream bs_used = innerRsgpFile ?? bs;
                                CompressMethod ??= rsgp.head.flags & 3;
                                using (BinaryStream bs_p0 = new BinaryStream())
                                {
                                    using (BinaryStream bs_p1 = new BinaryStream())
                                    {
                                        bs_used.Position = rback + rsgp.head.part0_Offset;
                                        using (BinaryStream bs_temp = new BinaryStream())
                                        {
                                            bs_temp.WriteBytes(bs_used.ReadBytes((int)rsgp.head.part0_ZSize));
                                            bs_temp.Position = 0;
                                            try
                                            {
                                                if (getlevel)
                                                {
                                                    compresslevel = bs_temp.PeekUInt16(Endian.Small) >> 14;
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
                                        bs_used.Position = rback + rsgp.head.part1_Offset;
                                        using (BinaryStream bs_temp = new BinaryStream())
                                        {
                                            bs_temp.WriteBytes(bs_used.ReadBytes((int)rsgp.head.part1_ZSize));
                                            bs_temp.Position = 0;
                                            try
                                            {
                                                if (getlevel)
                                                {
                                                    compresslevel = bs_temp.PeekUInt16(Endian.Small) >> 14;
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
                                        for (int xhi = 0; xhi < rsgp.fileList.Length; xhi++)
                                        {
                                            CompressString str = rsgp.fileList[xhi];
                                            string nname = Dir.FormatPath(outFolder + str.name);
                                            Dir.NewDir(nname, false);
                                            if (str.type == 1)
                                            {
                                                var p0ex = (RsgpPart0ExtraInfo)str.extraInfo;
                                                if (p0ex == null) throw new Exception();
                                                bs_p0.Position = p0ex.offset;
                                                using (BinaryStream bs4 = BinaryStream.Create(nname))
                                                {
                                                    bs4.WriteBytes(bs_p0.ReadBytes((int)p0ex.size));
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
                                                    bs4.Endian = bs_used.Endian;
                                                    bs4.WriteInt32(1886681137);
                                                    bs4.WriteInt32(1);
                                                    bs4.WriteUInt32(ptx.width);
                                                    bs4.WriteUInt32(ptx.height);
                                                    bs4.WriteUInt32(ptx.check);
                                                    bs4.WriteUInt32(ptx.format);
                                                    bs4.WriteUInt32(ptx.alphaSize);
                                                    bs4.WriteUInt32(ptx.alphaFormat);
                                                    bs4.WriteBytes(bs_p1.ReadBytes((int)p1ex.size));
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
                            }
                            finally
                            {
                                if (innerRsgpFile != null)
                                {
                                    innerRsgpFile.Dispose();
                                }
                                GC.Collect();
                            }
                        }
                        sw.WriteLine("</ResourcesGroupInfo>");
                    }
                    if (usePoolXml)
                    {
                        using (StreamWriter sw = new StreamWriter(lst + "POOL.XML", false))
                        {
                            sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                            sw.WriteLine("<!-- DO NOT EDIT THIS FILE. This file is generated by PopStudio. You can see this file only by special AutoPool parts. -->");
                            sw.WriteLine("<PoolInfo>");
                            for (int i = 0; i < rsb.autopoolInfo.Length; i++)
                            {
                                sw.WriteLine($"  <Pool id=\"{rsb.autopoolInfo[i].ID.Replace("&", "&amp;")}\" type=\"{rsb.autopoolInfo[i].type}\" />");
                            }
                            sw.WriteLine("</PoolInfo>");
                        }
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
                        sw.WriteLine("    <CompressMethod>" + (CompressMethod ?? 1) + "</CompressMethod>");
                        sw.WriteLine("    <CompressLevel>" + compresslevelinfo + "</CompressLevel>");
                        sw.WriteLine("    <PtxInfoLength>" + rsb.head.ptxInfo_EachLength + "</PtxInfoLength>");
                        sw.WriteLine("    <ZlibAll>" + usesmf + "</ZlibAll>");
                        sw.WriteLine("    <SpecialPool>" + usePoolXml + "</SpecialPool>");
                        sw.WriteLine("</PackInfo>");
                    }
                }
            }
        }
    }
}