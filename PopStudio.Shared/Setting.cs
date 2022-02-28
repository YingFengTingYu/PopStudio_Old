using System.Xml;

namespace PopStudio
{
    internal static class Setting
    {
        public static readonly string DefaultXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!--Describe some default behaviour-->
<Description version=""2"">
    <Dz>
        <!--The compressing method to pack dz-->
        <CompressMethod>
            <Default value=""Lzma"" />
            <Extension name="".png"" value=""Store"" />
            <Extension name="".jpg"" value=""Store"" />
            <Extension name="".compiled"" value=""Store"" />
            <Extension name="".txt"" value=""Gzip"" />
        </CompressMethod>
    </Dz>
    <PakPS3>
        <!--The compressing method to pack pak in ps3-->
        <CompressMethod>
            <Default value=""Store"" />
            <Extension name="".ptx"" value=""Zlib"" />
            <Extension name="".compiled"" value=""Zlib"" />
            <Extension name="".txt"" value=""Zlib"" />
            <Extension name="".xml"" value=""Zlib"" />
            <Extension name="".reanim"" value=""Zlib"" />
        </CompressMethod>
    </PakPS3>
    <Rsb>
        <Ptx>
            <!--Whether the texture 0 format ptx used ABGR8888 texture for decoding and encoding ptx when unpacking or packing rsb-->
            <ABGR8888Mode value=""False"" />
            <!--Whether the texture 0 format ptx used ABGR8888_Padding texture for encoding ptx when packing rsb-->
            <ARGB8888PaddingMode value=""False"" />
        </Ptx>
    </Rsb>
    <Cdat>
        <!--The key to decrypt and encrypt cdat in PVZ Free-->
        <Key value=""AS23DSREPLKL335KO4439032N8345NF"" />
    </Cdat>
    <Ptx>
        <!--Whether the texture 0 format ptx used ABGR8888 texture for decoding and encoding ptx-->
        <ABGR8888Mode value=""False"" />
    </Ptx>
    <RTON>
        <!--The key to decrypt and encrypt RTON in PVZ2(The default key is wrong and you need to fix it there)-->
        <Key value="""" />
    </RTON>
    <ImageString name=""PopStudio Example"">
        <!--Convert image integer to string or string to integer in reanim, particles and trail. Find the answer from libpvz.so or PVZ.s3e!-->
        <String id=""PopStudioExample"" value=""99999"" />
    </ImageString>
</Description>";

        public static readonly Dictionary<P_Package.Dz.CompressFlags, string> DzCompressMethodName = new Dictionary<P_Package.Dz.CompressFlags, string> { { P_Package.Dz.CompressFlags.ZLIB, "Gzip" }, { P_Package.Dz.CompressFlags.LZMA, "Lzma" }, { P_Package.Dz.CompressFlags.STORE, "Store" }, { P_Package.Dz.CompressFlags.BZIP, "Bzip2" } };
        public static readonly Dictionary<P_Package.Pak.CompressFlags, string> PakPS3CompressMethodName = new Dictionary<P_Package.Pak.CompressFlags, string> { { P_Package.Pak.CompressFlags.ZLIB, "Zlib" }, { P_Package.Pak.CompressFlags.STORE, "Store" } };

        public static readonly Dictionary<string, P_Package.Dz.CompressFlags> DzCompressMethodEnum = new Dictionary<string, P_Package.Dz.CompressFlags> { { "Gzip", P_Package.Dz.CompressFlags.ZLIB }, { "Lzma", P_Package.Dz.CompressFlags.LZMA }, { "Store", P_Package.Dz.CompressFlags.STORE }, { "Bzip2", P_Package.Dz.CompressFlags.BZIP } };
        public static readonly Dictionary<string, P_Package.Pak.CompressFlags> PakPS3CompressMethodEnum = new Dictionary<string, P_Package.Pak.CompressFlags> { { "Zlib", P_Package.Pak.CompressFlags.ZLIB }, { "Store", P_Package.Pak.CompressFlags.STORE } };

        public static void ResetXml(string xmlPath)
        {
            using (StreamWriter sw = new StreamWriter(xmlPath, false))
            {
                sw.Write(DefaultXml);
            }
            LoadFromXml(xmlPath);
        }

        public static void LoadFromXml(string xmlPath)
        {
            try
            {
                string xmlData;
                using (StreamReader sr = new StreamReader(xmlPath))
                {
                    xmlData = sr.ReadToEnd();
                }
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlData);
                XmlNode root = xml.SelectSingleNode("/Description");
                XmlNodeList childlist = root.ChildNodes;
                foreach (XmlNode child in childlist)
                {
                    switch (child.Name)
                    {
                        case "Dz":
                            {
                                XmlNodeList childchildlist = child.ChildNodes;
                                foreach (XmlNode childchild in childchildlist)
                                {
                                    switch (childchild.Name)
                                    {
                                        case "CompressMethod":
                                            {
                                                DzCompressDictionary.Clear();
                                                XmlNodeList childchildchildlist = childchild.ChildNodes;
                                                foreach (XmlNode childchildchild in childchildchildlist)
                                                {
                                                    switch (childchildchild.Name)
                                                    {
                                                        case "Default":
                                                            DzDefaultCompressMethod = DzCompressMethodEnum[childchildchild.Attributes["value"].Value];
                                                            break;
                                                        case "Extension":
                                                            DzCompressDictionary.Add(childchildchild.Attributes["name"].Value, DzCompressMethodEnum[childchildchild.Attributes["value"].Value]);
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "PakPS3":
                            {
                                XmlNodeList childchildlist = child.ChildNodes;
                                foreach (XmlNode childchild in childchildlist)
                                {
                                    switch (childchild.Name)
                                    {
                                        case "CompressMethod":
                                            {
                                                PakPS3CompressDictionary.Clear();
                                                XmlNodeList childchildchildlist = childchild.ChildNodes;
                                                foreach (XmlNode childchildchild in childchildchildlist)
                                                {
                                                    switch (childchildchild.Name)
                                                    {
                                                        case "Default":
                                                            PakPS3DefaultCompressMethod = PakPS3CompressMethodEnum[childchildchild.Attributes["value"].Value];
                                                            break;
                                                        case "Extension":
                                                            PakPS3CompressDictionary.Add(childchildchild.Attributes["name"].Value, PakPS3CompressMethodEnum[childchildchild.Attributes["value"].Value]);
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "Rsb":
                            {
                                XmlNodeList childchildlist = child.ChildNodes;
                                foreach (XmlNode childchild in childchildlist)
                                {
                                    switch (childchild.Name)
                                    {
                                        case "Ptx":
                                            {
                                                XmlNodeList childchildchildlist = childchild.ChildNodes;
                                                foreach (XmlNode childchildchild in childchildchildlist)
                                                {
                                                    switch (childchildchild.Name)
                                                    {
                                                        case "ABGR8888Mode":
                                                            RsbPtxABGR8888Mode = Convert.ToBoolean(childchildchild.Attributes["value"].Value);
                                                            break;
                                                        case "ARGB8888PaddingMode":
                                                            RsbPtxARGB8888PaddingMode = Convert.ToBoolean(childchildchild.Attributes["value"].Value);
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "Cdat":
                            {
                                XmlNodeList childchildlist = child.ChildNodes;
                                foreach (XmlNode childchild in childchildlist)
                                {
                                    switch (childchild.Name)
                                    {
                                        case "Key":
                                            {
                                                CdatKey = childchild.Attributes["value"].Value;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "Ptx":
                            {
                                XmlNodeList childchildlist = child.ChildNodes;
                                foreach (XmlNode childchild in childchildlist)
                                {
                                    switch (childchild.Name)
                                    {
                                        case "ABGR8888Mode":
                                            {
                                                PtxABGR8888Mode = Convert.ToBoolean(childchild.Attributes["value"].Value);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "RTON":
                            {
                                XmlNodeList childchildlist = child.ChildNodes;
                                foreach (XmlNode childchild in childchildlist)
                                {
                                    switch (childchild.Name)
                                    {
                                        case "Key":
                                            {
                                                RTONKey = childchild.Attributes["value"].Value;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "ImageString":
                            {
                                ImageConvertIntegerToString.Clear();
                                ImageConvertStringToInteger.Clear();
                                ImageConvertName = child.Attributes["name"]?.Value ?? string.Empty;
                                XmlNodeList childchildlist = child.ChildNodes;
                                foreach (XmlNode childchild in childchildlist)
                                {
                                    switch (childchild.Name)
                                    {
                                        case "String":
                                            {
                                                int value = Convert.ToInt32(childchild.Attributes["value"].Value);
                                                string id = childchild.Attributes["id"].Value;
                                                ImageConvertIntegerToString.Add(value, id);
                                                ImageConvertStringToInteger.Add(id, value);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                SaveAsXml(xmlPath);
            }
        }

        public static void SaveAsXml(string xmlPath)
        {
            using (StreamWriter sw = new StreamWriter(xmlPath, false))
            {
                sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<!--Describe some default behaviour-->\n<Description version=\"2\">\n    <Dz>\n        <!--The compressing method to pack dz-->\n        <CompressMethod>\n            <Default value=\"");
                sw.Write(DzCompressMethodName[DzDefaultCompressMethod]);
                sw.Write("\" />");
                foreach (KeyValuePair<string, P_Package.Dz.CompressFlags> keyValuePair in DzCompressDictionary)
                {
                    sw.Write("\n            <Extension name=\"");
                    sw.Write(keyValuePair.Key);
                    sw.Write("\" value=\"");
                    sw.Write(DzCompressMethodName[keyValuePair.Value]);
                    sw.Write("\" />");
                }
                sw.Write("\n        </CompressMethod>\n    </Dz>\n    <PakPS3>\n        <!--The compressing method to pack pak in ps3-->\n        <CompressMethod>\n            <Default value=\"");
                sw.Write(PakPS3CompressMethodName[PakPS3DefaultCompressMethod]);
                sw.Write("\" />");
                foreach (KeyValuePair<string, P_Package.Pak.CompressFlags> keyValuePair in PakPS3CompressDictionary)
                {
                    sw.Write("\n            <Extension name=\"");
                    sw.Write(keyValuePair.Key);
                    sw.Write("\" value=\"");
                    sw.Write(PakPS3CompressMethodName[keyValuePair.Value]);
                    sw.Write("\" />");
                }
                sw.Write("\n        </CompressMethod>\n    </PakPS3>\n    <Rsb>\n        <Ptx>\n            <!--Whether the texture 0 format ptx used ABGR8888 texture for decoding and encoding ptx when unpacking or packing rsb-->\n            <ABGR8888Mode value=\"");
                sw.Write(RsbPtxABGR8888Mode);
                sw.Write("\" />\n            <!--Whether the texture 0 format ptx used ABGR8888_Padding texture for encoding ptx when packing rsb-->\n            <ARGB8888PaddingMode value=\"");
                sw.Write(RsbPtxARGB8888PaddingMode);
                sw.Write("\" />\n        </Ptx>\n    </Rsb>\n    <Cdat>\n        <!--The key to decrypt and encrypt cdat in PVZ Free-->\n        <Key value=\"");
                sw.Write(CdatKey);
                sw.Write("\" />\n    </Cdat>\n    <Ptx>\n        <!--Whether the texture 0 format ptx used ABGR8888 texture for decoding and encoding ptx-->\n        <ABGR8888Mode value=\"");
                sw.Write(PtxABGR8888Mode);
                sw.Write("\" />\n    </Ptx>\n    <RTON>\n        <!--The key to decrypt and encrypt RTON in PVZ2(The default key is wrong and you need to fix it there)-->\n        <Key value=\"");
                sw.Write(RTONKey);
                sw.Write("\" />\n    </RTON>\n    <ImageString name=\"");
                sw.Write(ImageConvertName);
                sw.Write("\">\n        <!--Convert image integer to string or string to integer in reanim, particles and trail. Find the answer from libpvz.so or PVZ.s3e!-->");
                foreach (KeyValuePair<string, int> keyValuePair in ImageConvertStringToInteger)
                {
                    sw.Write("\n        <String id=\"");
                    sw.Write(keyValuePair.Key);
                    sw.Write("\" value=\"");
                    sw.Write(keyValuePair.Value);
                    sw.Write("\" />");
                }
                sw.Write("\n    </ImageString>\n</Description>");
            }
        }

        /// <summary>
        /// dz pack
        /// </summary>
        public static Dictionary<string, P_Package.Dz.CompressFlags> DzCompressDictionary = new Dictionary<string, P_Package.Dz.CompressFlags> { { ".png", P_Package.Dz.CompressFlags.STORE }, { ".jpg", P_Package.Dz.CompressFlags.STORE }, { ".compiled", P_Package.Dz.CompressFlags.STORE }, { ".txt", P_Package.Dz.CompressFlags.ZLIB } };
        public static P_Package.Dz.CompressFlags DzDefaultCompressMethod = P_Package.Dz.CompressFlags.LZMA;

        /// <summary>
        /// pak ps3 pack
        /// </summary>
        public static Dictionary<string, P_Package.Pak.CompressFlags> PakPS3CompressDictionary = new Dictionary<string, P_Package.Pak.CompressFlags> { { ".ptx", P_Package.Pak.CompressFlags.ZLIB }, { ".compiled", P_Package.Pak.CompressFlags.ZLIB }, { ".txt", P_Package.Pak.CompressFlags.ZLIB }, { ".xml", P_Package.Pak.CompressFlags.ZLIB }, { ".reanim", P_Package.Pak.CompressFlags.ZLIB } };
        public static P_Package.Pak.CompressFlags PakPS3DefaultCompressMethod = P_Package.Pak.CompressFlags.STORE;

        /// <summary>
        /// rsb ptx code
        /// </summary>
        public static bool RsbPtxABGR8888Mode = false;
        public static bool RsbPtxARGB8888PaddingMode = false;

        /// <summary>
        /// PVZ Free cdat key
        /// </summary>
        public static string CdatKey = "AS23DSREPLKL335KO4439032N8345NF";

        /// <summary>
        /// ptx decode
        /// </summary>
        public static bool PtxABGR8888Mode = false;

        /// <summary>
        /// PVZ2 RTON key (Should be enterred by yourself)
        /// </summary>
        public static string RTONKey = string.Empty;

        /// <summary>
        /// PVZ android, iOS, bada and blackberry reanim, particles and trail image integer and string converter
        /// </summary>
        public static string ImageConvertName = "PopStudio Example";
        public static Dictionary<string, int> ImageConvertStringToInteger = new Dictionary<string, int> { { "PopStudioExample", 99999 } };
        public static Dictionary<int, string> ImageConvertIntegerToString = new Dictionary<int, string> { { 99999, "PopStudioExample" } };

        public static void ClearImageConvertXml()
        {
            ImageConvertName = "Null";
            ImageConvertIntegerToString.Clear();
            ImageConvertStringToInteger.Clear();
        }

        public static void LoadImageConvertXml(string xmlPath)
        {
            string xmlData;
            using (StreamReader sr = new StreamReader(xmlPath))
            {
                xmlData = sr.ReadToEnd();
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlData);
            XmlNode root = xml.SelectSingleNode("/ImageString");
            XmlNodeList childlist = root.ChildNodes;
            ImageConvertIntegerToString.Clear();
            ImageConvertStringToInteger.Clear();
            ImageConvertName = root.Attributes["name"].Value;
            foreach (XmlNode child in childlist)
            {
                switch (child.Name)
                {
                    case "String":
                        {
                            int value = Convert.ToInt32(child.Attributes["value"].Value);
                            string id = child.Attributes["id"].Value;
                            ImageConvertIntegerToString.Add(value, id);
                            ImageConvertStringToInteger.Add(id, value);
                        }
                        break;
                }
            }
        }

        public static object GetImageStringFromInteger(object o)
        {
            if ((o is int a) && ImageConvertIntegerToString.ContainsKey(a))
            {
                return ImageConvertIntegerToString[a];
            }
            return o;
        }

        public static object GetImageIntegerFromString(object o)
        {
            if (o == null) return -1;
            if (o is string os)
            {
                if (ImageConvertStringToInteger.ContainsKey(os))
                {
                    return ImageConvertStringToInteger[os];
                }
                return -1;
            }
            return o;
        }
    }
}
