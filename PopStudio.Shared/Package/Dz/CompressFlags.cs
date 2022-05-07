namespace PopStudio.Package.Dz
{
    internal enum CompressFlags : ushort
    {
        /// <summary>
        /// Descript that this is a combuf chunk which will appended to each other by dz decoder.
        /// </summary>
        COMBUF = 1,
        /// <summary>
        /// Descript that this chunk is compressed by dz compression.
        /// </summary>
        DZ = 4,
        /// <summary>
        /// Descript that this chunk is compressed by gzip compression.
        /// </summary>
        ZLIB = 8,
        /// <summary>
        /// Descript that this chunk is compressed by bzip2 compression.
        /// </summary>
        BZIP = 16,
        /// <summary>
        /// Descript that this chunk is mp3.
        /// </summary>
        MP3 = 32,
        /// <summary>
        /// Descript that this chunk is jpg.
        /// </summary>
        JPEG = 64,
        /// <summary>
        /// Descript that this chunk is zero chunk.
        /// </summary>
        ZERO = 128,
        /// <summary>
        /// Descript that this chunk is just copy.
        /// </summary>
        STORE = 256,
        /// <summary>
        /// Descript that this chunk is compressed by lzma compression.
        /// </summary>
        LZMA = 512,
        /// <summary>
        /// Descript that this chunk should be randomly accessed by dz decoder.
        /// </summary>
        RANDOMACCESS = 1024,
    }
}