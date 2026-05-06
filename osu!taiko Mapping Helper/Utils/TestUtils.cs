using osu_taiko_Mapping_Helper.Models;

namespace osu_taiko_Mapping_Helper.Utils
{
    internal class TestUtils
    {
        internal static bool GetSnapsOnHitObjects(ref Beatmap beatmap)
        {
            const int HEXA_SNAP = 16;
            const int DUO_SNAP = 12;
            double hexaSnapLength;
            double duoSnapLength;
            try
            {
                // 赤線リストの取得
                var timingPoints = beatmap.timingPoints.Where(tp => tp.isRedLine).ToList();
                // タイミングポイントでループする
                for (int i = 0, hoIndex = 0; i < timingPoints.Count; i++)
                {
                    hexaSnapLength = 60000 / timingPoints[i].bpm / HEXA_SNAP;
                    duoSnapLength = 60000 / timingPoints[i].bpm / DUO_SNAP;
                    // ヒットオブジェクトでループする(次のタイミングポイントまで)
                    for (int j = hoIndex; j < beatmap.hitObjects.Count; j++)
                    {
                        // 次のタイミングポイントの時間と比較して、次のタイミングポイントの時間±2msの範囲にある場合は次のタイミングポイントで処理する
                        if (i != timingPoints.Count - 1)
                        {
                            if (beatmap.hitObjects[j].time >= timingPoints[i + 1].time - 2 &&
                                beatmap.hitObjects[j].time <= timingPoints[i + 1].time + 2)
                            {
                                hoIndex = j;
                                break;
                            }
                        }
                        // タイミングポイントとの差分を求める
                        int timingDiff = beatmap.hitObjects[j].time - timingPoints[i].time;
                        // 差分を1/16で何分割されるかを算出する
                        double hexaTicks = timingDiff / hexaSnapLength;
                        // 四捨五入して整数にする
                        int hexaTicksInt = (int)Math.Round(hexaTicks, MidpointRounding.AwayFromZero);
                        // 整数にした値と小数点以下の値の差を求める
                        double hexaTickDiff = Math.Abs(hexaTicks - hexaTicksInt);
                        // 差が0.15以下の場合は1/16とみなし、ヒットオブジェクトのsnapに1/16を設定し、rawTimeを求める
                        if (hexaTickDiff <= 0.15)
                        {

                            beatmap.hitObjects[j].snap = HEXA_SNAP;
                            beatmap.hitObjects[j].rawTime = timingPoints[i].time + hexaTicksInt * hexaSnapLength;
                            continue;
                        }
                        // 差分を1/12で何分割されるかを算出する
                        double duoTicks = timingDiff / duoSnapLength;
                        // 四捨五入して整数にする
                        int duoTicksInt = (int)Math.Round(duoTicks, MidpointRounding.AwayFromZero);
                        // 整数にした値と小数点以下の値の差を求める
                        double duoTickDiff = Math.Abs(duoTicks - duoTicksInt);
                        // 差が0.15以下の場合は1/12とみなし、ヒットオブジェクトのsnapに1/12を設定し、rawTimeを求める
                        if (duoTickDiff <= 0.15)
                        {
                            beatmap.hitObjects[j].snap = DUO_SNAP;
                            beatmap.hitObjects[j].rawTime = timingPoints[i].time + duoTicksInt * duoSnapLength;
                            continue;
                        }
                        // どちらにも該当しない場合は1/16とみなし、ヒットオブジェクトのsnapに1/16を設定し、設定されているtimingをrawTimeとする
                        beatmap.hitObjects[j].snap = HEXA_SNAP;
                        beatmap.hitObjects[j].rawTime = timingPoints[i].time;
                    }
                }
#if DEBUG
                // デバッグ用にHitObjectの時間、BPM、Snap、RawTime、NotesTypeを出力する
                foreach (var hitObject in beatmap.hitObjects)
                {
                    System.Diagnostics.Debug.WriteLine($"Time: {hitObject.time}, BPM: {hitObject.bpm}, Snap: {hitObject.snap}, RawTime: {hitObject.rawTime}, NotesType: {Constants.ConvertNoteType(hitObject.noteType)}");
                }
#endif
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
    }
}
