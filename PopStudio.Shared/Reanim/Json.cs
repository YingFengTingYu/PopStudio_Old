using System.Text.Json;

namespace PopStudio.Reanim
{
    internal static class ReanimJson
    {
        public static void Encode(Reanim reanim, string outFile)
        {
            using (StreamWriter sw = new StreamWriter(outFile, false))
            {
                sw.Write('{');
                sw.Write('\n');
                if (reanim.doScale != null)
                {
                    sw.Write("    \"doScale\":");
                    sw.Write(reanim.doScale);
                    sw.Write(',');
                    sw.Write('\n');
                }
                sw.Write("    \"fps\":");
                sw.Write(reanim.fps);
                sw.Write(",\n    \"tracks\":[");
                if (reanim.tracks.Length == 0)
                {
                    sw.Write("],");
                }
                else
                {
                    int times = reanim.tracks.Length;
                    for (int i = 0; i < times; i++)
                    {
                        if (i != 0) sw.Write(',');
                        sw.Write("\n        {");
                        ReanimTrack track = reanim.tracks[i];
                        if (!string.IsNullOrEmpty(track.name))
                        {
                            sw.Write("\n            \"name\":\"");
                            sw.Write(track.name.Replace("\\", "\\\\").Replace("\"", "\\\""));
                            sw.Write('\"');
                            sw.Write(',');
                        }
                        sw.Write("\n            \"transforms\":[");
                        int times2 = track.transforms.Length;
                        if (times2 == 0)
                        {
                            sw.Write(']');
                        }
                        else
                        {
                            for (int j = 0; j < times2; j++)
                            {
                                if (j != 0) sw.Write(',');
                                sw.Write("\n                {");
                                bool isnull = true;
                                ReanimTransform transform = track.transforms[j];
                                if (transform.x != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"x\":");
                                    sw.Write(transform.x);
                                }
                                if (transform.y != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"y\":");
                                    sw.Write(transform.y);
                                }
                                if (transform.kx != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"kx\":");
                                    sw.Write(transform.kx);
                                }
                                if (transform.ky != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"ky\":");
                                    sw.Write(transform.ky);
                                }
                                if (transform.sx != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"sx\":");
                                    sw.Write(transform.sx);
                                }
                                if (transform.sy != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"sy\":");
                                    sw.Write(transform.sy);
                                }
                                if (transform.f != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"f\":");
                                    sw.Write(transform.f);
                                }
                                if (transform.a != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"a\":");
                                    sw.Write(transform.a);
                                }
                                if (transform.i != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"i\":");
                                    if (transform.i is int)
                                    {
                                        sw.Write((int)transform.i);
                                    }
                                    else
                                    {
                                        sw.Write('\"');
                                        sw.Write(((string)transform.i).Replace("\\", "\\\\").Replace("\"", "\\\""));
                                        sw.Write('\"');
                                    }
                                }
                                if (transform.iPath != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"resource\":\"");
                                    sw.Write(transform.iPath.Replace("\\", "\\\\").Replace("\"", "\\\""));
                                    sw.Write('\"');
                                }
                                if (transform.i2 != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"i2\":\"");
                                    sw.Write(transform.i2.Replace("\\", "\\\\").Replace("\"", "\\\""));
                                    sw.Write('\"');
                                }
                                if (transform.i2Path != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"resource2\":\"");
                                    sw.Write(transform.i2Path.Replace("\\", "\\\\").Replace("\"", "\\\""));
                                    sw.Write('\"');
                                }
                                if (transform.font != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"font\":\"");
                                    sw.Write(transform.font.Replace("\\", "\\\\").Replace("\"", "\\\""));
                                    sw.Write('\"');
                                }
                                if (transform.text != null)
                                {
                                    if (isnull)
                                    {
                                        isnull = false;
                                    }
                                    else
                                    {
                                        sw.Write(',');
                                    }
                                    sw.Write("\n                    \"text\":\"");
                                    sw.Write(transform.text.Replace("\\", "\\\\").Replace("\"", "\\\""));
                                    sw.Write('\"');
                                }
                                if (isnull)
                                {
                                    sw.Write('}');
                                }
                                else
                                {
                                    sw.Write("\n                }");
                                }
                            }
                            sw.Write("\n            ]");
                        }
                        sw.Write("\n        }");
                    }
                    sw.Write("\n    ]");
                }
                sw.Write('\n');
                sw.WriteLine('}');
            }
        }

        public static Reanim Decode(string inFile)
        {
            string jsondata;
            using (StreamReader sr = new StreamReader(inFile))
            {
                jsondata = sr.ReadToEnd();
            }
            using JsonDocument json = JsonDocument.Parse(jsondata);
            JsonElement root = json.RootElement;
            Reanim reanim = new Reanim();
            JsonElement value;
            if (root.TryGetProperty("doScale", out value))
            {
                reanim.doScale = value.GetSByte();
            }
            if (root.TryGetProperty("fps", out value))
            {
                reanim.fps = value.GetSingle();
            }
            else
            {
                reanim.fps = 12F;
            }
            if (root.TryGetProperty("tracks", out value))
            {
                JsonElement tracks = value;
                int trackNum = tracks.GetArrayLength();
                reanim.tracks = new ReanimTrack[trackNum];
                for (int i = 0; i < trackNum; i++)
                {
                    ReanimTrack track = new ReanimTrack();
                    JsonElement jsontrack = tracks[i];
                    if (jsontrack.TryGetProperty("name", out value))
                    {
                        track.name = value.GetString();
                    }
                    if (jsontrack.TryGetProperty("transforms", out value))
                    {
                        JsonElement transforms = value;
                        int transformsNum = transforms.GetArrayLength();
                        track.transforms = new ReanimTransform[transformsNum];
                        for (int j = 0; j < transformsNum; j++)
                        {
                            ReanimTransform transform = new ReanimTransform();
                            JsonElement jsontransform = transforms[j];
                            if (jsontransform.TryGetProperty("x", out value))
                            {
                                transform.x = value.GetSingle();
                            }
                            if (jsontransform.TryGetProperty("y", out value))
                            {
                                transform.y = value.GetSingle();
                            }
                            if (jsontransform.TryGetProperty("kx", out value))
                            {
                                transform.kx = value.GetSingle();
                            }
                            if (jsontransform.TryGetProperty("ky", out value))
                            {
                                transform.ky = value.GetSingle();
                            }
                            if (jsontransform.TryGetProperty("sx", out value))
                            {
                                transform.sx = value.GetSingle();
                            }
                            if (jsontransform.TryGetProperty("sy", out value))
                            {
                                transform.sy = value.GetSingle();
                            }
                            if (jsontransform.TryGetProperty("f", out value))
                            {
                                transform.f = value.GetSingle();
                            }
                            if (jsontransform.TryGetProperty("a", out value))
                            {
                                transform.a = value.GetSingle();
                            }
                            if (jsontransform.TryGetProperty("i", out value))
                            {
                                if (value.ValueKind == JsonValueKind.String)
                                {
                                    transform.i = value.GetString();
                                }
                                else
                                {
                                    transform.i = value.GetInt32();
                                }
                            }
                            if (jsontransform.TryGetProperty("resource", out value))
                            {
                                transform.iPath = value.GetString();
                            }
                            if (jsontransform.TryGetProperty("i2", out value))
                            {
                                transform.i2 = value.GetString();
                            }
                            if (jsontransform.TryGetProperty("resource2", out value))
                            {
                                transform.i2Path = value.GetString();
                            }
                            track.transforms[j] = transform;
                        }
                    }
                    else
                    {
                        track.transforms = new ReanimTransform[0];
                    }
                    reanim.tracks[i] = track;
                }
            }
            else
            {
                reanim.tracks = new ReanimTrack[0];
            }
            return reanim;
        }
    }
}