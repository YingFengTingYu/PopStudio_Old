namespace PopStudio.PopAnim
{
    internal class PopAnimInfo
    {
        public static readonly uint Magic = 0xBAF01954;
        public static readonly int MaxVersion = 6;

        public int version { get; set; } = 6;
        public byte frame_rate { get; set; } = 30;
        public double[] position { get; set; } //x,y / 20
        public double[] size { get; set; } //width,height / 20
        public ImageInfo[] image { get; set; }
        public SpriteInfo[] sprite { get; set; }
        public SpriteInfo main_sprite { get; set; }

        public void Write(BinaryStream bs)
        {
            bs.WriteUInt32(Magic);
            bs.WriteInt32(version);
            if (version > MaxVersion)
            {
                throw new Exception(Str.Obj.TypeMisMatch);
            }
            bs.WriteByte(frame_rate);
            if (position == null || position.Length < 2)
            {
                bs.WriteInt16(0);
                bs.WriteInt16(0);
            }
            else
            {
                bs.WriteInt16((short)(position[0] * 20));
                bs.WriteInt16((short)(position[1] * 20));
            }
            if (size == null || size.Length < 2)
            {
                bs.WriteInt16(-1);
                bs.WriteInt16(-1);
            }
            else
            {
                bs.WriteInt16((short)(size[0] * 20));
                bs.WriteInt16((short)(size[1] * 20));
            }
            if (image == null || image.Length == 0)
            {
                bs.WriteInt16(0);
            }
            else
            {
                int imagesCount = image.Length;
                bs.WriteInt16((short)imagesCount);
                for (int i = 0; i < imagesCount; i++)
                {
                    image[i].Write(bs, version);
                }
            }
            if (sprite == null || sprite.Length == 0)
            {
                bs.WriteInt16(0);
            }
            else
            {
                int spriteCount = sprite.Length;
                bs.WriteInt16((short)spriteCount);
                for (int i = 0; i < spriteCount; i++)
                {
                    sprite[i].Write(bs, version);
                }
            }
            if (version <= 3)
            {
                SpriteInfo mMain = main_sprite ?? new SpriteInfo();
                mMain.Write(bs, version);
            }
            else
            {
                if (main_sprite == null)
                {
                    bs.WriteBoolean(false);
                }
                else
                {
                    bs.WriteBoolean(true);
                    main_sprite.Write(bs, version);
                }
            }
        }

        public PopAnimInfo Read(BinaryStream bs)
        {
            bs.IdUInt32(Magic);
            int version = bs.ReadInt32();
            this.version = version;
            if (version > MaxVersion)
            {
                throw new Exception(Str.Obj.TypeMisMatch);
            }
            byte frame_rate = bs.ReadByte();
            this.frame_rate = frame_rate;
            position = new double[2];
            for (int i = 0; i < 2; i++)
            {
                position[i] = bs.ReadInt16() / 20d;
            }
            size = new double[2];
            for (int i = 0; i < 2; i++)
            {
                size[i] = bs.ReadInt16() / 20d;
            }
            int imagesCount = bs.ReadInt16();
            image = new ImageInfo[imagesCount];
            for (int i = 0; i < imagesCount; i++)
            {
                Console.WriteLine(bs.Position);
                image[i] = new ImageInfo().Read(bs, version);
            }
            int spritesCount = bs.ReadInt16();
            sprite = new SpriteInfo[spritesCount];
            for (int i = 0; i < spritesCount; i++)
            {
                sprite[i] = new SpriteInfo().Read(bs, version);
                if (version < 4)
                {
                    sprite[i].frame_rate = frame_rate;
                }
            }
            if (version <= 3 || bs.ReadBoolean())
            {
                main_sprite = new SpriteInfo().Read(bs, version);
                if (version < 4)
                {
                    main_sprite.frame_rate = frame_rate;
                }
            }
            return this;
        }
    }
}
