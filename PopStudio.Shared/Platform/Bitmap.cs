namespace PopStudio.Platform
{
    public abstract class Bitmap : IDisposable
    {
        public virtual unsafe void MoveTo(Bitmap decimg, int mX, int mY)
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

        public virtual unsafe Bitmap Cut(int mX, int mY, int mWidth, int mHeight)
        {
            Bitmap ans = Create(mWidth, mHeight);
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

        public virtual unsafe Bitmap Rotate0()
        {
            int resH = Height;
            int resW = Width;
            Bitmap N = Create(resW, resH);
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

        public virtual unsafe Bitmap Rotate90()
        {
            int resH = Height;
            int resW = Width;
            Bitmap N = Create(resH, resW);
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

        public virtual unsafe Bitmap Rotate180()
        {
            int resH = Height;
            int resW = Width;
            Bitmap N = Create(resW, resH);
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

        public virtual unsafe Bitmap Rotate270()
        {
            int resH = Height;
            int resW = Width;
            Bitmap N = Create(resH, resW);
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

        public abstract int Width { get; }
        public abstract int Height { get; }

        /// <summary>
        /// must be BB GG RR AA
        /// </summary>
        /// <returns></returns>
        public abstract IntPtr GetPixels();

        public abstract void Save(string filePath);

        public abstract void Save(Stream stream);

        internal static Bitmap InternalCreateBitmap;

        public static void RegistPlatform<T>() where T : Bitmap, new() => InternalCreateBitmap = new T();

        public static void RegistPlatform(object o) => InternalCreateBitmap = (o is Bitmap bitmap) ? bitmap : InternalCreateBitmap;

        public static Bitmap Create(int width, int height) => InternalCreateBitmap?.InternalCreate(width, height);

        public static Bitmap Create(Stream stream) => InternalCreateBitmap?.InternalCreate(stream);

        public static Bitmap Create(string filePath) => InternalCreateBitmap?.InternalCreate(filePath);

        protected abstract Bitmap InternalCreate(int width, int height);
        protected abstract Bitmap InternalCreate(Stream stream);
        protected abstract Bitmap InternalCreate(string filePath);

        //public static Texture.TextureInfo EncodeTexture(Bitmap map, Texture.TextureFormat format) => Texture.Coder.Encode(map, format);

        //public static Bitmap DecodeTexture(Texture.TextureInfo info) => Texture.Coder.Decode(info);

        //public Texture.TextureInfo EncodeAsTexture(Texture.TextureFormat format) => Texture.Coder.Encode(this, format);

        public abstract void Dispose();
    }
}