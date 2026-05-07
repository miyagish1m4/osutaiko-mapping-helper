namespace osu_taiko_Mapping_Helper.Utils.Helper
{
    internal class BGHelper
    {
        /// <summary>
        /// BGを取得して、フォームの背景に設定する
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="size">サイズ</param>
        /// <returns>加工したBGデータ</returns>
        internal static Bitmap SetBgOnForm(string path, Point size)
        {
            const double DARKNESS_FACTORY = 0.5;
            Bitmap canvas = new(size.X, size.Y);
            try
            {
                if (path == "")
                {
                    throw new Exception("背景画像が見つかりませんでした。");
                }
                Bitmap image = new(path);
                float width = 0;
                float height = 0;
                float diffWidth = 0;
                float diffHeight = 0;
                float zoomRatio = 0;
                // 横長画像の場合
                if (((float)image.Height / (float)image.Width) < 0.5625f)
                {
                    // 縦の長さから拡大率を求める
                    zoomRatio = ((float)image.Height / (float)size.Y);
                    // 縦の長さからアスペクト比が16:9の場合の横の長さを求める
                    height = image.Height;
                    float heightRatio = (float)image.Height / 9f;
                    width = heightRatio * 16;
                    // 中央部を表示したいため、表示する際の横の座標を求める
                    diffWidth = ((float)image.Width - width) / 2;
                }
                else if (((float)image.Height / (float)image.Width) > 0.5625f)
                {
                    // 横の長さから拡大率を求める
                    zoomRatio = ((float)image.Width / (float)size.X);
                    // 横の長さからアスペクト比が16:9の場合の縦の長さを求める
                    width = image.Width;
                    float widthRatio = (float)image.Width / 16f;
                    height = widthRatio * 9;
                    // 中央部を表示したいため、表示する際の縦の座標を求める
                    diffHeight = ((float)image.Height - height) / 2;
                }
                else
                {
                    // 拡大率を求める
                    zoomRatio = ((float)image.Width / (float)size.X);
                    width = image.Width;
                    height = image.Height;
                }
                RectangleF destinationRect = new(0, 0, width / zoomRatio, height / zoomRatio);
                RectangleF sourceRect = new(diffWidth, diffHeight, width, height);
                Graphics graphic = Graphics.FromImage(canvas);
                // 補間方法を高品質双三次補間に設定
                graphic.InterpolationMode =
                    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                // 表示する
                graphic.DrawImage(image, destinationRect, sourceRect, GraphicsUnit.Pixel);
                image.Dispose();
                graphic.Dispose();
                // BGの明度を下げる
                for (int y = 0; y < size.Y; y++)
                {
                    for (int x = 0; x < size.X; x++)
                    {
                        Color pixel = canvas.GetPixel(x, y);
                        int r = (int)(pixel.R * DARKNESS_FACTORY);
                        int g = (int)(pixel.G * DARKNESS_FACTORY);
                        int b = (int)(pixel.B * DARKNESS_FACTORY);
                        Color darkPixel = Color.FromArgb(r, g, b);
                        canvas.SetPixel(x, y, darkPixel);
                    }
                }

                return canvas;
            }
            catch
            {
                Common.WriteWarningMessage("LOG_W-GET-BG");
                return canvas;
            }
        }

        /// <summary>
        /// BGを取得して、フォームの背景に設定する
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="size">サイズ</param>
        /// <param name="darknessFactor">BGの明度を下げる係数</param>
        /// <returns>加工したBGデータ</returns>
        internal static Bitmap SetBgOnForm(string path, Point size, double darknessFactor)
        {
            Point outputSize = new(size.X, size.Y);
            Bitmap canvas = new(size.X, size.Y);
            try
            {
                if (path == "")
                {
                    throw new Exception("背景画像が見つかりませんでした。");
                }
                Bitmap image = new(path);
                float width = 0;
                float height = 0;
                float diffWidth = 0;
                float diffHeight = 0;
                float zoomRatio = 0;
                // 横長画像の場合
                if (((float)image.Height / (float)image.Width) < 0.5625f)
                {
                    // 縦の長さから拡大率を求める
                    zoomRatio = ((float)image.Height / (float)size.Y);
                    // 縦の長さからアスペクト比が16:9の場合の横の長さを求める
                    height = image.Height;
                    float heightRatio = (float)image.Height / 9f;
                    width = heightRatio * 16;
                    // 中央部を表示したいため、表示する際の横の座標を求める
                    diffWidth = ((float)image.Width - width) / 2;
                }
                else if (((float)image.Height / (float)image.Width) >= 0.5625f)
                {
                    // 横の長さから拡大率を求める
                    zoomRatio = ((float)image.Width / (float)size.X);
                    height = image.Height;
                    width = image.Width;
                    outputSize = new Point((int)(width / zoomRatio), (int)(height / zoomRatio));
                }
                canvas = new(outputSize.X, outputSize.Y);
                RectangleF destinationRect = new(0, 0, width / zoomRatio, height / zoomRatio);
                RectangleF sourceRect = new(diffWidth, diffHeight, width, height);
                Graphics graphic = Graphics.FromImage(canvas);
                // 補間方法を高品質双三次補間に設定
                graphic.InterpolationMode =
                    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                // 表示する
                graphic.DrawImage(image, destinationRect, sourceRect, GraphicsUnit.Pixel);
                image.Dispose();
                graphic.Dispose();
                // BGの明度を下げる
                for (int y = 0; y < outputSize.Y; y++)
                {
                    for (int x = 0; x < outputSize.X; x++)
                    {
                        Color pixel = canvas.GetPixel(x, y);
                        int r = (int)(pixel.R * darknessFactor);
                        int g = (int)(pixel.G * darknessFactor);
                        int b = (int)(pixel.B * darknessFactor);
                        Color darkPixel = Color.FromArgb(r, g, b);
                        canvas.SetPixel(x, y, darkPixel);
                    }
                }

                return canvas;
            }
            catch
            {
                Common.WriteWarningMessage("LOG_W-GET-BG");
                return canvas;
            }
        }
        /// <summary>
        /// ピクセルをosu!のピクセルに変換する
        /// </summary>
        /// <param name="size">サイズ</param>
        /// <param name="clientSize">画面サイズ</param>
        /// <param name="dimension"></param>
        /// <param name="isWideScreen"></param>
        /// <returns></returns>
        internal static double ConvertPixelToOsuPixel(double size, double clientSize, int dimension = 1, bool isWideScreen = true)
        {
            double osuPixel = dimension == 0 ? (isWideScreen ? 854 : 640) : 480;
            if (clientSize <= 0)
            {
                return -1;
            }
            double convertRatio = osuPixel / clientSize;
            double retOsuPixel = size * convertRatio;
            return retOsuPixel;
        }
        /// <summary>
        /// osu!のピクセルをピクセルに変換する
        /// </summary>
        /// <param name="size">サイズ</param>
        /// <param name="clientSize">画面サイズ</param>
        /// <param name="dimension"></param>
        /// <param name="isWideScreen"></param>
        /// <returns></returns>
        internal static double ConvertOsuPixelToPixel(double size, double clientSize, int dimension = 1, bool isWideScreen = true)
        {
            double osuPixel = dimension == 0 ? (isWideScreen ? 854 : 640) : 480;
            if (clientSize <= 0)
            {
                return -1;
            }
            double convertRatio = clientSize / osuPixel;
            double retPixel = size * convertRatio;
            return retPixel;
        }

    }
}
