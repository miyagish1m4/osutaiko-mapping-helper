namespace osu_taiko_Mapping_Helper.Utils
{
    public static class ExtensionUtils
    {
        public static T? SafeGetIndex<T>(this List<T> collection, int index) =>
            index >= 0 && index < collection.Count ? collection[index] : default;
        public static T? SafeGetIndex<T>(this T[] collection, int index) =>
            index >= 0 && index < collection.Length ? collection[index] : default;
        public static bool IsWithin(this int num, int start, int end = int.MaxValue) =>
            num <= end && num >= start;
        public static bool IsWithin(this int num, double start, double end = double.MaxValue) =>
            num <= end && num >= start;
        public static bool IsWithin(this double num, int start, int end = int.MaxValue) =>
            num <= end && num >= start;
        public static bool IsWithin(this double num, double start, double end = double.MaxValue) =>
            num <= end && num >= start;


    }
}
