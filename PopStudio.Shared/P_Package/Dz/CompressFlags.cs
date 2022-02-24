namespace PopStudio.P_Package.Dz
{
    internal enum CompressFlags
    {
        /// <summary>
        /// Just support unpack
        /// </summary>
        COMBUF = 1,
        /// <summary>
        /// Just support unpack
        /// </summary>
        DZ = 4,
        /// <summary>
        /// Named zlib but it's gzip
        /// </summary>
        ZLIB = 8,
        /// <summary>
        /// bzip2 (maybe invalid in pvz)
        /// </summary>
        BZIP = 16,
        /// <summary>
        /// Just copy (Same as STORE)
        /// </summary>
        MP3 = 32,
        /// <summary>
        /// Just copy (Same as STORE)
        /// </summary>
        JPEG = 64,
        /// <summary>
        /// Just copy (Maybe empty file)
        /// </summary>
        ZERO = 128,
        /// <summary>
        /// Just copy
        /// </summary>
        STORE = 256,
        /// <summary>
        /// lzma86head
        /// </summary>
        LZMA = 512,
        /// <summary>
        /// Invalid, just copy, just support unpack
        /// </summary>
        RANDOMACCESS = 1024,
    }
}