namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// テスト用の.osuファイルの中身を作る
    /// </summary>
    /// <remarks>
    /// 何も指定しなければ、今のosu!で保存した形(v14)の小さい太鼓の譜面になる。
    /// テストで確かめたいセクションだけを差し替えて使う
    /// </remarks>
    internal class OsuFileBuilder
    {
        internal string Version { get; set; } = "osu file format v14";
        internal List<string> General { get; set; } = ["AudioFilename: audio.mp3", "AudioLeadIn: 0", "PreviewTime: 1000", "Mode: 1"];
        internal List<string> Editor { get; set; } = ["Bookmarks: 1500,2500", "DistanceSpacing: 1"];
        internal List<string> Metadata { get; set; } = ["Title:title", "TitleUnicode:title", "Artist:artist", "ArtistUnicode:artist", "Creator:creator", "Version:Oni", "Source:", "Tags:tag"];
        internal List<string> Difficulty { get; set; } = ["HPDrainRate:5", "CircleSize:5", "OverallDifficulty:5", "ApproachRate:5", "SliderMultiplier:1.4", "SliderTickRate:1"];
        internal List<string> Events { get; set; } = ["//Background and Video events", "0,0,\"bg.jpg\",0,0", "//Break Periods", "//Storyboard Layer 0 (Background)", "//Storyboard Layer 1 (Fail)", "//Storyboard Layer 2 (Pass)", "//Storyboard Layer 3 (Foreground)", "//Storyboard Layer 4 (Overlay)", "//Storyboard Sound Samples"];
        // BPM120 (beatLength 500)
        internal List<string> TimingPoints { get; set; } = ["0,500,4,1,0,100,1,0", "2000,-50,4,1,0,80,0,0"];
        internal List<string>? Colours { get; set; } = null;
        internal List<string> HitObjects { get; set; } = ["256,192,1000,1,0,0:0:0:0:", "256,192,1500,1,8,0:0:0:0:", "256,192,2000,2,0,L|300:192,1,70", "256,192,3000,12,0,4000,0:0:0:0:"];

        /// <summary>
        /// .osuファイルの行を作る
        /// </summary>
        internal List<string> Build()
        {
            List<string> lines = [Version, ""];
            AddSection(lines, "[General]", General);
            AddSection(lines, "[Editor]", Editor);
            AddSection(lines, "[Metadata]", Metadata);
            AddSection(lines, "[Difficulty]", Difficulty);
            AddSection(lines, "[Events]", Events);
            AddSection(lines, "[TimingPoints]", TimingPoints);
            if (Colours != null)
            {
                AddSection(lines, "[Colours]", Colours);
            }
            lines.Add("[HitObjects]");
            lines.AddRange(HitObjects);
            return lines;
        }

        private static void AddSection(List<string> lines, string header, List<string> body)
        {
            lines.Add(header);
            lines.AddRange(body);
            lines.Add("");
        }
    }
}
