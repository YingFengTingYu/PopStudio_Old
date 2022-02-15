namespace PopStudio.Package.Dz
{
    /// <summary>
    /// Dzip(DTRZ) is come from Marmalade SDK
    /// </summary>
    internal static class Dz
    {
        public static Dictionary<string, CompressFlags> compressDictionary = new Dictionary<string, CompressFlags> { { ".png", CompressFlags.STORE }, { ".jpg", CompressFlags.STORE }, { ".compiled", CompressFlags.STORE }, { ".txt", CompressFlags.ZLIB } };
        public static CompressFlags defaultcompressMethod = CompressFlags.LZMA;

        public static CompressFlags GetCompressMethod(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (compressDictionary.ContainsKey(extension))
            {
                return compressDictionary[extension];
            }
            return defaultcompressMethod;
        }

        public static void Unpack(string inFile, string outFolder)
        {
            Dir.FormatAndDeleteEndPathSeparator(ref inFile);
            Dir.FormatAndDeleteEndPathSeparator(ref outFolder);
            if (!File.Exists(inFile))
            {
                throw new Exception(string.Format(Str.Obj.FileNotFound, inFile));
            }
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                DzipDecoder(bs, outFolder);
            }
        }

        public static void Pack(string inFolder, string outFile)
        {
            Dir.FormatAndDeleteEndPathSeparator(ref inFolder);
            Dir.FormatAndDeleteEndPathSeparator(ref outFile);
            if (!Directory.Exists(inFolder))
            {
                throw new Exception(string.Format(Str.Obj.FolderNotFound, inFolder));
            }
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                DzipEncoder(bs, inFolder);
            }
        }

        public static void DzipEncoder(BinaryStream bs, string inFolder)
        {
            string[] files = Dir.GetFiles(inFolder);
            string[] fileName = new string[files.Length];
            string[] pathName = new string[files.Length];
            StringPool folderPool = new StringPool();
            int temp = inFolder.Length + 1;
            folderPool.ThrowInPool(string.Empty);
            for (int i = 0; i < files.Length; i++)
            {
                fileName[i] = Path.GetFileName(files[i]);
                string tp = Path.GetDirectoryName(files[i]) ?? string.Empty;
                pathName[i] = temp > tp.Length ? string.Empty : Dir.FormatWindowsPath(tp[temp..]);
                folderPool.ThrowInPool(pathName[i]);
            }
            DtrzInfo dz = new DtrzInfo();
            dz.fileNumber = (ushort)files.Length;
            dz.folderNumber = (ushort)folderPool.Length;
            dz.fileNameLibrary = fileName;
            dz.folderNameLibrary = new string[folderPool.Length];
            for (int i = 0; i < folderPool.Length; i++)
            {
                dz.folderNameLibrary[i] = folderPool[i].Value;
            }
            dz.matchInfoLibrary = new MatchInfo[files.Length];
            for (ushort i = 0; i < files.Length; i++)
            {
                dz.matchInfoLibrary[i] = new MatchInfo();
                dz.matchInfoLibrary[i].fileIndexForFileInfo = i;
                dz.matchInfoLibrary[i].folderIndex = (ushort)folderPool[pathName[i]].Index;
            }
            dz.fileInfoLibrary = new FileInfo[files.Length];
            dz.WritePart1(bs);
            long backupOffset = bs.Position;
            bs.Position += files.Length << 4;
            CompressFlags method;
            for (int i = 0; i < files.Length; i++)
            {
                dz.fileInfoLibrary[i] = new FileInfo();
                method = GetCompressMethod(files[i]);
                dz.fileInfoLibrary[i].compressMethod = method;
                dz.fileInfoLibrary[i].offset = (int)bs.Position;
                switch (method)
                {
                    case CompressFlags.COMBUF:
                    case CompressFlags.DZ:
                        throw new Exception();
                    case CompressFlags.ZLIB:
                        //It's gzip!
                        using (BinaryStream bs3 = new BinaryStream())
                        {
                            using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                            {
                                dz.fileInfoLibrary[i].size = (int)bs2.Length;
                                using (GZipStream gZipStream = new GZipStream(bs3, CompressionMode.Compress, true))
                                {
                                    bs2.CopyTo(gZipStream);
                                }
                            }
                            bs3.Position = 0;
                            bs3.CopyTo(bs);
                        }
                        break;
                    case CompressFlags.BZIP:
                        using (BinaryStream bs3 = new BinaryStream())
                        {
                            using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                            {
                                dz.fileInfoLibrary[i].size = (int)bs2.Length;
                                using (Ionic.BZip2.BZip2OutputStream bZip2Stream = new Ionic.BZip2.BZip2OutputStream(bs3, true))
                                {
                                    bs2.CopyTo(bZip2Stream);
                                }
                            }
                            bs3.Position = 0;
                            bs3.CopyTo(bs);
                        }
                        break;
                    case CompressFlags.MP3:
                    case CompressFlags.JPEG:
                    case CompressFlags.ZERO:
                    case CompressFlags.STORE:
                        using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                        {
                            dz.fileInfoLibrary[i].size = (int)bs2.Length;
                            bs2.CopyTo(bs);
                        }
                        break;
                    case CompressFlags.LZMA:
                        SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
                        using (BinaryStream bs3 = new BinaryStream())
                        {
                            using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                            {
                                dz.fileInfoLibrary[i].size = (int)bs2.Length;
                                coder.WriteCoderProperties(bs3);
                                bs3.WriteInt64(bs2.Length);
                                coder.Code(bs2, bs3, bs2.Length, -1, null);
                            }
                            bs3.Position = 0;
                            bs3.CopyTo(bs);
                        }
                        break;
                    case CompressFlags.RANDOMACCESS:
                        throw new Exception();
                }
            }
            bs.Position = backupOffset;
            dz.WritePart2(bs);
        }

        public static void DzipDecoder(BinaryStream bs, string outFolder)
        {
            outFolder += Const.PATHSEPARATOR;
            string tempName;
            DtrzInfo dz = new DtrzInfo().Read(bs);
            for (int i = 0; i < dz.fileNumber; i++)
            {
                tempName = outFolder + dz.folderNameLibrary[dz.matchInfoLibrary[i].folderIndex];
                tempName = Dir.FormatPath(tempName);
                Dir.NewDir(tempName);
                tempName += Const.PATHSEPARATOR + dz.fileNameLibrary[dz.matchInfoLibrary[i].fileIndex];
                tempName = Dir.FormatPath(tempName);
                bs.Position = dz.fileInfoLibrary[dz.matchInfoLibrary[i].fileIndexForFileInfo].offset;
                switch (dz.fileInfoLibrary[dz.matchInfoLibrary[i].fileIndexForFileInfo].compressMethod)
                {
                    case CompressFlags.COMBUF:
                    case CompressFlags.DZ:
                        using (BinaryStream bs2 = new BinaryStream())
                        {
                            bs2.WriteBytes(bs.ReadBytes(dz.fileInfoLibrary[dz.matchInfoLibrary[i].fileIndexForFileInfo].zsize));
                            bs2.Position = 0;
                            DzipDecoder(bs2, tempName);
                        }
                        break;
                    case CompressFlags.ZLIB:
                        //It's gzip!
                        using (BinaryStream bs2 = new BinaryStream())
                        {
                            bs2.WriteBytes(bs.ReadBytes(dz.fileInfoLibrary[dz.matchInfoLibrary[i].fileIndexForFileInfo].zsize));
                            bs2.Position = 0;
                            using (BinaryStream bs3 = new BinaryStream(tempName, FileMode.Create))
                            {
                                using (GZipStream gZipStream = new GZipStream(bs2, CompressionMode.Decompress))
                                {
                                    gZipStream.CopyTo(bs3);
                                }
                            }
                        }
                        break;
                    case CompressFlags.BZIP:
                        using (BinaryStream bs2 = new BinaryStream())
                        {
                            bs2.WriteBytes(bs.ReadBytes(dz.fileInfoLibrary[dz.matchInfoLibrary[i].fileIndexForFileInfo].zsize));
                            bs2.Position = 0;
                            using (BinaryStream bs3 = new BinaryStream(tempName, FileMode.Create))
                            {
                                using (Ionic.BZip2.BZip2InputStream bZip2Stream = new Ionic.BZip2.BZip2InputStream(bs2))
                                {
                                    bZip2Stream.CopyTo(bs3);
                                }
                            }
                        }
                        break;
                    case CompressFlags.MP3:
                    case CompressFlags.JPEG:
                    case CompressFlags.ZERO:
                    case CompressFlags.STORE:
                        using (BinaryStream bs2 = new BinaryStream(tempName, FileMode.Create))
                        {
                            bs2.WriteBytes(bs.ReadBytes(dz.fileInfoLibrary[dz.matchInfoLibrary[i].fileIndexForFileInfo].zsize));
                        }
                        break;
                    case CompressFlags.LZMA:
                        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
                        using (BinaryStream bs2 = new BinaryStream())
                        {
                            bs2.WriteBytes(bs.ReadBytes(dz.fileInfoLibrary[dz.matchInfoLibrary[i].fileIndexForFileInfo].zsize));
                            bs2.Position = 0;
                            byte[] properties = bs2.ReadBytes(5);
                            long fileLength = bs2.ReadInt64();
                            coder.SetDecoderProperties(properties);
                            using (BinaryStream bs3 = new BinaryStream(tempName, FileMode.Create))
                            {
                                coder.Code(bs2, bs3, bs2.Length, fileLength, null);
                            }
                        }
                        break;
                    case CompressFlags.RANDOMACCESS:
                        //(Invalid) I can just copy it out of dz.
                        using (BinaryStream bs2 = new BinaryStream(tempName, FileMode.Create))
                        {
                            bs2.WriteBytes(bs.ReadBytes(dz.fileInfoLibrary[dz.matchInfoLibrary[i].fileIndexForFileInfo].zsize));
                        }
                        break;
                }
            }
        }
    }
}