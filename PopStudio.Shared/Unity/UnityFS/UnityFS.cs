namespace PopStudio.Unity.UnityFS
{
    internal static class UnityFS
    {
        public static void Extract(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                bs.Endian = Endian.Big;
                FSHead head = new FSHead().Read(bs);
                //0x10对齐
                if (head.version >= 7)
                {
                    if ((bs.Position & 0b1111) != 0)
                    {
                        bs.Position = (bs.Position | 0b1111) + 1;
                    }
                }
                //解压块
                byte[] blocksInfoBytes;
                if ((head.flags & 0b10000000) != 0)
                {
                    long bp = bs.Position;
                    bs.Position = bs.Length - head.compressedBlocksInfoSize;
                    blocksInfoBytes = bs.ReadBytes(head.compressedBlocksInfoSize);
                    bs.Position = bp;
                }
                else
                {
                    blocksInfoBytes = bs.ReadBytes(head.compressedBlocksInfoSize);
                }
                MemoryStream blocksInfoUncompresseddStream;
                int uncompressedSize = head.uncompressedBlocksInfoSize;
                switch (head.flags & 0b00111111) //kArchiveCompressionTypeMask
                {
                    default: //None
                        {
                            blocksInfoUncompresseddStream = new MemoryStream(blocksInfoBytes);
                            break;
                        }
                    case 1: //LZMA
                        {
                            blocksInfoUncompresseddStream = new MemoryStream(uncompressedSize);
                            using (MemoryStream blocksInfoCompressedStream = new MemoryStream(blocksInfoBytes))
                            {
                                SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
                                byte[] properties = new byte[5];
                                blocksInfoCompressedStream.Read(properties, 0, 5);
                                coder.SetDecoderProperties(properties);
                                coder.Code(blocksInfoCompressedStream, blocksInfoUncompresseddStream, head.compressedBlocksInfoSize, head.uncompressedBlocksInfoSize, null);
                            }
                            blocksInfoUncompresseddStream.Position = 0;
                            break;
                        }
                    case 2: //LZ4
                    case 3: //LZ4HC
                        {
                            byte[] uncompressedBytes = new byte[uncompressedSize];
                            K4os.Compression.LZ4.LZ4Codec.Decode(blocksInfoBytes, uncompressedBytes);
                            blocksInfoUncompresseddStream = new MemoryStream(uncompressedBytes);
                            break;
                        }
                }
                StorageBlock[] m_BlocksInfo;
                Node[] m_DirectoryInfo;
                using (BinaryStream blocksInfoReader = new BinaryStream(blocksInfoUncompresseddStream))
                {
                    //using (BinaryStream bs2 = new BinaryStream("D:\\123", FileMode.Create))
                    //{
                    //    blocksInfoReader.Position = 0;
                    //    blocksInfoReader.CopyTo(bs2);
                    //}
                    blocksInfoReader.Endian = Endian.Big;
                    blocksInfoReader.Position = 0;
                    byte[] uncompressedDataHash = blocksInfoReader.ReadBytes(16);
                    int blocksInfoCount = blocksInfoReader.ReadInt32();
                    m_BlocksInfo = new StorageBlock[blocksInfoCount];
                    for (int i = 0; i < blocksInfoCount; i++)
                    {
                        m_BlocksInfo[i] = new StorageBlock().Read(blocksInfoReader);
                    }
                    int nodesCount = blocksInfoReader.ReadInt32();
                    m_DirectoryInfo = new Node[nodesCount];
                    for (int i = 0; i < nodesCount; i++)
                    {
                        m_DirectoryInfo[i] = new Node().Read(blocksInfoReader);
                    }
                }
                using (BinaryStream blocksStream = new BinaryStream())
                {
                    foreach (StorageBlock blockInfo in m_BlocksInfo)
                    {
                        switch (blockInfo.flags & 0b00111111) //kStorageBlockCompressionTypeMask
                        {
                            default: //None
                                {
                                    blocksStream.WriteBytes(bs.ReadBytes(blockInfo.compressedSize));
                                    break;
                                }
                            case 1: //LZMA
                                {
                                    using (MemoryStream blocksInfoCompressedStream = new MemoryStream(blockInfo.uncompressedSize))
                                    {
                                        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
                                        coder.SetDecoderProperties(bs.ReadBytes(5));
                                        coder.Code(bs, blocksInfoCompressedStream, blockInfo.compressedSize, blockInfo.uncompressedSize, null);
                                        blocksInfoCompressedStream.Position = 0;
                                        blocksInfoCompressedStream.CopyTo(blocksStream);
                                    }
                                    break;
                                }
                            case 2: //LZ4
                            case 3: //LZ4HC
                                {
                                    byte[] uncompressedBytes = new byte[blockInfo.uncompressedSize];
                                    K4os.Compression.LZ4.LZ4Codec.Decode(bs.ReadBytes(blockInfo.compressedSize), uncompressedBytes);
                                    blocksStream.WriteBytes(uncompressedBytes);
                                    break;
                                }
                        }
                    }
                    blocksStream.Position = 0;
                    //files
                    for (int i = 0; i < m_DirectoryInfo.Length; i++)
                    {
                        Node node = m_DirectoryInfo[i];
                        string path = outFile + Const.PATHSEPARATOR + node.path;
                        Dir.NewDir(path, false);
                        using (BinaryStream bs2 = new BinaryStream(path, FileMode.Create))
                        {
                            blocksStream.Position = node.offset;
                            bs2.WriteBytes(blocksStream.ReadBytes((int)node.size));
                        }
                    }
                }
            }
        }
    }
}