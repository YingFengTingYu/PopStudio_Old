using System.Text.Json;
using System.Text.Json.Nodes;

namespace PopStudio.Atlas
{
    internal class ResRTON
    {
        public static bool Splice(string inFolder, string outFile, string infoPath, string itemName, int width, int height)
        {
            Dictionary<string, SubImageInfo> cutinfo = CutImage.Splice(inFolder, outFile, width, height);
            if (string.IsNullOrEmpty(itemName) && File.Exists(inFolder + Const.PATHSEPARATOR + "AtlasID.txt"))
            {
                itemName = File.ReadAllText(inFolder + Const.PATHSEPARATOR + "AtlasID.txt").Replace("\r", "").Replace("\n", "");
            }
            string same1 = Path.GetFileNameWithoutExtension(outFile).ToLower();
            Func<JsonNode, bool> IsSame = string.IsNullOrEmpty(itemName) ? (node) => node["path"]?.AsArray()[^1]?.GetValue<string>()?.ToLower() == same1 : (node) => node["id"]?.GetValue<string>() == itemName;
            //First, convert RTON to json
            using (TempFilePool pool = new TempFilePool())
            {
                string tempfile = pool.Add();
                RTON.RTON.Decode(infoPath, tempfile);
                JsonNode json;
                using (BinaryStream bstemp = new BinaryStream(tempfile, FileMode.Open))
                {
                    json = JsonNode.Parse(bstemp, null, new JsonDocumentOptions { AllowTrailingCommas = true });
                }
                JsonArray array = json["groups"].AsArray();
                JsonArray ansnode = null;
                foreach (JsonNode node in array)
                {
                    if (node["type"]?.GetValue<string>() == "simple")
                    {
                        JsonArray res = node["resources"]?.AsArray();
                        if (res != null)
                        {
                            foreach (JsonNode node2 in res)
                            {
                                if (node2["type"]?.GetValue<string>() == "Image" && node2["atlas"]?.GetValue<bool>() == true && IsSame(node2))
                                {
                                    ansnode = res;
                                    itemName = node2["id"].GetValue<string>();
                                    node2["width"] = width;
                                    node2["height"] = height;
                                    break;
                                }
                            }
                        }
                        if (ansnode != null) break;
                    }
                }
                if (ansnode == null)
                {
                    return false;
                }
                foreach (JsonNode cd in ansnode)
                {
                    if (cd["type"]?.GetValue<string>() == "Image" && cd["parent"]?.GetValue<string>() == itemName)
                    {
                        SubImageInfo temp = cutinfo[cd["id"].GetValue<string>().ToLower()];
                        cd["ax"] = temp.X;
                        cd["ay"] = temp.Y;
                        cd["aw"] = temp.Width;
                        cd["ah"] = temp.Height;
                    }
                }
                using (BinaryStream bs = new BinaryStream(tempfile, FileMode.Create))
                {
                    using (Utf8JsonWriter u8w = new Utf8JsonWriter(bs, new JsonWriterOptions { Indented = true }))
                    {
                        json.WriteTo(u8w);
                    }
                }
                RTON.RTON.Encode(tempfile, infoPath);
                return true;
            }
        }

        public static bool Cut(string inFile, string outFolder, string infoPath, string itemName)
        {
            string same1 = Path.GetFileNameWithoutExtension(inFile).ToLower();
            Func<JsonNode, bool> IsSame = string.IsNullOrEmpty(itemName) ? (node) => node["path"]?.AsArray()[^1]?.GetValue<string>()?.ToLower() == same1 : (node) => node["id"]?.GetValue<string>() == itemName;
            //First, convert RTON to json
            using (TempFilePool pool = new TempFilePool())
            {
                string tempfile = pool.Add();
                RTON.RTON.Decode(infoPath, tempfile);
                JsonNode json;
                using (BinaryStream bstemp = new BinaryStream(tempfile, FileMode.Open))
                {
                    json = JsonNode.Parse(bstemp, null, new JsonDocumentOptions { AllowTrailingCommas = true });
                }
                JsonArray array = json["groups"].AsArray();
                JsonArray ansnode = null;
                foreach (JsonNode node in array)
                {
                    if (node["type"]?.GetValue<string>() == "simple")
                    {
                        JsonArray res = node["resources"]?.AsArray();
                        if (res != null)
                        {
                            foreach (JsonNode node2 in res)
                            {
                                if (node2["type"]?.GetValue<string>() == "Image" && node2["atlas"]?.GetValue<bool>() == true && IsSame(node2))
                                {
                                    ansnode = res;
                                    itemName = node2["id"].GetValue<string>();
                                    break;
                                }
                            }
                        }
                        if (ansnode != null) break;
                    }
                }
                if (ansnode == null)
                {
                    return false;
                }
                List<SubImageInfo> cutinfo = new List<SubImageInfo>();
                foreach (JsonNode cd in ansnode)
                {
                    if (cd["type"]?.GetValue<string>() == "Image" && cd["parent"]?.GetValue<string>() == itemName)
                    {
                        cutinfo.Add(new SubImageInfo(cd["ax"]?.GetValue<int>() ?? 0, cd["ay"]?.GetValue<int>() ?? 0, cd["aw"]?.GetValue<int>() ?? 0, cd["ah"]?.GetValue<int>() ?? 0, cd["id"]?.GetValue<string>()));
                    }
                }
                if (cutinfo.Count == 0) return false;
                CutImage.Cut(inFile, outFolder, cutinfo);
                File.WriteAllText(outFolder + Const.PATHSEPARATOR + "AtlasID.txt", itemName);
                return true;
            }
        }
    }
}
