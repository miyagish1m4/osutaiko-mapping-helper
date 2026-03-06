using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Views
{
    public partial class HistoryForm : Form
    {
        // 未実装
        List<UserInputData> userInputData = [];
        public HistoryForm()
        {
            InitializeComponent();
        }
        private void HistoryForm_Load(object sender, EventArgs e)
        {
            string format = "yyyy/MM/dd HH:mm:ss.fff";
            DateTime date;
            UserInputDataHelper.DeserializeUserInputData(ref userInputData);
            if (userInputData.Count > 0)
            {
                date = userInputData[0].createDate;
                lblCreateDateData.Text = date.ToString(format);
            }
            else
            {
                Common.WriteDialogMessage("W_A_EM-001");
            }
        }
    }
}
