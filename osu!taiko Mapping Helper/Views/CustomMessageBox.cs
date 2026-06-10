using osu_taiko_Mapping_Helper.Utils;

namespace osu_taiko_Mapping_Helper.Views
{
    internal sealed class CustomMessageBox : Form
    {
        private const int MIN_DIALOG_WIDTH = 220;
        private const int MIN_DIALOG_HEIGHT = 118;
        private const int MAX_DIALOG_HEIGHT = 480;
        private const int SCREEN_EDGE_MARGIN = 80;
        private const int HORIZONTAL_MARGIN = 22;
        private const int RIGHT_MARGIN = 10;
        private const int TOP_MARGIN = 17;
        private const int BOTTOM_MARGIN = 13;
        private const int ICON_MESSAGE_GAP = 15;
        private const int ICON_SIZE = 32;
        private const int BUTTON_WIDTH = 86;
        private const int BUTTON_HEIGHT = 28;
        private const int BUTTON_GAP = 8;
        private const int BUTTON_PANEL_HEIGHT = 46;
        private const int MIN_MESSAGE_WIDTH = 80;
        private const int MESSAGE_WIDTH_BUFFER = 8;

        private readonly MessageBoxButtons buttons;
        private readonly MessageBoxIcon icon;
        private readonly MessageBoxDefaultButton defaultButton;
        private readonly string message;

        internal CustomMessageBox(
            string message,
            string caption,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            this.message = message ?? string.Empty;
            this.buttons = buttons;
            this.icon = icon;
            this.defaultButton = defaultButton;

            Text = caption ?? string.Empty;
            Font = SystemFonts.MessageBoxFont;
            BackColor = SystemColors.Window;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true;

            BuildLayout();
        }

        internal CustomMessageBox(
            string messageCode,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            params object[] formatArgs)
            : this(
                ResolveMessage(messageCode, formatArgs),
                ResolveCaption(messageCode),
                buttons,
                ResolveIcon(messageCode))
        {
        }

        internal static DialogResult Show(
            IWin32Window? owner,
            string message,
            string caption,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            using CustomMessageBox dialog = new(message, caption, buttons, icon, defaultButton);
            return owner is null ? dialog.ShowDialog() : dialog.ShowDialog(owner);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape && CanCancelWithEscape(buttons))
            {
                SetResult(GetEscapeResult(buttons));
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        private void BuildLayout()
        {
            List<ButtonSpec> buttonSpecs = GetButtonSpecs(buttons).ToList();
            bool hasIcon = icon != MessageBoxIcon.None;

            int textLeft = HORIZONTAL_MARGIN + (hasIcon ? ICON_SIZE + ICON_MESSAGE_GAP : 0);

            Size measuredText = MeasureMessage();
            int messageWidth = Math.Max(measuredText.Width, MIN_MESSAGE_WIDTH);
            int rightMargin = RIGHT_MARGIN;

            int contentWidth = textLeft + messageWidth + rightMargin;
            int buttonsWidth = buttonSpecs.Count * BUTTON_WIDTH
                + Math.Max(0, buttonSpecs.Count - 1) * BUTTON_GAP
                + HORIZONTAL_MARGIN
                + rightMargin;

            int maxDialogWidth = GetMaxDialogWidth();
            int desiredDialogWidth = Math.Max(Math.Max(contentWidth, buttonsWidth), MIN_DIALOG_WIDTH);
            int dialogWidth = Math.Min(desiredDialogWidth, maxDialogWidth);

            int contentHeight = Math.Max(hasIcon ? ICON_SIZE : 0, measuredText.Height);
            int messageAreaHeight = contentHeight + TOP_MARGIN + BOTTOM_MARGIN;
            int clientHeight = Clamp(messageAreaHeight + BUTTON_PANEL_HEIGHT, MIN_DIALOG_HEIGHT, MAX_DIALOG_HEIGHT);
            int actualMessageAreaHeight = clientHeight - BUTTON_PANEL_HEIGHT;

            int iconTop = GetIconTop(actualMessageAreaHeight, measuredText.Height);

            ClientSize = new Size(dialogWidth, clientHeight);

            if (hasIcon)
            {
                Controls.Add(new PictureBox
                {
                    Image = GetIconImage(icon),
                    SizeMode = PictureBoxSizeMode.CenterImage,
                    Bounds = new Rectangle(HORIZONTAL_MARGIN, iconTop, ICON_SIZE, ICON_SIZE)
                });
            }

            Controls.Add(new MessageTextControl(message)
            {
                Font = Font,
                ForeColor = ForeColor,
                BackColor = BackColor,
                Bounds = new Rectangle(
                    textLeft,
                    0,
                    Math.Max(0, dialogWidth - textLeft - rightMargin),
                    actualMessageAreaHeight)
            });

            Panel buttonPanel = new()
            {
                BackColor = SystemColors.Control,
                Bounds = new Rectangle(0, clientHeight - BUTTON_PANEL_HEIGHT, dialogWidth, BUTTON_PANEL_HEIGHT)
            };
            Controls.Add(buttonPanel);

            int totalButtonWidth = buttonSpecs.Count * BUTTON_WIDTH + Math.Max(0, buttonSpecs.Count - 1) * BUTTON_GAP;
            int left = dialogWidth - rightMargin - totalButtonWidth;
            int defaultIndex = GetDefaultButtonIndex(defaultButton, buttonSpecs.Count);
            int buttonTop = (BUTTON_PANEL_HEIGHT - BUTTON_HEIGHT) / 2;

            for (int i = 0; i < buttonSpecs.Count; i++)
            {
                ButtonSpec spec = buttonSpecs[i];
                Button button = new()
                {
                    Text = spec.Text,
                    DialogResult = spec.Result,
                    Bounds = new Rectangle(left + i * (BUTTON_WIDTH + BUTTON_GAP), buttonTop, BUTTON_WIDTH, BUTTON_HEIGHT)
                };

                button.Click += (_, _) => SetResult(spec.Result);
                buttonPanel.Controls.Add(button);

                if (i == defaultIndex)
                {
                    AcceptButton = button;
                    ActiveControl = button;
                }

                if (spec.Result == DialogResult.Cancel)
                {
                    CancelButton = button;
                }
            }
        }

        private Size MeasureMessage()
        {
            string text = string.IsNullOrEmpty(message) ? " " : message;
            string[] lines = SplitMessageLines(text);

            int lineHeight = GetLineHeight();
            int width = 0;

            foreach (string line in lines)
            {
                string measureLine = string.IsNullOrEmpty(line) ? " " : line;
                using Label measureLabel = new()
                {
                    AutoSize = true,
                    Font = Font,
                    Text = measureLine,
                    UseMnemonic = false
                };

                Size lineSize = measureLabel.GetPreferredSize(Size.Empty);
                width = Math.Max(width, lineSize.Width + MESSAGE_WIDTH_BUFFER);
            }

            return new Size(width, lineHeight * Math.Max(1, lines.Length));
        }

        private int GetIconTop(int messageAreaHeight, int messageHeight)
        {
            int lineHeight = GetLineHeight();
            int lineCount = Math.Max(1, (int)Math.Ceiling((double)messageHeight / lineHeight));
            int anchorLineCount = Math.Min(lineCount, 3);
            int messageTop = Math.Max(0, (messageAreaHeight - messageHeight) / 2);
            int iconCenter = messageTop + (lineHeight * anchorLineCount / 2);

            return Clamp(iconCenter - (ICON_SIZE / 2), 0, Math.Max(0, messageAreaHeight - ICON_SIZE));
        }

        private void SetResult(DialogResult result)
        {
            DialogResult = result;

            // Existing call sites read this bool, so keep it in sync with DialogResult.
            Common.isDialogResult = result is DialogResult.OK or DialogResult.Yes or DialogResult.Retry;
            Close();
        }

        private static IEnumerable<ButtonSpec> GetButtonSpecs(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    yield return new ButtonSpec(GetLabel("LBL_DIALOG_OK", "OK"), DialogResult.OK);
                    break;
                case MessageBoxButtons.OKCancel:
                    yield return new ButtonSpec(GetLabel("LBL_DIALOG_OK", "OK"), DialogResult.OK);
                    yield return new ButtonSpec(GetLabel("LBL_DIALOG_CANCEL", "Cancel"), DialogResult.Cancel);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    yield return new ButtonSpec("Abort", DialogResult.Abort);
                    yield return new ButtonSpec("Retry", DialogResult.Retry);
                    yield return new ButtonSpec("Ignore", DialogResult.Ignore);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    yield return new ButtonSpec("Yes", DialogResult.Yes);
                    yield return new ButtonSpec("No", DialogResult.No);
                    yield return new ButtonSpec(GetLabel("LBL_DIALOG_CANCEL", "Cancel"), DialogResult.Cancel);
                    break;
                case MessageBoxButtons.YesNo:
                    yield return new ButtonSpec("Yes", DialogResult.Yes);
                    yield return new ButtonSpec("No", DialogResult.No);
                    break;
                case MessageBoxButtons.RetryCancel:
                    yield return new ButtonSpec("Retry", DialogResult.Retry);
                    yield return new ButtonSpec(GetLabel("LBL_DIALOG_CANCEL", "Cancel"), DialogResult.Cancel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }
        }

        private static string ResolveMessage(string messageCode, params object[] formatArgs)
        {
            string resolvedMessage = string.IsNullOrEmpty(messageCode) ? string.Empty : Common.WriteDialogMessage(messageCode);

            if (formatArgs.Length == 0)
            {
                return resolvedMessage;
            }

            return string.Format(resolvedMessage, formatArgs);
        }

        private static string ResolveCaption(string messageCode)
        {
            return GetMessageLevel(messageCode) switch
            {
                "W" => "Warning",
                "E" => "Error",
                _ => "Information"
            };
        }

        private static MessageBoxIcon ResolveIcon(string messageCode)
        {
            return GetMessageLevel(messageCode) switch
            {
                "W" => MessageBoxIcon.Warning,
                "E" => MessageBoxIcon.Error,
                _ => MessageBoxIcon.Information
            };
        }

        private static string GetMessageLevel(string messageCode)
        {
            return string.IsNullOrEmpty(messageCode) ? string.Empty : messageCode[..1];
        }

        private static Image GetIconImage(MessageBoxIcon icon)
        {
            if (icon == MessageBoxIcon.Error
                || icon == MessageBoxIcon.Hand
                || icon == MessageBoxIcon.Stop)
            {
                return SystemIcons.Error.ToBitmap();
            }

            if (icon == MessageBoxIcon.Warning || icon == MessageBoxIcon.Exclamation)
            {
                return SystemIcons.Warning.ToBitmap();
            }

            if (icon == MessageBoxIcon.Question)
            {
                return SystemIcons.Question.ToBitmap();
            }

            return SystemIcons.Information.ToBitmap();
        }

        private static int GetDefaultButtonIndex(MessageBoxDefaultButton defaultButton, int buttonCount)
        {
            int index = defaultButton switch
            {
                MessageBoxDefaultButton.Button2 => 1,
                MessageBoxDefaultButton.Button3 => 2,
                _ => 0
            };

            return Math.Min(index, buttonCount - 1);
        }

        private static bool CanCancelWithEscape(MessageBoxButtons buttons)
        {
            return buttons == MessageBoxButtons.OK
                || buttons == MessageBoxButtons.OKCancel
                || buttons == MessageBoxButtons.YesNoCancel
                || buttons == MessageBoxButtons.RetryCancel;
        }

        private static DialogResult GetEscapeResult(MessageBoxButtons buttons)
        {
            return buttons == MessageBoxButtons.OK ? DialogResult.OK : DialogResult.Cancel;
        }

        private static string GetLabel(string labelCode, string fallback)
        {
            Label label = new();
            Common.SetLabelText(label, labelCode);
            return string.IsNullOrWhiteSpace(label.Text) ? fallback : label.Text;
        }

        private static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        private int GetLineHeight()
        {
            return TextRenderer.MeasureText(
                "A",
                Font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding).Height;
        }

        private static string[] SplitMessageLines(string text)
        {
            return text
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split('\n');
        }

        private static int GetMaxDialogWidth()
        {
            Rectangle workingArea = Screen.PrimaryScreen?.WorkingArea ?? SystemInformation.WorkingArea;
            return Math.Max(MIN_DIALOG_WIDTH, workingArea.Width - SCREEN_EDGE_MARGIN);
        }

        private readonly struct ButtonSpec
        {
            internal ButtonSpec(string text, DialogResult result)
            {
                Text = text;
                Result = result;
            }

            internal string Text { get; }

            internal DialogResult Result { get; }
        }

        private sealed class MessageTextControl : Control
        {
            private readonly string[] lines;

            internal MessageTextControl(string text)
            {
                lines = SplitMessageLines(string.IsNullOrEmpty(text) ? " " : text);
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
                BackColor = SystemColors.Window;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                int lineHeight = TextRenderer.MeasureText(
                    "A",
                    Font,
                    new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding).Height;
                int totalTextHeight = lineHeight * lines.Length;
                int y = Math.Max(0, (Height - totalTextHeight) / 2);

                foreach (string line in lines)
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        string.IsNullOrEmpty(line) ? " " : line,
                        Font,
                        new Rectangle(0, y, Width, lineHeight),
                        ForeColor,
                        TextFormatFlags.SingleLine
                            | TextFormatFlags.NoPrefix
                            | TextFormatFlags.EndEllipsis
                            | TextFormatFlags.NoPadding
                            | TextFormatFlags.Left
                            | TextFormatFlags.VerticalCenter);
                    y += lineHeight;
                }
            }
        }
    }
}
