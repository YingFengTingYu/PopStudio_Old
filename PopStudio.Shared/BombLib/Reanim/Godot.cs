namespace PopStudio.Reanim
{
    internal static class Godot
    {
        public static void Encode(Reanim reanim, string outFile, string imgFolder = "reanim")
        {
            using StreamWriter writer = new StreamWriter(outFile);
            writer.WriteLine(@"[gd_scene load_steps=3 format=2]");
            writer.WriteLine();

            //writer.WriteLine($@"[ext_resource path=""res://{script}"" type=""Script"" id=1]");
            Dictionary<string, int> imgs = new();
            int resId = 1;
            List<GodotAnim> animList = new();
            int trackCount = reanim.tracks.Length;
            int frameCount = reanim.tracks[0].transforms.Length;
            for (int trackIndex = 0; trackIndex < trackCount;trackIndex++)
            {
                GodotAnim ganim = null;
                ReanimTrack track = reanim.tracks[trackIndex];
                if (track.name.StartsWith("anim"))
                {
                    ganim = new()
                    {
                        tracks = new GodotAnimTrack[trackCount],
                        visibles = new float[frameCount],
                        name = track.name
                    };
                    animList.Add(ganim);
                }
                for (int transformIndex = 0; transformIndex < frameCount; transformIndex++)
                {
                    ReanimTransform transform = track.transforms[transformIndex];
                    if (transform.i != null)
                    {
                        string res = transform.i.ToString();
                        if (!imgs.ContainsKey(res))
                        {
                            string filename = res.Replace("IMAGE_REANIM_", "");
                            filename = filename.ToLower();
                            writer.WriteLine($@"[ext_resource path=""res://{imgFolder}/{filename}.png"" type=""Texture"" id={resId}]");
                            imgs.Add(res, resId);
                            resId++;
                        }
                    }
                    if(ganim != null)
                    {
                        if(transform.f != null)
                        {
                            ganim.visibles[transformIndex] = (float)transform.f;
                        }
                        else if(transformIndex == 0)
                        {
                            ganim.visibles[transformIndex] = 0;
                        } 
                        else
                        {
                            ganim.visibles[transformIndex] = ganim.visibles[transformIndex - 1];
                        }
                    }
                }
            }

            List<ReanimTrack> tracks = new();
            for (int trackIndex = 0; trackIndex < trackCount; trackIndex++)
            {
                ReanimTrack track = reanim.tracks[trackIndex];
                bool isTrack = false;
                foreach (ReanimTransform transform in track.transforms)
                {
                    if (transform.i != null)
                    {
                        tracks.Add(track);
                        isTrack = true;
                        break;
                    }
                }
                if (isTrack)
                {
                    ReanimTransform2 tmp = new();
                    tmp.updateTransform();
                    for (int transformIndex = 0;transformIndex < frameCount;transformIndex++)
                    {
                        ReanimTransform transform = track.transforms[transformIndex];
                        tmp.x = transform.x ?? tmp.x;
                        tmp.y = transform.y ?? tmp.y;
                        tmp.sx = transform.sx ?? tmp.sx;
                        tmp.sy = transform.sy ?? tmp.sy;
                        tmp.kx = transform.kx ?? tmp.kx;
                        tmp.ky = transform.ky ?? tmp.ky;
                        tmp.i = transform.i ?? tmp.i;
                        tmp.f = transform.f ?? tmp.f;

                        foreach (GodotAnim ga in animList)
                            if (ga.visibles[transformIndex] == 0)
                            {
                                if (ga.tracks[trackIndex] == null)
                                {
                                    var gtrack = ga.tracks[trackIndex] = new GodotAnimTrack();
                                    gtrack.name = track.name;
                                }
                            }

                        bool updateTransform = transform.x != null || transform.y != null ||
                            transform.sx != null || transform.sy != null ||
                            transform.kx != null || transform.ky != null;
                        if(updateTransform)
                        {
                            tmp.updateTransform();
                        }

                        bool updateTexture = transform.i != null;
                        if (updateTexture)
                        {
                            tmp.textureString = $"ExtResource( {imgs[transform.i.ToString()]} )";
                        }


                        foreach (GodotAnim ga in animList)
                        {
                            var gtrack = ga.tracks[trackIndex];
                            if (ga.visibles[transformIndex] == 0 && gtrack != null)
                            {
                                float time = gtrack.frame * rfps;
                                if (updateTransform || transform.f == 0 || gtrack.init && transform.f != -1)
                                {
                                    gtrack.times.Add(time.ToString("N1"));
                                    gtrack.transitions.Add("1");
                                    gtrack.values.Add(tmp.transformString);

                                }
                                if (tmp.f == -1)
                                {
                                    if (gtrack.init || transform.f == -1)
                                    {
                                        gtrack.timesTex.Add(time.ToString("N1"));
                                        gtrack.transitionsTex.Add("1");
                                        gtrack.valuesTex.Add("null");
                                    }
                                }
                                else if (updateTexture || transform.f == 0 || gtrack.init)
                                {
                                    gtrack.timesTex.Add(time.ToString("N1"));
                                    gtrack.transitionsTex.Add("1");
                                    gtrack.valuesTex.Add(tmp.textureString);

                                }
                            }
                        }

                        foreach (GodotAnim ga in animList)
                            if (ga.visibles[transformIndex] == 0 && ga.tracks[trackIndex] != null)
                            {
                                ga.tracks[trackIndex].init = false;
                                ga.tracks[trackIndex].frame++;
                            }
                    }
                }
            }


            for (int animIndex = 0;animIndex < animList.Count;animIndex++)
            {
                GodotAnim ga = animList[animIndex];
                int trackId = 0;
                writer.WriteLine();
                writer.WriteLine($@"[sub_resource type=""Animation"" id={animIndex+1}]");
                writer.WriteLine($@"length = {ga.FrameCount * rfps}");
                writer.WriteLine("loop = true");
                foreach(GodotAnimTrack gtrack in ga.tracks)
                {
                    if(gtrack == null)
                    {
                        continue;
                    }
                    if(gtrack.times.Count == 0)
                    {
                        continue;
                    }
                    writer.WriteLine(
$@"tracks/{trackId}/type = ""value""
tracks/{trackId}/path = NodePath(""Sprites/{gtrack.name}:transform"")
tracks/{trackId}/interp = 1
tracks/{trackId}/loop_wrap = true
tracks/{trackId}/imported = false
tracks/{trackId}/enabled = true
tracks/{trackId}/keys = {{
""times"": PoolRealArray( {string.Join(", ", gtrack.times)} ),
""transitions"": PoolRealArray( {string.Join(", ", gtrack.transitions)} ),
""update"": 0,
""values"": [ { string.Join(", ", gtrack.values)} ]
}}");

                    trackId++;


                    writer.WriteLine(
$@"tracks/{trackId}/type = ""value""
tracks/{trackId}/path = NodePath(""Sprites/{gtrack.name}:texture"")
tracks/{trackId}/interp = 1
tracks/{trackId}/loop_wrap = true
tracks/{trackId}/imported = false
tracks/{trackId}/enabled = true
tracks/{trackId}/keys = {{
""times"": PoolRealArray( {string.Join(", ", gtrack.timesTex)} ),
""transitions"": PoolRealArray( {string.Join(", ", gtrack.transitionsTex)} ),
""update"": 1,
""values"": [ { string.Join(", ", gtrack.valuesTex)} ]
}}");

                    trackId++;
                }
            }



            writer.WriteLine();
            writer.WriteLine(@"[node name=""Node2D"" type=""Node2D""]");
            writer.WriteLine();
            writer.WriteLine(@"[node name=""AnimationPlayer"" type=""AnimationPlayer"" parent="".""]");

            for (int animIndex = 0; animIndex < animList.Count; animIndex++)
            {
                GodotAnim ga = animList[animIndex];
                writer.WriteLine($@"""anims/{ga.name}"" = SubResource({animIndex+1})");
            }
            //writer.WriteLine(@"anims /RESET = SubResource(1)");

            writer.WriteLine();
            writer.WriteLine(@"[node name=""Sprites"" type=""Node2D"" parent="".""]");
            writer.WriteLine("position = Vector2( 60, 60 )");

            foreach (var track in tracks)
            {
                writer.WriteLine();
                writer.WriteLine(
$@"[node name=""{track.name}"" type=""Sprite"" parent=""Sprites""]
centered = false
position = Vector2(0, 0)");
            }
        }

        public static Reanim Decode(string inFile)
        {
            throw new NotImplementedException();
        }


        private class ReanimTransform2
        {
            public float x = 0;
            public float y = 0;
            public float kx = 0;
            public float ky = 0;
            public float sx = 1;
            public float sy = 1;
            public object i = null;
            public float f = 0;
            public float a;
            public float b;
            public float c;
            public float d;
            public string transformString;
            public string textureString;
            public void updateTransform()
            {
                a = (float)(Math.Cos(kx / 180 * PI) * sx);
                b = (float)(Math.Sin(kx / 180 * PI) * sx);
                c = (float)(-Math.Sin(ky / 180 * PI) * sy);
                d = (float)(Math.Cos(ky / 180 * PI) * sy);
                transformString = $"Transform2D( {a.ToString("N6")}, {b.ToString("N6")}, {c.ToString("N6")}, {d.ToString("N6")}, {x}, {y} )";
            }
        }
        private class GodotAnim
        {
            public float[] visibles;
            public GodotAnimTrack[] tracks;
            public string name;
            public int FrameCount
            { 
                get
                {
                    foreach(var track in tracks)
                    {
                        if(track != null)
                        {
                            return track.frame;
                        }
                    }
                    return 0;
                }
            }
        }
        private class GodotAnimTrack
        {
            public List<string> times = new();
            public List<string> transitions = new();
            public List<string> values = new();
            public List<string> timesTex = new() { "0" };
            public List<string> transitionsTex = new() { "1" };
            public List<string> valuesTex = new() { "null" };
            public bool init = true;
            public string name;
            public int frame = 0;
        }
        static readonly float PI = (float)Math.PI;
        static readonly float rfps = 1 / 10f;
    }
}
