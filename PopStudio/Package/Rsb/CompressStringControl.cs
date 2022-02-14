using System.Text;

namespace PopStudio.Package.Rsb
{
    internal class CompressStringList
    {
        readonly List<CompressString> list = new List<CompressString>();
        readonly int type;

        public List<CompressString> List => list;

        public int Length => list.Count;

        public CompressString this[int index] => list[index];

        public void Add(CompressString cs)
        {
            list.Add(cs);
        }

        public void Remove(CompressString t) => list.Remove(t);

        public void RemoveAt(int index) => list.RemoveAt(index);

        public void Clear() => list.Clear();

        /// <summary>
        /// type=0代表rsb，type=1代表rsgp
        /// </summary>
        /// <param name="type"></param>
        public CompressStringList(int type) => this.type = type;

        public byte[] Write()
        {
            list.Sort(delegate (CompressString c1, CompressString c2)
            {
                return string.CompareOrdinal(c1.name, c2.name);
            });
            List<PrefixInfo> prefixList = new List<PrefixInfo>();                  //前缀名字列表
            prefixList.Add(new PrefixInfo(new byte[0], -1));                            //生成一个默认前缀，作用范围为整个段
            byte[] finishedBytes = new byte[0];
            // 一次循环写入一个名字
            // (算location的时候注意*4，算cover的时候注意/4）
            // -和缺省列表里对比，（更新缺省列表），创建剩余名字
            // -将剩余名字也加入到prefixList和带修改列表    
            // -写入剩余名字
            foreach (var tls in list)
            {
                string fullName = tls.name ?? string.Empty;
                //本次循环中，需要修改作用范围的prefix，
                //因为长度只有对比完缺省名字才知道，所以先存起来。（虽然对比过程中不断修改也行）	
                //等到修改时，将其cover全部修改为当前偏移位置即可（所以类里的cover没用啊）		
                //应该只从现有prefix里添加，不应新生成
                List<PrefixInfo> awaitList = new List<PrefixInfo>();
                byte[] thisRest = Encoding.UTF8.GetBytes(fullName);     //本次循环中，剩余名字的字节数组（要写入到文件中的部分）
                bool removeStart = false;
                //-和前缀列表对比
                for (int i = 0; i < prefixList.Count; i++)
                {
                    if (removeStart)
                    {
                        //一旦开始移除，就会一直移除到结尾
                        WriteInt24(finishedBytes, prefixList[i].location, 0);//在文件中先修改其cover为0，再从列表中移除
                        prefixList.RemoveAt(i--);
                        continue;
                    }
                    byte[] prefix = prefixList[i].prefix;
                    int j = -1;     //判断prefix去留
                    for (j = 0; j < prefix.Length; j++) if (thisRest[j] != prefix[j]) break;
                    //判断条件加了&& prefix.length!=0保证空字符串不会被处理
                    if (j == prefix.Length && prefix.Length != 0)
                    {
                        //该prefix还能用,看下一个prefix。
                        //待会修改prefix的作用范围，先存起来
                        awaitList.Add(prefixList[i]);
                        //减少剩余名字长度
                        byte[] tmpThisBytes = new byte[thisRest.Length - j]; //这里j就是prefix.length
                        Array.Copy(thisRest, j, tmpThisBytes, 0, tmpThisBytes.Length);
                        thisRest = tmpThisBytes;
                        //continue;	
                    }
                    else if (j > 0)
                    {
                        //prefix部分匹配，将prefix拆成两个，前一个修改作用范围并继续存在，后一个写入之前的作用范围到文件中并消失，如果后面还有prefix，是否改作用范围还不清楚，待会看看吧，反正应该要删除				
                        //--添加后半段前缀的作用范围（为当前finishedBytes长度/4）
                        int location_3 = prefixList[i].location + j * 4;
                        WriteInt24(finishedBytes, location_3, finishedBytes.Length / 4);
                        //--前半段继续生效，修改prefixList里的前缀名和作用范围
                        byte[] tmpPrefixBytes_2 = new byte[j];
                        Array.Copy(prefix, 0, tmpPrefixBytes_2, 0, tmpPrefixBytes_2.Length);
                        prefixList[i].prefix = tmpPrefixBytes_2;
                        awaitList.Add(prefixList[i]);
                        //减少剩余名字长度
                        byte[] tmpThisBytes = new byte[thisRest.Length - j];
                        Array.Copy(thisRest, j, tmpThisBytes, 0, tmpThisBytes.Length);
                        thisRest = tmpThisBytes;
                        //后面如果有prefix，删除
                        removeStart = true;
                        //break;
                    }
                    else if (j == 0 && prefix.Length != 0)
                    {
                        //prefix完全不匹配，删除该prefix，后面如果还有prefix，将后面的作用范围改为0并删除
                        prefixList.RemoveAt(i);
                        i--;
                        removeStart = true;
                        //break;
                    }
                }
                //-将全部加入到prefixList和带修改列表
                byte[] tmpThisBytes2 = new byte[thisRest.Length + 1];
                Array.Copy(thisRest, 0, tmpThisBytes2, 0, thisRest.Length);
                thisRest = tmpThisBytes2;
                prefixList.Add(new PrefixInfo(thisRest, finishedBytes.Length));
                awaitList.Add(prefixList[prefixList.Count - 1]);
                //-写入剩余名字
                //--生成四字节形式的名字
                int INFO_LENGTH = type == 0 ? 4 : (tls.type == 1 ? 12 : 32);
                byte[] thisFinishedBytes = new byte[thisRest.Length * 4 + INFO_LENGTH];
                for (int i = 0; i < thisRest.Length; i++)
                {
                    thisFinishedBytes[i * 4] = thisRest[i];
                }
                //--INFO_LENGTH 写入所属pgsrNum
                if (INFO_LENGTH == 4)
                {
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 4, ((RsbExtraInfo)tls.extraInfo).index);
                }
                else if (INFO_LENGTH == 12)
                {
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 12, ((RsgpPart0ExtraInfo)tls.extraInfo).type);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 8, ((RsgpPart0ExtraInfo)tls.extraInfo).offset);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 4, ((RsgpPart0ExtraInfo)tls.extraInfo).size);
                }
                else
                {
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 32, ((RsgpPart1ExtraInfo)tls.extraInfo).type);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 28, ((RsgpPart1ExtraInfo)tls.extraInfo).offset);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 24, ((RsgpPart1ExtraInfo)tls.extraInfo).size);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 20, ((RsgpPart1ExtraInfo)tls.extraInfo).index);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 16, ((RsgpPart1ExtraInfo)tls.extraInfo).empty1);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 12, ((RsgpPart1ExtraInfo)tls.extraInfo).empty2);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 8, ((RsgpPart1ExtraInfo)tls.extraInfo).width);
                    WriteInt32(thisFinishedBytes, thisFinishedBytes.Length - 4, ((RsgpPart1ExtraInfo)tls.extraInfo).height);
                }
                //--和已生成的文件组合
                int l = finishedBytes.Length;
                Array.Resize(ref finishedBytes, l + thisFinishedBytes.Length);
                Array.Copy(thisFinishedBytes, 0, finishedBytes, l, thisFinishedBytes.Length);
                //-修改被用到的前缀名字的作用范围
                for (int i = 0; i < awaitList.Count; i++)
                {
                    WriteInt24(finishedBytes, awaitList[i].location, finishedBytes.Length / 4);
                }
                //-收尾
                //Files.write(Paths.get("E:\\free改.txt"),finishedBytes);
            }
            //最后一个完成后，将prefixList里剩余的prefix，cover都改成0
            for (int i = 0; i < prefixList.Count; i++)
            {
                if (prefixList[i].prefix.Length == 0)
                {
                    continue; //空字符串跳过
                }
                else
                {
                    WriteInt24(finishedBytes, prefixList[i].location, 0);
                }
            }
            return finishedBytes;
        }

        static void WriteInt24(byte[] bytes, int location, int cover)
        {
            bytes[location + 1] = (byte)(cover & 0x000000ff);
            bytes[location + 2] = (byte)((cover & 0x0000ff00) >> 8);
            bytes[location + 3] = (byte)((cover & 0x00ff0000) >> 16);
        }

        static void WriteInt32(byte[] bytes, int location, int cover)
        {
            bytes[location] = (byte)(cover & 0x000000ff);
            bytes[location + 1] = (byte)((cover & 0x0000ff00) >> 8);
            bytes[location + 2] = (byte)((cover & 0x00ff0000) >> 16);
            bytes[location + 3] = (byte)((cover & 0xff000000) >> 24);
        }

        public CompressStringList Read(byte[] bytes)
        {
            int length = bytes.Length;
            using BinaryStream bs = new BinaryStream(bytes);
            long beginOffset = 0;
            long endOffset = length;
            list.Clear();
            List<Default> defaults = new List<Default>();
            defaults.Add(new Default(string.Empty, length));
            while (bs.Position < endOffset)
            {
                string temp = "";
                string temp_head = "";
                //1.如果有缺省，先将缺省名字补全，同时去除掉已经失效的缺省
                for (int i = 0; i < defaults.Count; i++)
                {
                    if (bs.Position < defaults[i].offset * 4 + beginOffset)
                    {
                        //如果在其生效范围内
                        temp_head += defaults[i].name;
                    }
                    else
                    {
                        //如果已经失效，删除
                        defaults.RemoveAt(i--);
                    }
                }
                //2.读取后半段名字，同时记录新的缺省名字
                int startIndex = 0; //记录待缺省名字在字符串中的起始位置
                int tpendOffset = defaults[defaults.Count - 1].offset;//记录待缺省名字的生效范围的结束偏移，默认为上一级末偏移
                int tmpEndOffset;
                while (bs.PeekByte() != 0)
                {
                    //读取rsb中记录的名字
                    temp += (char)bs.ReadByte();
                    tmpEndOffset = (int)bs.ReadUInt24();
                    if (tmpEndOffset != 0)
                    {
                        //如果有偏移
                        if (temp.Length != 1)
                        {
                            defaults.Add(new Default(temp.Substring(startIndex, temp.Length - 1 - startIndex), tpendOffset));
                        }
                        startIndex = temp.Length - 1; //当前偏移对应字母作为下一个缺省名字的开头
                        tpendOffset = tmpEndOffset; //记录下一个缺省名字的生效范围			
                    }
                }
                bs.Position++;
                tmpEndOffset = (int)bs.ReadUInt24();
                if (tmpEndOffset != 0)
                {
                    //如果有偏移
                    if (temp.Length != 1)
                    {
                        defaults.Add(new Default(temp.Substring(startIndex, temp.Length - startIndex), tpendOffset));
                        //defaults.Add(new Default(temp.Substring(startIndex, temp.Length - 1 - startIndex), tpendOffset));
                    }
                }
                //3.后续处理
                list.Add(new CompressString(temp_head + temp, type == 0 ? new RsbExtraInfo().Read(bs) : (bs.PeekInt32() == 0 ? new RsgpPart0ExtraInfo().Read(bs) : new RsgpPart1ExtraInfo().Read(bs))));
            }
            return this;
        }
    }

    internal class PrefixInfo
    {
        public byte[] prefix;
        public int location;

        public PrefixInfo(byte[] prefix, int location)
        {
            this.prefix = prefix;
            this.location = location;
        }
    }

    internal class Default
    {
        public string name;
        public int offset;
        public long bsoffset;

        public Default()
        {

        }

        public Default(string name, int offset)
        {
            this.name = name;
            this.offset = offset;
        }

        public Default(string name, int offset, long bsoffset)
        {
            this.name = name;
            this.offset = offset;
            this.bsoffset = bsoffset;
        }
    }

    internal class CompressString
    {
        public string name;
        public ExtraInfo extraInfo;
        public int type;

        public CompressString()
        {

        }

        public CompressString(string name, ExtraInfo extraInfo)
        {
            this.name = name;
            this.extraInfo = extraInfo;
            if (extraInfo is RsbExtraInfo)
            {
                type = 0;
            }
            else if (extraInfo is RsgpPart0ExtraInfo)
            {
                type = 1;
            }
            else if (extraInfo is RsgpPart1ExtraInfo)
            {
                type = 2;
            }
            else
            {
                type = -1;
            }
        }
    }

    internal abstract class ExtraInfo
    {
        public abstract ExtraInfo Read(BinaryStream bs);
    }

    internal class RsbExtraInfo : ExtraInfo
    {
        public int index;

        public RsbExtraInfo()
        {

        }

        public RsbExtraInfo(int index)
        {
            this.index = index;
        }

        public override ExtraInfo Read(BinaryStream bs)
        {
            index = bs.ReadInt32();
            return this;
        }
    }

    internal abstract class RsgpExtraInfo : ExtraInfo
    {

    }

    internal class RsgpPart0ExtraInfo : RsgpExtraInfo
    {
        public int type = 0x0;
        public int offset;
        public int size;

        public RsgpPart0ExtraInfo()
        {

        }

        public RsgpPart0ExtraInfo(int offset, int size)
        {
            this.offset = offset;
            this.size = size;
        }

        public override ExtraInfo Read(BinaryStream bs)
        {
            bs.IdInt32(type);
            offset = bs.ReadInt32();
            size = bs.ReadInt32();
            return this;
        }
    }

    internal class RsgpPart1ExtraInfo : RsgpExtraInfo
    {
        public int type = 0x1;
        public int offset;
        public int size;
        public int index;
        public int empty1 = 0x0;
        public int empty2 = 0x0;
        public int width;
        public int height;

        public RsgpPart1ExtraInfo()
        {

        }

        public RsgpPart1ExtraInfo(int offset, int size, int index, int width, int height)
        {
            this.offset = offset;
            this.size = size;
            this.index = index;
            this.width = width;
            this.height = height;
        }

        public override ExtraInfo Read(BinaryStream bs)
        {
            bs.IdInt32(type);
            offset = bs.ReadInt32();
            size = bs.ReadInt32();
            index = bs.ReadInt32();
            bs.IdInt32(empty1);
            bs.IdInt32(empty2);
            width = bs.ReadInt32();
            height = bs.ReadInt32();
            return this;
        }
    }
}
