using System.Text;

namespace PopStudio.Plugin
{
    internal static class EncodeHelper
    {
        public static Encoding ANSI => Setting.AppLanguage switch
        {
            Constant.Language.ZHCN => Gb2312,
            _ => Latin1
        };

        static readonly Encoding Latin1;
        static readonly Encoding Gb2312;

        static EncodeHelper()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Gb2312 = Encoding.GetEncoding("GB2312");
            Latin1 = Encoding.Latin1;
        }
    }
}
