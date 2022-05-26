using System.Drawing;
using System.Drawing.Imaging;

namespace PopStudio.Platform
{
    /// <summary>
    /// It's fast but just support Windows in dotnet 7 and later version
    /// </summary>
    internal class GDIBitmap : YFBitmap
    {
        public override int Width => m_width;
        public override int Height => m_height;

        readonly System.Drawing.Bitmap m_bitmap;
        BitmapData m_data;
        readonly int m_width;
        readonly int m_height;

        public GDIBitmap()
        {

        }

        public GDIBitmap(int width, int height)
        {
            m_bitmap = new System.Drawing.Bitmap(width, height);
            m_width = m_bitmap.Width;
            m_height = m_bitmap.Height;
            m_data = m_bitmap.LockBits(new Rectangle(0, 0, m_width, m_height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        }

        public GDIBitmap(Stream stream)
        {
            m_bitmap = new System.Drawing.Bitmap(System.Drawing.Image.FromStream(stream));
            m_width = m_bitmap.Width;
            m_height = m_bitmap.Height;
            m_data = m_bitmap.LockBits(new Rectangle(0, 0, m_width, m_height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        }

        public GDIBitmap(string filePath)
        {
            m_bitmap = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(filePath));
            m_width = m_bitmap.Width;
            m_height = m_bitmap.Height;
            m_data = m_bitmap.LockBits(new Rectangle(0, 0, m_width, m_height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        }

        protected override YFBitmap InternalCreate(int width, int height) => new GDIBitmap(width, height);
        protected override YFBitmap InternalCreate(Stream stream) => new GDIBitmap(stream);
        protected override YFBitmap InternalCreate(string filePath) => new GDIBitmap(filePath);

        /// <summary>
        /// BB GG RR AA
        /// </summary>
        /// <returns></returns>
        public override nint GetPixels() => m_data.Scan0;

        public override void Save(string filePath) => m_bitmap.Save(filePath, ImageFormat.Png);

        public override void Save(Stream stream) => m_bitmap.Save(stream, ImageFormat.Png);

        public override void Dispose()
        {
            if (m_data != null)
            {
                m_bitmap?.UnlockBits(m_data);
                m_data = null;
            }
            m_bitmap?.Dispose();
        }
    }
}
