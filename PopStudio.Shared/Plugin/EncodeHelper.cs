using System.Text;

namespace PopStudio.Plugin
{
    internal static class EncodeHelper
    {
        public static readonly Encoding ANSI = GetEncodingInternal();

        static Encoding GetEncodingInternal()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Setting.AppLanguage switch
            {
                Constant.Language.ZHCN => Encoding.GetEncoding("GB2312"),
                _ => Encoding.Latin1
            };
        }
    }
}
