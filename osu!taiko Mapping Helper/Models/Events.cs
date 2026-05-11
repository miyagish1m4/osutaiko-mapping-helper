
using osu_taiko_Mapping_Helper.Properties;

namespace osu_taiko_Mapping_Helper.Models
{
    internal class Events
    {
        // イベントコード
        // 0 : BG and Video
        // 1 : Break
        // 2 : Storyboard Layer 0
        // 3 : Storyboard Layer 1
        // 4 : Storyboard Layer 2
        // 5 : Storyboard Layer 3
        // 6 : Storyboard Layer 4
        // 7 : Storyboard Sound Samples
        internal int eventCode { get; set; }
        // コメントフラグ
        internal bool isComment { get; set; } = false;

        internal string comment { get; set; } = string.Empty;
        internal int startTime { get; set; }
        #region BG,Video
        internal bool isVideo { get; set; } = false;
        internal string fileName { get; set; } = string.Empty;
        internal int xOffset { get; set; } = int.MinValue;
        internal int yOffset { get; set; } = int.MinValue;
        #endregion
        #region Breaks
        internal int endTime { get; set; } = int.MinValue;
        #endregion
        #region Storyboard
        internal string layer { get; set; } = string.Empty;
        #endregion

        internal Events(string line, int eventCode)
        {
            this.eventCode = eventCode;
            comment = line;
            // "//"を最初に含む行はコメントとみなす
            if (line.StartsWith("//"))
            {
                isComment = true;
                return;
            }
            // イベントコードが2以上の場合はStoryboardとみなす
            if (eventCode >= 2)
            {
                layer = line;
            }
            else
            {
                switch (eventCode)
                {
                    case 0:
                        string eventType = line.Split(',')[0];
                        if (eventType == "0")
                        {
                            var bgParams = line.Split(',');
                            startTime = int.Parse(bgParams[1]);
                            fileName = bgParams[2];
                            xOffset = int.Parse(bgParams[3]);
                            yOffset = int.Parse(bgParams[4]);
                        }
                        else
                        {
                            isVideo = true;
                            var videoParams = line.Split(',');
                            startTime = int.Parse(videoParams[1]);
                            fileName = videoParams[2];
                            if (videoParams.Length >= 4)
                            {
                                xOffset = int.Parse(videoParams[3]);
                                yOffset = int.Parse(videoParams[4]);
                            }
                        }
                        break;
                    case 1:
                        var breakParams = line.Split(',');
                        startTime = int.Parse(breakParams[1]);
                        endTime = int.Parse(breakParams[2]);
                        break;
                }

            }
        }
    }
}
