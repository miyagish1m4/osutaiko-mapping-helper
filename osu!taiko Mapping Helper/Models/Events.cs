
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
            // ストーリーボードの命令の行(先頭が" "か"_")は、動画などの下にあっても書き出し時にそのまま出力するよう、コメントと同じく元の行のまま扱う
            if (line.StartsWith(' ') || line.StartsWith('_'))
            {
                isComment = true;
                return;
            }
            // 行の種類は、見出しのコメントではなく行の先頭の値で判定する
            var parameters = line.Split(',');
            switch (parameters[0].Trim())
            {
                case "0":
                    // BG
                    this.eventCode = Constants.BG_AND_VIDEO_CODE;
                    startTime = int.Parse(parameters[1]);
                    fileName = parameters[2];
                    if (parameters.Length >= 4)
                    {
                        xOffset = int.Parse(parameters[3]);
                        yOffset = parameters.Length >= 5 ? int.Parse(parameters[4]) : 0;
                    }
                    break;
                case "1":
                case "Video":
                    // 動画
                    this.eventCode = Constants.BG_AND_VIDEO_CODE;
                    isVideo = true;
                    startTime = int.Parse(parameters[1]);
                    fileName = parameters[2];
                    if (parameters.Length >= 4)
                    {
                        xOffset = int.Parse(parameters[3]);
                        yOffset = parameters.Length >= 5 ? int.Parse(parameters[4]) : 0;
                    }
                    break;
                case "2":
                case "Break":
                    // 休憩
                    this.eventCode = Constants.BREAK_PERIODS_CODE;
                    startTime = int.Parse(parameters[1]);
                    endTime = int.Parse(parameters[2]);
                    break;
                default:
                    // それ以外(ストーリーボードのオブジェクトなど)は、書き出し時にそのまま出力するよう、コメントと同じく元の行のまま扱う
                    isComment = true;
                    break;
            }
        }
    }
}
