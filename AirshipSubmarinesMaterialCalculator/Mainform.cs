using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Kuranado.Moe.FFXIV
{
    public partial class Mainform : Form
    {
        /// <summary>
        /// 潜艇50级以后每级对应的额外属性奖励
        /// </summary>
        private short[] ExtraPerf = Array.CreateInstance(typeof(short), new int[] { 40 }, new int[] { 50 }) as short[];
        /// <summary>
        /// 每列列首的文本标签
        /// </summary>
        private Label[] ColumnTitleLabels = { new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" } };
        /// <summary>
        /// 每行行首的文本标签
        /// </summary>
        private Label[] RowTitleLabels = { new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" }, new Label() { Text = "" } };
        /// <summary>
        /// 飞空艇列首文本
        /// </summary>
        private string[] AirshipColumnTitle = { "部位", "船体", "舾装", "船首", "船尾" };
        /// <summary>
        /// 潜水艇列首文本
        /// </summary>
        private string[] SubmarineColumnTitle = { "部位", "船体", "船尾", "船首", "舰桥" };
        /// <summary>
        /// 数据锁
        /// </summary>
        private readonly object _lock = new object();
        /// <summary>
        /// 是否正在刷新数据
        /// </summary>
        private bool IsRefreshing { get; set; } = false;
        /// <summary>
        /// 当前是否切换到了潜艇
        /// </summary>
        private bool? IsSubmarine { get; set; } = null;
        /// <summary>
        /// 记录部件对应的数量
        /// </summary>
        private Dictionary<TroopExploration.PartInfo, int> PartsMap = new Dictionary<TroopExploration.PartInfo, int>();
        /// <summary>
        /// 不能购买/兑换的素材及其数量
        /// </summary>
        private Dictionary<TroopExploration.ItemSimInfo, int> NormalMaterial = new Dictionary<TroopExploration.ItemSimInfo, int>();
        /// <summary>
        /// 可以购买/兑换的素材及其数量
        /// </summary>
        private Dictionary<TroopExploration.ItemSimInfo, int> NPCMaterial = new Dictionary<TroopExploration.ItemSimInfo, int>();

        public Mainform()
        {
            InitializeComponent();
            this.Load += this.MainformLoad;
            this.FormClosing += this.MainformFormClosing;
            this.btnSwitch.Click += (x, y) => SwitchPartMenu();

            for (int i = 0; i < ColumnTitleLabels.Length; ++i)
            {
                ColumnTitleLabels[i].MouseHover += ShowText;
                tlpPartsMenu.Controls.Add(ColumnTitleLabels[i], i, 0);
            }
            for (int i = 0; i < RowTitleLabels.Length; ++i)
            {
                RowTitleLabels[i].MouseHover += ShowText;
                tlpPartsMenu.Controls.Add(RowTitleLabels[i], 0, i + 1);
            }
            for (int rowIndex = 0; rowIndex < 10; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < 4; ++columnIndex)
                {
                    var btn = new Button();
                    btn.Name = rowIndex + "_" + columnIndex;
                    btn.MouseDown += this.BtnPartSelected;
                    btn.MouseHover += ShowText;
                    tlpPartsMenu.Controls.Add(btn, columnIndex + 1, rowIndex + 1);
                }
            }
            var listItem = new ListViewItem();
            listItem.Text = "右上角切换潜水艇/飞空艇之后, 左键单击左侧的按钮添加对应部件到计算列表, 右键单击按钮从计算列表中删除对应部件";
            lvMaterialDetails.Items.Add(listItem);
            lvMaterialDetails.Columns[0].Width = -1;
            tabMainLayout.Selected += (x, y) =>
              {
                  if (y.TabPageIndex == tabMainLayout.TabPages.IndexOf(tpSuitDesign))
                  {
                      MessageBox.Show("还没有实现哦~~~");
                      tabMainLayout.SelectedIndex = 0;
                  }
              };
        }

        /// <summary>
        /// 在潜水艇和飞空艇之间切换
        /// </summary>
        private void SwitchPartMenu()
        {
            lock (_lock)
            {
                if (IsRefreshing)
                {
                    MessageBox.Show("数据更新尚未完成,请稍后");
                    return;
                }
            }
            string[] columnTitle = null;
            var partHull = TroopExploration.AirshipHull;
            if (btnSwitch.Text == "切换到潜水艇")
            {
                IsSubmarine = true;
                btnSwitch.Text = "切换到飞空艇";
                columnTitle = SubmarineColumnTitle;
                partHull = TroopExploration.SubmarineHull;
                for (int i = 15; i < tlpPartsMenu.Controls.Count; ++i)
                {
                    tlpPartsMenu.Controls[i].Visible = true;
                    if (i - 15 + 1 <= 20)
                        tlpPartsMenu.Controls[i].Text = ((i - 15) / 4 + 1).ToString();
                    else
                        tlpPartsMenu.Controls[i].Text = ((i - 15 - 20) / 4 + 1) + "改";
                }
            }
            else
            {
                IsSubmarine = false;
                btnSwitch.Text = "切换到潜水艇";
                columnTitle = AirshipColumnTitle;
                for (int i = RowTitleLabels.Length - 1; i > RowTitleLabels.Length - 1 - 3; --i)
                    RowTitleLabels[i].Text = "";
                for (int i = tlpPartsMenu.Controls.Count - 1; i > tlpPartsMenu.Controls.Count - 1 - 12; --i)
                    tlpPartsMenu.Controls[i].Visible = false;
                for (int i = 15; i < tlpPartsMenu.Controls.Count - 12; ++i)
                {
                    tlpPartsMenu.Controls[i].Visible = true;
                    tlpPartsMenu.Controls[i].Text = ((i - 15) / 4 + 1).ToString();
                }
            }
            for (int i = 0; i < columnTitle.Length; ++i)
                ColumnTitleLabels[i].Text = columnTitle[i];

            var rowTitle = (from item in partHull
                            select item.Name.Split(new string[] { "级" }, StringSplitOptions.None)[0] + "级").ToArray();
            for (int i = 0; i < rowTitle.Length && i < RowTitleLabels.Length; ++i)
                RowTitleLabels[i].Text = rowTitle[i];
        }

        /// <summary>
        /// 鼠标悬停时显示文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowText(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.AutoPopDelay = 5000;
            tip.InitialDelay = 500;
            tip.ReshowDelay = 500;
            tip.ShowAlways = true;

            var control = sender as Control;
            tip.SetToolTip(control, control.Text);
        }

        /// <summary>
        /// 所有部件选择区的按钮公用的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPartSelected(object sender, MouseEventArgs e)
        {
            if (IsSubmarine == null)
                return;

            var btn = sender as Button;
            var index = btn.Name.Split('_');
            var (rowIndex, columnIndex) = (int.Parse(index[0]), int.Parse(index[1]));
            ReadOnlyCollection<TroopExploration.PartInfo> dataSource = null;
            switch (columnIndex)
            {
                case 0: dataSource = IsSubmarine == true ? TroopExploration.SubmarineHull : TroopExploration.AirshipHull; break;
                case 1: dataSource = IsSubmarine == true ? TroopExploration.SubmarineStern : TroopExploration.AirshipOutfitting; break;
                case 2: dataSource = IsSubmarine == true ? TroopExploration.SubmarineBow : TroopExploration.AirshipBow; break;
                case 3: dataSource = IsSubmarine == true ? TroopExploration.SubmarineBridge : TroopExploration.AirshipStern; break;
            }
            var part = dataSource[rowIndex];
            var delta = e.Button == MouseButtons.Left ? 1 : -1;
            if (PartsMap.ContainsKey(part))
            {
                PartsMap[part] += delta;
            }
            else
            {
                PartsMap[part] = delta;
            }
            if (PartsMap[part] <= 0)
                PartsMap.Remove(part);


            lvMaterialDetails.BeginUpdate();

            lvMaterialDetails.Items.Clear();
            NormalMaterial.Clear();
            NPCMaterial.Clear();
            foreach (var partInfo in PartsMap.Keys)
            {
                var partCount = PartsMap[partInfo];
                foreach (var (item, num) in partInfo.MeterialList)
                {
                    if (item.Convertible)
                    {
                        if (NPCMaterial.ContainsKey(item))
                            NPCMaterial[item] += num * partCount;
                        else
                            NPCMaterial[item] = num * partCount;
                    }
                    else
                    {
                        if (NormalMaterial.ContainsKey(item))
                            NormalMaterial[item] += num * partCount;
                        else
                            NormalMaterial[item] = num * partCount;
                    }
                }
            }

            var maxIndex = Math.Max(PartsMap.Count, Math.Max(NormalMaterial.Count, NPCMaterial.Count));
            var parts = PartsMap.Keys.ToArray();
            var normalMaterial = NormalMaterial.Keys.ToArray();
            var npcMaterial = NPCMaterial.Keys.ToArray();
            for (int i = 0; i < maxIndex; ++i)
            {
                var listItem = new ListViewItem();
                if (i < parts.Length)
                    listItem.Text = $"{parts[i].Name} x {PartsMap[parts[i]]}";
                else
                    listItem.Text = "";
                if (i < normalMaterial.Length)
                    listItem.SubItems.Add($"{normalMaterial[i].Name} x {NormalMaterial[normalMaterial[i]]}");
                else
                    listItem.SubItems.Add("");
                if (i < npcMaterial.Length)
                    listItem.SubItems.Add($"{npcMaterial[i].Name}[{(npcMaterial[i].Cost > 0 ? npcMaterial[i].Cost + "G" : "兑换")}] x {NPCMaterial[npcMaterial[i]]}");
                else
                    listItem.SubItems.Add("");
                lvMaterialDetails.Items.Add(listItem);
            }
            foreach (ColumnHeader column in lvMaterialDetails.Columns)
                column.Width = -1;

            lvMaterialDetails.EndUpdate();
        }

        /// <summary>
        /// 主窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainformLoad(object sender, EventArgs e)
        {
            if (!Directory.Exists(CommonSetting.WikiDataDir))
                Directory.CreateDirectory(CommonSetting.WikiDataDir);
            if (!Directory.Exists(CommonSetting.DataDir))
                Directory.CreateDirectory(CommonSetting.DataDir);

            void start()
            {
                lock (_lock)
                    IsRefreshing = true;
                TroopExploration.LoadArchive();
                lock (_lock)
                    IsRefreshing = false;
                this.Invoke(new Action(() => MessageBox.Show("数据已更新!")));
            }
            new Thread(start).Start();
        }

        /// <summary>
        /// 主窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainformFormClosing(object sender, FormClosingEventArgs e)
        {
            TroopExploration.SaveArchive();
        }
    }
}
