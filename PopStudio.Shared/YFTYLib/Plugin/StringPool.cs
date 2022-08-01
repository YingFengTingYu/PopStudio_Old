namespace PopStudio.Plugin
{
    internal class StringPool
    {
        Dictionary<string, PoolInfo> stringPool;
        long position;
        int index;
        bool autoPool = false;

        public StringPool(bool autoPool)
        {
            stringPool = new Dictionary<string, PoolInfo>();
            position = 0;
            index = 0;
            this.autoPool = autoPool;
        }

        public StringPool()
        {
            stringPool = new Dictionary<string, PoolInfo>();
            position = 0;
            index = 0;
        }

        public int Length => index;

        public PoolInfo this[int index]
        {
            get
            {
                if (index > this.index)
                {
                    return null;
                }
                return stringPool.ElementAt(index).Value;
            }
        }

        public PoolInfo this[string id]
        {
            get
            {
                if (!stringPool.ContainsKey(id))
                {
                    if (autoPool)
                    {
                        return ThrowInPool(id);
                    }
                    return null;
                }
                return stringPool[id];
            }
        }

        public bool Exist(string id)
        {
            return stringPool.ContainsKey(id);
        }

        public void Clear()
        {
            stringPool.Clear();
            position = 0;
            index = 0;
        }

        /// <summary>
        /// 向字符常量池中丢入字符元素
        /// </summary>
        /// <param name="poolKey"></param>
        /// <returns></returns>
        public PoolInfo ThrowInPool(string poolKey)
        {
            if (!stringPool.ContainsKey(poolKey))
            {
                stringPool.Add(poolKey, new PoolInfo(position, index++, poolKey));
                position += poolKey.Length + 1; //+1是为了留出空白
            }
            return stringPool[poolKey];
        }
    }

    internal class PoolInfo
    {
        public long Offset;
        public int Index;
        public string Value;

        public PoolInfo()
        {

        }

        public PoolInfo(long offset, int index, string value)
        {
            Offset = offset;
            Index = index;
            Value = value;
        }
    }
}
