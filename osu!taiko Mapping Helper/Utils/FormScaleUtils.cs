namespace osu_taiko_Mapping_Helper.Utils
{
    /// <summary>
    /// 画面の大きさを、Windows の拡大率 (拡大/縮小) とモニターの大きさに合わせる処理
    /// </summary>
    /// <remarks>
    /// 画面は FullHD (1920×1080・拡大率100%) で入りきる大きさで作っている。
    /// 拡大率が 100% でない場合は拡大率の分だけ画面全体を大きくし、それでモニターに入りきらない場合は、入りきる大きさまで縮める<br/>
    /// WinForms の自動の拡大 (AutoScaleMode が Font) は、画面の文字の幅と高さを測って部品を大きくするため、
    /// 拡大率どおりに大きくなる文字と部品の大きさがずれ、文字が欄からはみ出す。そのため、この処理を使う画面は AutoScaleMode を None にして、
    /// 部品と文字を同じ割合で大きくする
    /// </remarks>
    internal static class FormScaleUtils
    {
        // 縮める割合の下限 (モニターの大きさが取れなかった場合などに、画面が無くならないようにする)
        private const float MIN_SCALE = 0.1f;
        // 縮める場合に、文字を部品よりも少し強めに縮める割合
        // (小さい文字は、割合どおりよりも少し大きめに描かれるため、部品と同じ割合で縮めると文字が枠からはみ出す)
        internal static float fontScaleRatio = 0.92f;
        // 拡大率が 100% のときの DPI
        private const float BASE_DPI = 96f;

        /// <summary>
        /// 画面を、モニターの作業領域 (タスクバーを除いた領域) に入りきる大きさにするための割合を求める
        /// </summary>
        /// <param name="formSize">画面の大きさ (枠を含む)</param>
        /// <param name="clientSize">画面の中身の大きさ (枠を含まない)</param>
        /// <param name="workingArea">モニターの作業領域</param>
        /// <returns>縮める割合。入りきる場合は 1 (大きくはしない)</returns>
        /// <remarks>
        /// タイトルバーや境界線などの枠は縮まないため、中身を縮めて入りきるようにする
        /// </remarks>
        internal static float GetFitScale(Size formSize, Size clientSize, Rectangle workingArea)
        {
            if (clientSize.Width <= 0 || clientSize.Height <= 0)
            {
                return 1f;
            }
            int frameWidth = formSize.Width - clientSize.Width;
            int frameHeight = formSize.Height - clientSize.Height;
            float scaleX = (float)(workingArea.Width - frameWidth) / clientSize.Width;
            float scaleY = (float)(workingArea.Height - frameHeight) / clientSize.Height;
            float scale = Math.Min(scaleX, scaleY);
            if (scale >= 1f)
            {
                return 1f;
            }
            return Math.Max(scale, MIN_SCALE);
        }
        /// <summary>
        /// 画面の大きさを、Windows の拡大率とメインモニターの大きさに合わせる
        /// </summary>
        /// <param name="form">対象の画面 (AutoScaleMode が None で、100% のときの大きさのもの)</param>
        /// <returns>部品の位置・大きさに掛けた割合 (拡大率 × 入りきる割合。何もしなかった場合は 1)</returns>
        internal static float FitToPrimaryScreen(Form form)
        {
            var screen = Screen.PrimaryScreen;
            if (screen == null)
            {
                return 1f;
            }
            return ScaleForDisplay(form, form.DeviceDpi / BASE_DPI, screen.WorkingArea);
        }
        /// <summary>
        /// 画面の大きさを、拡大率と作業領域に合わせる
        /// </summary>
        /// <param name="form">対象の画面 (AutoScaleMode が None で、100% のときの大きさのもの)</param>
        /// <param name="dpiScale">Windows の拡大率 (150% なら 1.5)</param>
        /// <param name="workingArea">モニターの作業領域</param>
        /// <returns>部品の位置・大きさに掛けた割合 (拡大率 × 入りきる割合。何もしなかった場合は 1)</returns>
        /// <remarks>
        /// 部品の位置・大きさには「拡大率 × 入りきる割合」を掛ける。
        /// 文字は Windows が拡大率の分だけ大きく描くため、文字の大きさには「入りきる割合」だけを掛ける<br/>
        /// 枠 (タイトルバー・境界線) は、Windows が拡大率に合わせて描く
        /// </remarks>
        internal static float ScaleForDisplay(Form form, float dpiScale, Rectangle workingArea)
        {
            if (dpiScale <= 0f)
            {
                dpiScale = 1f;
            }
            var clientSize = new Size((int)Math.Ceiling(form.ClientSize.Width * dpiScale),
                                      (int)Math.Ceiling(form.ClientSize.Height * dpiScale));
            var frameSize = form.Size - form.ClientSize;
            float fitScale = GetFitScale(clientSize + frameSize, clientSize, workingArea);
            float boundsScale = dpiScale * fitScale;
            if (boundsScale == 1f)
            {
                return 1f;
            }
            bool isShrunk = fitScale < 1f;
            ScaleForm(form, boundsScale, isShrunk ? fitScale * fontScaleRatio : 1f, isShrunk);
            return boundsScale;
        }
        /// <summary>
        /// 拡大率が 100% の場合に、画面が作業領域に入りきらなければ入りきる大きさに縮める
        /// </summary>
        /// <param name="form">対象の画面</param>
        /// <param name="workingArea">モニターの作業領域</param>
        /// <returns>縮めた割合 (縮めなかった場合は 1)</returns>
        internal static float FitToWorkingArea(Form form, Rectangle workingArea)
        {
            return ScaleForDisplay(form, 1f, workingArea);
        }
        /// <summary>
        /// 画面全体 (部品の位置・大きさ・文字・タブの見出し・画像) を指定の割合で縮める
        /// </summary>
        /// <param name="form">対象の画面</param>
        /// <param name="scale">縮める割合</param>
        internal static void ScaleForm(Form form, float scale)
        {
            ScaleForm(form, scale, scale * fontScaleRatio, scale < 1f);
        }
        /// <summary>
        /// 画面全体 (部品の位置・大きさ・文字・タブの見出し・画像) の大きさを変える
        /// </summary>
        /// <param name="form">対象の画面</param>
        /// <param name="boundsScale">部品の位置・大きさ・タブの見出しに掛ける割合</param>
        /// <param name="fontScale">文字の大きさに掛ける割合</param>
        /// <param name="isShrunk">縮める場合は true (ボタンなどの文字の描き方を変える)</param>
        /// <remarks>
        /// 部品の位置と大きさは Scale で変える。文字・タブの見出し・画像は Scale では変わらないため、ここで変える<br/>
        /// 画面そのものの文字は変えない (AutoScaleMode が Font の画面は、画面の文字を変えると部品の大きさが自動で変わるため)
        /// </remarks>
        internal static void ScaleForm(Form form, float boundsScale, float fontScale, bool isShrunk)
        {
            // 文字の大きさは、先に全部の部品の分を覚えておく
            // (親の文字を先に変えると、親の文字をそのまま使っている子の文字が二重に変わるため)
            var controls = GetAllControls(form).ToList();
            var fonts = controls.ToDictionary(control => control, control => control.Font);
            var toolStripItems = controls.OfType<ToolStrip>().SelectMany(toolStrip => GetAllItems(toolStrip.Items)).ToList();
            var toolStripItemFonts = toolStripItems.ToDictionary(item => item, item => item.Font);

            form.SuspendLayout();
            form.Scale(new SizeF(boundsScale, boundsScale));
            foreach (var control in controls)
            {
                if (fontScale != 1f)
                {
                    control.Font = ScaleFont(fonts[control], fontScale);
                }
                if (isShrunk && control is ButtonBase buttonBase)
                {
                    // ボタン・チェックボックス・ラジオボタンは、枠の内側に決まった大きさのすき間をとって文字を描く (GDI) ため、
                    // 縮めると文字を描ける高さが足りずに文字の下が切れる。縮めた場合は、行の高さで中央にそろえる描き方 (GDI+) にする
                    buttonBase.UseCompatibleTextRendering = true;
                }
                switch (control)
                {
                    case TabControl tabControl:
                        // タブの見出しの大きさ (WinForms は拡大率に合わせて大きくしないため、部品と同じ割合を掛ける)
                        // (見出しの幅の合計がタブの幅を超えると左右の矢印が出るため、切り捨ててから 1 ピクセル余裕を持たせる)
                        tabControl.ItemSize = new Size((int)(tabControl.ItemSize.Width * boundsScale) - 1,
                                                       (int)(tabControl.ItemSize.Height * boundsScale));
                        break;
                    case PictureBox pictureBox when pictureBox.SizeMode == PictureBoxSizeMode.Normal:
                        // 画像は、枠に合わせて表示する
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        break;
                }
            }
            if (fontScale != 1f)
            {
                foreach (var item in toolStripItems)
                {
                    item.Font = ScaleFont(toolStripItemFonts[item], fontScale);
                }
            }
            form.ResumeLayout(true);
        }
        /// <summary>
        /// 文字の大きさを指定の割合で変える
        /// </summary>
        private static Font ScaleFont(Font font, float scale)
        {
            return new Font(font.FontFamily, font.SizeInPoints * scale, font.Style, GraphicsUnit.Point, font.GdiCharSet);
        }
        /// <summary>
        /// 画面の中の全部の部品 (子の部品も含む。画面そのものは含まない)
        /// </summary>
        private static IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;
                foreach (var child in GetAllControls(control))
                {
                    yield return child;
                }
            }
        }
        /// <summary>
        /// メニューの全部の項目 (子の項目も含む)
        /// </summary>
        private static IEnumerable<ToolStripItem> GetAllItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                yield return item;
                if (item is ToolStripDropDownItem dropDownItem)
                {
                    foreach (var child in GetAllItems(dropDownItem.DropDownItems))
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}
