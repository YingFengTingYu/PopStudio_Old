using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PopStudio.PopAnim
{
    internal class Float64ArrayWriteOnlyConverter : JsonConverter<double[]>
    {
        static readonly string space = "  ";

        public override double[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => null;

        public override void Write(Utf8JsonWriter writer, double[] doubleValue, JsonSerializerOptions options)
        {
            if (doubleValue == null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStartArray();
            if (doubleValue.Length != 0)
            {
                int depth = writer.CurrentDepth;
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine);
                for (int i = 0; i < depth; i++)
                {
                    sb.Append(space);
                }
                for (int i = 0; i < doubleValue.Length; i++)
                {
                    writer.WriteRawValue($"{sb}{ValueToString(doubleValue[i])}");
                }
            }
            writer.WriteEndArray();
        }

        static string ValueToString(double value)
        {
            string ans = value.ToString("F7");
            int i = 1;
            while (true)
            {
                char c = ans[^i];
                if (c == '0')
                {
                    i++;
                }
                else if (c == '.')
                {
                    i -= 2;
                    break;
                }
                else
                {
                    i--;
                    break;
                }
            }
            //if (ans.IndexOf('.') < 0)
            //{
            //    ans += ".0";
            //}
            return ans[..^i];
        }
    }

    internal class Float64WriteOnlyConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 0;

        public override void Write(Utf8JsonWriter writer, double doubleValue, JsonSerializerOptions options) => writer.WriteRawValue(ValueToString(doubleValue));//writer.Write(ValueToString(doubleValue));

        static string ValueToString(double value)
        {
            string ans = value.ToString("F7");
            int i = 1;
            while (true)
            {
                char c = ans[^i];
                if (c == '0')
                {
                    i++;
                }
                else if (c == '.')
                {
                    i -= 2;
                    break;
                }
                else
                {
                    i--;
                    break;
                }
            }
            //if (ans.IndexOf('.') < 0)
            //{
            //    ans += ".0";
            //}
            return ans[..^i];
        }
    }
}
