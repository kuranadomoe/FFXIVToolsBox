using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kuranado.Moe.FunctionalModule;

namespace Kuranado.Moe.FFXIV.WorkshopModule
{
    [ModuleInfo("捡垃圾统计", true)]
    public partial class HarvestStatistics : UserControl
    {
        public HarvestStatistics()
        {
            InitializeComponent();
        }

        #region 控件事件响应
        private void dgvHarvestData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DataGridView dgv))
                return;
            var cost = 0;
            var input = dgv[e.ColumnIndex, e.RowIndex].Value.ToString();
            var output = string.Empty;
            var time = new TimeSpan();
            foreach (var logRow in input.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                output += logRow + Environment.NewLine;
                var (timeSpan, harvest, count) = AnalysisHarvest(logRow.Trim());
                if (!JewelryValue.ContainsKey(harvest) || count <= 0)
                    continue;
                cost += JewelryValue[harvest] * count;
                time = timeSpan;
            }
            dgv[0, e.RowIndex].Value = DateTime.Today.Add(time).ToString();
            dgv[1, e.RowIndex].Value = output;
            dgv[2, e.RowIndex].Value = cost;
            dgv[3, e.RowIndex].Value = 0;
        }

        private void tsmiLoadCsv_Click(object sender, EventArgs e)
        {

        }

        private void tsmiSaveCsv_Click(object sender, EventArgs e)
        {

        }

        private void tsmiSaveAsCsv_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 私有字段/常量
        /// <summary>
        /// HQ图标的字符
        /// </summary>
        private const string HqChar = @"";
        /// <summary>
        /// 道具图标的字符
        /// </summary>
        private const string ItemChar = @"";
        /// <summary>
        /// <para>沉船首饰对应的金币价格</para>
        /// <para>key: 沉船首饰名</para>
        /// <para>value: 变卖金币</para>
        /// </summary>
        private readonly Dictionary<string, int> JewelryValue = new Dictionary<string, int>
        {
            { "沉船耳饰", 10000 },
            { "沉船项链", 13000 },
            { "沉船手镯", 9000 },
            { "沉船戒指", 8000 },
            { "上等沉船耳饰", 30000 },
            { "上等沉船项链", 34500 },
            { "上等沉船手镯", 28500 },
            { "上等沉船戒指", 27000 },
        };
        #endregion

        #region 私有方法(功能逻辑实现)
        /// <summary>
        /// 解析ffxiv系统日志
        /// </summary>
        /// <param name="logRow"></param>
        /// <returns></returns>
        private (TimeSpan time, string harvest, int count) AnalysisHarvest(string logRow)
        {
            try
            {
                var regex = new Regex($@"\[(?<hour>\d{2}):(?<minute>\d{2})\][^“]+“{ItemChar}(?<itemName>[^”]+)”×(?<count>\d+)。", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
                var match = regex.Match(logRow);
                if (match.Success)
                {
                    var hour = int.Parse(match.Groups["hour"].Value);
                    var minute = int.Parse(match.Groups["minute"].Value);
                    var time = new TimeSpan(hour, minute, 0);
                    var harvest = match.Groups["itemName"].Value;
                    var count = int.Parse(match.Groups["count"].Value);
                    return (time, harvest, count);
                }
            }
            catch (Exception e)
            {
                this.BeginInvoke(new Action(() => MessageBox.Show(e.ToString())));
            }
            return (new TimeSpan(0, 0, 0), "", 0);
        }
        #endregion

    }// end of class
}// end of namespace
