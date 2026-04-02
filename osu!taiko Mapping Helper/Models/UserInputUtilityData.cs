namespace osu_taiko_Mapping_Helper.Models
{
    internal class UserInputUtilityData
    {
        internal int utilityCode { set; get; }
        internal int offset { set; get; }
        internal int hitSoundCode { set; get; }
        internal int notesPositionCode { set; get; }
        internal string tags { set; get; }
        internal int settingCopierCode { set; get; }
        internal int resnapTimingFrom { set; get; }
        internal int resnapTimingTo { set; get; }
        internal int beatSnap { set; get; }
        internal bool isAllNotes { set; get; }
        internal int donX { get; set; }
        internal int donY { get; set; }
        internal int katX { get; set; }
        internal int katY { get; set; }
        internal int finisherDonX { get; set; }
        internal int finisherDonY { get; set; }
        internal int finisherKatX { get; set; }
        internal int finisherKatY { get; set; }
        internal UserInputUtilityData()
        {
            utilityCode = -1;
            hitSoundCode = 0;
            notesPositionCode = 0;
            tags = string.Empty;
            settingCopierCode = 0;
            resnapTimingFrom = 0;
            resnapTimingTo = 0;
            beatSnap = -1;
            isAllNotes = false;
            donX = -1;
            donY = -1;
            katX = -1;
            katY = -1;
            finisherDonX = -1;
            finisherDonY = -1;
            finisherKatX = -1;
            finisherKatY = -1;
        }
    }
}
