namespace Kuranado.Moe.FFXIV.WorkshopModule
{
    partial class HarvestStatistics
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvHarvestData = new System.Windows.Forms.DataGridView();
            this.dgvcDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcHarvestLog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcIncome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcRepairCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiLoadCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAsCsv = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHarvestData)).BeginInit();
            this.cmsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvHarvestData
            // 
            this.dgvHarvestData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvHarvestData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvHarvestData.BackgroundColor = System.Drawing.Color.White;
            this.dgvHarvestData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHarvestData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcDate,
            this.dgvcHarvestLog,
            this.dgvcIncome,
            this.dgvcRepairCost});
            this.dgvHarvestData.ContextMenuStrip = this.cmsMenu;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHarvestData.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvHarvestData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHarvestData.Location = new System.Drawing.Point(0, 0);
            this.dgvHarvestData.Name = "dgvHarvestData";
            this.dgvHarvestData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHarvestData.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvHarvestData.RowTemplate.Height = 23;
            this.dgvHarvestData.Size = new System.Drawing.Size(815, 485);
            this.dgvHarvestData.TabIndex = 1;
            this.dgvHarvestData.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHarvestData_CellEndEdit);
            // 
            // dgvcDate
            // 
            this.dgvcDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcDate.HeaderText = "日期";
            this.dgvcDate.Name = "dgvcDate";
            // 
            // dgvcHarvestLog
            // 
            this.dgvcHarvestLog.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcHarvestLog.HeaderText = "收成消息";
            this.dgvcHarvestLog.Name = "dgvcHarvestLog";
            // 
            // dgvcIncome
            // 
            this.dgvcIncome.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcIncome.HeaderText = "收益";
            this.dgvcIncome.Name = "dgvcIncome";
            // 
            // dgvcRepairCost
            // 
            this.dgvcRepairCost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcRepairCost.HeaderText = "修理支出";
            this.dgvcRepairCost.Name = "dgvcRepairCost";
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLoadCsv,
            this.tsmiSaveCsv,
            this.tsmiSaveAsCsv});
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.Size = new System.Drawing.Size(181, 92);
            // 
            // tsmiLoadCsv
            // 
            this.tsmiLoadCsv.Name = "tsmiLoadCsv";
            this.tsmiLoadCsv.Size = new System.Drawing.Size(180, 22);
            this.tsmiLoadCsv.Text = "加载记录(csv)";
            this.tsmiLoadCsv.Click += new System.EventHandler(this.tsmiLoadCsv_Click);
            // 
            // tsmiSaveCsv
            // 
            this.tsmiSaveCsv.Name = "tsmiSaveCsv";
            this.tsmiSaveCsv.Size = new System.Drawing.Size(180, 22);
            this.tsmiSaveCsv.Text = "保存记录(csv)";
            this.tsmiSaveCsv.Click += new System.EventHandler(this.tsmiSaveCsv_Click);
            // 
            // tsmiSaveAsCsv
            // 
            this.tsmiSaveAsCsv.Name = "tsmiSaveAsCsv";
            this.tsmiSaveAsCsv.Size = new System.Drawing.Size(180, 22);
            this.tsmiSaveAsCsv.Text = "另存为(csv)";
            this.tsmiSaveAsCsv.Click += new System.EventHandler(this.tsmiSaveAsCsv_Click);
            // 
            // HarvestStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvHarvestData);
            this.Name = "HarvestStatistics";
            this.Size = new System.Drawing.Size(815, 485);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHarvestData)).EndInit();
            this.cmsMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHarvestData;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcHarvestLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcIncome;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcRepairCost;
        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoadCsv;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveCsv;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAsCsv;
    }
}
