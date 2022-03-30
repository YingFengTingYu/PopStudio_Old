using System.Text;

namespace PopStudio.Plugin
{
    internal static class EncodeHelper
    {
        public static readonly Encoding ANSI = GetEncodingInternal();

        static Encoding GetEncodingInternal()
        {
            int ansi = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
            //Can Get?
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding ansiencode = Encoding.GetEncoding(ansi);
                return ansiencode;
            }
            catch (Exception)
            {

            }
            //Is Exist?
            try
            {
                foreach (EncodingInfo i in Encoding.GetEncodings())
                {
                    Encoding encode = i.GetEncoding();
                    if (encode.WindowsCodePage == ansi && encode.CodePage != 20127)
                    {
                        //encode is ANSI
                        return encode;
                    }
                }
            }
            catch (Exception)
            {

            }
            return Encoding.ASCII;
        }
    }
}
