using PopStudio.Platform;

namespace PopStudio.Plugin
{
    internal struct SubImageInfo
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public string ID;
        public bool rotate270;
        public YFBitmap Image = null;

        public SubImageInfo()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
            ID = null;
            rotate270 = false;
        }

        public SubImageInfo(int X, int Y, int Width, int Height, string ID)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.ID = ID;
            rotate270 = false;
        }

        public SubImageInfo(string X, string Y, string Width, string Height, string ID)
        {
            this.X = Convert.ToInt32(X);
            this.Y = Convert.ToInt32(Y);
            this.Width = Convert.ToInt32(Width);
            this.Height = Convert.ToInt32(Height);
            this.ID = ID;
            rotate270 = false;
        }

        public SubImageInfo(YFBitmap Image, string ID)
        {
            this.Image = Image;
            X = 0;
            Y = 0;
            Width = Image.Width;
            Height = Image.Height;
            this.ID = ID;
            rotate270 = false;
        }

        public void ResetCoordinate(SubImageInfo s)
        {
            X = s.X;
            Y = s.Y;
        }

        public void ResetCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void ClearImage()
        {
            Image = null;
        }
    }
}
