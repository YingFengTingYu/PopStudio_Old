namespace PopStudio.Package.Dz
{
    /// <summary>
    /// Marmalade SDK - Derbh API
    /// dzip archive
    /// It can use dz, gzip, lzma, bzip2, store and zero to save the file
    /// dz compression is only in Derbh API and dzip.exe
    /// I can't support it, so the program will only copy it out.
    /// </summary>
    internal class Dz
    {
        public static Dictionary<string, CompressFlags> compressDictionary;
        public static CompressFlags defaultcompressMethod;

        public static CompressFlags GetCompressMethod(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (compressDictionary.ContainsKey(extension))
            {
                return compressDictionary[extension];
            }
            return defaultcompressMethod;
        }

        public static void Unpack(string inFile, string outFolder, bool changeimage = false, bool delete = false)
        {
            Dir.FormatAndDeleteEndPathSeparator(ref inFile);
            Dir.FormatAndDeleteEndPathSeparator(ref outFolder);
            if (!File.Exists(inFile))
            {
                throw new Exception(string.Format(Str.Obj.FileNotFound, inFile));
            }
            using (IDisposablePool pool = new IDisposablePool())
            {
                using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
                {
                    bs.Encode = EncodeHelper.ANSI;
                    outFolder += Const.PATHSEPARATOR;
                    string tempName;
                    DtrzInfo dz = new DtrzInfo();
                    dz.Read(bs);
                    //ReadArchives
                    int archivesCount = dz.ArchivesCount;
                    BinaryStream[] bsLib = new BinaryStream[archivesCount];
                    string mFilePath = Path.GetDirectoryName(inFile) + Const.PATHSEPARATOR;
                    for (int i = 0; i < archivesCount; i++)
                    {
                        if (dz.ArchiveNameLibrary[i] == null)
                        {
                            bsLib[i] = bs;
                        }
                        else
                        {
                            bsLib[i] = pool.Add(new BinaryStream(mFilePath + dz.ArchiveNameLibrary[i], FileMode.Open));
                        }
                    }
                    int chunksCount = dz.ChunksCount;
                    BinaryStream tempbs;
                    for (int i = 0; i < chunksCount; i++)
                    {
                        ChunkInfo SubInfo = dz.Chunks[i];
                        tempbs = bsLib[SubInfo.ArchiveIndex];
                        tempName = Dir.FormatPath(outFolder + dz.FolderNameLibrary[SubInfo.FolderNameIndex]);
                        Dir.NewDir(tempName);
                        tempName = Dir.FormatPath(tempName + Const.PATHSEPARATOR + dz.FileNameLibrary[SubInfo.FileNameIndex]);
                        if (SubInfo.MultiIndex != 0)
                        {
                            //Multi saved file
                            string ex = Path.GetExtension(tempName);
                            tempName = $"{tempName[..^ex.Length]}_multi_{SubInfo.MultiIndex}{ex}";
                        }
                        tempbs.Position = SubInfo.Offset;
                        if (SubInfo.ZSize_For_Compress == -1)
                        {
                            SubInfo.ZSize_For_Compress = (int)(tempbs.Length - tempbs.Position);
                        }
                        CompressFlags flags = SubInfo.Flags;
                        if ((flags & CompressFlags.DZ) != 0)
                        {
                            //dz decompress
                            //unsupported
                            //just copy
                            using (BinaryStream bs2 = new BinaryStream(tempName, FileMode.Create))
                            {
                                tempbs.CopyTo(bs2, SubInfo.ZSize_For_Dz);
                            }
                        }
                        else if ((flags & CompressFlags.ZLIB) != 0)
                        {
                            //gzip decompress
                            using (BinaryStream bs2 = new BinaryStream())
                            {
                                tempbs.CopyTo(bs2, SubInfo.ZSize_For_Compress);
                                bs2.Position = 0;
                                using (BinaryStream bs3 = new BinaryStream(tempName, FileMode.Create))
                                {
                                    using (GZipStream gZipStream = new GZipStream(bs2, CompressionMode.Decompress))
                                    {
                                        gZipStream.CopyTo(bs3);
                                    }
                                }
                            }
                        }
                        else if ((flags & CompressFlags.BZIP) != 0)
                        {
                            //bzip2 decompress
                            using (BinaryStream bs2 = new BinaryStream())
                            {
                                tempbs.CopyTo(bs2, SubInfo.ZSize_For_Compress);
                                bs2.Position = 0;
                                using (BinaryStream bs3 = new BinaryStream(tempName, FileMode.Create))
                                {
                                    using (Ionic.BZip2.BZip2InputStream bZip2Stream = new Ionic.BZip2.BZip2InputStream(bs2))
                                    {
                                        bZip2Stream.CopyTo(bs3);
                                    }
                                }
                            }
                        }
                        else if ((flags & CompressFlags.ZERO) != 0)
                        {
                            //zero chunk
                            int copytimes = SubInfo.Size / 81920;
                            int copyelse = SubInfo.Size % 81920;
                            byte[] temp = new byte[81920];
                            using (BinaryStream bs2 = new BinaryStream(tempName, FileMode.Create))
                            {
                                for (int j = 0; j < copytimes; j++)
                                {
                                    bs2.Write(temp, 0, 81920);
                                }
                                if (copyelse != 0)
                                {
                                    bs2.Write(temp, 0, copyelse);
                                }
                            }
                        }
                        else if ((flags & CompressFlags.STORE) != 0)
                        {
                            //copy only
                            using (BinaryStream bs2 = new BinaryStream(tempName, FileMode.Create))
                            {
                                tempbs.CopyTo(bs2, SubInfo.Size);
                            }
                        }
                        else if ((flags & CompressFlags.LZMA) != 0)
                        {
                            //lzma decompress
                            SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
                            byte[] properties = tempbs.ReadBytes(5);
                            long fileLength = tempbs.ReadInt64();
                            coder.SetDecoderProperties(properties);
                            using (BinaryStream bs3 = new BinaryStream(tempName, FileMode.Create))
                            {
                                coder.Code(tempbs, bs3, SubInfo.ZSize_For_Compress - 13, fileLength, null);
                            }
                        }
                        else
                        {
                            //unknow chunk
                            //copy only
                            using (BinaryStream bs2 = new BinaryStream(tempName, FileMode.Create))
                            {
                                tempbs.CopyTo(bs2, SubInfo.Size);
                            }
                        }
                        if (changeimage)
                        {
                            string ex = Path.GetExtension(tempName).ToLower();
                            if (ex == ".tex")
                            {
                                Image.Tex.Tex.Decode(tempName, Path.ChangeExtension(tempName, ".png"));
                                if (delete) File.Delete(tempName);
                            }
                            else if (ex == ".txz")
                            {
                                Image.Txz.Txz.Decode(tempName, Path.ChangeExtension(tempName, ".png"));
                                if (delete) File.Delete(tempName);
                            }
                        }
                    }
                }
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
                compressDictionary = Setting.DzCompressDictionary;
                defaultcompressMethod = Setting.DzDefaultCompressMethod;
                bs.Encode = EncodeHelper.ANSI;
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
                dz.FileNameNumber = (ushort)files.Length;
                dz.FolderNameNumber = (ushort)folderPool.Length;
                dz.FileNameLibrary = fileName;
                dz.FolderNameLibrary = new string[folderPool.Length];
                for (int i = 0; i < folderPool.Length; i++)
                {
                    dz.FolderNameLibrary[i] = folderPool[i].Value;
                }
                dz.Chunks = new ChunkInfo[files.Length];
                for (ushort i = 0; i < files.Length; i++)
                {
                    dz.Chunks[i] = new ChunkInfo((ushort)folderPool[pathName[i]].Index, i, i);
                }
                dz.WritePart1(bs);
                long backupOffset = bs.Position;
                bs.Position += files.Length << 4;
                CompressFlags flags;
                for (int i = 0; i < files.Length; i++)
                {
                    ChunkInfo SubInfo = dz.Chunks[i];
                    flags = GetCompressMethod(files[i]);
                    SubInfo.Flags = flags;
                    SubInfo.Offset = (int)bs.Position;
                    if ((flags & CompressFlags.DZ) != 0)
                    {
                        flags &= ~CompressFlags.DZ;
                        flags |= CompressFlags.STORE;
                        SubInfo.Flags = flags;
                        using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                        {
                            SubInfo.Size = (int)bs2.Length;
                            SubInfo.ZSize_For_Dz = (int)bs2.Length;
                            bs2.CopyTo(bs);
                        }
                    }
                    else if ((flags & CompressFlags.ZLIB) != 0)
                    {
                        //gzip decompress
                        using (BinaryStream bs3 = new BinaryStream())
                        {
                            using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                            {
                                SubInfo.Size = (int)bs2.Length;
                                SubInfo.ZSize_For_Dz = (int)bs2.Length;
                                using (GZipStream gZipStream = new GZipStream(bs3, CompressionMode.Compress, true))
                                {
                                    bs2.CopyTo(gZipStream);
                                }
                            }
                            bs3.Position = 0;
                            bs3.CopyTo(bs);
                        }
                    }
                    else if ((flags & CompressFlags.BZIP) != 0)
                    {
                        //bzip2 decompress
                        using (BinaryStream bs3 = new BinaryStream())
                        {
                            using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                            {
                                SubInfo.Size = (int)bs2.Length;
                                SubInfo.ZSize_For_Dz = (int)bs2.Length;
                                using (Ionic.BZip2.BZip2OutputStream bZip2Stream = new Ionic.BZip2.BZip2OutputStream(bs3, true))
                                {
                                    bs2.CopyTo(bZip2Stream);
                                }
                            }
                            bs3.Position = 0;
                            bs3.CopyTo(bs);
                        }
                    }
                    else if ((flags & CompressFlags.ZERO) != 0)
                    {
                        //zero chunk
                        using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                        {
                            SubInfo.Size = (int)bs2.Length;
                            SubInfo.ZSize_For_Dz = 0;
                        }
                    }
                    else if ((flags & CompressFlags.STORE) != 0)
                    {
                        //copy only
                        using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                        {
                            SubInfo.Size = (int)bs2.Length;
                            SubInfo.ZSize_For_Dz = (int)bs2.Length;
                            bs2.CopyTo(bs);
                        }
                    }
                    else if ((flags & CompressFlags.LZMA) != 0)
                    {
                        //lzma decompress
                        SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
                        using (BinaryStream bs3 = new BinaryStream())
                        {
                            using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                            {
                                SubInfo.Size = (int)bs2.Length;
                                SubInfo.ZSize_For_Dz = (int)bs2.Length;
                                coder.WriteCoderProperties(bs3);
                                bs3.WriteInt64(bs2.Length);
                                coder.Code(bs2, bs3, bs2.Length, -1, null);
                            }
                            bs3.Position = 0;
                            bs3.CopyTo(bs);
                        }
                    }
                    else
                    {
                        //unknow chunk
                        //copy only
                        flags |= CompressFlags.STORE;
                        SubInfo.Flags = flags;
                        using (BinaryStream bs2 = new BinaryStream(files[i], FileMode.Open))
                        {
                            SubInfo.Size = (int)bs2.Length;
                            SubInfo.ZSize_For_Dz = (int)bs2.Length;
                            bs2.CopyTo(bs);
                        }
                    }
                }
                bs.Position = backupOffset;
                dz.WritePart2(bs);
                compressDictionary = null;
            }
        }
    }
}
