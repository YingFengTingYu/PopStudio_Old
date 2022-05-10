using System.Text.Encodings.Web;
using System.Text.Json;

namespace PopStudio.PopAnim
{
    internal class Pam
    {
        public static void Encode(string inFile, string outFile)
        {
            PopAnimInfo pam = new PopAnimInfo();
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                pam = JsonSerializer.Deserialize<PopAnimInfo>(bs, new JsonSerializerOptions { AllowTrailingCommas = true });
            }
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                pam.Write(bs);
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            PopAnimInfo pam = new PopAnimInfo();
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                pam.Read(bs);
            }
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                var setting = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                setting.Converters.Add(new Float64WriteOnlyConverter());
                setting.Converters.Add(new Float64ArrayWriteOnlyConverter());
                JsonSerializer.Serialize(bs, pam, setting);
            }
        }
    }
}
