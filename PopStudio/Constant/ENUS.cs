﻿namespace PopStudio.Constant
{
    internal class ENUS : ILanguage
    {
        public string EndOfFile => "Cannot read out of stream";

        public string VarIntTooBig => "The varint is too big to read";

        public string DataMisMatch => "The data is mismatched";

        public string AppAuthor => "yingfengtingyu";

        public string FileNotFound => "File {0} is not exist";

        public string FolderNotFound => "Folder {0} is not exist";

        public string XmemCompressInvalid => "Xmem compress is invalid";
    }
}
