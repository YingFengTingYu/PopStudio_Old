using System.Xml;

namespace PopStudio
{
    internal static class Setting
    {
        public static bool CanSave = true;
        public static Constant.Language AppLanguage = System.Globalization.CultureInfo.CurrentCulture.Name.ToLower() switch
        {
            "zh-cn" => Constant.Language.ZHCN,
            "zh-hans-cn" => Constant.Language.ZHCN,
            _ => Constant.Language.ENUS
        };

        public static readonly Dictionary<string, Constant.Language> LanguageEnum = new Dictionary<string, Constant.Language> { { "English", Constant.Language.ENUS }, { "\u7B80\u4F53\u4E2D\u6587", Constant.Language.ZHCN } };
        public static readonly Dictionary<Constant.Language, string> LanguageName = new Dictionary<Constant.Language, string> { { Constant.Language.ENUS, "English" }, { Constant.Language.ZHCN, "\u7B80\u4F53\u4E2D\u6587" } };

        public static readonly string DefaultXml_P1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!--Describe some default behaviour-->
<Description version=""2"">
    <Language>";
        public static readonly string DefaultXml_P2 = @"</Language>
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
            <!--Whether the texture 0 format ptx used ABGR8888_Padding texture for decoding and encoding ptx when unpacking or packing rsb-->
            <ARGB8888PaddingMode value=""False"" />
        </Ptx>
    </Rsb>
    <Cdat>
        <!--The key to decrypt and encrypt cdat in PVZ Free-->
        <Key value=""AS23DSREPLKL335KO4439032N8345NF"" />
    </Cdat>
    <Ptx>
        <!--Whether the texture 0 format ptx used ABGR8888 texture for decoding ptx-->
        <ABGR8888Mode value=""False"" />
        <!--Whether the texture 0 format ptx used ABGR8888_Padding texture for decoding ptx-->
        <ARGB8888PaddingMode value=""False"" />
    </Ptx>
    <RTON>
        <!--The key to decrypt and encrypt RTON in PVZ2(The default key is wrong and you need to fix it there)-->
        <Key value="""" />
    </RTON>
    <ImageString name=""PopStudio Example"">
        <!--Convert image integer to string or string to integer in reanim, particles and trail. Find the answer from libpvz.so or PVZ.s3e!-->
        <String id=""PopStudioExample"" value=""99999"" />
    </ImageString>
    <ReanimXfl width=""80"" height=""80"" uselabelname=""0"" scalex=""1"" scaley=""1"" />
    <AD>True</AD>
</Description>";

        public static bool OpenProgramAD = true;

        public static float ReanimXflWidth = 80;
        public static float ReanimXflHeight = 80;
        public static double ReanimXflScaleX = 1;
        public static double ReanimXflScaleY = 1;
        public static int ReanimXflLabelName = 0;

        public static readonly Dictionary<Package.Dz.CompressFlags, string> DzCompressMethodName = new Dictionary<Package.Dz.CompressFlags, string> { { Package.Dz.CompressFlags.ZLIB, "Gzip" }, { Package.Dz.CompressFlags.LZMA, "Lzma" }, { Package.Dz.CompressFlags.STORE, "Store" }, { Package.Dz.CompressFlags.BZIP, "Bzip2" } };
        public static readonly Dictionary<Package.Pak.CompressFlags, string> PakPS3CompressMethodName = new Dictionary<Package.Pak.CompressFlags, string> { { Package.Pak.CompressFlags.ZLIB, "Zlib" }, { Package.Pak.CompressFlags.STORE, "Store" } };

        public static readonly Dictionary<string, Package.Dz.CompressFlags> DzCompressMethodEnum = new Dictionary<string, Package.Dz.CompressFlags> { { "Gzip", Package.Dz.CompressFlags.ZLIB }, { "Lzma", Package.Dz.CompressFlags.LZMA }, { "Store", Package.Dz.CompressFlags.STORE }, { "Bzip2", Package.Dz.CompressFlags.BZIP } };
        public static readonly Dictionary<string, Package.Pak.CompressFlags> PakPS3CompressMethodEnum = new Dictionary<string, Package.Pak.CompressFlags> { { "Zlib", Package.Pak.CompressFlags.ZLIB }, { "Store", Package.Pak.CompressFlags.STORE } };

        public static void ResetXml(string xmlPath)
        {
            using (StreamWriter sw = new StreamWriter(xmlPath, false))
            {
                sw.Write(DefaultXml_P1);
                sw.Write(LanguageName[AppLanguage]);
                sw.Write(DefaultXml_P2);
            }
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
                        case "Language":
                            {
                                AppLanguage = LanguageEnum[child.InnerText];
                            }
                            break;
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
                                        case "ARGB8888PaddingMode":
                                            {
                                                PtxARGB8888PaddingMode = Convert.ToBoolean(childchild.Attributes["value"].Value);
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
                        case "ReanimXfl":
                            {
                                if (child.Attributes["width"] != null)
                                {
                                    ReanimXflWidth = Convert.ToSingle(child.Attributes["width"].Value);
                                }
                                if (child.Attributes["height"] != null)
                                {
                                    ReanimXflHeight = Convert.ToSingle(child.Attributes["height"].Value);
                                }
                                if (child.Attributes["uselabelname"] != null)
                                {
                                    ReanimXflLabelName = Convert.ToInt32(child.Attributes["uselabelname"].Value);
                                }
                                if (child.Attributes["scalex"] != null)
                                {
                                    ReanimXflScaleX = Convert.ToDouble(child.Attributes["scalex"].Value);
                                }
                                if (child.Attributes["scaley"] != null)
                                {
                                    ReanimXflScaleY = Convert.ToDouble(child.Attributes["scaley"].Value);
                                }
                            }
                            break;
                        case "AD":
                            {
                                OpenProgramAD = Convert.ToBoolean(child.InnerText);
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
            try
            {


                if (!CanSave) return;
                using (StreamWriter sw = new StreamWriter(xmlPath, false))
                {
                    sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<!--Describe some default behaviour-->\n<Description version=\"2\">\n    <Language>");
                    sw.Write(LanguageName[AppLanguage]);
                    sw.Write("</Language>\n    <Dz>\n        <!--The compressing method to pack dz-->\n        <CompressMethod>\n            <Default value=\"");
                    sw.Write(DzCompressMethodName[DzDefaultCompressMethod]);
                    sw.Write("\" />");
                    foreach (KeyValuePair<string, Package.Dz.CompressFlags> keyValuePair in DzCompressDictionary)
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
                    foreach (KeyValuePair<string, Package.Pak.CompressFlags> keyValuePair in PakPS3CompressDictionary)
                    {
                        sw.Write("\n            <Extension name=\"");
                        sw.Write(keyValuePair.Key);
                        sw.Write("\" value=\"");
                        sw.Write(PakPS3CompressMethodName[keyValuePair.Value]);
                        sw.Write("\" />");
                    }
                    sw.Write("\n        </CompressMethod>\n    </PakPS3>\n    <Rsb>\n        <Ptx>\n            <!--Whether the texture 0 format ptx used ABGR8888 texture for decoding and encoding ptx when unpacking or packing rsb-->\n            <ABGR8888Mode value=\"");
                    sw.Write(RsbPtxABGR8888Mode);
                    sw.Write("\" />\n            <!--Whether the texture 0 format ptx used ABGR8888_Padding texture for decoding and encoding ptx when unpacking or packing rsb-->\n            <ARGB8888PaddingMode value=\"");
                    sw.Write(RsbPtxARGB8888PaddingMode);
                    sw.Write("\" />\n        </Ptx>\n    </Rsb>\n    <Cdat>\n        <!--The key to decrypt and encrypt cdat in PVZ Free-->\n        <Key value=\"");
                    sw.Write(CdatKey);
                    sw.Write("\" />\n    </Cdat>\n    <Ptx>\n        <!--Whether the texture 0 format ptx used ABGR8888 texture for decoding ptx-->\n        <ABGR8888Mode value=\"");
                    sw.Write(PtxABGR8888Mode);
                    sw.Write("\" />\n        <!--Whether the texture 0 format ptx used ABGR8888_Padding texture for decoding ptx-->\n        <ARGB8888PaddingMode value=\"");
                    sw.Write(PtxARGB8888PaddingMode);
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
                    sw.Write("\n    </ImageString>\n    <ReanimXfl width=\"");
                    sw.Write(ReanimXflWidth);
                    sw.Write("\" height=\"");
                    sw.Write(ReanimXflHeight);
                    sw.Write("\" uselabelname=\"");
                    sw.Write(ReanimXflLabelName);
                    sw.Write("\" scalex=\"");
                    sw.Write(ReanimXflScaleX);
                    sw.Write("\" scaley=\"");
                    sw.Write(ReanimXflScaleY);
                    sw.Write("\" />\n    <AD>");
                    sw.Write(OpenProgramAD);
                    sw.Write("</AD>\n</Description>");
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// dz pack
        /// </summary>
        public static Dictionary<string, Package.Dz.CompressFlags> DzCompressDictionary = new Dictionary<string, Package.Dz.CompressFlags> { { ".png", Package.Dz.CompressFlags.STORE }, { ".jpg", Package.Dz.CompressFlags.STORE }, { ".compiled", Package.Dz.CompressFlags.STORE }, { ".txt", Package.Dz.CompressFlags.ZLIB } };
        public static Package.Dz.CompressFlags DzDefaultCompressMethod = Package.Dz.CompressFlags.LZMA;

        /// <summary>
        /// pak ps3 pack
        /// </summary>
        public static Dictionary<string, Package.Pak.CompressFlags> PakPS3CompressDictionary = new Dictionary<string, Package.Pak.CompressFlags> { { ".ptx", Package.Pak.CompressFlags.ZLIB }, { ".compiled", Package.Pak.CompressFlags.ZLIB }, { ".txt", Package.Pak.CompressFlags.ZLIB }, { ".xml", Package.Pak.CompressFlags.ZLIB }, { ".reanim", Package.Pak.CompressFlags.ZLIB } };
        public static Package.Pak.CompressFlags PakPS3DefaultCompressMethod = Package.Pak.CompressFlags.STORE;

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
        public static bool PtxARGB8888PaddingMode = false;

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
                try
                {
                    return Convert.ToInt32(os);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            return o;
        }
    }
}
