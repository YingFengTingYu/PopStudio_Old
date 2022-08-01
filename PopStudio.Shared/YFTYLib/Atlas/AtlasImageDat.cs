namespace PopStudio.Atlas
{
    internal static class AtlasImageDat
    {
        public static bool Splice(string inFolder, string outFile, string infoPath, string itemName, int width, int height)
        {
            Dictionary<string, SubImageInfo> cutinfo = CutImage.Splice(inFolder, outFile, width, height);
            if (string.IsNullOrEmpty(itemName))
            {
                if (File.Exists(inFolder + Const.PATHSEPARATOR + "AtlasID.txt"))
                {
                    itemName = File.ReadAllText(inFolder + Const.PATHSEPARATOR + "AtlasID.txt").Replace("\r", "").Replace("\n", "");
                }
                else
                {
                    return false;
                }
            }
            using (BinaryStream bs = new BinaryStream(infoPath, FileMode.Open))
            {
                int Number = bs.ReadInt32();
                for (int i = 0; i < Number; i++)
                {
                    bs.IdInt32(0x4);
                    string ID = bs.ReadStringByUInt16Head();
                    bs.ReadStringByUInt16Head();
                    bs.ReadStringByUInt16Head();
                    bs.Position += 1;
                    long pos = bs.Position;
                    bs.Position += 48;
                    if (bs.ReadStringByUInt16Head() == itemName)
                    {
                        long pos2 = bs.Position;
                        bs.Position = pos;
                        SubImageInfo temp = cutinfo[ID.ToLower()];
                        bs.WriteInt32(temp.X);
                        bs.WriteInt32(temp.Y);
                        bs.WriteInt32(temp.Width);
                        bs.WriteInt32(temp.Height);
                        bs.Position = pos2;
                    }
                    bs.Position += 4;
                }
            }
            return true;
        }

        public static bool Cut(string inFile, string outFolder, string infoPath, string itemName)
        {
            if (string.IsNullOrEmpty(itemName)) return false;
            List<SubImageInfo> cutinfo = new List<SubImageInfo>();
            using (BinaryStream bs = new BinaryStream(infoPath, FileMode.Open))
            {
                int Number = bs.ReadInt32();
                SubImageInfo empty = new SubImageInfo();
                for (int i = 0; i < Number; i++)
                {
                    bs.IdInt32(0x4);
                    empty.ID = bs.ReadStringByUInt16Head();
                    bs.ReadStringByUInt16Head();
                    bs.ReadStringByUInt16Head();
                    bs.Position += 1;
                    empty.X = bs.ReadInt32();
                    empty.Y = bs.ReadInt32();
                    empty.Width = bs.ReadInt32();
                    empty.Height = bs.ReadInt32();
                    bs.Position += 32;
                    if (bs.ReadStringByUInt16Head() == itemName)
                    {
                        cutinfo.Add(empty); //Struct not Class
                    }
                    bs.Position += 4;
                }
            }
            if (cutinfo.Count == 0) return false;
            CutImage.Cut(inFile, outFolder, cutinfo);
            File.WriteAllText(outFolder + Const.PATHSEPARATOR + "AtlasID.txt", itemName);
            return true;
        }
    }
}
