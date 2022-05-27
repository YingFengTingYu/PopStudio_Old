namespace PopStudio.Platform
{
    public abstract unsafe class YFBitmap : IDisposable
    {
        internal static YFBitmap InternalCreateBitmap;

        public static void RegistPlatform<T>() where T : YFBitmap, new() => InternalCreateBitmap = new T();

        public static void RegistPlatform(object o) => InternalCreateBitmap = (o is YFBitmap bitmap) ? bitmap : InternalCreateBitmap;

        public static YFBitmap Create(int width, int height) => InternalCreateBitmap?.InternalCreate(width, height);

        public static YFBitmap Create(Stream stream) => InternalCreateBitmap?.InternalCreate(stream);

        public static YFBitmap Create(string filePath) => InternalCreateBitmap?.InternalCreate(filePath);


        public abstract int Width { get; }
        public abstract int Height { get; }
        /// <summary>
        /// must be BB GG RR AA
        /// </summary>
        /// <returns></returns>
        public abstract IntPtr GetPixels();
        public abstract void Save(string filePath);
        public abstract void Save(Stream stream);
        protected abstract YFBitmap InternalCreate(int width, int height);
        protected abstract YFBitmap InternalCreate(Stream stream);
        protected abstract YFBitmap InternalCreate(string filePath);
        public abstract void Dispose();

        public virtual int Square => Width * Height;
        public virtual void MoveTo(YFBitmap decimg, int mX, int mY)
        {
            uint* res = (uint*)GetPixels().ToPointer();
            uint* dec = (uint*)decimg.GetPixels().ToPointer();
            for (int y = 0; y < Height; y++)
            {
                uint* resraw = res + y * Width;
                uint* decraw = dec + (y + mY) * decimg.Width + mX;
                for (int x = 0; x < Width; x++)
                {
                    if (((x + mX) >= decimg.Width) || ((y + mY) >= decimg.Height) || ((x + mX) < 0) || ((y + mY) < 0))
                    {
                        decraw++;
                        resraw++;
                    }
                    else
                    {
                        *decraw++ = *resraw++;
                    }
                }
            }
        }
        public virtual YFBitmap Cut(int mX, int mY, int mWidth, int mHeight)
        {
            YFBitmap ans = Create(mWidth, mHeight);
            uint* res = (uint*)GetPixels().ToPointer();
            uint* dec = (uint*)ans.GetPixels().ToPointer();
            for (int y = 0; y < mHeight; y++)
            {
                uint* decraw = dec + y * mWidth;
                uint* resraw = res + (y + mY) * Width + mX;
                for (int x = 0; x < mWidth; x++)
                {
                    if (((x + mX) >= Width) || ((y + mY) >= Height) || ((x + mX) < 0) || ((y + mY) < 0))
                    {
                        *decraw++ = 0;
                        resraw++;
                    }
                    else
                    {
                        *decraw++ = *resraw++;
                    }
                }
            }
            return ans;
        }
        public virtual YFBitmap Rotate0()
        {
            int resH = Height;
            int resW = Width;
            YFBitmap N = Create(resW, resH);
            uint* res = (uint*)GetPixels().ToPointer();
            uint* dec = (uint*)N.GetPixels().ToPointer();
            for (int y = 0; y < resH; y++)
            {
                for (int x = 0; x < resW; x++)
                {
                    *dec++ = *res++;
                }
            }
            return N;
        }
        public virtual YFBitmap Rotate90()
        {
            int resH = Height;
            int resW = Width;
            YFBitmap N = Create(resH, resW);
            uint* res = (uint*)GetPixels().ToPointer();
            uint* dec = (uint*)N.GetPixels().ToPointer();
            for (int y = 0; y < resH; y++)
            {
                for (int x = 0; x < resW; x++)
                {
                    dec[x * resH + (resH - y - 1)] = *res++;
                }
            }
            return N;
        }
        public virtual YFBitmap Rotate180()
        {
            int resH = Height;
            int resW = Width;
            YFBitmap N = Create(resW, resH);
            uint* res = (uint*)GetPixels().ToPointer();
            uint* dec = (uint*)N.GetPixels().ToPointer() + resH * resW;
            for (int y = 0; y < resH; y++)
            {
                for (int x = 0; x < resW; x++)
                {
                    *--dec = *res++;
                }
            }
            return N;
        }
        public virtual YFBitmap Rotate270()
        {
            int resH = Height;
            int resW = Width;
            YFBitmap N = Create(resH, resW);
            uint* res = (uint*)GetPixels().ToPointer();
            uint* dec = (uint*)N.GetPixels().ToPointer();
            for (int y = 0; y < resH; y++)
            {
                for (int x = 0; x < resW; x++)
                {
                    dec[(resW - x - 1) * resH + y] = *res++;
                }
            }
            return N;
        }
    }
}