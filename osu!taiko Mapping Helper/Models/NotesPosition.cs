namespace osu_taiko_Mapping_Helper.Models
{
    internal class NotesPosition
    {
        internal string donX { set; get; }
        internal string donY { set; get; }
        internal string katX { set; get; }
        internal string katY { set; get; }
        internal string finisherDonX { set; get; }
        internal string finisherDonY { set; get; }
        internal string finisherKatX { set; get; }
        internal string finisherKatY { set; get; }
        internal NotesPosition()
        {
            donX = string.Empty;
            donY = string.Empty;
            katX = string.Empty;
            katY = string.Empty;
            finisherDonX = string.Empty;
            finisherDonY = string.Empty;
            finisherKatX = string.Empty;
            finisherKatY = string.Empty;
        }
    }
}
