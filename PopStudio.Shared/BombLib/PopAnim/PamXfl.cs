using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PopStudio.PopAnim
{
    internal static class PamXfl
    {
        private class Model
        {
            public bool? state { get; set; }
            public int index { get; set; }
            public int resource { get; set; }
            public bool sprite { get; set; }
            public int frame_start { get; set; }
            public int frame_duration { get; set; }
            public double[] color { get; set; }
            public double[] transform { get; set; }
        }
        private class FlashPackage
        {
            public class Library
            {
                public XElement[] image { get; set; }
                public XElement[] sprite { get; set; }
                public XElement main_sprite { get; set; }
            }
            public PopAnimInfo extra { get; set; }
            public XElement document { get; set; }
            public Library library { get; set; }
        }

        public static void Encode(PopAnimInfo pamInfo, string outFolder, int resolution)
        {
            Directory.CreateDirectory(outFolder + "/LIBRARY/image");
            Directory.CreateDirectory(outFolder + "/LIBRARY/sprite");
            Directory.CreateDirectory(outFolder + "/LIBRARY/source");
            Directory.CreateDirectory(outFolder + "/LIBRARY/media");
            var ripe = from(pamInfo);
            save_flash_package(ripe, outFolder);
            create_fsh(outFolder, pamInfo, resolution);
            create_xfl_content_file(outFolder);
            create_image_default(outFolder, pamInfo);
        }

        public static PopAnimInfo Decode(string inFolder)
        {
            var ripe = load_flash_package(inFolder);
            return to(ripe);
        }

        public static void Resize(string inFolder, int resolution)
        {
            resize_fs(inFolder, resolution);
        }


        // ------------- to -------------------------------

        private static double[] parse_transform_origin(XElement x_Matrix)
        {
            return new double[] {
                double.Parse((string)x_Matrix.Attribute("x") ?? "0"),
                double.Parse((string)x_Matrix.Attribute("y") ?? "0"),
            };
        }

        private static double[] parse_transform(XElement x_Matrix)
        {
            return new double[] {
                double.Parse((string)x_Matrix.Attribute("a") ?? "1"),
                double.Parse((string)x_Matrix.Attribute("b") ?? "0"),
                double.Parse((string)x_Matrix.Attribute("c") ?? "0"),
                double.Parse((string)x_Matrix.Attribute("d") ?? "1"),
                double.Parse((string)x_Matrix.Attribute("tx") ?? "0"),
                double.Parse((string)x_Matrix.Attribute("ty") ?? "0"),
            };
        }
        private static double parse_color_compute(string multiplier_s, string offset_s)
        {
            return Math.Max(0, Math.Min(255, double.Parse(multiplier_s ?? "1") * 255 + double.Parse(offset_s ?? "0"))) / 255;
        }
        private static double[] parse_color(XElement x_Matrix)
        {
            return new double[] {
                parse_color_compute((string)x_Matrix.Attribute("redMultiplier"), (string)x_Matrix.Attribute("redOffset")),
                parse_color_compute((string)x_Matrix.Attribute("greenMultiplier"), (string)x_Matrix.Attribute("greenOffset")),
                parse_color_compute((string)x_Matrix.Attribute("blueMultiplier"), (string)x_Matrix.Attribute("blueOffset")),
                parse_color_compute((string)x_Matrix.Attribute("alphaMultiplier"), (string)x_Matrix.Attribute("alphaOffset")),
            };
        }

        private static double[] parse_image_document(XElement x_DOMSymbolItem, int index)
        {
            if (x_DOMSymbolItem.Name.LocalName != "DOMSymbolItem")
            {
                throw new Exception("");
            }
            if ((string)x_DOMSymbolItem.Attribute("name") != $"image/image_{index + 1}")
            {
                throw new Exception("");
            }
            var x_timeline_list = x_DOMSymbolItem.Elements("timeline").ToArray();
            if (x_timeline_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_timeline = x_timeline_list[0];
            var x_DOMTimeline_list = x_timeline.Elements("DOMTimeline").ToArray();
            if (x_DOMTimeline_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_DOMTimeline = x_DOMTimeline_list[0];
            if ((string)x_DOMTimeline.Attribute("name") != $"image_{index + 1}")
            {
                throw new Exception("");
            }
            var x_layers_list = x_DOMTimeline.Elements("layers").ToArray();
            if (x_layers_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_layers = x_layers_list[0];
            var x_DOMLayer_list = x_layers.Elements("DOMLayer").ToArray();
            if (x_DOMLayer_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_DOMLayer = x_DOMLayer_list[0];
            var x_frames_list = x_DOMLayer.Elements("frames").ToArray();
            if (x_frames_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_frames = x_frames_list[0];
            var x_DOMFrame_list = x_frames.Elements("DOMFrame").ToArray();
            if (x_DOMFrame_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_DOMFrame = x_DOMFrame_list[0];
            var x_elements_list = x_DOMFrame.Elements("elements").ToArray();
            if (x_elements_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_elements = x_elements_list[0];
            var x_DOMSymbolInstance_list = x_elements.Elements("DOMSymbolInstance").ToArray();
            if (x_DOMSymbolInstance_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_DOMSymbolInstance = x_DOMSymbolInstance_list[0];
            if ((string)x_DOMSymbolInstance.Attribute("libraryItemName") != $"source/source_{index + 1}")
            {
                throw new Exception("");
            }
            var x_matrix_list = x_DOMSymbolInstance.Elements("matrix").ToArray();
            if (x_matrix_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_matrix = x_matrix_list[0];
            var x_Matrix_list = x_matrix.Elements("Matrix").ToArray();
            if (x_Matrix_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_Matrix = x_Matrix_list[0];
            var x_transformationPoint_list = x_DOMSymbolInstance.Elements("transformationPoint").ToArray();
            if (x_transformationPoint_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_transformationPoint = x_transformationPoint_list[0];
            var x_Point_list = x_transformationPoint.Elements("Point").ToArray();
            if (x_Point_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_Point = x_Point_list[0];
            double[] transform = parse_transform(x_Matrix);
            double[] transform_origin = parse_transform_origin(x_Point);
            if (transform[4] != -transform_origin[0] || transform[5] != -transform_origin[1])
            {
                throw new Exception("");
            }
            return transform;
        }

        private static FrameInfo[] parse_sprite_document(XElement x_DOMSymbolItem, int index)
        {
            Model model = null;
            List<FrameInfo> result = new();
            if (x_DOMSymbolItem.Name.LocalName != "DOMSymbolItem")
            {
                throw new Exception("");
            }
            if ((string)x_DOMSymbolItem.Attribute("name") != (index == -1 ? "main_sprite" : $"sprite/sprite_{index + 1}"))
            {
                throw new Exception("");
            }
            var x_timeline_list = x_DOMSymbolItem.Elements("timeline").ToArray();
            if (x_timeline_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_timeline = x_timeline_list[0];
            var x_DOMTimeline_list = x_timeline.Elements("DOMTimeline").ToArray();
            if (x_DOMTimeline_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_DOMTimeline = x_DOMTimeline_list[0];
            if ((string)x_DOMTimeline.Attribute("name") != (index == -1 ? "main_sprite" : $"sprite_{index + 1}"))
            {
                throw new Exception("");
            }
            var x_layers_list = x_DOMTimeline.Elements("layers").ToArray();
            if (x_layers_list.Length != 1)
            {
                throw new Exception("");
            }
            var x_layers = x_layers_list[0];
            var x_DOMLayer_list = x_layers.Elements("DOMLayer").ToList();
            x_DOMLayer_list.Reverse();
            int layer_count = 0;
            var get_frame_at = (int index) =>
            {
                if (result.Count <= index)
                {
                    result.AddRange(new FrameInfo[index - result.Count + 1]);
                }
                if (result[index] == null)
                {
                    result[index] = new()
                    {
                        label = null,
                        stop = false,
                        command = new(),
                        remove = new(),
                        append = new(),
                        change = new(),
                    };
                }
                return result[index];
            };
            x_DOMLayer_list.ForEach((x_DOMLayer) =>
            {
                var x_frames_list = x_DOMLayer.Elements("frames").ToArray();
                if (x_frames_list.Length != 1)
                {
                    throw new Exception("");
                }
                var x_frames = x_frames_list[0];
                var x_DOMFrame_list = x_frames.Elements("DOMFrame").ToList();
                var colse_current_model_if_need = () =>
                {
                    if (model != null)
                    {
                        var target_frame = get_frame_at(model.frame_start + model.frame_duration);
                        target_frame.remove.Add(new FrameInfo.RemovesInfo
                        {
                            index = model.index,
                        });
                        model = null;
                    }
                };
                x_DOMFrame_list.ForEach((x_DOMFrame) =>
                {
                    int frame_index = (int)x_DOMFrame.Attribute("index");
                    int frame_duration = int.Parse((string)x_DOMFrame.Attribute("duration") ?? "1");
                    double[] transform;
                    double[] color;
                    var x_elements_list = x_DOMFrame.Elements("elements").ToArray();
                    if (x_elements_list.Length == 0)
                    {
                        colse_current_model_if_need();
                        return;
                    }
                    if (x_elements_list.Length != 1)
                    {
                        throw new Exception("");
                    }
                    var x_elements = x_elements_list[0];
                    var x_DOMSymbolInstance_list = x_elements.Elements("DOMSymbolInstance").ToArray();
                    if (x_DOMSymbolInstance_list.Length == 0)
                    {
                        return;
                    }
                    if (x_DOMSymbolInstance_list.Length != 1)
                    {
                        throw new Exception("");
                    }
                    var x_DOMSymbolInstance = x_DOMSymbolInstance_list[0];
                    var name_match = Regex.Matches((string)x_DOMSymbolInstance.Attribute("libraryItemName"), "(image|sprite)/(image|sprite)_([0-9]+)").First();
                    if (name_match == null)
                    {
                        throw new Exception("invalid name");
                    }
                    if (name_match.Groups[1].Value != name_match.Groups[2].Value)
                    {
                        throw new Exception("invalid name x");
                    }
                    FrameInfo.AddsInfo current_instance = new()
                    {
                        resource = int.Parse(name_match.Groups[3].Value) - 1,
                        sprite = name_match.Groups[1].Value == "sprite"
                    };
                    {
                        var x_matrix_list = x_DOMSymbolInstance.Elements("matrix").ToArray();
                        if (x_matrix_list.Length == 0)
                        {
                            transform = new double[] { 0.0, 0.0 };
                        }
                        else if (x_matrix_list.Length == 1)
                        {
                            var x_matrix = x_matrix_list[0];
                            var x_Matrix_list = x_matrix.Elements("Matrix").ToArray();
                            if (x_Matrix_list.Length != 1)
                            {
                                throw new Exception("");
                            }
                            var x_Matrix = x_Matrix_list[0];
                            transform = comput_variant_transform_from_standard(parse_transform(x_Matrix));
                        }
                        else
                        {
                            throw new Exception("");
                        }
                    }
                    {
                        var x_color_list = x_DOMSymbolInstance.Elements("color").ToArray();
                        if (x_color_list.Length == 0)
                        {
                            color = (double[])k_initial_color.Clone();
                        }
                        else if (x_color_list.Length == 1)
                        {
                            var x_color = x_color_list[0];
                            var x_Color_list = x_color.Elements("Color").ToArray();
                            if (x_Color_list.Length != 1)
                            {
                                throw new Exception("");
                            }
                            var x_Color = x_Color_list[0];
                            color = parse_color(x_Color);
                        }
                        else
                        {
                            throw new Exception("");
                        }
                    }
                    var target_frame = get_frame_at(frame_index);
                    if (model == null)
                    {
                        model = new()
                        {
                            index = layer_count,
                            resource = current_instance.resource,
                            sprite = current_instance.sprite,
                            frame_start = frame_index,
                            frame_duration = frame_duration,
                            color = (double[])k_initial_color.Clone(),
                        };
                        target_frame.append.Add(new FrameInfo.AddsInfo
                        {
                            index = model.index,
                            name = null,
                            resource = current_instance.resource,
                            sprite = current_instance.sprite,
                        });
                        ++layer_count;
                    }
                    else
                    {
                        if (current_instance.resource != model.resource || current_instance.sprite != model.sprite)
                        {
                            throw new Exception("");
                        }
                    }
                    model.frame_start = frame_index;
                    model.frame_duration = frame_duration;
                    bool color_is_changed = !(model.color[0] == color[0] && model.color[1] == color[1] && model.color[2] == color[2] && model.color[3] == color[3]);
                    if (color_is_changed)
                    {
                        model.color = color;
                    }
                    target_frame.change.Add(new FrameInfo.MovesInfo
                    {
                        index = model.index,
                        transform = transform,
                        color = color_is_changed ? color : null,
                    });
                });
                colse_current_model_if_need();
            });
            for (int i = 0; i < result.Count; ++i)
            {
                if (result[i] == null)
                {
                    result[i] = new()
                    {
                        label = null,
                        stop = false,
                        command = new(),
                        remove = new(),
                        append = new(),
                        change = new(),
                    };
                }
            }
            return result.Take(result.Count - 1).ToArray();    // TODO result.slice(0, -1)
        }

        private static PopAnimInfo to(FlashPackage flash)
        {
            var x_DOMDocument = flash.document;
            if (x_DOMDocument.Name.LocalName != "DOMDocument")
            {
                throw new Exception("");
            }
            {
                var x_media_list = x_DOMDocument.Elements("media").ToArray();
                if (x_media_list.Length != 1)
                {
                    throw new Exception("");
                }
                var x_media = x_media_list[0];
                var x_DOMBitmapItem_list = x_media.Elements("DOMBitmapItem").ToArray();
            }
            {
                var x_symbols_list = x_DOMDocument.Elements("symbols").ToArray();
                if (x_symbols_list.Length != 1)
                {
                    throw new Exception("");
                }
                var x_symbols = x_symbols_list[0];
                var x_Include_list = x_symbols.Elements("Include").ToArray();
            }
            var main_sprite_frame = parse_sprite_document(flash.library.main_sprite, -1);
            {
                var x_timelines_list = x_DOMDocument.Elements("timelines").ToArray();
                if (x_timelines_list.Length != 1)
                {
                    throw new Exception("");
                }
                var x_timelines = x_timelines_list[0];
                var x_DOMTimeline_list = x_timelines.Elements("DOMTimeline").ToArray();
                if (x_DOMTimeline_list.Length != 1)
                {
                    throw new Exception("");
                }
                var x_DOMTimeline = x_DOMTimeline_list[0];
                if (((string)x_DOMTimeline.Attribute("name")) != "animation")
                {
                    throw new Exception("");
                }
                var x_layers_list = x_DOMTimeline.Elements("layers").ToArray();
                if (x_layers_list.Length != 1)
                {
                    throw new Exception("");
                }
                var x_layers = x_layers_list[0];
                var x_DOMLayer_list = x_layers.Elements("DOMLayer").ToArray();
                if (x_DOMLayer_list.Length != 3)
                {
                    throw new Exception("");
                }
                {
                    var x_DOMLayer_flow = x_DOMLayer_list[0];
                    var x_frames_list = x_DOMLayer_flow.Elements("frames").ToArray();
                    if (x_frames_list.Length != 1)
                    {
                        throw new Exception("");
                    }
                    var x_frames = x_frames_list[0];
                    var x_DOMFrame_list = x_frames.Elements("DOMFrame").ToList();
                    x_DOMFrame_list.ForEach((x_DOMFrame) =>
                    {
                        int frame_index = int.Parse(x_DOMFrame.Attribute("index").Value);
                        if (x_DOMFrame.Attribute("name") != null)
                        {
                            if (((string)x_DOMFrame.Attribute("labelType")) != "name")
                            {
                                throw new Exception("");
                            }
                            main_sprite_frame[frame_index].label = ((string)x_DOMFrame.Attribute("name"));
                        }
                        var x_Actionscript_list = x_DOMFrame.Elements("Actionscript").ToArray();
                        if (x_Actionscript_list.Length == 0)
                        {
                            return;
                        }
                        if (x_Actionscript_list.Length != 1)
                        {
                            throw new Exception("");
                        }
                        var x_Actionscript = x_Actionscript_list[0];
                        if (x_Actionscript.Elements().Count() != 1)
                        {
                            throw new Exception("");
                        }
                        var x_script_list = x_Actionscript.Elements("script").ToArray();
                        if (x_script_list.Length != 1)
                        {
                            throw new Exception("");
                        }
                        var x_script = x_script_list[0];
                        if (x_script.Nodes().Count() != 1)
                        {
                            throw new Exception("");
                        }
                        var x_script_text = x_script.FirstNode;
                        if (x_script_text.NodeType != XmlNodeType.CDATA)
                        {
                            throw new Exception("");
                        }
                        if (((XCData)x_script_text).Value.Trim() != "stop();")  // TODO x_script_text.value.value.trim() ?
                        {
                            throw new Exception("");
                        }
                        main_sprite_frame[frame_index].stop = true;
                    });
                }
                {
                    var x_DOMLayer_command = x_DOMLayer_list[1];
                    var x_frames_list = x_DOMLayer_command.Elements("frames").ToArray();
                    if (x_frames_list.Length != 1)
                    {
                        throw new Exception("");
                    }
                    var x_frames = x_frames_list[0];
                    var x_DOMFrame_list = x_frames.Elements("DOMFrame").ToList();
                    x_DOMFrame_list.ForEach((x_DOMFrame) =>
                    {
                        int frame_index = int.Parse(x_DOMFrame.Attribute("index").Value);
                        var x_Actionscript_list = x_DOMFrame.Elements("Actionscript").ToArray();
                        if (x_Actionscript_list.Length == 0)
                        {
                            return;
                        }
                        if (x_Actionscript_list.Length != 1)
                        {
                            throw new Exception("");
                        }
                        var x_Actionscript = x_Actionscript_list[0];
                        if (x_Actionscript.Elements().Count() != 1)
                        {
                            throw new Exception("");
                        }
                        var x_script_list = x_Actionscript.Elements("script").ToArray();
                        if (x_script_list.Length != 1)
                        {
                            throw new Exception("");
                        }
                        var x_script = x_script_list[0];
                        if (x_script.Nodes().Count() != 1)
                        {
                            throw new Exception("");
                        }
                        var x_script_text = x_script.FirstNode;
                        if (x_script_text.NodeType != XmlNodeType.CDATA)
                        {
                            throw new Exception("");
                        }
                        var command_string = ((XCData)x_script_text).Value.Trim().Split("\n");  // TODO pt_text.value.value.trim(). ?
                        foreach (var e in command_string)
                        {
                            var regex_result = Regex.Matches(e.Trim(), "fscommand\\(\"(.*)\", \"(.*)\"\\);").First();
                            if (regex_result == null)
                            {
                                throw new Exception("invalid command string");
                            }
                            main_sprite_frame[frame_index].command.Add(new FrameInfo.CommandsInfo
                            {
                                command = regex_result.Groups[1].Value,
                                parameter = regex_result.Groups[2].Value,
                            });
                        }
                    });
                }
                {
                    var x_DOMLayer_sprite = x_DOMLayer_list[2];
                    // TODO : check
                }
            }
            int frame_rate = int.Parse(x_DOMDocument.Attribute("frameRate").Value);
            int width = int.Parse(x_DOMDocument.Attribute("width").Value);
            int height = int.Parse(x_DOMDocument.Attribute("height").Value);
            return new PopAnimInfo
            {
                version = flash.extra.version,
                frame_rate = (byte)frame_rate,
                position = flash.extra.position,
                size = new double[] { width, height },
                image = flash.extra.image.Select((e, i) => new ImageInfo { name = e.name, size = e.size, transform = parse_image_document(flash.library.image[i], i) }).ToArray(),
                sprite = flash.extra.sprite.Select((e, i) =>
                {
                    var frame = parse_sprite_document(flash.library.sprite[i], i);
                    return new SpriteInfo { name = e.name, frame_rate = frame_rate, work_area = new int[] { 0, frame.Length }, frame = frame };
                }).ToArray(),
                main_sprite = new() { name = flash.extra.main_sprite.name, frame_rate = frame_rate, work_area = new int[] { 0, main_sprite_frame.Length }, frame = main_sprite_frame },
            };
        }

        // ------------------------------------------------

        //	private static void to_fs(
        //		string raw_file,
        //		string ripe_directory
        //	)
        //{
        //	var ripe = load_flash_package(ripe_directory);
        //	var raw = to(ripe);
        //	jsonWriteFile(raw_file, raw);
        //	return;
        //}

        // ------------------------------------------------


        // -------------- from ----------------------------

        private static XElement make_image_document(int index, ImageInfo image)
        {
            return new XElement("DOMSymbolItem",
                k_xmlns_attribute,
                new XAttribute("name", $"image/image_{index + 1}"),
                new XAttribute("symbolType", "graphic"),
                new XElement("timeline",
                    new XElement("DOMTimeline",
                        new XAttribute("name", $"image_{index + 1}"),
                        new XElement("layers",
                            new XElement("DOMLayer",
                                new XElement("frames",
                                    new XElement("DOMFrame",
                                        new XAttribute("index", "0"),
                                        new XElement("elements",
                                            new XElement("DOMSymbolInstance",
                                                new XAttribute("libraryItemName", $"source/source_{index + 1}"),
                                                new XAttribute("symbolType", "graphic"),
                                                new XAttribute("loop", "loop"),
                                             new XElement("matrix",
                                                    new XElement("Matrix",
                                                        new XAttribute("a", image.transform[0].ToString("N6")),
                                                        new XAttribute("b", image.transform[1].ToString("N6")),
                                                        new XAttribute("c", image.transform[2].ToString("N6")),
                                                        new XAttribute("d", image.transform[3].ToString("N6")),
                                                        new XAttribute("tx", image.transform[4].ToString("N6")),
                                                        new XAttribute("ty", image.transform[5].ToString("N6"))
                                                    )
                                                ),
                                                new XElement("transformationPoint",
                                                    new XElement("Point",
                                                        new XAttribute("x", (-image.transform[4]).ToString("N6")),
                                                        new XAttribute("y", (-image.transform[5]).ToString("N6"))
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }

        private static XElement make_sprite_document(int index, SpriteInfo sprite, SpriteInfo[] sub_sprite)
        {
            Dictionary<int, Model> model = new();
            Dictionary<int, List<XElement>> frame_node_list = new();
            sprite.frame.Select((frame, frame_index) =>
            {
                foreach (var e in frame.remove)
                {
                    model[e.index].state = false;
                }
                foreach (var e in frame.append)
                {
                    model[e.index] = new()
                    {
                        state = null,
                        resource = e.resource,
                        sprite = e.sprite,
                        transform = k_initial_transform,
                        color = k_initial_color,
                        frame_start = frame_index,
                        frame_duration = frame_index,
                    };
                    frame_node_list[e.index] = new();
                    if (frame_index > 0)
                    {
                        frame_node_list[e.index].Add(
                            new XElement("DOMFrame",
                                new XAttribute("index", "0"),
                                new XAttribute("duration", frame_index),
                                new XElement("elements")
                            )
                        );
                    }
                }
                foreach (var e in frame.change)
                {
                    var layer = model[e.index];
                    layer.state = true;
                    layer.transform = compute_standard_transform_from_variant(e.transform);
                    if (e.color != null)
                    {
                        layer.color = e.color;
                    }
                }
                foreach (var layer_index in model.Keys)
                {
                    var layer = model[layer_index];
                    var frame_node = frame_node_list[layer_index];
                    if (layer.state != null)
                    {
                        if (frame_node.Count > 0)
                        {
                            (frame_node[frame_node.Count - 1] as XElement).SetAttributeValue("duration", layer.frame_duration);
                        }
                    }
                    if (layer.state == true)
                    {
                        frame_node.Add(
                            new XElement("DOMFrame",
                                new XAttribute("index", frame_index),
                                    new XAttribute("duration", ""),
                                    new XElement("elements",
                                        new XElement("DOMSymbolInstance",
                                            !layer.sprite ?
                                            new XAttribute[]
                                            {
                                                new XAttribute("libraryItemName", $"image/image_{layer.resource + 1}"),
                                                new XAttribute("symbolType", "graphic"),
                                                new XAttribute("loop", "loop"),
                                            }
                                            :
                                            new XAttribute[]
                                            {
                                                new XAttribute("libraryItemName", $"sprite/sprite_{layer.resource + 1}"),
                                                new XAttribute("symbolType", "graphic"),
                                                new XAttribute("loop", "loop"),
                                                new XAttribute("firstFrame", (frame_index - (layer.frame_start)) % (sub_sprite[(layer.resource)].frame.Length)),
                                            },
                                            new XElement("matrix",
                                                new XElement("Matrix",
                                                    new XAttribute("a", layer.transform[0].ToString("N6")),
                                                    new XAttribute("b", layer.transform[1].ToString("N6")),
                                                    new XAttribute("c", layer.transform[2].ToString("N6")),
                                                    new XAttribute("d", layer.transform[3].ToString("N6")),
                                                    new XAttribute("tx", layer.transform[4].ToString("N6")),
                                                    new XAttribute("ty", layer.transform[5].ToString("N6"))
                                                )
                                            ),
                                            new XElement("color",
                                                new XElement("Color",
                                                    new XAttribute("redMultiplier", layer.color[0].ToString("N6")),
                                                    new XAttribute("greenMultiplier", layer.color[1].ToString("N6")),
                                                    new XAttribute("blueMultiplier", layer.color[2].ToString("N6")),
                                                    new XAttribute("alphaMultiplier", layer.color[3].ToString("N6"))
                                                )
                                            )
                                        )
                                    )
                                )
                            );
                        layer.state = null;
                        layer.frame_duration = 0;
                    }
                    if (layer.state == false)
                    {
                        model.Remove(layer_index);
                    }
                    ++layer.frame_duration;
                }
                return string.Empty;
            }).ToArray();
            foreach (var layer_index in model.Keys)
            {
                var layer = model[layer_index];
                var frame_node = frame_node_list[layer_index];
                frame_node[frame_node.Count - 1].SetAttributeValue("duration", layer.frame_duration);
                model.Remove(layer_index);
            }
            return new XElement("DOMSymbolItem",
                k_xmlns_attribute,
                new XAttribute("name", index == -1 ? "main_sprite" : $"sprite/sprite_{index + 1}"),
                new XAttribute("symbolType", "graphic"),
                new XElement("timeline",
                    new XElement("DOMTimeline",
                        new XAttribute("name", index == -1 ? "main_sprite" : $"sprite_{index + 1}"),
                        new XElement("layers", frame_node_list.Keys.OrderByDescending(i => i).Select((layer_index) =>
                            new XElement("DOMLayer",
                                new XAttribute("name", layer_index + 1),
                                new XElement("frames", frame_node_list[layer_index])
                            )
                        ).ToArray())
                    )
                )
            );
        }

        private class PrevEnd
        {
            public int flow { get; set; }
            public int command { get; set; }
        }

        private static XElement make_main_document(PopAnimInfo animation)
        {
            PrevEnd prev_end = new()
            {
                flow = -1,
                command = -1
            };
            List<XElement> flow_node = new();
            List<XElement> command_node = new();
            animation.main_sprite.frame.Select((frame, frame_index) =>
            {
                if (frame.label != null || frame.stop)
                {
                    if (prev_end.flow + 1 < frame_index)
                    {
                        flow_node.Add(new XElement("DOMFrame",
                                        new XAttribute("index", prev_end.flow + 1),
                                        new XAttribute("duration", frame_index - (prev_end.flow + 1)),
                                        new XElement("elements")
                                    ));
                    }
                    var node = new XElement("DOMFrame",
                        new XAttribute("index", frame_index),
                        new XElement("elements")
                    );
                    var node_element = node;
                    if (frame.label != null)
                    {
                        node_element.SetAttributeValue("name", frame.label);
                        node_element.SetAttributeValue("labelType", "name");
                    }
                    if (frame.stop)
                    {
                        node_element.AddFirst(new XElement("Actionscript",
                            new XElement("script",
                                new XCData("stop();")
                                            )
                                        ));
                    }
                    flow_node.Add(node);
                    prev_end.flow = frame_index;
                }
                if (frame.command.Count > 0)
                {
                    if (prev_end.command + 1 < frame_index)
                    {
                        command_node.Add(new XElement("DOMFrame",
                        new XAttribute("index", prev_end.command + 1),
                                        new XAttribute("duration", frame_index - (prev_end.command + 1))
                                    ));
                    }
                    command_node.Add(new XElement("DOMFrame",
                        new XAttribute("index", frame_index),
                                    new XElement("Actionscript",
                                        new XElement("script",
                                            new XCData(string.Join("\n", frame.command.Select((e) => $"fscommand(\"{e.command}\", \"{e.parameter}\");")))
                                        )
                                    )
                                ));
                    prev_end.command = frame_index;
                }
                return string.Empty;
            }).ToArray();
            if (prev_end.flow + 1 < animation.main_sprite.frame.Length)
            {
                flow_node.Add(new XElement("DOMFrame",
                new XAttribute("index", prev_end.flow + 1),
                            new XAttribute("duration", animation.main_sprite.frame.Length - (prev_end.flow + 1))
                        ));
            }
            if (prev_end.command + 1 < animation.main_sprite.frame.Length)
            {
                command_node.Add(new XElement("DOMFrame",
                new XAttribute("index", prev_end.command + 1),
                            new XAttribute("duration", animation.main_sprite.frame.Length - (prev_end.command + 1))
                        ));
            }
            return new XElement("DOMDocument",
                k_xmlns_attribute,
                new XAttribute("frameRate", animation.main_sprite.frame_rate),
                new XAttribute("width", animation.size[0]),
                new XAttribute("height", animation.size[1]),
                new XAttribute("xflVersion", k_xfl_version),
                new XElement("folders",
                    new[] { "media", "source", "image", "sprite" }.Select((e) => new XElement("DOMFolderItem",
                        new XAttribute("name", e),
                        new XAttribute("isExpanded", "true")
                    )).ToArray()
                ),
                new XElement("media", animation.image.Select((e) =>
                    new XElement("DOMBitmapItem",
                    new XAttribute("name", $"media/{e.name.Split("|")[0]}"),
                    new XAttribute("href", $"media/{e.name.Split("|")[0]}.png")
                )).ToArray()
                ),
                new XElement("symbols",
                    animation.image.Select((e, i) =>
                        new XElement("Include",
                            new XAttribute("href", $"source/source_{i + 1}.xml")
                        )
                    ).ToArray(),
                    animation.image.Select((e, i) =>
                        new XElement("Include",
                            new XAttribute("href", $"image/image_{i + 1}.xml")
                        )
                    ).ToArray(),
                    animation.sprite.Select((e, i) =>
                        new XElement("Include",
                            new XAttribute("href", $"sprite/sprite_{i + 1}.xml")
                        )
                    ).ToArray(),
                    new XElement("Include",
                        new XAttribute("href", "main_sprite.xml")
                    )
                ),
                new XElement("timelines",
                    new XElement("DOMTimeline",
                        new XAttribute("name", "animation"),
                        new XElement("layers",
                            new XElement("DOMLayer",
                                new XAttribute("name", "flow"),
                                new XElement("frames", flow_node)
                            ),
                            new XElement("DOMLayer",
                                new XAttribute("name", "command"),
                                new XElement("frames", command_node)
                            ),
                            new XElement("DOMLayer",
                                new XAttribute("name", "sprite"),
                                new XElement("frames",
                                    new XElement("DOMFrame",
                                        new XAttribute("index", "0"),
                                        new XAttribute("duration", animation.main_sprite.frame.Length),
                                        new XElement("elements",
                                            new XElement("DOMSymbolInstance",
                                                new XAttribute("libraryItemName", "main_sprite"),
                                                new XAttribute("symbolType", "graphic"),
                                                new XAttribute("loop", "loop")
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }

        private static FlashPackage from(PopAnimInfo animation)
        {
            return new FlashPackage
            {
                extra = new()
                {
                    version = animation.version,
                    position = animation.position,
                    image = animation.image.Select((e) => new ImageInfo
                    {
                        name = e.name,
                        size = e.size,
                    }).ToArray(),
                    sprite = animation.sprite.Select((e) => new SpriteInfo
                    {
                        name = e.name,
                    }).ToArray(),
                    main_sprite = new()
                    {
                        name = animation.main_sprite.name,
                    },
                },
                document = make_main_document(animation),
                library = new()
                {
                    image = animation.image.Select((e, i) => make_image_document(i, e)).ToArray(),
                    sprite = animation.sprite.Select((e, i) => make_sprite_document(i, e, animation.sprite)).ToArray(),
                    main_sprite = make_sprite_document(-1, animation.main_sprite, animation.sprite)
                }
            };
        }


        // ------------ source manager --------------------

        private static XAttribute[] make_scale_matrix(int resolution)
        {
            double scale = (double)(k_standard_resolution) / resolution;
            return new XAttribute[] {
                new XAttribute("a", scale.ToString("N6")),
                new XAttribute("d", scale.ToString("N6"))
            };
        }

        // ------------------------------------------------

        private static XElement create_one(int index, ImageInfo image, int resolution)
        {
            return new XElement("DOMSymbolItem",
                k_xmlns_attribute,
                new XAttribute("name", $"source/source_{index + 1}"),
                new XAttribute("symbolType", "graphic"),
                new XElement("timeline",
                    new XElement("DOMTimeline",
                        new XAttribute("name", $"source_{index + 1}"),
                        new XElement("layers",
                            new XElement("DOMLayer",
                                new XElement("frames",
                                    new XElement("DOMFrame",
                                        new XAttribute("index", "0"),
                                        new XElement("elements",
                                            new XElement("DOMBitmapInstance",
                                                new XAttribute("libraryItemName", $"media/{image.name.Split("|")[0]}"),
                                                new XElement("matrix",
                                                    new XElement("Matrix", make_scale_matrix(resolution))
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }

        private static XElement[] create(PopAnimInfo animation, int resolution)
        {
            return animation.image.Select((e, i) => create_one(i, e, resolution)).ToArray();
        }

        // ------------------------------------------------

        private static void resize_one(XElement document, int resolution)
        {
            XElement scale_matrix = document.XPathSelectElement("./timeline/DOMTimeline/layers/DOMLayer/frames/DOMFrame/elements/DOMBitmapInstance/matrix/Matrix");
            scale_matrix.RemoveAttributes();
            scale_matrix.Add(make_scale_matrix(resolution));
        }

        private static void resize(XElement[] document, int resolution)
        {
            Array.ForEach(document, (e) => resize_one(e, resolution));
        }

        // ------------------------------------------------

        private static void create_fsh(string directory, PopAnimInfo data, int resolution = k_standard_resolution)
        {
            create(data, resolution).Select((e, i) =>
            {
                xmlWriteFile($"{directory}/LIBRARY/source/source_{i + 1}.xml", e);
                return string.Empty;
            }).ToArray();
        }

        private static void resize_fs(string directory, int resolution)
        {
            var extra = jsonReadFile<PopAnimInfo>($"{directory}/extra.json");
            var document = extra.image.Select((e, i) => (xmlReadFile($"{directory}/LIBRARY/source/source_{i + 1}.xml"))).ToArray();
            resize(document, resolution);
            document.Select((e, i) =>
            {
                xmlWriteFile($"{directory}/LIBRARY/source/source_{i + 1}.xml", e);
                return string.Empty;
            }).ToArray();
        }

        // ------------------------------------------------





        // --------------- flash common -------------------

        private static double[] k_initial_transform = { 1.0, 0.0, 0.0, 1.0, 0.0, 0.0 };

        //private static double[] mix_transform(double[] source, double[] change)
        //{
        //    return new double[] {
        //        change[0] * source[0] + change[2] * source[1],
        //        change[1] * source[0] + change[3] * source[1],
        //        change[0] * source[2] + change[2] * source[3],
        //        change[1] * source[2] + change[3] * source[3],
        //        change[0] * source[4] + change[2] * source[5] + change[4],
        //        change[1] * source[4] + change[3] * source[5] + change[5]
        //    };
        //}

        private static double[] compute_standard_transform_from_variant(double[] transform)
        {
            double[] result;
            if (transform.Length == 2)
            {
                result = new double[] {
                    1.0, 0.0, 0.0, 1.0,
                    transform[1 - 1],
                    transform[2 - 1]
                };
            }
            else if (transform.Length == 6)
            {
                result = (double[])transform.Clone();
            }
            else if (transform.Length == 3)
            {
                double cos_value = Math.Cos(transform[1 - 1]);
                double sin_value = Math.Sin(transform[1 - 1]);
                result = new double[] {
                    cos_value,
                    sin_value,
                    -sin_value,
                    cos_value,
                    transform[2 - 1],
                    transform[3 - 1],
                };
            }
            else
            {
                throw new Exception("invalid transform size");
            }
            return result;
        }

        private static double[] comput_variant_transform_from_standard(double[] data)
        {
            if (data[0] == data[3] && data[1] == -data[2])
            {
                if (data[0] == 1.0 && data[1] == 0.0)
                {
                    return new double[] { data[4], data[5] };
                }
                double acos_value = Math.Acos(data[0]);
                double asin_value = Math.Asin(data[1]);
                if (Math.Abs(Math.Abs(acos_value) - Math.Abs(asin_value)) <= 1e-2)
                {
                    return new double[] { asin_value, data[4], data[5] };
                }
            }
            return (double[])data.Clone();
        }

        // ------------------------------------------------

        private static readonly double[] k_initial_color = { 1.0, 1.0, 1.0, 1.0 };

        // ------------------------------------------------




        // -------------- flash convert common ---

        private const int k_standard_resolution = 1200;

        // ------------------------------------------------

        private const string k_xfl_content = "PROXY-CS5";

        private const string k_xfl_version = "2.971";

        private static XAttribute k_xmlns_attribute = new(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance");

        private static XNamespace xflns = "http://ns.adobe.com/xfl/2008/";

        // ------------------------------------------------

        private static void save_flash_package(FlashPackage data, string directory)
        {
            jsonWriteFile($"{directory}/extra.json", data.extra);
            xmlWriteFile($"{directory}/DOMDocument.xml", data.document);
            data.library.image.Select((e, i) =>
            {
                xmlWriteFile($"{directory}/LIBRARY/image/image_{i + 1}.xml", e);
                return string.Empty;
            }).ToArray();
            data.library.sprite.Select((e, i) =>
            {
                xmlWriteFile($"{directory}/LIBRARY/sprite/sprite_{i + 1}.xml", e);
                return string.Empty;
            }).ToArray();
            xmlWriteFile($"{directory}/LIBRARY/main_sprite.xml", data.library.main_sprite);
            return;
        }

        private static FlashPackage load_flash_package(string directory)
        {
            var extra = jsonReadFile<PopAnimInfo>($"{directory}/extra.json");
            var document = xmlReadFile($"{directory}/DOMDocument.xml");
            FlashPackage.Library library = new()
            {
                image = extra.image.Select((e, i) => (xmlReadFile($"{directory}/LIBRARY/image/image_{i + 1}.xml"))).ToArray(),
                sprite = extra.sprite.Select((e, i) => (xmlReadFile($"{directory}/LIBRARY/sprite/sprite_{i + 1}.xml"))).ToArray(),
                main_sprite = xmlReadFile($"{directory}/LIBRARY/main_sprite.xml")
            };
            return new FlashPackage
            {
                extra = extra,
                document = document,
                library = library,
            };
        }

        // ------------------------------------------------

        private static void create_xfl_content_file(
            string directory
        )
        {
            File.WriteAllText($"{directory}/main.xfl", k_xfl_content);
        }

        // ------------------------------------------------

        private static void create_image_default(string outFolder, PopAnimInfo pamInfo)
        {
            string path = outFolder + "/LIBRARY/media/";
            foreach (string p in pamInfo.image.Select((e) => path + e.name.Split("|")[0] + ".png"))
            {
                Dir.NewDir(p, false);
                if (!File.Exists(p))
                {
                    using (FileStream fs = File.Create(p))
                    {
                        byte[] img = Reanim.FlashXfl.defaultPicture;
                        fs.Write(img, 0, img.Length);
                    }
                }
            }
        }

        // ------------------------------------------------
        private static void xmlWriteFile(string outFile, XElement data)
        {
            foreach (var e in data.DescendantsAndSelf())
            {
                e.Name = xflns + e.Name.LocalName;
            }
            XmlWriterSettings settings = new()
            {
                Indent = true,
                IndentChars = "\t",
                OmitXmlDeclaration = true
            };
            using var writer = XmlWriter.Create(outFile, settings);
            XDocument document = new(new XDeclaration("1.0", "utf-8", null), data);
            document.Save(writer);
        }
        private static XElement xmlReadFile(string file)
        {
            XElement data = XDocument.Load(file).Root!;
            foreach (var e in data.DescendantsAndSelf())
            {
                e.Name = e.Name.LocalName;
            }
            return data;
        }
        private static void jsonWriteFile(string outFile, PopAnimInfo data)
        {
            using FileStream file = File.OpenWrite(outFile);
            JsonSerializerOptions setting = new()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            JsonSerializer.Serialize(file, data, setting);
        }

        private static T jsonReadFile<T>(string file)
        {
            string jsonString = File.ReadAllText(file);

            return JsonSerializer.Deserialize<T>(jsonString)!;// XDocument.Load(file).Root;
        }
    }
}
