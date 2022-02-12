namespace PopStudio.Constant
{
    internal interface ILanguage
    {
        public string EndOfFile { get; }
        public string VarIntTooBig { get; }
        public string DataMisMatch { get; }
        public string AppName => "PopStudio";
        public string AppVersion => "1.0.0";
        public string AppAuthor { get; }
        public string FileNotFound { get; }
        public string FolderNotFound { get; }
        public string XmemCompressInvalid { get; }
    }
}
